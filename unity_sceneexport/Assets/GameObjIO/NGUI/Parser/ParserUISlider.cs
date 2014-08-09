#if NGUI
using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

class CParserUISlider : IComponentParser
{
    public Type parserType
    {
        get
        {
            return typeof(UISlider);
        }
    }
#if UNITY_EDITOR
    public MyJson.IJsonNode Parser(Component com, NeedList needlist)
    {

        UISlider t = com as UISlider;
        var json = new MyJson.JsonNode_Object();

        json["value"] = new MyJson.JsonNode_ValueNumber(t.value);
        json["alpha"] = new MyJson.JsonNode_ValueNumber(t.alpha);
        json["numberOfSteps"] = new MyJson.JsonNode_ValueNumber(t.numberOfSteps);
        if (t.foregroundWidget != null)
        {
            json["foregroundWidget"] = new MyJson.JsonNode_ValueNumber(GameObjParser.ObjToID(t.foregroundWidget.gameObject));
        }
        if (t.backgroundWidget != null)
        {
            json["backgroundWidget"] = new MyJson.JsonNode_ValueNumber(GameObjParser.ObjToID(t.backgroundWidget.gameObject));
        }
        if (t.thumb != null)
        {
            json["thumb"] = new MyJson.JsonNode_ValueNumber(GameObjParser.ObjToID(t.thumb.gameObject));
        }
        json["fillDirection"] = new MyJson.JsonNode_ValueString(t.fillDirection.ToString());

        return json;

    }
#endif

    public void Fill(Component com, MyJson.IJsonNode json)
    {

        UISlider t = com as UISlider;
        var jsono = json as MyJson.JsonNode_Object;

        t.value = jsono["value"] as MyJson.JsonNode_ValueNumber;
        t.alpha = jsono["alpha"] as MyJson.JsonNode_ValueNumber;
        t.numberOfSteps = jsono["numberOfSteps"] as MyJson.JsonNode_ValueNumber;
        if (jsono.ContainsKey("foregroundWidget"))
        {
            t.foregroundWidget = (GameObjParser.IDToObj(jsono["foregroundWidget"] as MyJson.JsonNode_ValueNumber)).GetComponent<UISprite>();
        }
        if (jsono.ContainsKey("backgroundWidget"))
        {
            t.backgroundWidget = (GameObjParser.IDToObj(jsono["backgroundWidget"] as MyJson.JsonNode_ValueNumber)).GetComponent<UISprite>();
        }
        if (jsono.ContainsKey("thumb"))
        {
            t.thumb = (GameObjParser.IDToObj(jsono["thumb"] as MyJson.JsonNode_ValueNumber)).transform;
        }
        t.fillDirection = (UIProgressBar.FillDirection)Enum.Parse(typeof(UIProgressBar.FillDirection), jsono["fillDirection"] as MyJson.JsonNode_ValueString);
    }
}
#endif