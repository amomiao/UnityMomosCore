using Momos.Tools.EditorTools.Control;
using Momos.Tools.EditorTools.Expansion;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Momos.Tools.EditorTools.Window
{
    /// <summary> 声明了一个抽象表格,以及一些事件 </summary>
    /// <typeparam name="T"> ConfigAsset </typeparam>
    /// <typeparam name="I"> ConfigAsset的数据条目 </typeparam>
    /// <typeparam name="L"> 描述如何加载资源的<see cref="ConfigLoader{T}"/>子类 </typeparam>
    public abstract class SingletonConfigGridWindow<T, I, L> : SingletonConfigWindow<T, L> where T : ScriptableObject where L : ConfigLoader<T>, new()
    {
        ScrollViewGrid<I> svg;
        protected ScrollViewGrid<I> SVG => svg ??= GetScrollViewGrid();

        /// <summary> 从position中得到的size </summary>
        protected Rect WinRect { get; private set; }
        private Rect topRect;

        /// <summary> 创建表格前的顶部间隔 </summary>
        protected virtual float PaddingTop => 20;
        /// <summary> 表格外部功能按钮的宽度 </summary>
        protected virtual float BtnWidth => 60;
        /// <summary> 表格外部功能按钮的高度 </summary>
        protected virtual float BtnHeight => PaddingTop;

        public void ResetData() => SVG.ResetData(GetItems(Config));

        protected void DrawNewDataBtn(Rect rect, string text = "新建")
        {
            if (GUI.Button(rect, text))
            {
                AddNewDataItemEvt();
                ResetData();
            }
        }
        protected void DrawRemoveDataBtn(I item, Rect rect, string text = "移除")
        {
            if (GUI.Button(rect, text) && VerRemoveDataEvt(item))
            {
                RemoveDataItemEvt(item);
                ResetData();
            }
        }
        protected void DrawSaveDataBtn(Rect rect, string text = "保存")
        {
            if (GUI.Button(rect, text))
            {
                SaveConfigEvt(Config);
                Config.SaveInAssetDatabase();
            }
        }

        /// <summary> 绘制表格之上区域的内容 </summary>
        protected virtual void DrawTop(Rect rect) { }
        /// <summary> 点击'移除Btn'之后, 继续进行是否删除的验证 </summary>
        protected virtual bool VerRemoveDataEvt(I item) => EditorUtility.DisplayDialog("二次确认", "确定要移除吗?", "确定", "取消");
        /// <summary> 对Config进行保存的事件 </summary>
        protected virtual void SaveConfigEvt(T config) { }

        /// <summary> 从ConfigAsset中取出数据 </summary>
        protected abstract I[] GetItems(T config);
        /// <summary> 新增条目事件 </summary>
        protected abstract void AddNewDataItemEvt();
        /// <summary> 移除条目事件 </summary>
        protected abstract void RemoveDataItemEvt(I item);
        /// <summary> 得到一个SVG对象 </summary>
        protected abstract ScrollViewGrid<I> GetScrollViewGrid();

        protected override void OnDrawConfigGUI()
        {
            WinRect = position.OnlySize();

            // 自带'新建'和'保存'按钮
            DrawNewDataBtn(WinRect.CaptureFromLeftUp(BtnWidth, BtnHeight));
            DrawSaveDataBtn(WinRect.CaptureFromRightUp(BtnWidth, BtnHeight));
            // 展示一下当前的ConfigAsset
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.ObjectField(WinRect.CaptureFromLeftUp(BtnWidth + BtnWidth, BtnHeight).PaddingLeft(BtnWidth), Config, typeof(T), false);
            EditorGUI.EndDisabledGroup();

            // 允许在首行绘制其他空间
            topRect = WinRect.CaptureFromUp(PaddingTop).PaddingHorizontal(BtnWidth + BtnWidth, BtnWidth);
            GUILayout.BeginArea(topRect);
            DrawTop(topRect);
            GUILayout.EndArea();
            // 绘制表格
            SVG.OnGUI(position.OnlySize().PaddingTop(PaddingTop), UnityEngine.Event.current.mousePosition.y - PaddingTop);
        }
    }
}