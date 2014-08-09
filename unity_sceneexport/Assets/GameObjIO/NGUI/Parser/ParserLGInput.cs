#if NGUI
using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

class CParserLGInput : IComponentParser
{
    public Type parserType
    {
        get
        {
            return typeof(LGInput);
        }
    }
#if UNITY_EDITOR
    public MyJson.IJsonNode Parser(Component com, NeedList needlist)
    {

        var json = new MyJson.JsonNode_Object();

        return json;

    }
#endif

    public void Fill(Component com, MyJson.IJsonNode json)
    {

        var jsono = json as MyJson.JsonNode_Object;

    }
}
#endif