#if NGUI
using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

class CParserUIAnchor : IComponentParser
{
    public Type parserType
    {
        get
        {
            return typeof(UIAnchor);
        }
    }
#if UNITY_EDITOR
    public MyJson.IJsonNode Parser(Component com,NeedList list)
    {
        UIAnchor t = com as UIAnchor;
        var json = new MyJson.JsonNode_Object();
        json["uiCamera"] = new MyJson.JsonNode_ValueNumber(GameObjParser.ComponentToID(t.uiCamera));
        json["container"] = new MyJson.JsonNode_ValueNumber(GameObjParser.ObjToID(t.container));
        json["side"] = new MyJson.JsonNode_ValueString(t.side.ToString());
        json["runOnlyOnce"] = new MyJson.JsonNode_ValueNumber(t.runOnlyOnce);
        json["relativeOffset"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.Vector2ToString(t.relativeOffset));
        json["pixelOffset"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.Vector2ToString(t.pixelOffset));

        return json;
    }
#endif


    public void Fill(Component com, MyJson.IJsonNode json)
    {
        UIAnchor t = com as UIAnchor;
        var jsono = json as MyJson.JsonNode_Object;

        t.uiCamera = GameObjParser.IDToComponent<Camera>(jsono["uiCamera"] as MyJson.JsonNode_ValueNumber);
        t.container = GameObjParser.IDToObj(jsono["container"] as MyJson.JsonNode_ValueNumber);
        t.side = (UIAnchor.Side)Enum.Parse(typeof(UIAnchor.Side), jsono["side"] as MyJson.JsonNode_ValueString);
        t.runOnlyOnce = jsono["runOnlyOnce"] as MyJson.JsonNode_ValueNumber;
        t.relativeOffset = ComponentTypeConvert.StringToVector2(jsono["relativeOffset"] as MyJson.JsonNode_ValueString);
        t.pixelOffset = ComponentTypeConvert.StringToVector2(jsono["pixelOffset"] as MyJson.JsonNode_ValueString);

       
    }
}
#endif