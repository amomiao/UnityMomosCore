using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Momos.Tools.EditorTools.Style
{
    public class GUIStyleDrawor
    {
        /// <summary> 居中 </summary>
        public virtual GUIStyle GetTitleStyle(GUIStyle style)
        {
            style = new GUIStyle(style);
            style.alignment = TextAnchor.MiddleCenter;
            return style;
        }
        /// <summary> 加粗 </summary>
        public GUIStyle GetH1Style(GUIStyle style)
        {
            style = new GUIStyle(style);
            style.fontStyle = FontStyle.Bold;
            return style;
        }
        /// <summary> 原样输出 </summary>
        public GUIStyle GetH2Style(GUIStyle style)
        {
            // style = new GUIStyle(style);
            return style;
        }
    }
}