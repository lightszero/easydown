using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

class CParserMeshRenderer : IComponentParser
{
    public Type parserType
    {
        get
        {
            return typeof(MeshRenderer);
        }
    }
#if UNITY_EDITOR
    public MyJson.IJsonNode Parser(Component com, NeedList list)
    {
        MeshRenderer t = com as MeshRenderer;
        var json = new MyJson.JsonNode_Object();
        json["castShadows"] = new MyJson.JsonNode_ValueNumber(t.castShadows);
        json["receiveShadows"] = new MyJson.JsonNode_ValueNumber(t.receiveShadows);
        json["useLightProbes"] = new MyJson.JsonNode_ValueNumber(t.useLightProbes);
        int meshcount = t.sharedMaterials.Length;

        json["materials"] = new MyJson.JsonNode_Array();
        foreach (var m in t.sharedMaterials)
        {
            MyJson.JsonNode_Object matobj = new MyJson.JsonNode_Object();
            json["materials"].AddArrayValue(matobj);

            matobj.SetDictValue("name", m.name);
            matobj.SetDictValue("shader", m.shader.name);
            matobj.SetDictValue("shaderkeyword", new MyJson.JsonNode_Array());
            foreach (var shaderkey in m.shaderKeywords)
            {
                matobj["shaderkeyword"].AddArrayValue(shaderkey);
            }
            if (m.mainTexture != null)
            {
                string name = AssetMgr.SaveTexture(m.mainTexture, System.IO.Path.Combine(Application.streamingAssetsPath, "texture"));
                matobj.SetDictValue("maintex", name);
                list.AddDependTexture(name);
            }
        }
        return json;
    }
#endif

    public void Fill(Component com, MyJson.IJsonNode json)
    {
        MeshRenderer t = com as MeshRenderer;
        t.castShadows = json.GetDictItem("castShadows").AsBool();
        t.receiveShadows = json.GetDictItem("receiveShadows").AsBool();
        t.useLightProbes = json.GetDictItem("useLightProbes").AsBool();
        List<Material> mats = new List<Material>();
        foreach (var item in json.GetDictItem("materials").AsList())
        {
            Material mat = new Material(Shader.Find(item.GetDictItem("shader").AsString()));
            mat.name = item.GetDictItem("name").AsString();
            List<string> keywords = new List<string>();
            foreach (var key in item.GetDictItem("shaderkeyword").AsList())
            {
                keywords.Add(key.AsString());
            }
            mat.shaderKeywords = keywords.ToArray();
            if (item.HaveDictItem("maintex"))
            {
                mat.mainTexture = AssetMgr.Instance.GetTexture(item.GetDictItem("maintex").AsString());
            }
            mats.Add(mat);
        }
        t.sharedMaterials = mats.ToArray();
    }
}