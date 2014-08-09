#if NGUI
using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

class CParserUITexture : IComponentParser
{
    public Type parserType
    {
        get
        {
            return typeof(UITexture);
        }
    }
    #if UNITY_EDITOR
    public MyJson.IJsonNode Parser(Component com,NeedList list)
    {

        UITexture t = com as UITexture;
        var json = new MyJson.JsonNode_Object();


        if(t.mainTexture!=null)
        {
            string needtex = AtlasMgr.SaveTexture(t.mainTexture, System.IO.Path.Combine(Application.streamingAssetsPath, "nguitex"));
            json["mainTexture"] = new MyJson.JsonNode_ValueString(needtex);
            list.AddDependTexture(needtex);
            if(t.shader!=null)
            {
                string shader = t.shader.name;
                json["shader"] = new MyJson.JsonNode_ValueString(shader);
            }
        }
        else
        {
            Debug.LogWarning("不支持 导出使用材质的UITexture");
        }

        json["uvRect"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.RectToString(t.uvRect));
        ComponentParser.ParserWidget(json, t);

        return json;

    }

#endif

    public void Fill(Component com, MyJson.IJsonNode json)
    {
        UITexture t = com as UITexture;
        var jsono = json as MyJson.JsonNode_Object;

        if(jsono.ContainsKey("mainTexture"))
        {
            string needtex = jsono["mainTexture"] as MyJson.JsonNode_ValueString;
            needtex = needtex.ToLower();
            var tex = AtlasMgr.Instance.GetTexture(needtex);
            if(tex!=null)
            {
                //贴图的恢复
                t.mainTexture = tex;
            }
            else
            {
                Debug.LogWarning(com.name + "Can't find texture:" + needtex);
            }
           
            if(jsono.ContainsKey("shader"))
            {
                string shader = jsono["shader"] as MyJson.JsonNode_ValueString;
                Shader s = Shader.Find(shader);
                if (s != null)
                {
                    t.shader = s;

                }
                else
                {
                    Debug.LogWarning(com.name+ "Can't find shader:" + shader);
                }
            }

        }


        ComponentParser.FillWidget(t, jsono);
    }
}
#endif