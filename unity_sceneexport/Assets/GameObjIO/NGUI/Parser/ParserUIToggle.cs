#if NGUI
using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;



class CParserUIToggle : IComponentParser
{
	public Type parserType
	{
		get
		{
			return typeof(UIToggle);
		}
	}
	#if UNITY_EDITOR
	public MyJson.IJsonNode Parser(Component com,NeedList list)
	{
		UIToggle t = com as UIToggle;
		var json = new MyJson.JsonNode_Object();

		if (t.activeSprite != null) {
			json["activeSprite"] = new MyJson.JsonNode_ValueNumber(GameObjParser.ObjToID(t.activeSprite.gameObject));
			Debug.Log(t.gameObject + "&&&&&&&&&&&&&&&&&&&&&"+t.activeSprite.gameObject);
		}	 
		//json["activeSprite"] = new MyJson.JsonNode_ValueNumber(GameObjParser.ObjToID(t));
		//json["group"] = new MyJson.JsonNode_ValueNumber(t.group);
		//json["value"] = new MyJson.JsonNode_ValueNumber(t.value);
	//	json ["sprite"] = new MyJson.JsonNode_ValueString (t.activeSprite);
	//	json ["onChange"] = new MyJson.JsonNode_ValueString (t.onChange);
		return json;
	}
	#endif

	public void Fill(Component com, MyJson.IJsonNode json)
	{
		UIToggle t = com as UIToggle;
		var jsono = json as MyJson.JsonNode_Object;
		//t.group = jsono["group"] as MyJson.JsonNode_ValueNumber;
		//t.value = jsono["value"] as MyJson.JsonNode_ValueNumber;
	//	t.activeSprite = jsono["sprite"] as MyJson.JsonNode_ValueString;
	//	t.onChange = jsono["onChange"] as MyJson.JsonNode_ValueString;
	//	
		if (jsono.ContainsKey("activeSprite")) {
			t.activeSprite = (GameObjParser.IDToObj(jsono["activeSprite"] as MyJson.JsonNode_ValueNumber)).GetComponent<UISprite>();
		}
	}
}
#endif