using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Momos.Tools.ProjectViewDrawor
{
    [InitializeOnLoad]
    public static class CustomProjectWindowItem
    {
        static Dictionary<string, Color> ItemColorDic;

        static CustomProjectWindowItem()
        {
            ProjectViewItemConfigLoader loader = new ProjectViewItemConfigLoader();
            ProjectViewItemConfigAsset asset = null;
            loader.TryLoad(ref asset);
            SetProjectItem(asset);

            EditorApplication.projectWindowItemOnGUI -= OnProjectWindowItemGUI;
            EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
        }

        public static void SetProjectItem(ProjectViewItemConfigAsset config)
        {
            // 无数据清空字典
            if (config == null)
                return;
            // 重置字典
            if(ItemColorDic == null)
                ItemColorDic = new Dictionary<string, Color>();
            else
                ItemColorDic.Clear();
            // 加写颜色绘制
            foreach (var item in config.highlightFloderItemList)
            {
                if (item.isDrawColor)
                    ItemColorDic.Add(item.guid, item.color);
            }
        }

        private static void OnProjectWindowItemGUI(string guid, Rect selectionRect)
        {
            if (ItemColorDic == null || ItemColorDic.Count == 0)
                return;

            if(ItemColorDic.ContainsKey(guid))
                EditorGUI.DrawRect(selectionRect, ItemColorDic[guid]);

            //// 获取文件的完整路径
            //string path = AssetDatabase.GUIDToAssetPath(guid);
            //// 判断文件类型并设置图标和颜色
            //if (path.EndsWith(".txt"))
            //{
            //    // 修改背景颜色
            //    EditorGUI.DrawRect(selectionRect, new Color(0.9f, 0.9f, 0.6f, 0.3f));
            //    //// 添加自定义图标
            //    //Texture icon = EditorGUIUtility.IconContent("TextAsset Icon").image;
            //    //GUI.DrawTexture(new Rect(selectionRect.x, selectionRect.y, selectionRect.height, selectionRect.height), icon);
            //}
        }
    }
}