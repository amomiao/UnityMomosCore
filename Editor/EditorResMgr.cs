using Momos.Core;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Momos.Tools.EditorTools.Asset
{
    /// <summary>
    /// 编辑器资源管理器
    /// 注意：只有在开发时能使用该管理器加载资源 用于开发功能
    /// 发布后 是无法使用该管理器的 因为它需要用到编辑器相关功能
    /// </summary>
    public class EditorResMgr : BaseManager<EditorResMgr>
    {
        //用于放置需要打包进AB包中的资源路径 
        private string rootPath = "Assets/GameData/";

        private T LoadBase<T>(string allPath) where T : Object => AssetDatabase.LoadAssetAtPath<T>(allPath);

        // 加载单个资源的
        public T LoadEditorRes<T>(string path) where T : Object
        {
            string suffixName = "";
            //预设体、纹理（图片）、材质球、音效等等
            if (typeof(T) == typeof(GameObject))
                suffixName = ".prefab";
            else if (typeof(T) == typeof(Material))
                suffixName = ".mat";
            else if (typeof(T) == typeof(Texture))
                suffixName = ".png";
            else if (typeof(T) == typeof(AudioClip))
                suffixName = ".mp3";
            T res = AssetDatabase.LoadAssetAtPath<T>(rootPath + path + suffixName);
            return res;
        }

        /// <summary> 加载单个ScriptableObject资源 </summary>
        public T LoadScriptableObject<T>(string path) where T : ScriptableObject
        {
            T res = AssetDatabase.LoadAssetAtPath<T>(rootPath + path + ".asset");
            return res;
        }

        /// <summary> 加载一个文件夹下的全部 某种ScriptableObject资源 </summary>
        public T[] LoadScriptableObjects<T>(string path) where T : ScriptableObject
        {
            Queue<T> queue = new Queue<T>();
            string[] paths;
            if (Directory.Exists(rootPath + path))
            {
                paths = Directory.GetFiles(rootPath + path, "*.asset");
                foreach (string p in paths)
                {
                    queue.Enqueue(LoadBase<T>(p));
                }
            }
            return queue.ToArray();
        }

        // 加载图集相关资源的
        public Sprite LoadSprite(string path, string spriteName)
        {
            //加载图集中的所有子资源 
            Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(rootPath + path);
            //遍历所有子资源 得到同名图片返回
            foreach (var item in sprites)
            {
                if (spriteName == item.name)
                    return item as Sprite;
            }
            return null;
        }

        // 加载图集文件中的所有子图片并返回给外部
        public Dictionary<string, Sprite> LoadSprites(string path)
        {
            Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>();
            Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(rootPath + path);
            foreach (var item in sprites)
            {
                spriteDic.Add(item.name, item as Sprite);
            }
            return spriteDic;
        }

        // 将字符串数据写入txt存入StreamingAssets文件夹
        public void String2TxtInStreamingAssets(string name, string content, string prefixPath = "")
        {
            string path = Application.streamingAssetsPath + "/";
            if (!string.IsNullOrEmpty(prefixPath))
            {
                path += prefixPath;
                if (prefixPath[prefixPath.Length - 1] != '/') //必然大于0
                {
                    path += "/";
                    Debug.Log($"{nameof(EditorResMgr)} {nameof(String2TxtInStreamingAssets)}:参数{nameof(prefixPath)}书写错误!");
                }
            }
            if (Directory.Exists(path))
            {
                path += name + ".txt";
                File.WriteAllText(path, content);
                Debug.Log($"{nameof(EditorResMgr)} {nameof(String2TxtInStreamingAssets)}:保存成功=>{path}");
            }
            else
            {
                Debug.Log($"{nameof(EditorResMgr)} {nameof(String2TxtInStreamingAssets)}:{path}不存在!");
            }
        }

        // 将StreamingAssets文件夹中的txt读入String
        public string Txt2StringInStreamingAssets(string name, string prefixPath = "")
        {
            string path = Application.streamingAssetsPath + "/";
            if (!string.IsNullOrEmpty(prefixPath))
            {
                path += prefixPath;
                if (prefixPath[prefixPath.Length - 1] != '/') //必然大于0
                {
                    path += "/";
                    Debug.Log($"{nameof(EditorResMgr)} {nameof(Txt2StringInStreamingAssets)}:参数{nameof(prefixPath)}书写错误!");
                }
            }
            if (Directory.Exists(path))
            {
                path += name + ".txt";
                if (File.Exists(path))
                {
                    return File.ReadAllText(path);
                }
            }
            Debug.Log($"{nameof(EditorResMgr)} {nameof(String2TxtInStreamingAssets)}:读取失败<={path}");
            return string.Empty;
        }
    }
}