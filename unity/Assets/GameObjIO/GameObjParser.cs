using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;


public static class GameObjParser
{
#if UNITY_EDITOR
    public static MyJson.IJsonNode Parser(GameObject obj,NeedList needList=null)
    {
        AssetMgr.Reset();
        MyJson.JsonNode_Object json = new MyJson.JsonNode_Object();
        Debug.Log("ParserObj:" + obj.name);
        var comps = obj.GetComponents<Component>();
        MyJson.JsonNode_Array jsoncs = new MyJson.JsonNode_Array();
        json["name"] = new MyJson.JsonNode_ValueString(obj.name);
        json["active"] = new MyJson.JsonNode_ValueNumber(obj.activeSelf);
        json["_id"] = new MyJson.JsonNode_ValueNumber(ObjToID(obj));
        json["coms"] = jsoncs;
        foreach (var c in comps)
        {

            if (c == null)
            {

                Debug.LogWarning("--组件丢失");
            }
            else
            {

                Type type = c.GetType();
                var pp = ComponentParser.Instance.GetParser(type);
                if (pp != null)
                {
                    MyJson.JsonNode_Object jsonc = new MyJson.JsonNode_Object();
                    jsonc["type"] = new MyJson.JsonNode_ValueString(type.ToString());
                    if (c is Behaviour)
                    {
                        jsonc["enabled"] = new MyJson.JsonNode_ValueNumber((c as Behaviour).enabled);
                    }
                    jsonc["param"] = pp.Parser(c, needList);
                    Debug.Log("--" + c.name + "<" + type.ToString() + ">:" + jsonc["param"].ToString());
                    jsoncs.Add(jsonc);
                }
                else
                {
                    Debug.LogWarning("--" + c.name + "<" + type.ToString() + ">" + "没有处理器");
                }
            }
        }
        if (obj.transform.childCount > 0)
        {
            MyJson.JsonNode_Array jsonsubarray = new MyJson.JsonNode_Array();
            json["child"] = jsonsubarray;
            foreach (Transform t in obj.transform)
            {
                jsonsubarray.Add(Parser(t.gameObject, needList));
            }
        }
        Debug.Log("ParserObjFinish:" + obj.name);
        AssetMgr.Reset();

        return json;
    }
#endif
    public static GameObject FromJson(MyJson.IJsonNode json, GameObject parent = null)
    {
        //建立OBJ树和idcache
        objidcache.Clear();
        var obj = ResumeGameObject(json as MyJson.JsonNode_Object, parent);
        //处理obj组件
        FillGameObject(json as MyJson.JsonNode_Object, obj);

        objidcache.Clear();

        EnableGameObject(json as MyJson.JsonNode_Object, obj);
        return obj;
    }
    static GameObject ResumeGameObject(MyJson.JsonNode_Object json, GameObject parent, int dep = 0)
    {
        var gthis = new GameObject();
        gthis.name = json["name"] as MyJson.JsonNode_ValueString;

        //if(dep==0)
        {
            gthis.SetActive(false);
        }
        //else
        //{
        //    bool ba = json["active"] as MyJson.JsonNode_ValueNumber;
        //    gthis.SetActive(ba);
        //}


        uint oid = json["_id"] as MyJson.JsonNode_ValueNumber;

        objidcache[oid] = gthis;
        foreach (MyJson.JsonNode_Object c in json["coms"] as MyJson.JsonNode_Array)
        {
            //Debug.Log(c.ToString());
            string type = c["type"] as MyJson.JsonNode_ValueString;
            if (type.Contains("UnityEngine."))
            {
                type = type.Substring("UnityEngine.".Length);
            }
            var _cc = gthis.GetComponent(type);
            Behaviour cc = _cc as Behaviour;
            if (_cc == null) cc = gthis.AddComponent(type) as Behaviour;
            if (cc != null)
            {
                bool be = c["enabled"] as MyJson.JsonNode_ValueNumber;
                cc.enabled = be;
            }
        }
        if (json.ContainsKey("child"))
        {
            foreach (MyJson.JsonNode_Object c in json["child"] as MyJson.JsonNode_Array)
            {
                ResumeGameObject(c, gthis, dep + 1);
            }
        }
        if (parent != null)
        {
            gthis.transform.parent = parent.transform;
        }

        return gthis;
    }
    static void FillGameObject(MyJson.JsonNode_Object json, GameObject obj)
    {
        string name = json["name"] as MyJson.JsonNode_ValueString;
        if (obj.name != name)
        {
            Debug.LogError("物件名字对不上" + obj.name + "," + name);
        }
        MyJson.IJsonNode ctrans = null;
        foreach (MyJson.JsonNode_Object c in json["coms"] as MyJson.JsonNode_Array)
        {
            //Debug.Log(c.ToString());
            string type = c["type"] as MyJson.JsonNode_ValueString;
            if (type.Contains("UnityEngine."))
            {
                type = type.Substring("UnityEngine.".Length);
            }
            if (type == "Transform")
            {
                ctrans = c["param"];
                continue;
            }
            var com = obj.GetComponent(type);
            if (com == null)
            {



                Debug.LogError("type:" + type);
            }
            ComponentParser.Instance.GetParser(com.GetType()).Fill(com, c["param"]);
        }
        if (ctrans != null)
        {
            ComponentParser.Instance.GetParser(typeof(Transform)).Fill(obj.transform, ctrans);
        }
        if (json.ContainsKey("child"))
        {
            for (int i = 0; i < (json["child"] as MyJson.JsonNode_Array).Count; i++)
            {
                MyJson.JsonNode_Object c = (json["child"] as MyJson.JsonNode_Array)[i] as MyJson.JsonNode_Object;
                string childname = c["name"] as MyJson.JsonNode_ValueString;
                for (int j = 0; j < obj.transform.childCount; j++)
                {
                    GameObject o = obj.transform.GetChild(j).gameObject;
                    if (o.name == childname)
                    {
                        FillGameObject(c, o);
                        continue;
                    }
                }




            }

        }
    }
    static void EnableGameObject(MyJson.JsonNode_Object json, GameObject obj)
    {
        string name = json["name"] as MyJson.JsonNode_ValueString;
        if (obj.name != name)
        {
            Debug.LogError("物件名字对不上" + obj.name + "," + name);

        }
        bool ba = json["active"] as MyJson.JsonNode_ValueNumber;
        obj.SetActive(ba);

        if (json.ContainsKey("child"))
        {
            for (int i = 0; i < (json["child"] as MyJson.JsonNode_Array).Count; i++)
            {
                MyJson.JsonNode_Object c = (json["child"] as MyJson.JsonNode_Array)[i] as MyJson.JsonNode_Object;
                string childname = c["name"] as MyJson.JsonNode_ValueString;
                for (int j = 0; j < obj.transform.childCount; j++)
                {
                    GameObject o = obj.transform.GetChild(j).gameObject;
                    if (o.name == childname)
                    {
                        //FillGameObject(c, o);
                        EnableGameObject(c, o);
                        continue;
                    }
                }
            }

            //for (int i = 0; i < obj.transform.childCount; i++)
            //{
            //    MyJson.JsonNode_Object c = (json["child"] as MyJson.JsonNode_Array)[i] as MyJson.JsonNode_Object;
            //    GameObject o = obj.transform.GetChild(i).gameObject;
            //    EnableGameObject(c, o);
            //}

        }
    }
    static Dictionary<uint, GameObject> objidcache = new Dictionary<uint, GameObject>();
    public static GameObject IDToObj(uint id)
    {
        if (objidcache.ContainsKey(id)) return objidcache[id];
        return null;
    }
    public static T IDToComponent<T>(uint id) where T : Component
    {
        var obj = IDToObj(id);
        if (obj == null) return null;
        else return obj.GetComponent<T>();
    }
    public static uint ComponentToID(Component id)
    {
        if (id == null) return 0;
        else return ObjToID(id.gameObject);
    }
    public static uint ObjToID(GameObject id)
    {
        if (id == null) return 0;
        else return (uint)id.GetInstanceID();
    }
}

