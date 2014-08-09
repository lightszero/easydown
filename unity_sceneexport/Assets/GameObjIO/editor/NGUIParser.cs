using UnityEngine;
using System.Collections;
using UnityEditor;

public class GameObjectExport
{
    [MenuItem("GameObjectExport/ShowWindow")]
    static void ShowWindow()
    {
        GameObjectExportWindow.Init();
    }

}
