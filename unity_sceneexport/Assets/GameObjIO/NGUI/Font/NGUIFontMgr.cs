using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;
#if NGUI
    public class NGUIFontMgr
    {
        UIFont bmFontDef = null;
        Dictionary<string, UIFont> bmFont = new Dictionary<string, UIFont>();
        static NGUIFontMgr g_this;
        public static NGUIFontMgr Instance
        {
            get
            {
                if(g_this==null)g_this=new FontMgr();
                return g_this;
            }
        }


        public void RegUIFontDef(UIFont font)
        {
            bmFontDef = font;
        }
        public void RegUIFont(string name, UIFont font)
        {
            bmFont[name] = font;
        }
        public UIFont GetUIFont(string name)
        {
            if (bmFont.ContainsKey(name)) return bmFont[name];
            else return bmFontDef;

        }
    }

#endif