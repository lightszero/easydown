using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

class CParserTransform : IComponentParser
{
    public Type parserType
    {
        get
        {
            return typeof(Transform);
        }
    }
#if UNITY_EDITOR
    public MyJson.IJsonNode Parser(Component com,NeedList list)
    {
        Transform t = com as Transform;
        var json = new MyJson.JsonNode_Object();
        json["P"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.Vector3ToString(t.localPosition));
        json["R"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.Vector3ToString(t.localEulerAngles));
        json["S"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.Vector3ToString(t.localScale));
        return json;
    }
#endif

    public void Fill(Component com, MyJson.IJsonNode json)
    {
        Transform t = com as Transform;
        var jsono = json as MyJson.JsonNode_Object;
        t.localPosition = ComponentTypeConvert.StringToVector3(jsono["P"] as MyJson.JsonNode_ValueString);
        t.localEulerAngles = ComponentTypeConvert.StringToVector3(jsono["R"] as MyJson.JsonNode_ValueString);
        t.localScale = ComponentTypeConvert.StringToVector3(jsono["S"] as MyJson.JsonNode_ValueString);
    }
}