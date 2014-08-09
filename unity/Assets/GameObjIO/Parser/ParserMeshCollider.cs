using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

class CParserBoxCollider : IComponentParser
{
    public Type parserType
    {
        get
        {
            return typeof(UnityEngine.MeshCollider);
        }
    }
#if UNITY_EDITOR
    public MyJson.IJsonNode Parser(Component com,NeedList list)
    {
        MeshCollider t = com as MeshCollider;
        var json = new MyJson.JsonNode_Object();

        json["isTrigger"] = new MyJson.JsonNode_ValueNumber(t.isTrigger);
        var name = AssetMgr.SaveMesh(t.sharedMesh, System.IO.Path.Combine(Application.streamingAssetsPath, "mesh"));
        list.AddDependMesh(name);
        json.SetDictValue("mesh", name);
        json.SetDictValue("convex", t.convex);
        json.SetDictValue("smoothSphereCollisions", t.smoothSphereCollisions);
        return json;
    }
#endif

    public void Fill(Component com, MyJson.IJsonNode json)
    {
        MeshCollider t = com as MeshCollider;
        var jsono = json as MyJson.JsonNode_Object;

        t.isTrigger = jsono["isTrigger"] as MyJson.JsonNode_ValueNumber;
        t.sharedMesh = AssetMgr.Instance.GetMesh(jsono["mesh"].ToString());
        t.convex = json.GetDictItem("convex").AsBool();
        t.smoothSphereCollisions = json.GetDictItem("smoothSphereCollisions").AsBool();
    }
}