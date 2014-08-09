using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Text;

public class GameObjectExportWindow : EditorWindow
{

    public static void Init()
    {
        // Get existing open window or if none, make a new one:
        //获取现有的打开窗口或如果没有，创建一个新的



        var window = EditorWindow.GetWindow<GameObjectExportWindow>(false, "GameObjectExport");
        //window.title = "才";
        window.SelfInit();
    }

    public void SelfInit()
    {


    }
    GameObject active;
    public void Update()
    {
        if (Selection.activeGameObject != active)
        {

            active = Selection.activeGameObject;
            mapVisTag.Clear();
        }
    }
    Vector2 v1 = Vector2.zero;
    public void OnGUI()
    {
        GUILayout.Label("Select:" + (active == null ? "Null" : active.name));
        Layout_DrawSeparator(Color.white);
        if (active != null)
        {
            v1 = GUILayout.BeginScrollView(v1, GUILayout.Height(150));
            GUI_TreeObj(active, "", 0);

            GUILayout.EndScrollView();
            Layout_DrawSeparator(Color.white);
            if (GUILayout.Button("Parser", GUILayout.Width(100)))
            {
                NeedList nlist = new NeedList();
                var json = GameObjParser.Parser(active, nlist) as MyJson.JsonNode_Object ;
                string sname = json["name"] as MyJson.JsonNode_ValueString;
                json["name"] = new MyJson.JsonNode_ValueString(sname.ToLower());
                string path = System.IO.Path.Combine(Application.streamingAssetsPath, "layout");
                if(System.IO.Directory.Exists(path)==false)
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                StringBuilder sb =new StringBuilder();
                json.ConvertToStringWithFormat(sb,0);
                System.IO.File.WriteAllText(System.IO.Path.Combine(path, active.name.ToLower() +".layout.txt"),sb.ToString()) ;
                System.IO.File.WriteAllText(System.IO.Path.Combine(path, active.name.ToLower() + ".depend.txt"), nlist.ToString());

            }   

            //if (GUILayout.Button("Export", GUILayout.Width(100)))
            //{

            //}
        }
        else
        {
            GUILayout.Label("Select Null:");
        }
    }
    public static void Layout_DrawSeparator(Color color, float height = 4f)
    {

        Rect rect = GUILayoutUtility.GetLastRect();
        GUI.color = color;
        GUI.DrawTexture(new Rect(0f, rect.yMax, Screen.width, height), EditorGUIUtility.whiteTexture);
        GUI.color = Color.white;
        GUILayout.Space(height);
    }
    public static void Layout_DrawSeparatorV(Color color, float width = 4f)
    {

        Rect rect = GUILayoutUtility.GetLastRect();
        GUI.color = color;
        GUI.DrawTexture(new Rect(rect.xMax, rect.yMin, width, Screen.height), EditorGUIUtility.whiteTexture);
        GUI.color = Color.white;
        GUILayout.Space(width);
    }
    Dictionary<string, bool> mapVisTag = new Dictionary<string, bool>();
    void GUI_TreeObj(GameObject obj, string path, int dep)
    {
        path += "/" + obj.name;
        bool bvis = true;
        if (mapVisTag.ContainsKey(path))
        {
            bvis = mapVisTag[path];
        }
        string name = "";
        for (int i = 0; i < dep; i++)
        {
            if (i == dep - 1)
            {
                name += "+";
            }
            else
                name += "  ";
        }
        name += obj.name;
        bvis = GUILayout.Toggle(bvis, name, GUILayout.Height(15));
        mapVisTag[path] = bvis;
        if (bvis)
        {
            foreach (Transform i in obj.transform)
            {
                GUI_TreeObj(i.gameObject, path, dep + 1);
            }
        }
    }

}

