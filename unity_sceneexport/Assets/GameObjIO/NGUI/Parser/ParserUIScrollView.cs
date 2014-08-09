#if NGUI
using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

class CParserUIScrollView : IComponentParser
{
    public Type parserType
    {
        get
        {
            return typeof(UIScrollView);
        }
    }
#if UNITY_EDITOR
    public MyJson.IJsonNode Parser(Component com, NeedList needlist)
    {

        UIScrollView sv = com as UIScrollView;
        var json = new MyJson.JsonNode_Object();

        json["movement"] = new MyJson.JsonNode_ValueString(sv.movement.ToString());
        json["dragEffect"] = new MyJson.JsonNode_ValueString(sv.dragEffect.ToString());
        json["scrollWheelFactor"] = new MyJson.JsonNode_ValueNumber(sv.scrollWheelFactor);
        json["momentumAmount"] = new MyJson.JsonNode_ValueNumber(sv.momentumAmount);
        json["restrictWithinPanel"] = new MyJson.JsonNode_ValueNumber(sv.restrictWithinPanel);
        json["disableDragIfFits"] = new MyJson.JsonNode_ValueNumber(sv.disableDragIfFits);
        json["smoothDragStart"] = new MyJson.JsonNode_ValueNumber(sv.smoothDragStart);
        json["iOSDragEmulation"] = new MyJson.JsonNode_ValueNumber(sv.iOSDragEmulation);
        return json;
    }
#endif

    public void Fill(Component com, MyJson.IJsonNode json)
    {
        UIScrollView sv = com as UIScrollView;
        var jsono = json as MyJson.JsonNode_Object;

        sv.movement = (UIScrollView.Movement)Enum.Parse(typeof(UIScrollView.Movement), jsono["movement"] as MyJson.JsonNode_ValueString);
        sv.dragEffect = (UIScrollView.DragEffect)Enum.Parse(typeof(UIScrollView.DragEffect), jsono["dragEffect"] as MyJson.JsonNode_ValueString);
        sv.scrollWheelFactor = jsono["scrollWheelFactor"] as MyJson.JsonNode_ValueNumber;
        sv.momentumAmount = jsono["momentumAmount"] as MyJson.JsonNode_ValueNumber;
        sv.restrictWithinPanel = jsono["restrictWithinPanel"] as MyJson.JsonNode_ValueNumber;
        sv.disableDragIfFits = jsono["disableDragIfFits"] as MyJson.JsonNode_ValueNumber;
        sv.smoothDragStart = jsono["smoothDragStart"] as MyJson.JsonNode_ValueNumber;
        sv.iOSDragEmulation = jsono["iOSDragEmulation"] as MyJson.JsonNode_ValueNumber;

    }
}
#endif