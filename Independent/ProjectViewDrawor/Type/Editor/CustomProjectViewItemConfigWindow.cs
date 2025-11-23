using Momos.Tools.EditorTools;
using Momos.Tools.EditorTools.Control;
using Momos.Tools.EditorTools.Window;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Momos.Tools.ProjectViewDrawor
{
    using static Momos.Tools.EditorTools.Control.ScrollViewGrid<HighlightFolderItem>;

    public class CustomProjectViewItemConfigWindow : SingletonConfigGridWindow<ProjectViewItemConfigAsset, HighlightFolderItem, ProjectViewItemConfigLoader>
    {
        [MenuItem("Tools/ProjectViewConfig", priority = 1)]
        public static void ShowWindow() => EditorCommandSet.ShowWindow<CustomProjectViewItemConfigWindow>(new Rect(200, 200, 1000, 200), "Project面板重绘配置");

        protected override HighlightFolderItem[] GetItems(ProjectViewItemConfigAsset config) => config.highlightFloderItemList.ToArray();
        protected override void AddNewDataItemEvt() => Config.highlightFloderItemList.Add(new HighlightFolderItem());
        protected override void RemoveDataItemEvt(HighlightFolderItem item) => Config.highlightFloderItemList.Remove(item); 

        protected override LackResourcesScriptableObjectView<ProjectViewItemConfigAsset> GetLackView()
        {
            var createItem = new LackResourcesScriptableObjectView<ProjectViewItemConfigAsset>.FuncButtonItem("创建",
                () => 
                {
                    EditorCommandSet.TrySaveScriptableObject<ProjectViewItemConfigAsset>(null,Loader.AssetName);
                });
            return new LackResourcesScriptableObjectView<ProjectViewItemConfigAsset>(Loader.ResourcePath, createItem);
        }
        protected override ScrollViewGrid<HighlightFolderItem> GetScrollViewGrid()
        {
            return new ScrollViewGrid<HighlightFolderItem>(18, 50, Config.highlightFloderItemList.ToArray(),
                new ScrollViewColumnItem((rect, item) => item.localPath = EditorGUI.TextField(rect, item.localPath),
                    "本地目录路径", 200, 0.7f, E_ColumnStyle.Single),
                new ScrollViewColumnItem((rect, item) => item.color = EditorGUI.ColorField(rect, item.color),
                    "颜色", 100, 0, E_ColumnStyle.Single),
                new ScrollViewColumnItem((rect, item) => item.isDrawColor = EditorGUI.Toggle(rect, item.isDrawColor),
                    "是否色彩绘制", 100, 0, E_ColumnStyle.Single),
                new ScrollViewColumnItem((rect, item) => EditorGUI.Toggle(rect, item.isExist),
                    "是否存在", 100, 0, E_ColumnStyle.Single, true),
                new ScrollViewColumnItem((rect, item) => EditorGUI.TextField(rect, item.guid),
                    "guid", 100, 0.3f, E_ColumnStyle.Single, true),
                new ScrollViewColumnItem((rect, item) => DrawRemoveDataBtn(item, rect),
                    "删除", 100, 0f, E_ColumnStyle.Full)
            );
        }

        protected override void DrawTop(Rect rect)
        {
            GUI.TextField(Rect.zero, "");   // 需要直辖一个控件来,让窗口感知值类型的重绘。
            // color = EditorGUI.ColorField(new Rect(0, 0, 100, 20), color);
            if (GUI.Button(new Rect(100, 0, 100, rect.height), "创建文件夹") &&
                EditorUtility.DisplayDialog("二次确认", "确定要创建配置中的文件夹吗?", "确定", "取消"))
            {
                CreateLocalFolder();
            }
            if (GUI.Button(new Rect(200, 0, 100, rect.height), "刷新"))
            {
                Refresh();
            }
        }

        private void CreateLocalFolder()
        {
            string fullPath;
            foreach (var item in Config.highlightFloderItemList)
            {
                fullPath = GetFullPath(item.localPath);
                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);
            }
            // 刷新数据视图
            AssetDatabase.Refresh();
            Refresh();
        }

        private void Refresh()
        {
            foreach (var item in Config.highlightFloderItemList)
            {
                item.isExist = Directory.Exists(GetFullPath(item.localPath));
                item.guid = AssetDatabase.AssetPathToGUID("Assets/" + item.localPath);
            }
            CustomProjectWindowItem.SetProjectItem(Config);
            // 重新渲染Project视窗
            EditorApplication.RepaintProjectWindow();
        }

        private string GetFullPath(string localPath) => Path.Combine(Application.dataPath, localPath);

    }
}