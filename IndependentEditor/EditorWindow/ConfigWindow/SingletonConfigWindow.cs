using Momos.Tools.EditorTools.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Momos.Tools.EditorTools.Window
{
    public abstract class SingletonConfigWindow<T, L> : EditorWindow where T : ScriptableObject where L : ConfigLoader<T>, new()
    {
        private LackResourcesScriptableObjectView<T> lackView;
        private T config;
        private L loader;

        protected T Config { get => config; set => config = value; }
        protected L Loader => loader ??= new L();

        /// <summary> 描述缺少Config时绘制怎样的提示面板 </summary>
        protected abstract LackResourcesScriptableObjectView<T> GetLackView();

        protected virtual void OnEnable()
        {
            Loader.TryLoad(ref config);
        }

        protected virtual void OnDisable() { }

        [Obsolete("子类非必要不要修改这里的逻辑,去实现抽象方法。")]
        protected virtual void OnGUI()
        {
            lackView ??= GetLackView();

            if (config == null)
            {
                lackView.OnGUI(new Rect(Vector2.zero, position.size), config);
                Loader.TryLoad(ref config);
            }
            else
            {
                OnDrawConfigGUI();
            }
        }

        protected abstract void OnDrawConfigGUI();
    }
}