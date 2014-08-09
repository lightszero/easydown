using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

class CParserLight : IComponentParser
{
    public Type parserType
    {
        get
        {
            return typeof(Light);
        }
    }
#if UNITY_EDITOR
    public MyJson.IJsonNode Parser(Component com, NeedList list)
    {
        Light t = com as Light;
        var json = new MyJson.JsonNode_Object();
        json.SetDictValue("type", (int)t.type);
        if (t.type == LightType.Spot)
        {
            json.SetDictValue("range", t.range);
            json.SetDictValue("spotangle", t.spotAngle);


        }
        if (t.type == LightType.Point)
        {
            json.SetDictValue("range", t.range);

        }

        if (t.type != LightType.Area)
        {
            json.SetDictValue("color", ComponentTypeConvert.ColorToString(t.color));
            json.SetDictValue("intensity", t.intensity);
            //t.cookie
            //t.cookiesize
            json.SetDictValue("shadowtype", (int)t.shadows);
            if (t.shadows != LightShadows.None)
            {
                json.SetDictValue("shadowStrength", t.shadowStrength);
                json.SetDictValue("shadowBias", t.shadowBias);
                //shadow质量找不到怎么操作
            }

            if (t.shadows == LightShadows.Soft)
            {
                json.SetDictValue("shadowSoftness", t.shadowSoftness);
                json.SetDictValue("shadowSoftnessFade", t.shadowSoftnessFade);
            }
            //Drawhalo not found
            //flare
            json.SetDictValue("renderMode", (int)t.renderMode);

            json.SetDictValue("cullingMask", t.cullingMask);

            //lightmapping not found


        }
        return json;
    }
#endif

    public void Fill(Component com, MyJson.IJsonNode json)
    {
        Light t = com as Light;
        t.type = (LightType)json.GetDictItem("type").AsInt();
        if (t.type == LightType.Spot)
        {
            t.range = (float)json.GetDictItem("range").AsDouble();
            t.spotAngle = (float)json.GetDictItem("spotangle").AsDouble();




        }
        if (t.type == LightType.Point)
        {
            t.range = (float)json.GetDictItem("range").AsDouble();

        }

        if (t.type != LightType.Area)
        {
            t.color = ComponentTypeConvert.StringToColor(json.GetDictItem("color").AsString());
            t.intensity = (float)json.GetDictItem("intensity").AsDouble();

            //t.cookie
            //t.cookiesize
            t.shadows = (LightShadows)json.GetDictItem("shadowtype").AsInt();

            if (t.shadows != LightShadows.None)
            {
                t.shadowStrength = (float)json.GetDictItem("shadowStrength").AsDouble();
                t.shadowBias = (float)json.GetDictItem("shadowBias").AsDouble();
                //shadow质量找不到怎么操作
            }

            if (t.shadows == LightShadows.Soft)
            {
                t.shadowSoftness = (float)json.GetDictItem("shadowSoftness").AsDouble();
                t.shadowSoftnessFade = (float)json.GetDictItem("shadowSoftnessFade").AsDouble();

            }
            //Drawhalo not found
            //flare
            t.renderMode = (LightRenderMode)json.GetDictItem("renderMode").AsInt();

            json.SetDictValue("renderMode", (int)t.renderMode);
            t.cullingMask = json.GetDictItem("cullingMask").AsInt();

            //lightmapping not found


        }
    }
}