using System;
using System.Collections.Generic;

using System.Text;


public class platform
{
#if UNITY_EDITOR
    static Dictionary<string, UnityEditor.BuildTarget> buildTargets;
    public static Dictionary<string, UnityEditor.BuildTarget> GetBuildTargets()
    {
        if (buildTargets == null)
        {
            buildTargets = new Dictionary<string, UnityEditor.BuildTarget>();
            buildTargets["win"] = UnityEditor.BuildTarget.StandaloneWindows;
            buildTargets["osx"] = UnityEditor.BuildTarget.StandaloneOSXIntel;
            buildTargets["web"] = UnityEditor.BuildTarget.WebPlayer;
            buildTargets["ios"] = UnityEditor.BuildTarget.iPhone;
            buildTargets["android"] = UnityEditor.BuildTarget.Android;
            buildTargets["metro"] = UnityEditor.BuildTarget.MetroPlayer;
            buildTargets["wp8"] = UnityEditor.BuildTarget.WP8Player;

        }
        return buildTargets;
    }
#endif
    public static string platformString
    {
        get
        {

#if UNITY_STANDALONE_WIN
            return "win";
#elif UNITY_STANDALONE_OSX
        return "win";
#elif UNITY_WEBPLAYER
        return "web";
#elif UNITY_IPHONE
        return "ios";
#elif UNITY_ANDROID
        return "android";
#elif UNITY_METRO
        return "metro";
#elif UNITY_WP8
        return "wp8";
#else
        return "unknown";
#endif
        }

    }
    public static string platformStringInEditor
    {
        get
        {
#if UNITY_EDITOR_WIN
            return "win";
#else
            return "win";
#endif
        }
    }
}

