#if NGUI
using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

class CParserUIInput : IComponentParser
{
    public Type parserType
    {
        get
        {
            return typeof(UIInput);
        }
    }
#if UNITY_EDITOR
    public MyJson.IJsonNode Parser(Component com, NeedList needlist)
    {

        UIInput inp = com as UIInput;
        var json = new MyJson.JsonNode_Object();

        if (inp.label != null)
        {
            json["label"] = new MyJson.JsonNode_ValueNumber(GameObjParser.ComponentToID(inp.label.GetComponent<UILabel>()));
        }
        json["value"] = new MyJson.JsonNode_ValueString(inp.value);
        json["savedAs"] = new MyJson.JsonNode_ValueString(inp.savedAs);
        json["activeTextColor"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.ColorToString(inp.activeTextColor));
        json["caretColor"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.ColorToString(inp.caretColor));
        json["selectionColor"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.ColorToString(inp.selectionColor));
        if (inp.selectOnTab != null)
        {
            json["selectOnTab"] = new MyJson.JsonNode_ValueNumber(GameObjParser.ObjToID(inp.selectOnTab.gameObject));
        }
        json["inputType"] = new MyJson.JsonNode_ValueString(inp.inputType.ToString());
        json["validation"] = new MyJson.JsonNode_ValueString(inp.validation.ToString());
        json["characterLimit"] = new MyJson.JsonNode_ValueNumber(inp.characterLimit);
        return json;
    }
#endif

    public void Fill(Component com, MyJson.IJsonNode json)
    {
        UIInput inp = com as UIInput;
        var jsono = json as MyJson.JsonNode_Object;

        if (jsono.ContainsKey("label"))
        {
            inp.label = GameObjParser.IDToComponent<UILabel>(jsono["label"] as MyJson.JsonNode_ValueNumber);
        }
        inp.value = jsono["value"] as MyJson.JsonNode_ValueString;
        inp.savedAs = jsono["savedAs"] as MyJson.JsonNode_ValueString;
        inp.activeTextColor = ComponentTypeConvert.StringToColor(jsono["activeTextColor"] as MyJson.JsonNode_ValueString);
        inp.caretColor = ComponentTypeConvert.StringToColor(jsono["caretColor"] as MyJson.JsonNode_ValueString);
        inp.selectionColor = ComponentTypeConvert.StringToColor(jsono["selectionColor"] as MyJson.JsonNode_ValueString);
        if (jsono.ContainsKey("selectOnTab"))
        {
            inp.selectOnTab = GameObjParser.IDToObj(jsono["selectOnTab"] as MyJson.JsonNode_ValueNumber);
        }        
        inp.inputType = (UIInput.InputType)Enum.Parse(typeof(UIInput.InputType), jsono["inputType"] as MyJson.JsonNode_ValueString);
        inp.validation = (UIInput.Validation)Enum.Parse(typeof(UIInput.Validation), jsono["validation"] as MyJson.JsonNode_ValueString);
        inp.characterLimit = jsono["characterLimit"] as MyJson.JsonNode_ValueNumber;

    }
}
#endif