#if NGUI
using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

class CParserUIPanel : IComponentParser
{
    public Type parserType
    {
        get
        {
            return typeof(UIPanel);
        }
    }
#if UNITY_EDITOR
    public MyJson.IJsonNode Parser(Component com,NeedList list)
    {
        UIPanel t = com as UIPanel;
        var json = new MyJson.JsonNode_Object();

        json["alpha"] = new MyJson.JsonNode_ValueNumber(t.alpha);
        json["depth"] = new MyJson.JsonNode_ValueNumber(t.depth);
        json["clipping"] = new MyJson.JsonNode_ValueString(t.clipping.ToString());
        if(t.clipping!= UIDrawCall.Clipping.None)
        {
            json["offset"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.Vector2ToString(t.clipOffset));
            json["region"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.Vector4ToString(t.baseClipRegion));
            if(t.clipping== UIDrawCall.Clipping.SoftClip)
            {

                json["softness"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.Vector2ToString(t.clipSoftness));
            }
        }
       
        return json;
    }
#endif

    public void Fill(Component com, MyJson.IJsonNode json)
    {
        //Debug.Log(json.ToString());
        UIPanel t = com as UIPanel;
        var jsono = json as MyJson.JsonNode_Object;
        t.alpha = jsono["alpha"] as MyJson.JsonNode_ValueNumber;
        t.depth = jsono["depth"] as MyJson.JsonNode_ValueNumber;
        t.clipping = (UIDrawCall.Clipping)Enum.Parse(typeof(UIDrawCall.Clipping), jsono["clipping"] as MyJson.JsonNode_ValueString);
        if (t.clipping != UIDrawCall.Clipping.None)
        {
            t.clipOffset = ComponentTypeConvert.StringToVector2(jsono["offset"] as MyJson.JsonNode_ValueString);
            t.baseClipRegion = ComponentTypeConvert.StringToVector4(jsono["region"] as MyJson.JsonNode_ValueString);
         
            if (t.clipping == UIDrawCall.Clipping.SoftClip)
            {
                t.clipSoftness = ComponentTypeConvert.StringToVector2(jsono["softness"] as MyJson.JsonNode_ValueString);
               
            }
        }
       
    }
}
#endif