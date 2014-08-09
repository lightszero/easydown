using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if NGUI
    class NGUIParser
    {
        public static void ParserWidget(MyJson.JsonNode_Object json, UIWidget widget)
        {
            json["w_color"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.ColorToString(widget.color));
            json["w_pivot"] = new MyJson.JsonNode_ValueString(widget.pivot.ToString());
            json["w_depth"] = new MyJson.JsonNode_ValueNumber(widget.depth);
            json["w_dimensions"] = new MyJson.JsonNode_ValueString(ComponentTypeConvert.Vector2ToString(new Vector2(widget.width, widget.height)));
            json["w_keepasp"] = new MyJson.JsonNode_ValueString(widget.keepAspectRatio.ToString());
            //json["w_asp"] = new MyJson.JsonNode_ValueNumber(widget.aspectRatio);
        }
        public static void FillWidget(UIWidget widget, MyJson.JsonNode_Object json)
        {
            widget.color = ComponentTypeConvert.StringToColor(json["w_color"] as MyJson.JsonNode_ValueString);
            widget.pivot = (UIWidget.Pivot)Enum.Parse(typeof(UIWidget.Pivot), json["w_pivot"] as MyJson.JsonNode_ValueString);
            widget.depth = json["w_depth"] as MyJson.JsonNode_ValueNumber;

            widget.keepAspectRatio = (UIWidget.AspectRatioSource)Enum.Parse(typeof(UIWidget.AspectRatioSource), json["w_keepasp"] as MyJson.JsonNode_ValueString);

            Vector2 demensions = ComponentTypeConvert.StringToVector2(json["w_dimensions"] as MyJson.JsonNode_ValueString);
            widget.SetDimensions((int)demensions.x, (int)demensions.y);

        }
    }
#endif