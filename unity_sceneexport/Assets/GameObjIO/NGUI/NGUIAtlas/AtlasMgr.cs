using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;
#if NGUI
public interface IUserAtlasMgr
{
    UIAtlas GetAtlas(string name);
    Texture2D GetTexture(string name);
}
public class AtlasMgr
{
    public IUserAtlasMgr realmgr
    {
        get;
        set;
    }
    private static AtlasMgr g_this = null;
    public static AtlasMgr Instance
    {
        get
        {
            if (g_this == null)
            {
                g_this = new AtlasMgr();
            }
            return g_this;
        }
    }

#if UNITY_EDITOR
    public static string SaveAtlas(UIAtlas atlas, string path,string pathtex)
    {
        if (System.IO.Directory.Exists(path) == false)
        {
            System.IO.Directory.CreateDirectory(path);
        }
        string tname = SaveTexture(atlas.texture, pathtex);

        MyJson.JsonNode_Object obj = new MyJson.JsonNode_Object();

        obj["pixelSize"] = new MyJson.JsonNode_ValueNumber(atlas.pixelSize);
        var jsonmat = new MyJson.JsonNode_Object();
        obj["mat"] = jsonmat;
        jsonmat["shader"] = new MyJson.JsonNode_ValueString(atlas.spriteMaterial.shader.name);
        MyJson.JsonNode_Array shaderparams = new MyJson.JsonNode_Array();
        jsonmat["shaderparam"] = shaderparams;
        foreach (var p in atlas.spriteMaterial.shaderKeywords)
        {
            shaderparams.Add(new MyJson.JsonNode_ValueString(p));
        }

        jsonmat["img"] = new MyJson.JsonNode_ValueString(tname);
        var jsonsparray = new MyJson.JsonNode_Object();
        obj["sprites"] = jsonsparray;
        foreach (var s in atlas.spriteList)
        {
            MyJson.JsonNode_Array sarray = new MyJson.JsonNode_Array();
            jsonsparray[s.name] = sarray;
            sarray.Add(new MyJson.JsonNode_ValueNumber(s.x));
            sarray.Add(new MyJson.JsonNode_ValueNumber(s.y));
            sarray.Add(new MyJson.JsonNode_ValueNumber(s.width));
            sarray.Add(new MyJson.JsonNode_ValueNumber(s.height));
            sarray.Add(new MyJson.JsonNode_ValueNumber(s.borderLeft));
            sarray.Add(new MyJson.JsonNode_ValueNumber(s.borderRight));
            sarray.Add(new MyJson.JsonNode_ValueNumber(s.borderTop));
            sarray.Add(new MyJson.JsonNode_ValueNumber(s.borderBottom));
            sarray.Add(new MyJson.JsonNode_ValueNumber(s.paddingLeft));
            sarray.Add(new MyJson.JsonNode_ValueNumber(s.paddingRight));
            sarray.Add(new MyJson.JsonNode_ValueNumber(s.paddingTop));
            sarray.Add(new MyJson.JsonNode_ValueNumber(s.paddingBottom));
        }
        string atalsName = atlas.name.ToLower();
        System.IO.File.WriteAllText(System.IO.Path.Combine(path, atalsName + ".atlas.txt"), obj.ToString());

        return atalsName;
    }
    public static string SaveTexture(Texture tex, string path)
    {
        if (System.IO.Directory.Exists(path) == false)
        {
            System.IO.Directory.CreateDirectory(path);
        }
        string tname = tex.name.ToLower();
        string[] tt = System.IO.Directory.GetFiles(Application.dataPath, tname.ToLower() + ".png", System.IO.SearchOption.AllDirectories);
        List<string> pngs = new List<string>();
        bool bpng = false;
        foreach (var i in tt)
        {
            if (i.ToLower().Contains("streamingassets") == false)
                pngs.Add(i);
            bpng = true;
        }
        tt = System.IO.Directory.GetFiles(Application.dataPath, tname.ToLower() + ".jpg", System.IO.SearchOption.AllDirectories);
         foreach (var i in tt)
         {
             if (i.ToLower().Contains("streamingassets") == false)
                 pngs.Add(i);
             bpng = false;
         }
         tt = System.IO.Directory.GetFiles(Application.dataPath, tname.ToLower() + ".jpeg", System.IO.SearchOption.AllDirectories);
         foreach (var i in tt)
         {
             if (i.ToLower().Contains("streamingassets") == false)
                 pngs.Add(i);
             bpng = false;
         }
        if (pngs.Count == 0)
        {
            Debug.LogError("贴图" + tname.ToLower() + "没找到");
            return tex.name;
        }
        else if (pngs.Count > 1)
        {
            Debug.LogError("贴图" + tname.ToLower() + "有重名");
            return tex.name;
        }
        string fname = tname + (bpng ? ".png" : ".jpg");
        System.IO.File.Copy(pngs[0], System.IO.Path.Combine(path, fname.ToLower()), true);

        return fname;
    }

#endif
    //从Atlas 加载
    public static UIAtlas UIAtlasFromJson(MyJson.JsonNode_Object objJson,string name)
    {
        GameObject _obj = new GameObject();
        _obj.hideFlags = HideFlags.HideInHierarchy;// false;
        _obj.SetActive(false);
        UIAtlas atlas = _obj.AddComponent<UIAtlas>();
        MyJson.JsonNode_Object obj = objJson;
        atlas.name = name;
        atlas.pixelSize = obj["pixelSize"] as MyJson.JsonNode_ValueNumber;
        obj["pixelSize"] = new MyJson.JsonNode_ValueNumber(atlas.pixelSize);
        MyJson.JsonNode_Object jsonmat = obj["mat"] as MyJson.JsonNode_Object;
        string shader = jsonmat["shader"] as MyJson.JsonNode_ValueString;
        List<string> pp = new List<string>();
        foreach (MyJson.JsonNode_ValueString p in jsonmat["shaderparam"] as MyJson.JsonNode_Array)
        {
            pp.Add(p);
        }
        Material mat = new Material(Shader.Find(shader));
        mat.shaderKeywords = pp.ToArray();
        if(jsonmat.ContainsKey("img"))
        {
            string texname =jsonmat["img"] as MyJson.JsonNode_ValueString;
            texname = texname.ToLower();
            var tex = AtlasMgr.Instance.GetTexture(texname);
            if(tex!=null)
            {
                mat.mainTexture = tex;

            }
            else
            {
                Debug.Log("tex:" + texname + " not found");

            }
        }
        else
        {

            Debug.Log("tex   not exist.");

        }
        

        atlas.spriteMaterial = mat;
        MyJson.JsonNode_Object sps = obj["sprites"] as MyJson.JsonNode_Object;
        //jsonmat["img"] = new MyJson.JsonNode_ValueString(tname);
        foreach (var s in sps)
        {
            UISpriteData data = new UISpriteData();
            data.name = s.Key;

            MyJson.JsonNode_Array sarray = s.Value as MyJson.JsonNode_Array;
            data.x = sarray[0] as MyJson.JsonNode_ValueNumber;
            data.y = sarray[1] as MyJson.JsonNode_ValueNumber;
            data.width = sarray[2] as MyJson.JsonNode_ValueNumber;
            data.height = sarray[3] as MyJson.JsonNode_ValueNumber;
            data.borderLeft = sarray[4] as MyJson.JsonNode_ValueNumber;
            data.borderRight = sarray[5] as MyJson.JsonNode_ValueNumber;
            data.borderTop = sarray[6] as MyJson.JsonNode_ValueNumber;
            data.borderBottom = sarray[7] as MyJson.JsonNode_ValueNumber;
            data.paddingLeft = sarray[8] as MyJson.JsonNode_ValueNumber;
            data.paddingRight = sarray[9] as MyJson.JsonNode_ValueNumber;
            data.paddingTop = sarray[10] as MyJson.JsonNode_ValueNumber;
            data.paddingBottom = sarray[11] as MyJson.JsonNode_ValueNumber;
            atlas.spriteList.Add(data);
        }
        return atlas;
    }


    public Texture2D GetTexture(string name)
    {
        return realmgr.GetTexture(name);
    }

    public UIAtlas GetAtlas(string name)
    {

        return realmgr.GetAtlas(name);
    }
}
#endif