using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Momos.Core.Asset
{
    [CreateAssetMenu(fileName = "ScriptObjectSet", menuName = "CustomScriptObject/ScriptObjectSet", order = 0)]
    public class ScriptObjectSetAsset : ScriptableObject
    {
        [SerializeField]
        private ScriptableObject[] items;

        // private string[] description;

        /// <summary> 加载一个ScriptableObject实例 </summary>
        public T GetScriptableObejct<T>(string name = "") where T : ScriptableObject
        {
            T so = null;
            foreach (var item in items)
            {
                if (item is T t)
                {
                    if (string.IsNullOrEmpty(name) || item.name.Equals(name))
                    {
                        so = t;
                        break;
                    }
                }
            }
            return so;
        }

        /// <summary> 返回一个ScriptableObject调用者自己隐式转换 </summary>
        public ScriptableObject GetScriptableObejct(Type type,string name ="")
        {
            ScriptableObject so = null;
            foreach (var item in items)
            {
                if (item.GetType().Equals(type))
                {
                    if (string.IsNullOrEmpty(name) || item.name.Equals(name))
                    {
                        so = item;
                        break;
                    }
                }
            }
            return so;
        }
    }
}