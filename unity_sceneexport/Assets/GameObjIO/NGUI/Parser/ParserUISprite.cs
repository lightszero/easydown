#if NGUI
using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

class CParserUISprite : IComponentParser
{
    public Type parserType
    {
        get
        {
            return typeof(UISprite);
        }
    }
#if UNITY_EDITOR
    public MyJson.IJsonNode Parser(Component com,NeedList needlist)
    {

        UISprite t = com as UISprite;
        var json = new MyJson.JsonNode_Object();

        //t.atlas atlas 有特殊的打包逻辑
        string atlas = AtlasMgr.SaveAtlas(t.atlas, System.IO.Path.Combine(Application.streamingAssetsPath, "nguiatlas"), System.IO.Path.Combine(Application.streamingAssetsPath, "nguitex"));
        json["atlas"] =new MyJson.JsonNode_ValueString( atlas);
        if(needlist!=null)
        {
            needlist.AddDependAtlas(atlas); 
        }

        json["spriteName"] = new MyJson.JsonNode_ValueString(t.spriteName);
        json["spriteType"] = new MyJson.JsonNode_ValueString(t.type.ToString());

        if (t.type == UISprite.Type.Simple)
        {
            json["flip"] = new MyJson.JsonNode_ValueString(t.flip.ToString());
        }
        else if (t.type == UISprite.Type.Sliced)
        {
            json["centerType"] = new MyJson.JsonNode_ValueString(t.centerType.ToString());
            json["flip"] = new MyJson.JsonNode_ValueString(t.flip.ToString());
        }
        else if (t.type == UISprite.Type.Tiled)
        {

        }
        else if (t.type == UISprite.Type.Filled)
        {
            json["flip"] = new MyJson.JsonNode_ValueString(t.flip.ToString());
            json["fillDirection"] = new MyJson.JsonNode_ValueString(t.fillDirection.ToString());
            json["fillAmount"] = new MyJson.JsonNode_ValueNumber(t.fillAmount);
            json["invert"] = new MyJson.JsonNode_ValueNumber(t.invert);
        }
        else if(t.type== UISprite.Type.Advanced)
        {
            json["leftType"] = new MyJson.JsonNode_ValueString(t.leftType.ToString());
            json["rightType"] = new MyJson.JsonNode_ValueString(t.rightType.ToString());
            json["topType"] = new MyJson.JsonNode_ValueString(t.topType.ToString());
            json["bottomType"] = new MyJson.JsonNode_ValueString(t.bottomType.ToString());
            json["centerType"] = new MyJson.JsonNode_ValueString(t.centerType.ToString());
            json["flip"] = new MyJson.JsonNode_ValueString(t.flip.ToString());
        }

        ComponentParser.ParserWidget(json, t);

        return json;


    }
#endif

    public void Fill(Component com, MyJson.IJsonNode json)
    {
        UISprite t = com as UISprite;
        var jsono = json as MyJson.JsonNode_Object;

        //t.atlas atlas 有特殊的打包逻辑
        string atlas = jsono["atlas"] as MyJson.JsonNode_ValueString;
        t.atlas = AtlasMgr.Instance.GetAtlas(atlas);
        //Debug.Log("atals:" + atlas + "," + t.atlas);
        t.spriteName = jsono["spriteName"] as MyJson.JsonNode_ValueString;
        t.type = (UISprite.Type)Enum.Parse(typeof(UISprite.Type),jsono["spriteType"] as MyJson.JsonNode_ValueString);
        if (t.type == UISprite.Type.Simple)
        {
            t.flip = (UISprite.Flip)Enum.Parse(typeof(UISprite.Flip), jsono["flip"] as MyJson.JsonNode_ValueString);

        }
        else if (t.type == UISprite.Type.Sliced)
        {
            t.centerType = (UISprite.AdvancedType)Enum.Parse(typeof(UISprite.AdvancedType), jsono["centerType"] as MyJson.JsonNode_ValueString);
            t.flip = (UISprite.Flip)Enum.Parse(typeof(UISprite.Flip), jsono["flip"] as MyJson.JsonNode_ValueString);
        }
        else if (t.type == UISprite.Type.Tiled)
        {

        }
        else if (t.type == UISprite.Type.Filled)
        {
            t.flip = (UISprite.Flip)Enum.Parse(typeof(UISprite.Flip), jsono["flip"] as MyJson.JsonNode_ValueString);
            t.fillDirection = (UISprite.FillDirection)Enum.Parse(typeof(UISprite.FillDirection), jsono["fillDirection"] as MyJson.JsonNode_ValueString);
            t.fillAmount = jsono["fillAmount"] as MyJson.JsonNode_ValueNumber;
            t.invert = jsono["invert"] as MyJson.JsonNode_ValueNumber;
        }
        else if (t.type == UISprite.Type.Advanced)
        {
            t.leftType = (UISprite.AdvancedType)Enum.Parse(typeof(UISprite.AdvancedType), jsono["leftType"] as MyJson.JsonNode_ValueString);
            t.rightType = (UISprite.AdvancedType)Enum.Parse(typeof(UISprite.AdvancedType), jsono["rightType"] as MyJson.JsonNode_ValueString);
            t.topType = (UISprite.AdvancedType)Enum.Parse(typeof(UISprite.AdvancedType), jsono["topType"] as MyJson.JsonNode_ValueString);
            t.bottomType = (UISprite.AdvancedType)Enum.Parse(typeof(UISprite.AdvancedType), jsono["bottomType"] as MyJson.JsonNode_ValueString);
            t.centerType = (UISprite.AdvancedType)Enum.Parse(typeof(UISprite.AdvancedType), jsono["centerType"] as MyJson.JsonNode_ValueString);
            t.flip = (UISprite.Flip)Enum.Parse(typeof(UISprite.Flip), jsono["flip"] as MyJson.JsonNode_ValueString);
        }
        ComponentParser.FillWidget(t,jsono);
    }
}
#endif