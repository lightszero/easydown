#if NGUI
using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

class CParserUIDragScrollView : IComponentParser
{
    public Type parserType
    {
        get
        {
            return typeof(UIDragScrollView);
        }
    }
#if UNITY_EDITOR
    public MyJson.IJsonNode Parser(Component com, NeedList needlist)
    {

        UIDragScrollView t = com as UIDragScrollView;
        var json = new MyJson.JsonNode_Object();

        if (t.scrollView != null)
        {
            json["scrollView"] = new MyJson.JsonNode_ValueNumber(GameObjParser.ObjToID(t.scrollView.gameObject));
        }
        
        return json;

    }
#endif

    public void Fill(Component com, MyJson.IJsonNode json)
    {

        UIDragScrollView t = com as UIDragScrollView;
        var jsono = json as MyJson.JsonNode_Object;

        if (jsono.ContainsKey("scrollView"))
        {
            t.scrollView = (GameObjParser.IDToObj(jsono["scrollView"] as MyJson.JsonNode_ValueNumber)).GetComponent<UIScrollView>();
        }

    }
}
#endif