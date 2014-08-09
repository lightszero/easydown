using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

    public class FontMgr
    {
        Font unityFontDef = null;
        Dictionary<string, Font> unityFont = new Dictionary<string, Font>();
        static FontMgr g_this;
        public static FontMgr Instance
        {
            get
            {
                if(g_this==null)g_this=new FontMgr();
                return g_this;
            }
        }
        public void RegUnityFontDef(Font font)
        {
            unityFontDef = font;
        }
        public void RegUnityFont(string name,Font font)
        {
            unityFont[name] = font;
        }
        public Font GetUnityFont(string name)
        {
            if(unityFont.ContainsKey(name))return unityFont[name];
            else return unityFontDef;

        }

    }

