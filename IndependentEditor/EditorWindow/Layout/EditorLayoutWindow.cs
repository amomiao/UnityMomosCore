using Momos.Tools.EditorTools.Expansion;
using Momos.Tools.EditorTools.Style;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Momos.Tools.EditorTools.Window
{
    public class EditorLayoutWindow : EditorWindow
    {
        GUIStyleDrawor style;
        // 每绘制帧初始化
        Vector2 rectXYPointer;
        Vector2 tempRectXYPointer;  // 仍保存着rectXYPointer上一次移动前的数值

        /// <summary> 
        /// 对于不同语义下,
        /// rectXYPointer既可以是position,
        /// 又可以是size。 
        /// </summary>
        public Rect NowRect => new Rect(rectXYPointer, rectXYPointer);
        /// <summary> GUI标准高度 </summary>
        public virtual float StandardGUIHeight => 18;
        
        public virtual GUIStyleDrawor GetGUIStyleDrawor() => new GUIStyleDrawor();

        // 绘制简单控件
        public void DrawTitle(string title) => GUI.Label(ApplyRect(), title, style.GetTitleStyle(GUI.skin.label));
        public void DrawH1(string text) => GUI.Label(ApplyRect(), text, style.GetH1Style(GUI.skin.label));
        public void DrawH2(string text) => GUI.Label(ApplyRect(), text, style.GetH2Style(GUI.skin.label));
        // Rect指针向下移动,返回向下移动的量
        public float PaddingRect() => PaddingRect(StandardGUIHeight);
        public float PaddingRect(float height)  // Logic In
        {
            rectXYPointer.y += height;
            return height;
        }
        // Rect指针注册空间,返回 注册的Rect在水平方向上Padding后的值
        public Rect ApplyRectPadding(float value) => ApplyRect().PaddingHorizontal(value);
        // Rect指针注册空间,返回注册得到的Rect
        public Rect ApplyRect() => ApplyRect(this.position.width, StandardGUIHeight);
        public Rect ApplyRect(float height) => ApplyRect(this.position.width, height);
        public Rect ApplyRect(float width, float height) // Logic In
        {
            Rect rect = new Rect(rectXYPointer, new Vector2(width, height));
            tempRectXYPointer = rectXYPointer;
            PaddingRect(height + 2); // 多给2像素的空隙
            return rect;
        }

        protected virtual void OnEnable()
        {
            style = GetGUIStyleDrawor();
        }

        protected virtual void OnDisable() { }


        protected virtual void OnGUI()
        {
            rectXYPointer = Vector2.zero;
            tempRectXYPointer = Vector2.zero;


        }
    }
}