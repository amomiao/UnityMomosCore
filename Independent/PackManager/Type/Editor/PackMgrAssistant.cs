using Momos.Tools.EditorTools;
using Momos.Tools.EditorTools.Expansion;
using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Momos.Tools.PackManager
{
    internal class PackMgrAssistant
    {
        /// <summary> 打开详情面板 </summary>
        internal void ShowPackDeltaWindow(PackMgrConfigAsset config, PackItem packItem)
        {
            PackDeltaWindow win = EditorWindow.GetWindow<PackDeltaWindow>();
            win.titleContent = new GUIContent("PackDelta");
            win.position = new Rect(200, 200, 360, 600);
            win.config = config;
            win.packItem = packItem;
            win.Show();
        }
        /// <summary> 打开上传面板 </summary>
        internal PackUploadWindow ShowPackUploadWindow(PackMgrConfigAsset config)
        {
            PackUploadWindow win = EditorWindow.GetWindow<PackUploadWindow>();
            win.titleContent = new GUIContent("PackUpload");
            win.position = new Rect(200, 200, 240, 120);
            win.config = config;
            win.Show();
            return win;
        }
        /// <summary> 打开选择依赖面板 </summary>
        internal void ShowPackItemDependsWindow(PackMgrConfigAsset config, PackItem packItem)
        {
            PackItemDependsWindow win = EditorWindow.GetWindow<PackItemDependsWindow>();
            win.titleContent = new GUIContent($"选择{packItem.packName}的依赖");
            win.position = new Rect(400, 200, 320, 200);
            win.config = config;
            win.packItem = packItem;
            win.Show();
        }

        /// <summary> 返回项目信息(有时间) </summary>
        internal string GetProjectMessage()
        {
            StringBuilder sb = new StringBuilder();
            // 日期
            sb.Append(GetTimeShort());
            sb.Append('_');
            // 编辑器版本
            sb.Append(Application.unityVersion);
            sb.Append('_');
            // 发包平台
            sb.Append(Application.platform);
            sb.Append('_');
            // 项目名
            sb.Append(Application.productName);
            return sb.ToString();
        }
        /// <summary> 返回项目信息(无时间) </summary>
        internal string GetProjectMessageNoTime()
        {
            StringBuilder sb = new StringBuilder();
            // 项目名
            sb.Append(Application.productName);
            sb.Append('_');
            // 编辑器版本
            sb.Append(Application.unityVersion);
            sb.Append('_');
            // 发包平台
            sb.Append(Application.platform);
            return sb.ToString();
        }

        /// <summary> 输出紧凑格式的时间 </summary>
        internal string GetTimeShort() => DateTime.Now.ToString("yyyyMMddHHmmss");

        /// <summary> 输出正常格式的时间 </summary>
        internal string GetTime() => DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

        /// <summary> 保存ConfigAsset并重写本地存储的Json数据 </summary>
        internal void SavePackMgrConfigAssetData(PackMgrConfigAsset config)
        {
            config.SaveInAssetDatabase();
            File.WriteAllText(config.localJsonConfigPath, config.ToJson());
        }

        /// <summary> 打开URL链接 </summary>
        internal void OpenURL(string url) => EditorCommandSet.OpenBrowser(url);

        /// <summary> 资源管理器打开目录 </summary>
        internal void OpenExplorer(string path) => EditorCommandSet.OpenExplorer(path);
    }
}