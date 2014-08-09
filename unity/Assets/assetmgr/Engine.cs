using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public interface IEngine
{
    void Update(float deltatime);
    void Destory();
}
class Engine
{

    static IEngine engine;
    public static void UpdateEngine(float delta)
    {
        if (engine != null)
        {
            engine.Update(delta);
        }
    }
    public static void UseEngine(IEngine engine)
    {
        if (engine != null)
        {
            engine.Destory();
        }
        Engine.engine = engine;
    }
}
public interface IProxy<T>
{
    void Unload();
    string name
    {
        get;
    }
    T obj
    {
        get;
    }

}


public class Engine001 : IEngine
{
    #region Static&Instance
    static Engine001 g_this;
    public static void Init()
    {
        if (g_this == null)
        {
            g_this = new Engine001();
            Engine.UseEngine(g_this);
        }

    }
    public static void Exit()
    {
        g_this.Destory();
        g_this = null;
    }
    public static Engine001 Instance
    {
        get
        {
            return g_this;
        }
    }
    #endregion

    class ProxyLayout : IProxy<GameObject>
    {
        public ProxyLayout(GameObject obj, string name)
        {
            this.name = name;
            this.obj = obj;
        }
        public void Unload()
        {//处理依赖
            GameObject.Destroy(obj);
            Engine001.Instance.UnloadLayout(name);
        }

        public string name
        {
            get;
            private set;
        }

        public GameObject obj
        {
            get;
            private set;
        }
    }
    class ProxyTexture2D : IProxy<Texture2D>
    {
        public ProxyTexture2D(Texture2D tex, string name)
        {
            this.name = name;
            this.obj = tex;
        }
        public void Unload()
        {//处理依赖

            Engine001.Instance.UnloadTexture2D(name);
        }

        public string name
        {
            get;
            private set;
        }

        public Texture2D obj
        {
            get;
            private set;
        }
    }
    class ProxyMesh : IProxy<Mesh>
    {
        public ProxyMesh(Mesh mesh, string name)
        {
            this.name = name;
            this.obj = mesh;
        }
        public void Unload()
        {//处理依赖

            Engine001.Instance.UnloadMesh(name);
        }

        public string name
        {
            get;
            private set;
        }

        public Mesh obj
        {
            get;
            private set;
        }
    }
    private Engine001()
    {
        assetMgrIO = new RealAssetMgr();
        AssetMgr.Instance.realmgr = assetMgrIO;
    }
    public void Destory()
    {

    }
    public void Update(float delta)
    {

    }
    RealAssetMgr assetMgrIO = null;
    public Dictionary<string, MyJson.JsonNode_Object> layoutDepends = new Dictionary<string, MyJson.JsonNode_Object>();
    public void LoadLayout(string respath, string layoutname, Action<IProxy<GameObject>> finish)
    {
        Action Finish = () =>
        {
            ResmgrNative.Instance.LoadStringFromCache(respath + "/layout/" + layoutname + ".layout.txt",
                "",
                (code, tag) =>
                {
                    var json = MyJson.Parse(code);
                    var obj = GameObjParser.FromJson(json);
                    finish(new ProxyLayout(obj, layoutname));
                }
                );
        };
        Action<MyJson.IJsonNode> dependLoaded = (json) =>
            {

                if (json.HaveDictItem("mesh"))
                {
                    var list = json.GetDictItem("mesh").AsList();
                    foreach (var l in list)
                    {
                        ResmgrNative.Instance.LoadBytesFromCache(respath + "/mesh/" + l.AsString() + ".mesh.bytes",
                            l.AsString(),
                            (bytes, tagName) =>
                            {
                                Mesh mesh = AssetMgr.ReadMesh(bytes, tagName);
                                assetMgrIO.AddMesh(tagName, mesh);
                                assetMgrIO.MeshAddRef(tagName);
                            });
                        Debug.Log("dependmesh:" + l.AsString());
                    }
                }
                if (json.HaveDictItem("tex"))
                {
                    var list = json.GetDictItem("tex").AsList();
                    foreach (var l in list)
                    {
                        ResmgrNative.Instance.LoadTexture2DFromCache(respath + "/texture/" + l.AsString(),
                            l.AsString(),
                            (tex, tagName) =>
                            {

                                assetMgrIO.AddTexture2D(tagName, tex);
                                assetMgrIO.TextureAddRef(tagName);
                            });
                    }
                }
                ResmgrNative.Instance.WaitForTaskFinish(Finish);
            };

        if (layoutDepends.ContainsKey(layoutname))
        {
            dependLoaded(layoutDepends[layoutname]);
        }
        else
        {
            ResmgrNative.Instance.LoadStringFromCache(respath + "/layout/" + layoutname + ".depend.txt", "", (code, tag) =>
                {
                    layoutDepends[layoutname] = MyJson.Parse(code) as MyJson.JsonNode_Object;
                    dependLoaded(layoutDepends[layoutname]);
                });
        }
    }

    protected void UnloadLayout(string layoutname)
    {
        var json = layoutDepends[layoutname];
        if (json.HaveDictItem("mesh"))
        {
            var list = json.GetDictItem("mesh").AsList();
            foreach (var l in list)
            {
                assetMgrIO.MeshDecRef(l.AsString());
            }
        }
        if (json.HaveDictItem("tex"))
        {
            var list = json.GetDictItem("tex").AsList();
            foreach (var l in list)
            {
                assetMgrIO.TextureDecRef(l.AsString());
            }
        }
    }
    public void LoadTexture(string respath, string name, Action<IProxy<Texture2D>> finish)
    {
        var tex = assetMgrIO.GetTexture(name);
        if (tex != null)
        {
            var ptex = new ProxyTexture2D(tex, name);
            assetMgrIO.TextureAddRef(name);
            finish(ptex);
        }
        else
        {
            ResmgrNative.Instance.LoadTexture2DFromCache(respath + "/texture/" + name,
                          name,
                           (ltex, tagName) =>
                           {

                               assetMgrIO.AddTexture2D(tagName, ltex);
                               var ptex = new ProxyTexture2D(ltex, name);
                               assetMgrIO.TextureAddRef(tagName);
                               finish(ptex);
                           });
        }

    }
    public void LoadMesh(string respath, string name, Action<IProxy<Mesh>> finish)
    {
        var mesh = assetMgrIO.GetMesh(name);
        if (mesh != null)
        {
            var pmesh = new ProxyMesh(mesh, name);
            assetMgrIO.MeshAddRef(name);
            finish(pmesh);
        }
        else
        {
            ResmgrNative.Instance.LoadBytesFromCache(respath + "/mesh/" + name + ".mesh.bytes",
                name,
                (bytes, tagName) =>
                {
                    Mesh lmesh = AssetMgr.ReadMesh(bytes, tagName);
                    assetMgrIO.AddMesh(tagName, lmesh);
                    var pmesh = new ProxyMesh(lmesh, name);
                    assetMgrIO.MeshAddRef(tagName);
                    finish(pmesh);
                });
        }

    }
    public void UnloadTexture2D(string name)
    {
        assetMgrIO.TextureDecRef(name);
    }
    public void UnloadMesh(string name)
    {
        assetMgrIO.MeshDecRef(name);
    }
}

