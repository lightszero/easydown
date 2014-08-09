using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;
public class ComponentParser
{
    public static ComponentParser Instance
    {
        get
        {
            if (g_this == null)
            {
                g_this = new ComponentParser();
            }
            return g_this;
        }
    }

    static ComponentParser g_this;
    private ComponentParser()
    {
        RegParser(new CParserTransform());
        RegParser(new CParserBoxCollider());
        RegParser(new CParserBoxCollider2D());
        RegParser(new CParserMeshFilter());
        RegParser(new CParserMeshRenderer());
        RegParser(new CParserMeshCollider());
        RegParser(new CParserLight());

#if NGUI
        RegParser(new CParserUIPanel());
        RegParser(new CParserUIAnchor());
        RegParser(new CParserUIButton());
        RegParser(new CParserUISprite());
        RegParser(new CParserUILabel());
        RegParser(new CParserUIScrollView());
        RegParser(new CParserUITexture());
        RegParser(new CParserUIGrid());
        RegParser(new CParserLGButton());
        RegParser(new CParserUIDragScrollView());
        RegParser(new CParserUIDragDropContainer());
        RegParser(new CParserUIInput());

        RegParser(new CParserUISlider());
        RegParser(new CParserUITimeScale());
        RegParser(new CParserLGInput());
		RegParser(new CParserUIToggle());
#endif
    }
    private void RegParser(IComponentParser p)
    {
        mapParser[p.parserType] = p;
    }
    private Dictionary<Type, IComponentParser> mapParser = new Dictionary<Type, IComponentParser>();
   
    public IComponentParser GetParser(Type type)
    {
        if (mapParser.ContainsKey(type) == false)
            return null;
        else
            return mapParser[type];

    }


}
public static class ComponentTypeConvert
{
    static public string RectToString(Rect vec)
    {
        return vec.x + "," + vec.y + "," + vec.width+","+vec.height;
    }
    static public Rect StringToRect(string str)
    {

        string[] ss = str.Split(',');
        Rect rect = new Rect();
        rect.x = float.Parse(ss[0]);
        rect.y = float.Parse(ss[1]);
        rect.width = float.Parse(ss[2]);
        rect.height = float.Parse(ss[3]);
        return rect;
    }
    static public string Vector3ToString(Vector3 vec)
    {
        return vec.x + "," + vec.y + "," + vec.z;
    }
    static public Vector3 StringToVector3(string str)
    {
        string[] ss = str.Split(',');
        Vector3 vec;
        vec.x = float.Parse(ss[0]);
        vec.y = float.Parse(ss[1]);
        vec.z = float.Parse(ss[2]);
        return vec;
    }
    static public string Vector2ToString(Vector2 vec)
    {
        return vec.x + "," + vec.y ;
    }
    static public Vector2 StringToVector2(string str)
    {
        string[] ss = str.Split(',');
        Vector2 vec;
        vec.x = float.Parse(ss[0]);
        vec.y = float.Parse(ss[1]);
        return vec;
    }
    static public string Vector4ToString(Vector4 vec)
    {
        return vec.x + "," + vec.y + "," + vec.z +","+vec.w;
    }
    static public Vector4 StringToVector4(string str)
    {
        string[] ss = str.Split(',');
        Vector4 vec;
        vec.x = float.Parse(ss[0]);
        vec.y = float.Parse(ss[1]);
        vec.z = float.Parse(ss[2]);
        vec.w = float.Parse(ss[3]);
        return vec;
    }
    static public string ColorToString(Color c)
    {
        return (byte)(c.r * 255) + "," + (byte)(c.g * 255) + "," + (byte)(c.b * 255) + "," + (byte)(c.a * 255);
    }
    static public Color StringToColor(string str)
    {
        string[] ss = str.Split(',');
        Color c;
        c.r = ((float)byte.Parse(ss[0])) / 255.0f;
        c.g = ((float)byte.Parse(ss[1])) / 255.0f;
        c.b = ((float)byte.Parse(ss[2])) / 255.0f; 
        c.a = ((float)byte.Parse(ss[3])) / 255.0f;
        return c;
    }
}
public class NeedList
{
    public NeedList()
    {

    }
    public Dictionary<string, List<string>> needList =new Dictionary<string,List<string>>();
    public void AddDependTFont(string name)
    {
        if (needList.ContainsKey("tfont") == false) needList["tfont"] = new List<string>();
        if (needList["tfont"].Contains(name) == false) needList["tfont"].Add(name);
    }
    public void AddDependAtlas(string name)
    {
        if (needList.ContainsKey("atlas") == false) needList["atlas"] = new List<string>();
        if (needList["atlas"].Contains(name) == false) needList["atlas"].Add(name);
    }
    public void AddDependTexture(string name)
    {
        if (needList.ContainsKey("tex") == false) needList["tex"] = new List<string>();
        if (needList["tex"].Contains(name) == false) needList["tex"].Add(name);
    }
    public void AddDependMesh(string name)
    {
        if (needList.ContainsKey("mesh") == false) needList["mesh"] = new List<string>();
        if (needList["mesh"].Contains(name) == false) needList["mesh"].Add(name);
    }
    public override string ToString()
    {
        MyJson.JsonNode_Object obj = new MyJson.JsonNode_Object();
        foreach(var list in needList)
        {
            MyJson.JsonNode_Array array = new MyJson.JsonNode_Array();
            obj[list.Key] = array;
            foreach(var item in list.Value)
            {
                array.Add(new MyJson.JsonNode_ValueString(item));
            }
        }
        return obj.ToString();
    }
}
public interface IComponentParser
{
    Type parserType
    {
        get;
    }
#if UNITY_EDITOR
    MyJson.IJsonNode Parser(Component com,NeedList list=null);
#endif
    void Fill(Component com, MyJson.IJsonNode json);
}