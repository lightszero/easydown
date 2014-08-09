#if NGUI
using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

class CParserUIGrid : IComponentParser
{
    public Type parserType
    {
        get
        {
            return typeof(UIGrid);
        }
    }
#if UNITY_EDITOR
    public MyJson.IJsonNode Parser(Component com, NeedList needlist)
    {

        UIGrid t = com as UIGrid;
        var json = new MyJson.JsonNode_Object();

        json["arrangement"] = new MyJson.JsonNode_ValueString(t.arrangement.ToString());
        json["sorting"] = new MyJson.JsonNode_ValueString(t.sorting.ToString());
        json["pivot"] = new MyJson.JsonNode_ValueString(t.pivot.ToString());
        json["maxPerLine"] = new MyJson.JsonNode_ValueNumber(t.maxPerLine);
        json["cellWidth"] = new MyJson.JsonNode_ValueNumber(t.cellWidth);
        json["cellHeight"] = new MyJson.JsonNode_ValueNumber(t.cellHeight);
        json["animateSmoothly"] = new MyJson.JsonNode_ValueNumber(t.animateSmoothly);
        json["hideInactive"] = new MyJson.JsonNode_ValueNumber(t.hideInactive);
        json["keepWithinPanel"] = new MyJson.JsonNode_ValueNumber(t.keepWithinPanel);

        return json;

    }
#endif

    public void Fill(Component com, MyJson.IJsonNode json)
    {

        UIGrid t = com as UIGrid;
        var jsono = json as MyJson.JsonNode_Object;

        t.arrangement = (UIGrid.Arrangement)Enum.Parse(typeof(UIGrid.Arrangement), jsono["arrangement"] as MyJson.JsonNode_ValueString);
        t.sorting = (UIGrid.Sorting)Enum.Parse(typeof(UIGrid.Sorting), jsono["sorting"] as MyJson.JsonNode_ValueString);
        t.pivot = (UIWidget.Pivot)Enum.Parse(typeof(UIWidget.Pivot), jsono["pivot"] as MyJson.JsonNode_ValueString);
        t.maxPerLine = jsono["maxPerLine"] as MyJson.JsonNode_ValueNumber;
        t.cellWidth = jsono["cellWidth"] as MyJson.JsonNode_ValueNumber;
        t.cellHeight = jsono["cellHeight"] as MyJson.JsonNode_ValueNumber;
        t.animateSmoothly = jsono["animateSmoothly"] as MyJson.JsonNode_ValueNumber;
        t.hideInactive = jsono["hideInactive"] as MyJson.JsonNode_ValueNumber;
        t.keepWithinPanel = jsono["keepWithinPanel"] as MyJson.JsonNode_ValueNumber;

    }
}
#endif