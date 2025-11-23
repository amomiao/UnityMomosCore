using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Momos.Tools.EditorTools.Control
{
    public enum E_ColumnStyle
    { 
        Single,
        Full,
    }

    /// <summary> 绘制一个表格,为数据的每一列声明绘制方式。 </summary>
    /// <typeparam name="T"> 要绘制的数据Item </typeparam>
    public class ScrollViewGrid<T>
    {
        private int titleRowHeight;
        private int rowHeight;
        private ScrollViewColumnItem[] cols;
        private T[] itemDatas;
        int[] colsWidth;
        Vector2 sv;

        public T[] Ts { set => itemDatas = value; }

        public class ScrollViewColumnItem
        {
            public int fix;
            public float percent;
            public E_ColumnStyle columnStyle;
            public string title;
            public UnityAction<Rect, T> colDrawEvt;
            public bool isDisable;

            /// <param name="title"> 列标题 </param>
            /// <param name="fix"> 本列的最小宽度值 </param>
            /// <param name="percent"> 对Rect水平上剩余空间的分配比例 </param>
            /// <param name="colStyle"> 
            /// <see cref="E_ColumnStyle.Single"/>:24像素,
            /// <see cref="E_ColumnStyle.Full"/>:占满空间 
            /// </param>
            /// <param name="colDraw">
            /// 描述如何绘制<see cref="T"/>中数据,不要使用Layout,如EditorGUI.IntField(rect, item.id);
            /// </param>
            public ScrollViewColumnItem(UnityAction<Rect, T> colDraw, string title, int fix, float percent = 0, E_ColumnStyle colStyle = E_ColumnStyle.Full, bool isDisable = false)
            {
                this.colDrawEvt = colDraw;
                this.title = title;
                this.fix = fix;
                this.percent = percent;
                this.columnStyle = colStyle;
                this.isDisable = isDisable;
            }

            public void DrawTitle(Rect rect)
            {
                GUI.TextField(rect, title);
            }
            public void DrawContentGUI(Rect rect, T t)
            {
                colDrawEvt?.Invoke(rect, t);
            }
        }

        /// <param name="titleRowHeight"> 标准值18 </param>
        /// <param name="rowHeight"> 内容行高 </param>
        /// <param name="ts"> 数据Item数组 </param>
        /// <param name="items"> 动态计算宽度的Item的percent和为1 </param>
        public ScrollViewGrid(int titleRowHeight, int rowHeight,T[] ts, params ScrollViewColumnItem[] items)
        {
            this.titleRowHeight = titleRowHeight;   
            this.rowHeight = rowHeight;
            this.itemDatas = ts;
            this.cols = items;
            colsWidth = new int[items.Length];
            sv = Vector2.zero;
        }

        /// <summary> 重设绘制数据 </summary>
        public void ResetData(T[] datas)
        {
            this.itemDatas = datas;
        }

        /// <summary> 计算列宽 </summary>
        private void GetColumnsWidth(float width)
        {
            // 第一遍先把固定宽定下来
            // 并计算宽度余量
            for (int i = 0; i < cols.Length; i++)
            {
                colsWidth[i] = cols[i].fix;
                width -= cols[i].fix;
            }
            // 如果宽度余量 <=0 那么不能动态调整
            if (width <= 0)
                return;
            // 动态分配余量空间
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].percent > 0)
                    colsWidth[i] += (int)(cols[i].percent * width);
            }
        }

        /// <summary> 默认将<see cref="Event.current.mousePosition.y"/>作为一个float参数 </summary>
        /// <param name="position"> .width里有20个像素用来给滑动条 </param>
        public void OnGUI(Rect position) => OnGUI(position,Event.current.mousePosition.y);

        /// <param name="position"> .width里有20个像素用来给滑动条 </param>
        /// <param name="relativeMouseY"> 将鼠标位置 相对组件顶部的Y值(向下增长) </param>
        public void OnGUI(Rect position,float relativeMouseY)
        {
            GetColumnsWidth(position.width);

            GUILayout.BeginArea(position);  // 1
            // 1.1
            GUILayout.BeginArea(new Rect(0, 0, position.width, titleRowHeight));
            Rect rect = new Rect(0, 0, 0, titleRowHeight);
            for (int i = 0; i < cols.Length; i++)
            {
                rect.width = colsWidth[i];
                cols[i].DrawTitle(rect);
                rect.x += rect.width;
            }
            GUILayout.EndArea();    // 1.1

            // 1.2
            sv = GUI.BeginScrollView(
                new Rect(0, titleRowHeight, position.width, position.height - titleRowHeight),
                sv,
                new Rect(0, 0, position.width - 20, itemDatas.Length * rowHeight));
            // -20给竖直进度条留出空间
            position.width -= 20;

            // 绘制悬浮高光
            SetHover(position, relativeMouseY - titleRowHeight + sv.y);

            rect = new Rect(0, 0, 0, 0);
            // 按列绘制
            for (int i = 0; i < cols.Length; i++)
            {
                rect.width = colsWidth[i];
                rect.height = // 单行风格给24个像素, 填满风格给整个行高
                    cols[i].columnStyle == E_ColumnStyle.Single ? 18 : rowHeight;

                EditorGUI.BeginDisabledGroup(cols[i].isDisable);    // 1.3
                for (int t = 0; t < itemDatas.Length; t++)
                {
                    cols[i].DrawContentGUI(rect, itemDatas[t]);
                    rect.y += rowHeight;
                }
                EditorGUI.EndDisabledGroup();   // 1.3

                rect.y = 0;
                rect.x += rect.width;
            }

            GUI.EndScrollView();  // 1.2

            GUILayout.EndArea();    // 1
        }

        /// <summary> 设置悬浮高光 </summary>
        /// <param name="position"> 组件Rect </param>
        /// <param name="scrollAreaMouseY"> 鼠标位置相对于滚动页面的y值 </param>
        private void SetHover(Rect position, float scrollAreaMouseY)
        {
            if (scrollAreaMouseY >= 0)
            {
                Rect rect;
                int index = -1;
                index = (int)scrollAreaMouseY / (int)rowHeight;
                rect = new Rect(
                    0,
                    // 归正到行的起始高度
                    index * rowHeight,
                    position.width,
                    rowHeight);
                // 只在有条目时显示
                if (itemDatas.Length > index)
                    GUI.Box(rect, GUIContent.none);
            }
        }
    }
}