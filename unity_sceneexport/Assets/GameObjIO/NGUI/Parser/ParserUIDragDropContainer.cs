#if NGUI
using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

class CParserUIDragDropContainer : IComponentParser
{
    public Type parserType
    {
        get
        {
            return typeof(UIDragDropContainer);
        }
    }
#if UNITY_EDITOR
    public MyJson.IJsonNode Parser(Component com, NeedList needlist)
    {

        UIDragDropContainer t = com as UIDragDropContainer;
        var json = new MyJson.JsonNode_Object();

        if (t.reparentTarget != null)
        {
            json["reparentTarget"] = new MyJson.JsonNode_ValueNumber(GameObjParser.ObjToID(t.reparentTarget.gameObject));
        }
        
        return json;

    }
#endif

    public void Fill(Component com, MyJson.IJsonNode json)
    {

        UIDragDropContainer t = com as UIDragDropContainer;
        var jsono = json as MyJson.JsonNode_Object;

        if (jsono.ContainsKey("reparentTarget"))
        {
            t.reparentTarget = (GameObjParser.IDToObj(jsono["reparentTarget"] as MyJson.JsonNode_ValueNumber)).transform;
        }

    }
}
#endif