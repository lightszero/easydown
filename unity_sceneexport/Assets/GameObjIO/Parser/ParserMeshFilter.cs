using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

class CParserMeshFilter : IComponentParser
{
    public Type parserType
    {
        get
        {
            return typeof(MeshFilter);
        }
    }
#if UNITY_EDITOR
    public MyJson.IJsonNode Parser(Component com, NeedList list)
    {
        MeshFilter t = com as MeshFilter;
        var json = new MyJson.JsonNode_Object();
        var name = AssetMgr.SaveMesh(t.sharedMesh, System.IO.Path.Combine(Application.streamingAssetsPath, "mesh"));
        list.AddDependMesh(name);
        json["mesh"] = new MyJson.JsonNode_ValueString(name);
        return json;
    }
#endif

    public void Fill(Component com, MyJson.IJsonNode json)
    {
        MeshFilter t = com as MeshFilter;
        MyJson.JsonNode_Object obj = json as MyJson.JsonNode_Object;
        t.sharedMesh= AssetMgr.Instance.GetMesh(obj["mesh"].ToString());
    }
}