using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class RealAssetMgr : IUserAssetMgr
{
    //public Transform uiroot;
    public RealAssetMgr()
    {
        //uiroot = GameObject.Find("UIRoot2D").transform;

    }

    public Texture2D GetTexture(string name)
    {
        name = name.ToLower();
        Debug.Log("GotTex:" + name);
        if (mapTexture.ContainsKey(name))
            return mapTexture[name].obj;
        else
            return null;
    }
    public Mesh GetMesh(string name)
    {
        name = name.ToLower();
        if (mapMesh.ContainsKey(name))
            return mapMesh[name].obj;
        else
            return null;
    }

    public void AddTexture2D(string name, Texture2D tex)
    {
        name = name.ToLower();
        Debug.Log("AddTex:" + name);
        if (!mapTexture.ContainsKey(name))
        {
            mapTexture[name] = new RefCountObj<Texture2D>(tex);
            tex.name = name;
        }
        else
        {

            Debug.Log("重复加载" + name);
        }
    }

    public void TextureAddRef(string name)
    {
        name = name.ToLower();

        if (mapTexture.ContainsKey(name))
            mapTexture[name].AddRef();
        else 
        {
            Debug.Log("TextureAddRef fail:" + name);
        }
    }
    public void TextureDecRef(string name)
    {
        name = name.ToLower();

        if (mapTexture.ContainsKey(name))
        {
            if (mapTexture[name].DecRef() == 0)
            {
                mapTexture.Remove(name);
            }
        }
        else
        {
            Debug.Log("TextureDecRef fail:" + name);
        }



    }
    Dictionary<string, RefCountObj<Texture2D>> mapTexture = new Dictionary<string, RefCountObj<Texture2D>>();

    public void AddMesh(string name, Mesh mesh)
    {
        name = name.ToLower();

        if (!mapMesh.ContainsKey(name))
        {
            mapMesh[name] = new RefCountObj<Mesh>(mesh);
            mesh.name = name;
        }
        else
        {

            Debug.Log("重复加载" + name);
        }
    }
    public void MeshAddRef(string name)
    {
        name = name.ToLower();

        if (mapMesh.ContainsKey(name))
            mapMesh[name].AddRef();
        else
        {
            Debug.Log("MeshAddRef fail:" + name);
        }
    }
    public void MeshDecRef(string name)
    {
        name = name.ToLower();

        if (mapMesh.ContainsKey(name))
        {
            if (mapMesh[name].DecRef() == 0)
            {
                mapMesh.Remove(name);
            }
        }
        else
        {
            Debug.Log("MeshDecRef fail:" + name);
        }



    }
    Dictionary<string, RefCountObj<Mesh>> mapMesh = new Dictionary<string, RefCountObj<Mesh>>();

}