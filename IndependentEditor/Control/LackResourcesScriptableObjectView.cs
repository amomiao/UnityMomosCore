using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Momos.Tools.EditorTools.Control
{
    /// <summary> 
    /// [期望独占一个区域的面板]
    /// 缺失一个使用<see cref="Resources"/>进行加载的<see cref="ScriptableObject"/>时, 
    /// 进行'提示'并'允许进行创建'的一个视窗。
    /// </summary>
    public class LackResourcesScriptableObjectView<T> where T : ScriptableObject
    {
        public class FuncButtonItem
        {
            public string btnText;
            public UnityAction btnEvt;

            public FuncButtonItem(string btnText, UnityAction btnEvt)
            {
                this.btnText = btnText;
                this.btnEvt = btnEvt;
            }
        }

        string resLoadPath;
        FuncButtonItem[] items;

        public LackResourcesScriptableObjectView(string resLoadPath,params FuncButtonItem[] items)
        {
            this.resLoadPath = resLoadPath;
            this.items = items;
        }

        public void OnGUI(Rect position,T asset)
        {
            GUILayout.BeginArea(position);
            if (asset != null)
                EditorGUILayout.HelpBox($"并未缺失{typeof(T).Name},不需要渲染这个组件。", MessageType.None);
            else
            {
                GUILayout.Label($"未加载到一个{typeof(T).Name}");

                GUILayout.BeginHorizontal();
                GUILayout.Label("检查任一Resources目录下是否存在");
                GUILayout.TextField(resLoadPath + ".asset");
                GUILayout.EndHorizontal();

                GUILayout.Label("或是进行创建操作");

                // 按钮事件
                foreach (var item in items)
                {
                    if (GUILayout.Button(item.btnText))
                        item.btnEvt?.Invoke();
                }
            }
            GUILayout.EndArea();
        }
    }
}