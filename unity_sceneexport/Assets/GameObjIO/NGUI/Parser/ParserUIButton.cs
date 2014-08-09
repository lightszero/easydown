#if NGUI
using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

class CParserUIButton : IComponentParser
{
    public Type parserType
    {
        get
        {
            return typeof(UIButton);
        }
    }
#if UNITY_EDITOR
    public MyJson.IJsonNode Parser(Component com,NeedList list)
    {
        UIButton t = com as UIButton;
        var json = new MyJson.JsonNode_Object();


        json["tweenTarget"] = new MyJson.JsonNode_ValueNumber(GameObjParser.ObjToID(t.tweenTarget));
        json["dragHighlight"] = new MyJson.JsonNode_ValueNumber(t.dragHighlight);
        json["duration"] = new MyJson.JsonNode_ValueNumber(t.duration);
        json["colorN"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.ColorToString(t.defaultColor));
        json["colorH"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.ColorToString(t.hover));
        json["colorP"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.ColorToString(t.pressed));
        json["colorD"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.ColorToString(t.disabledColor));

            

        json["spriteN"] = new MyJson.JsonNode_ValueString(t.normalSprite);
        json["springH"] = new MyJson.JsonNode_ValueString(t.hoverSprite);
        json["springP"] = new MyJson.JsonNode_ValueString(t.pressedSprite);
        json["springD"] = new MyJson.JsonNode_ValueString(t.disabledSprite);
        json["pixelSnap"] = new MyJson.JsonNode_ValueNumber(t.pixelSnap);
        return json;
    }
#endif

    public void Fill(Component com, MyJson.IJsonNode json)
    {
        UIButton t = com as UIButton;
        var jsono = json as MyJson.JsonNode_Object;

        t.tweenTarget = GameObjParser.IDToObj(jsono["tweenTarget"] as MyJson.JsonNode_ValueNumber);
        t.dragHighlight = jsono["dragHighlight"] as MyJson.JsonNode_ValueNumber;
        t.duration = jsono["duration"] as MyJson.JsonNode_ValueNumber;

        t.defaultColor = ComponentTypeConvert.StringToColor(jsono["colorN"] as MyJson.JsonNode_ValueString);
        t.hover = ComponentTypeConvert.StringToColor(jsono["colorH"] as MyJson.JsonNode_ValueString);
        t.pressed = ComponentTypeConvert.StringToColor(jsono["colorP"] as MyJson.JsonNode_ValueString);
        t.disabledColor = ComponentTypeConvert.StringToColor(jsono["colorD"] as MyJson.JsonNode_ValueString);

        t.normalSprite = jsono["spriteN"] as MyJson.JsonNode_ValueString;
        t.hoverSprite = jsono["springH"] as MyJson.JsonNode_ValueString;
        t.pressedSprite = jsono["springP"] as MyJson.JsonNode_ValueString;
        t.disabledSprite = jsono["springD"] as MyJson.JsonNode_ValueString;
        t.pixelSnap = jsono["pixelSnap"] as MyJson.JsonNode_ValueNumber;
    }
}
#endif