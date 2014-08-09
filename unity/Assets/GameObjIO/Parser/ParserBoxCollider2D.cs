using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

class CParserBoxCollider2D : IComponentParser
{
    public Type parserType
    {
        get
        {
            return typeof(BoxCollider2D);
        }
    }
#if UNITY_EDITOR
    public MyJson.IJsonNode Parser(Component com, NeedList needlist)
    {

        BoxCollider2D box = com as BoxCollider2D;
        var json = new MyJson.JsonNode_Object();
        
        json["isTrigger"] = new MyJson.JsonNode_ValueNumber(box.isTrigger);
        json["center"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.Vector3ToString(box.center));
        json["size"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.Vector3ToString(box.size));

        return json;
    }
#endif

    public void Fill(Component com, MyJson.IJsonNode json)
    {
        BoxCollider2D t = com as BoxCollider2D;
        var jsono = json as MyJson.JsonNode_Object;

        t.isTrigger = jsono["isTrigger"] as MyJson.JsonNode_ValueNumber;
        t.center = ComponentTypeConvert.StringToVector3(jsono["center"] as MyJson.JsonNode_ValueString);
        t.size = ComponentTypeConvert.StringToVector3(jsono["size"] as MyJson.JsonNode_ValueString);
    }
}