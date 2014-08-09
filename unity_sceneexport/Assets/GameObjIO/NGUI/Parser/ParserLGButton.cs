#if NGUI
using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

class CParserLGButton : IComponentParser
{
    public Type parserType
    {
        get
        {
            return typeof(LGButton);
        }
    }
#if UNITY_EDITOR
    public MyJson.IJsonNode Parser(Component com, NeedList needlist)
    {

        LGButton btn = com as LGButton;
        var json = new MyJson.JsonNode_Object();

        json["Tag"] = new MyJson.JsonNode_ValueString(btn.Tag.ToString());
        json["Team"] = new MyJson.JsonNode_ValueString(btn.Team.ToString());
        return json;
    }
#endif

    public void Fill(Component com, MyJson.IJsonNode json)
    {
        LGButton btn = com as LGButton;
        var jsono = json as MyJson.JsonNode_Object;

        btn.Tag = jsono["Tag"] as MyJson.JsonNode_ValueString;
        btn.Team = jsono["Team"] as MyJson.JsonNode_ValueString;
    }
}
#endif