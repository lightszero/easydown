using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

class CParserMeshCollider : IComponentParser
{
    public Type parserType
    {
        get
        {
            return typeof(UnityEngine.BoxCollider);
        }
    }
#if UNITY_EDITOR
    public MyJson.IJsonNode Parser(Component com,NeedList list)
    {
        BoxCollider t = com as BoxCollider;
        var json = new MyJson.JsonNode_Object();

        json["isTrigger"] = new MyJson.JsonNode_ValueNumber(t.isTrigger);
        json["center"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.Vector3ToString(t.center));
        json["size"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.Vector3ToString(t.size));
        return json;
    }
#endif

    public void Fill(Component com, MyJson.IJsonNode json)
    {
        BoxCollider t = com as BoxCollider;
        var jsono = json as MyJson.JsonNode_Object;

        t.isTrigger = jsono["isTrigger"] as MyJson.JsonNode_ValueNumber;
        t.center = ComponentTypeConvert.StringToVector3(jsono["center"] as MyJson.JsonNode_ValueString);
        t.size = ComponentTypeConvert.StringToVector3(jsono["size"] as MyJson.JsonNode_ValueString);

    }
}