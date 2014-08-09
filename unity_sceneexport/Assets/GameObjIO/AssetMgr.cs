using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;
public interface IUserAssetMgr
{
    Mesh GetMesh(string name);
    Texture2D GetTexture(string name);
}
public class AssetMgr
{
    public IUserAssetMgr realmgr
    {
        get;
        set;
    }
    private static AssetMgr g_this = null;
    public static AssetMgr Instance
    {
        get
        {
            if (g_this == null)
            {
                g_this = new AssetMgr();
            }
            return g_this;
        }
    }

#if UNITY_EDITOR
    public static void Reset()
    {
        savedMeshes.Clear();
    }
    static Dictionary<string, Mesh> savedMeshes = new Dictionary<string, Mesh>();
    public static string SaveMesh(Mesh mesh, string path)
    {
        if (savedMeshes.ContainsKey(mesh.name))
        {
            if (savedMeshes[mesh.name].GetInstanceID() != mesh.GetInstanceID())
            {
                Debug.LogError("有重名mesh:"+mesh.name);
            }
            return mesh.name;
        }
        if (System.IO.Directory.Exists(path) == false)
        {
            System.IO.Directory.CreateDirectory(path);
        }
        using (System.IO.Stream s = System.IO.File.Open(System.IO.Path.Combine(path, mesh.name + ".mesh.bytes"), System.IO.FileMode.Create))
        {
            byte[] buf = BitHelper.getBytes(mesh.bounds);
            s.Write(buf, 0, 24);
            UInt32 vc = (UInt32)mesh.vertexCount;
            buf = BitConverter.GetBytes(vc);
            s.Write(buf, 0, 4);
            if (mesh.vertices != null && mesh.vertices.Length != 0)
            {
                s.WriteByte(1);//1 vb pos tag
                for (int i = 0; i < vc; i++)
                {
                    s.Write(BitHelper.getBytes(mesh.vertices[i]), 0, 12);
                    if (i == 0)
                    {
                        Debug.Log("pos0:" + mesh.vertices[i]);
                    }
                }
            }
            if (mesh.colors32 != null && mesh.colors32.Length != 0)
            {

                s.WriteByte(2);//2 vb color tag;
                for (int i = 0; i < vc; i++)
                {
                    s.Write(BitHelper.getBytes(mesh.colors32[i]), 0, 4);
                    Debug.Log("color0:" + mesh.colors32[i]);
                    if (i == 0)
                    {
                        Debug.Log("pos0:" + mesh.vertices[i]);
                    }
                }
            }
            if (mesh.normals != null && mesh.normals.Length != 0)
            {
                s.WriteByte(3);//3 vb normal tag;
                for (int i = 0; i < vc; i++)
                {
                    s.Write(BitHelper.getBytes(mesh.normals[i]), 0, 12);
                    if (i == 0)
                    {
                        Debug.Log("normal0:" + mesh.normals[i]);
                    }
                }
            }
            if (mesh.uv != null && mesh.uv.Length != 0)
            {
                s.WriteByte(4);//4 vb uv tag;
                for (int i = 0; i < vc; i++)
                {
                    s.Write(BitHelper.getBytes(mesh.uv[i]), 0, 8);
                    if (i == 0)
                    {
                        Debug.Log("uv0:" + mesh.uv[i]);
                    }
                }
            }
            if (mesh.uv1 != null && mesh.uv1.Length != 0)
            {
                s.WriteByte(5);//5 vb uv1 tag;
                for (int i = 0; i < vc; i++)
                {
                    s.Write(BitHelper.getBytes(mesh.uv1[i]), 0, 8);
                }
            }
            if (mesh.uv2 != null && mesh.uv2.Length != 0)
            {
                s.WriteByte(6);//6 vb uv2 tag;
                for (int i = 0; i < vc; i++)
                {
                    s.Write(BitHelper.getBytes(mesh.uv[i]), 0, 8);
                }
            }
            s.WriteByte(255);//vb end
            int sub = mesh.subMeshCount;
            s.WriteByte((byte)sub);
            {
                Debug.Log("sub:" + sub);
            }
            for (int i = 0; i < sub; i++)
            {
                int tv = (int)mesh.GetTopology(i);//绘制方式
                s.Write(BitConverter.GetBytes(tv), 0, 4);
                var indices = mesh.GetIndices(i);//索引
                UInt16 length = (UInt16)indices.Length;
                s.Write(BitConverter.GetBytes(length), 0, 2);
                for (int j = 0; j < length; j++)
                {
                    s.Write(BitConverter.GetBytes(indices[j]), 0, 4);
                }
            }
        }
        savedMeshes[mesh.name] = mesh;
        return mesh.name;
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
    public static Mesh ReadMesh(byte[] data, string name)
    {
        Mesh m = new Mesh();
        int seek = 0;
        m.bounds = BitHelper.ToBounds(data, seek);
        seek += 24;
        UInt32 vcount = BitConverter.ToUInt32(data, seek);
        seek += 4;
        while (true)
        {
            byte tag = data[seek];
            seek += 1;
            if (tag == 255) break;
            if (tag == 1)//pos
            {
                Vector3[] poss = new Vector3[vcount];
                for (int i = 0; i < vcount; i++)
                {
                    poss[i] = BitHelper.ToVector3(data, seek);
                    seek += 12;
                    if (i == 0)
                    {
                        Debug.Log("vertices0:" + poss[i]);
                    }
                }
                m.vertices = poss;
            }
            if (tag == 2)//color
            {
                Color32[] colors = new Color32[vcount];
                for (int i = 0; i < vcount; i++)
                {
                    colors[i] = BitHelper.ToColor32(data, seek);
                    seek += 4;
                    if (i == 0)
                    {
                        Debug.Log("colors320:" + colors[i]);
                    }
                }
                m.colors32 = colors;
            }
            if (tag == 3)//normal
            {
                Vector3[] normals = new Vector3[vcount];
                for (int i = 0; i < vcount; i++)
                {
                    normals[i] = BitHelper.ToVector3(data, seek);
                    seek += 12;
                    if (i == 0)
                    {
                        Debug.Log("normals0:" + normals[i]);
                    }
                }
                m.normals = normals;
            }
            if (tag == 4)//uv
            {
                Vector2[] uvs = new Vector2[vcount];
                for (int i = 0; i < vcount; i++)
                {
                    uvs[i] = BitHelper.ToVector2(data, seek);
                    seek += 8;
                    if (i == 0)
                    {
                        Debug.Log("uvs0:" + uvs[i]);
                    }
                }
                m.uv = uvs;
            }
            if (tag == 5)//uv1
            {
                Vector2[] uvs = new Vector2[vcount];
                for (int i = 0; i < vcount; i++)
                {
                    uvs[i] = BitHelper.ToVector2(data, seek);
                    seek += 8;
                }
                m.uv1 = uvs;
            }
            if (tag == 6)//uv2
            {
                Vector2[] uvs = new Vector2[vcount];
                for (int i = 0; i < vcount; i++)
                {
                    uvs[i] = BitHelper.ToVector2(data, seek);
                    seek += 8;
                }
                m.uv2 = uvs;
            }
        }
        int subcount = data[seek];
        seek += 1;
        Debug.Log("sub=" + subcount);
        for (int i = 0; i < subcount; i++)
        {
            int tv = BitConverter.ToInt32(data, seek);
            seek += 4;


            UInt16 length = BitConverter.ToUInt16(data, seek);
            seek += 2;
            int[] indices = new int[length];
            for (int j = 0; j < length; j++)
            {
                indices[j] = BitConverter.ToInt32(data, seek);
                seek += 4;
            }
            MeshTopology ttv=(MeshTopology)tv;
            m.SetIndices(indices, (MeshTopology)tv, i);
        }
        m.name = name;
        return m;
    }


    public Texture2D GetTexture(string name)
    {
        return realmgr.GetTexture(name);
    }

    public Mesh GetMesh(string name)
    {

        return realmgr.GetMesh(name);
    }
}
