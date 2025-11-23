using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Momos.Tools.Development {
    public static class GUITools {
        static Color tempColor;
        static int windowID;

        public static int ApplyWindowID() => windowID++;

        /// <param name="other"> Source: <see cref="GUI.skin"/> </param>
        public static GUIStyle CreateStyle(GUIStyle other, int fontSize = 12, FontStyle fontStyle = FontStyle.Normal, float r = 1,float g = 1,float b = 1, Func<GUIStyle, GUIStyle> changeFunc = null) { 
            GUIStyle style = new GUIStyle(other);
            style.fontSize = fontSize;
            style.fontStyle = fontStyle;
            style.normal.textColor = new Color(r, g, b);
            if (changeFunc != null) {
                changeFunc.Invoke(style);
            }
            return style;
        }

        public static void StartColor(Color color) {
            tempColor = GUI.color;
            GUI.color = color;
        }
        public static void EndColor() => GUI.color = tempColor;

        /// <param name="style"> <see cref="GUI.skin.textField"/>等预设可能会限制所有文本在一行 </param>
        public static float CalcHeight(GUIStyle style,float width, string content) =>
            style.CalcHeight(new GUIContent(content), width);

        public static void DrawColorLayer(Rect rect, Color color) {
            StartColor(color);
            GUI.Box(rect, string.Empty);
            EndColor();
        }

        public static int DrawTab(Rect position, int selected, string[] texts) =>
            GUI.Toolbar(position, selected, texts);
        public static int DrawTab(Rect position, int selected, string[] texts, int colNum) =>
            GUI.SelectionGrid(position, selected, texts, colNum);
        public static int DrawTabLayout(int selected, string[] texts) =>
            GUILayout.Toolbar(selected, texts);
        public static int DrawTabLayout(int selected, string[] texts, int colNum) =>
            GUILayout.SelectionGrid(selected, texts, colNum);
    }
}