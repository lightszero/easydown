#if NGUI
using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

class CParserUILabel : IComponentParser
{
    public Type parserType
    {
        get
        {
            return typeof(UILabel);
        }
    }

#if UNITY_EDITOR
    public MyJson.IJsonNode Parser(Component com,NeedList list)
    {
        UILabel t = com as UILabel;
        var json = new MyJson.JsonNode_Object();

        //font 有特殊的打包逻辑
        if (t.bitmapFont != null)
        {
            json["bfont"] = new MyJson.JsonNode_ValueString(t.bitmapFont.name);
        }
        else if (t.trueTypeFont != null)
        {
            json["tfont"] = new MyJson.JsonNode_ValueString(t.trueTypeFont.name);
            if(list!=null)
            {
                list.AddDependTFont(t.trueTypeFont.name);

            }
        }
        else
        {
            Debug.LogError("UILabel" + com.name + " 未指定Font");
        }

        json["fontsize"] = new MyJson.JsonNode_ValueNumber(t.fontSize);

        //材质有特殊的打包逻辑
        //json["mate"] = t.material;

        json["text"] = new MyJson.JsonNode_ValueString(t.text);
        json["overflowMethod"] = new MyJson.JsonNode_ValueString(t.overflowMethod.ToString());
        json["alignment"] = new MyJson.JsonNode_ValueString(t.alignment.ToString());
        json["keepCrispWhenShrunk"] = new MyJson.JsonNode_ValueString(t.keepCrispWhenShrunk.ToString());
        json["applyGradient"] = new MyJson.JsonNode_ValueNumber(t.applyGradient);
        json["gradientTop"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.ColorToString(t.gradientTop));
        json["gradientBottom"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.ColorToString(t.gradientBottom));
        json["effectStyle"] = new MyJson.JsonNode_ValueString(t.effectStyle.ToString());
        json["effectColor"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.ColorToString(t.effectColor));
        json["effectDistance"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.Vector2ToString(t.effectDistance));
        json["spacing"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.Vector2ToString(new Vector2(t.spacingX, t.spacingY)));
        json["maxLineCount"] = new MyJson.JsonNode_ValueNumber(t.maxLineCount);


        ComponentParser.ParserWidget(json, t);

        return json;
    }

#endif
    public void Fill(Component com, MyJson.IJsonNode json)
    {
        UILabel t = com as UILabel;
        var jsono = json as MyJson.JsonNode_Object;

        //font 有特殊的打包逻辑
        if(jsono.ContainsKey("bfont"))
        {
            string bfontname = jsono["bfont"] as MyJson.JsonNode_ValueString;
            t.bitmapFont = FontMgr.Instance.GetUIFont(bfontname);
        }
        else if(jsono.ContainsKey("tfont"))
        {
            string tfontname = jsono["tfont"] as MyJson.JsonNode_ValueString;
            t.trueTypeFont= FontMgr.Instance.GetUnityFont(tfontname);
        }
        else
        {
            Debug.LogWarning("UILabel" + com.name + " 未指定Font");
        }
        t.fontSize = (jsono["fontsize"] as MyJson.JsonNode_ValueNumber);
      

        //材质有特殊的打包逻辑
        //json["mate"] = t.material;

       
        t.text = jsono["text"] as MyJson.JsonNode_ValueString;
        t.overflowMethod = (UILabel.Overflow)Enum.Parse(typeof(UILabel.Overflow), jsono["overflowMethod"] as MyJson.JsonNode_ValueString);
        t.alignment = (NGUIText.Alignment)Enum.Parse(typeof(NGUIText.Alignment), jsono["alignment"] as MyJson.JsonNode_ValueString);
        t.keepCrispWhenShrunk = (UILabel.Crispness)Enum.Parse(typeof(UILabel.Crispness), jsono["keepCrispWhenShrunk"] as MyJson.JsonNode_ValueString);
        t.applyGradient = (jsono["applyGradient"] as MyJson.JsonNode_ValueNumber);
        t.gradientTop = ComponentTypeConvert.StringToColor(jsono["gradientTop"] as MyJson.JsonNode_ValueString);
        t.gradientBottom = ComponentTypeConvert.StringToColor(jsono["gradientBottom"] as MyJson.JsonNode_ValueString);
        t.effectStyle = (UILabel.Effect)Enum.Parse(typeof(UILabel.Effect), jsono["effectStyle"] as MyJson.JsonNode_ValueString);
        t.effectColor = ComponentTypeConvert.StringToColor(jsono["effectColor"] as MyJson.JsonNode_ValueString);
        t.effectDistance = ComponentTypeConvert.StringToVector2(jsono["effectDistance"] as MyJson.JsonNode_ValueString);
        var spacing = ComponentTypeConvert.StringToVector2(jsono["spacing"] as MyJson.JsonNode_ValueString);
        t.spacingX = (int)spacing.x;
        t.spacingY = (int)spacing.y;
        t.maxLineCount = (jsono["maxLineCount"] as MyJson.JsonNode_ValueNumber);
     
        ComponentParser.FillWidget(t, jsono);
    }
}
#endif