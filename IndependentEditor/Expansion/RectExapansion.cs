using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Momos.Tools.EditorTools.Expansion
{
    public static class RectExapansion
    {
        // 取值
        public static Rect OnlySize(this Rect rect) => new Rect(Vector2.zero, rect.size);
        // 截取
        public static Rect CaptureFromUp(this Rect rect, float height) => new Rect(rect.x, rect.y, rect.width, height);
        public static Rect CaptureFromLeftUp(this Rect rect, float width, float height) => new Rect(rect.x, rect.y, width, height);
        public static Rect CaptureFromRightUp(this Rect rect, float width, float height) => new Rect(rect.x + rect.width - width, rect.y, width, height);
        // 仅移动
        public static Rect Move(this Rect rect, float x,float y)
            => new Rect(rect.x + x, rect.y + y, rect.width, rect.height);
        public static Rect HorizontalMove(this Rect rect, float value) => Move(rect, value, 0);
        public static Rect VerticalMove(this Rect rect, float value) => Move(rect, 0, value);
        // Padding
        public static Rect Padding(this Rect rect, float top, float bottom, float left, float right)
            => new Rect(rect.x + left, rect.y + top, rect.width - left - right, rect.height - top - bottom);
        public static Rect PaddingHorizontal(this Rect rect, float value)
            => PaddingHorizontal(rect, value, value);
        public static Rect PaddingHorizontal(this Rect rect, float left, float right)
            => Padding(rect, 0, 0, left, right);
        public static Rect PaddingVertical(this Rect rect, float value)
            => Padding(rect, value, value, 0, 0);
        public static Rect PaddingVertical(this Rect rect, float top, float bottom)
            => Padding(rect, top, bottom, 0, 0);
        public static Rect PaddingTop(this Rect rect, float top)
            => Padding(rect, top, 0, 0, 0);
        public static Rect PaddingBottom(this Rect rect, float bottom)
            => Padding(rect, 0, bottom,  0, 0);
        public static Rect PaddingLeft(this Rect rect, float left)
            => Padding(rect, 0, 0, left,  0);
        public static Rect PaddingRight(this Rect rect, float right)
            => Padding(rect, 0, 0, 0, right);
    }
}