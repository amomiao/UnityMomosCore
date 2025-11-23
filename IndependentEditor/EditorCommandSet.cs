using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Momos.Tools.EditorTools
{
    public static class EditorCommandSet
    {
        public static class Inspector {
            public static void LockInspector(GameObject obj = null) {
                Selection.activeGameObject = obj;
                ActiveEditorTracker.sharedTracker.isLocked = true;
            }
            public static void UnlockInspector() {
                ActiveEditorTracker.sharedTracker.isLocked = false;
            }
            public static void Repaint() {
                //ActiveEditorTracker.sharedTracker.ForceRebuild();
                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            }
        }

        public static class ScriptObject {
            public static void SafeSave(UnityEngine.Object asset) {
                EditorUtility.SetDirty(asset);
                AssetDatabase.SaveAssetIfDirty(asset);
                AssetDatabase.Refresh();
            }
        }

        /// <summary> 加载一个窗口 </summary>
        /// <typeparam name="T"> 窗口类 </typeparam>
        /// <param name="position"> 初始Rect </param>
        /// <param name="title"> 标题 </param>
        /// <param name="evt"> 调用<see cref="EditorWindow.Show"/>前做 </param>
        public static T ShowWindow<T>(Rect position, string title = null, UnityAction<T> evt = null) where T : EditorWindow
        { 
            if(string.IsNullOrEmpty(title))
                title = typeof(T).Name;
            T win = EditorWindow.GetWindow<T>();
            win.titleContent = new GUIContent(title);
            win.position = position;
            evt?.Invoke(win);
            win.Show();
            return win;
        }

        /// <summary> 在浏览器中打开<paramref name="url"/>链接 </summary>
        public static void OpenBrowser(string url)
        {
            if (!string.IsNullOrEmpty(url))
                Application.OpenURL(url);
        }

        /// <summary> 资源管理器打开<paramref name="path"/>目录 </summary>
        public static void OpenExplorer(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                path = Path.GetFullPath(path);
                System.Diagnostics.Process.Start("explorer.exe", path);
            }
        }

        #region TrySaveScriptableObject 创建并保存可编辑物体
        /// <summary> 尝试 选择路径并创建 一个可编程物体 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="defaultName"> 保存文件的默认名称 </param>
        /// <param name="callback"> 创建<see cref="ScriptableObject"/>后进行的操作 </param>
        /// <param name="verPathUsableFunc"> 验证路径是否可用 </param>
        public static bool TrySaveScriptableObject<T>(UnityAction<T> callback, string defaultName = null, string defaultSaveDirectoryPath = null, Func<string, bool> verPathUsableFunc = null) where T : ScriptableObject
        {
            if (SavePathScriptableObjectInProject<T>(defaultName, defaultSaveDirectoryPath, out string path) &&
                (verPathUsableFunc == null || verPathUsableFunc.Invoke(path) == true))
            {
                CreateScriptableObject<T>(path, callback);
                return true;
            }
            return false;
        }
        /// <summary> 选择保存路径 给一个可编程物体 </summary>
        /// <returns> true:点击'确定'给出了一个正确的路径 </returns>
        public static bool SavePathScriptableObjectInProject<T>(string defaultName, string defaultSaveDirectoryPath, out string path) where T : ScriptableObject
        { 
            path = EditorUtility.SaveFilePanelInProject(
                    $"创建{typeof(T).Name}",
                    $"{defaultName}",
                    "asset",
                    "选择保存位置");
            return !string.IsNullOrEmpty(path);
        }
        /// <summary> 给予路径去创建 一个可编程物体 </summary>
        public static T CreateScriptableObject<T>(string saveAssetsPath, UnityAction<T> callback) where T : ScriptableObject
        { 
            T asset = ScriptableObject.CreateInstance<T>();
            callback?.Invoke(asset);
            AssetDatabase.CreateAsset(asset, saveAssetsPath);
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssetIfDirty(asset);
            AssetDatabase.Refresh();
            return asset;
        }
        #endregion TrySaveScriptableObject 创建并保存可编辑物体
    
    }
}
