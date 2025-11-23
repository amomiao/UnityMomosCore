using Momos.Tools.EditorTools.Control;
using UnityEditor;
using UnityEngine;

namespace Momos.Tools.PackManager
{
    internal class PackDeltaWindow : EditorWindow
    {
        internal PackMgrConfigAsset config;
        internal PackItem packItem;
        private bool isChangable = false;
        private Vector2 usv;
        private Vector2 dsv;

        private ScrollViewGrid<UploadRecordBody> uploadSVG;
        private ScrollViewGrid<UsageRecordBody> downSVG;

        private PackMgrAssistant assistant;
        private PackMgrAssistant Assistant => assistant ??= new PackMgrAssistant();

        private void OnGUI()
        {

            isChangable = EditorGUILayout.Toggle("准许修改", isChangable);
            EditorGUILayout.BeginHorizontal(); // 0
            #region 首行按钮
            if (GUILayout.Button("保存"))
            {
                EditorUtility.SetDirty(config);
                AssetDatabase.SaveAssetIfDirty(config);
            }
            if (GUILayout.Button("发布新版"))
            {
                Assistant.ShowPackUploadWindow(config).UploadUpdate(packItem.packName, packItem.directoryName);
            }
            if (GUILayout.Button("打开目录"))
            {
                Assistant.OpenExplorer(config.GetPackItemDirectoryPath(packItem));
            }
            if (GUILayout.Button("打开Git目录"))
            {
                Assistant.OpenExplorer(packItem.gitDirectoryPath);
            }
            if (GUILayout.Button("跳转到Git页面"))
            {
                Assistant.OpenURL(packItem.gitUrl);
            }
            #endregion 首行按钮
            GUILayout.EndHorizontal(); // 0

            EditorGUILayout.BeginHorizontal(); // 1
            #region 信息修改控件
            // 行名
            EditorGUILayout.BeginVertical(); // 1.1
            GUILayout.Label("包名:");
            GUILayout.Label("说明:");
            GUILayout.Label("UnionNamespace:");
            GUILayout.Label("HasReadme:");
            GUILayout.Label("Git目录:");
            GUILayout.Label("GitURL:");
            EditorGUILayout.EndVertical(); // 1.1

            // 行内容
            EditorGUILayout.BeginVertical(); // 1.2
            EditorGUI.BeginDisabledGroup(!isChangable);
            // 包名
            packItem.packName = GUILayout.TextField(packItem.packName);
            // 描述
            GUILayout.Label(packItem.Description);
            // 命名空间
            packItem.isMomos = EditorGUILayout.Toggle(packItem.isMomos);
            // 拥有Readme
            packItem.hasReadme = EditorGUILayout.Toggle(packItem.hasReadme);
            // git目录
            packItem.gitDirectoryPath = GUILayout.TextField(packItem.gitDirectoryPath);
            // gitURL
            packItem.gitUrl = GUILayout.TextField(packItem.gitUrl);
            // ...
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical(); // 1.2
            #endregion 信息修改控件
            EditorGUILayout.EndHorizontal(); // 1 

            #region 依赖设置
            GUILayout.BeginHorizontal();    // 2

            GUILayout.Label("依赖1:在本管理器能直接访问到的包");
            if (GUILayout.Button("选择依赖"))
            {
                Assistant.ShowPackItemDependsWindow(config, packItem);
            }
            GUILayout.EndHorizontal();  // 2
            if (packItem.depends1.Count == 0)
                GUILayout.Label("无");
            else
            {
                foreach (var item in packItem.depends1)
                {
                    GUILayout.BeginHorizontal();    // 3
                    // 依赖名称
                    GUILayout.Label(item);
                    // 是否安装了依赖
                    EditorGUILayout.Toggle(config.installedPacks.Contains(item));
                    // ...
                    GUILayout.EndHorizontal();  // 3
                }
            }
            GUILayout.Label("依赖2:需要从外部导入的包");
            packItem.depends2 = GUILayout.TextArea(packItem.depends2);
            #endregion 依赖

            #region 上下记录
            // https://discussions.unity.com/t/how-to-use-guilayoututility-getrect-properly/78949
            // GUILayoutUtility.GetRect(position.width, 100);
            // Debug.Log(GUILayoutUtility.GetLastRect());
            if (uploadSVG == null)
                uploadSVG = new ScrollViewGrid<UploadRecordBody>(18, 18, packItem.uploadRecords.ToArray(),
                    new ScrollViewGrid<UploadRecordBody>.ScrollViewColumnItem(
                        (rect, record) =>
                        {
                            if (GUI.Button(rect, "打开目录"))
                                Assistant.OpenExplorer(config.GetPackItemDirectoryPath(packItem, record));
                        },
                        "打开目录", 100, 0, E_ColumnStyle.Full),
                    new ScrollViewGrid<UploadRecordBody>.ScrollViewColumnItem((rect, record) => GUI.Label(rect, record.dataTime), "上传时间", 200, 0, E_ColumnStyle.Single, true),
                    new ScrollViewGrid<UploadRecordBody>.ScrollViewColumnItem((rect, record) => GUI.Label(rect, record.projectMessage), "上传自项目", 100, 0.3f, E_ColumnStyle.Single, true),
                    new ScrollViewGrid<UploadRecordBody>.ScrollViewColumnItem((rect, record) => GUI.Label(rect, record.directoryName), "目录名", 100, 0.3f, E_ColumnStyle.Single, true),
                    new ScrollViewGrid<UploadRecordBody>.ScrollViewColumnItem((rect, record) => GUI.Label(rect, record.description), "描述", 100, 0.4f, E_ColumnStyle.Single, true)
                );
            GUILayout.BeginArea(new Rect(0, 250, position.width, 150));
            uploadSVG.OnGUI(new Rect(0, 0, position.width, 150));
            GUILayout.EndArea();

            if (downSVG == null)
                downSVG = new ScrollViewGrid<UsageRecordBody>(18, 18, packItem.usageRecords.ToArray(),
                     new ScrollViewGrid<UsageRecordBody>.ScrollViewColumnItem((rect, record) => GUI.Label(rect, record.dataTime), "下载时间", 200, 0.5f, E_ColumnStyle.Single, true),
                     new ScrollViewGrid<UsageRecordBody>.ScrollViewColumnItem((rect, record) => GUI.Label(rect, record.projectMessage), "下载自项目", 200, 0.5f, E_ColumnStyle.Single, true)
                 );
            GUILayout.BeginArea(new Rect(0, 430, position.width, 150));
            downSVG.OnGUI(new Rect(0, 0, position.width, 150));
            GUILayout.EndArea();
            #endregion 上下记录
        }
    }
}