using Momos.Tools.EditorTools;
using Momos.Tools.EditorTools.Control;
using Momos.Tools.EditorTools.Window;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Momos.Tools.PackManager
{
    using static ScrollViewGrid<PackItem>;

    internal class PackManagerConfigWindow : SingletonConfigGridWindow<PackMgrConfigAsset, PackItem, PackMgrConfigLoader>
    {
        [MenuItem("Tools/PackManagerConfig", priority = 0)]
        internal static void ShowWindow() => EditorCommandSet.ShowWindow<PackManagerConfigWindow>(new Rect(200, 200, 1000, 200), "PackManagerConfig");

        private PackMgrAssistant assistant;
        private PackMgrAssistant Assistant => assistant ??= new PackMgrAssistant();

        protected override float PaddingTop => 40;
        protected override float BtnHeight => 20;

        protected override void OnEnable()
        {
            base.OnEnable();
            // isInstal是private的 未持久化,需要时不时重写进内存。
            if (Config != null)
            {
                foreach (var item in Config.packItemList)
                {
                    if (Config.installedPacks.Contains(item.packName))
                        item.IsInstal = true;
                }
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            //if (Config != null)
            //    SaveConfigEvt(Config);
        }

        protected override LackResourcesScriptableObjectView<PackMgrConfigAsset> GetLackView()
        {
            return new LackResourcesScriptableObjectView<PackMgrConfigAsset>(Loader.ResourcePath,
                // .MPack是放置在项目外的文件
                new LackResourcesScriptableObjectView<PackMgrConfigAsset>.FuncButtonItem("创建.MPack文件",
                    () =>
                    {
                        string path = EditorUtility.SaveFilePanel(
                            "创建.MPack",
                            $"{Application.dataPath}",
                            $"{Loader.AssetName}",
                            "MPack"
                        );
                        if (!string.IsNullOrEmpty(path))
                        {
                            string dirPath = path.Substring(0, path.LastIndexOf(Path.DirectorySeparatorChar));
                            if ((Directory.GetFiles(dirPath).Length == 0 && Directory.GetDirectories(dirPath).Length == 0) ||
                                EditorUtility.DisplayDialog("非空文件夹", $"{dirPath}目录非空,是否创建?", "确定", "取消"))
                            {
                                string jsonContent = new PackMgrConfigAsset.ConfigData(path).ToJson();
                                File.WriteAllText(path, jsonContent);
                            }
                        }
                    }),
                new LackResourcesScriptableObjectView<PackMgrConfigAsset>.FuncButtonItem("读入.MPack文件 创建ConfigAsset",
                    () =>
                    {
                        string path = EditorUtility.OpenFilePanel(
                            "读入.MPack文件",
                            $"{Application.dataPath}",
                            $"MPack");
                        string json = File.ReadAllText(path);
                        EditorCommandSet.TrySaveScriptableObject<PackMgrConfigAsset>(
                            (config) => config.FromJson(json),
                            Loader.AssetName,
                            string.Empty,
                            (path) => Loader.IsUsablePath(path) || EditorUtility.DisplayDialog("警告", Loader.IsUnusablePathWarning(path), "是", "否"));
                    }));
        }

        protected override PackItem[] GetItems(PackMgrConfigAsset config) => config.packItemList.ToArray();
        protected override void AddNewDataItemEvt() => Assistant.ShowPackUploadWindow(Config).UploadCreate();
        protected override void RemoveDataItemEvt(PackItem item) => Config.packItemList.Remove(item);
        protected override void SaveConfigEvt(PackMgrConfigAsset config)
        {
            base.SaveConfigEvt(config);
            // 其他页面不会涉及'已安装'相关逻辑
            foreach (PackItem item in Config.packItemList)
            {
                // 被标记为已安装的包 不在记录列表里
                if (item.IsInstal && !Config.installedPacks.Contains(item.packName))
                {
                    // 记录
                    Config.installedPacks.Add(item.packName);
                    item.usageRecords.Add(new UsageRecordBody(Assistant.GetTime(), "添加:" + Assistant.GetProjectMessageNoTime()));
                }
                // 被标记为未安装的包 在记录列表里
                else if (!item.IsInstal && Config.installedPacks.Contains(item.packName))
                {
                    // 删除
                    Config.installedPacks.Remove(item.packName);
                    item.usageRecords.Add(new UsageRecordBody(Assistant.GetTime(), "移除:" + Assistant.GetProjectMessageNoTime()));
                }
            }
            Assistant.SavePackMgrConfigAssetData(Config);
        }

        protected override void DrawTop(Rect rect)
        {
            GUI.Label(new Rect(0, 0, rect.width, 20), "LocalJsonPath:");
            GUI.Label(new Rect(0, 20, rect.width, 20), $"{base.Config.localJsonConfigPath}");
        }

        protected override ScrollViewGrid<PackItem> GetScrollViewGrid()
        {
            return new ScrollViewGrid<PackItem>(18, 48, Config.packItemList.ToArray(),
                // 可写
                new ScrollViewColumnItem((rect, item) => item.packName = GUI.TextField(rect, item.packName),
                    "包名", 150, 0.1f, E_ColumnStyle.Full),
                // 只读
                new ScrollViewColumnItem((rect, item) => GUI.TextField(rect, item.directoryName),
                    "目录名", 150, 0.1f, E_ColumnStyle.Single, true),
                // 本页只读
                new ScrollViewColumnItem((rect, item) => GUI.TextField(rect, item.Description),
                    "说明", 200, 0.8f, E_ColumnStyle.Single, true),
                // 运算只读
                new ScrollViewColumnItem((rect, item) => EditorGUI.Toggle(rect, item.IsCompletedDepends(Config)),
                    "依赖完整", 80, 0, E_ColumnStyle.Single, true),
                // 可写
                new ScrollViewColumnItem((rect, item) => item.IsInstal = EditorGUI.Toggle(rect, item.IsInstal),
                    "已安装", 50, 0, E_ColumnStyle.Single),
                // 按钮
                new ScrollViewColumnItem((rect, item) =>
                    {
                        if (GUI.Button(rect, "详情"))
                            Assistant.ShowPackDeltaWindow(Config, item);
                    },
                    "详情", 100, 0, E_ColumnStyle.Full),
                // 按钮
                new ScrollViewColumnItem((rect, item) => DrawRemoveDataBtn(item, rect),
                    "删除", 100, 0, E_ColumnStyle.Full)
            );
        }
    }
}