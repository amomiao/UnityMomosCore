using Momos.Core.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Momos.Core.Asset
{
    /// <summary>
    /// 资源加载模块
    /// 1.异步加载
    /// 2.委托和 lambda表达式
    /// 3.协程
    /// 4.泛型
    /// </summary>
    public class ResMgr : BaseManager<ResMgr>
    {
        /// <summary> 对加载到的对象返回前进行预处理 </summary>
        private T Preprocess<T>(T res) where T : Object
        {
            if (res != null)
            {
                //如果对象是一个GameObject类型的 把他实例化后 再返回出去
                if (res is GameObject || res is MonoBehaviour)
                    return GameObject.Instantiate(res);
            }
            //TextAsset AudioClip
            return res;
        }

        // 同步加载资源
        // Load负责仅加载,并尽可能保证返回不为空
        public T Load<T>(string name) where T : Object
        {
            T res = Resources.Load<T>(name);
            if (res == null)
            {
                // 如果对象是一个ScriptableObject子类实例,加载失败后再去专门的ScriptableObject管理器访问一下
                // T is ScriptableObject时,并不会返回true,但不会有这种清空
                if (typeof(T).IsSubclassOf(typeof(ScriptableObject)))
                {
                    ScriptableObject so = ScriptObjectAssetMgr.GetInstance().Load(typeof(T), name);
                    if (so != null)
                        res = so as T;
                }
            }
            return res;
        }
        /// <summary> LoadInstance会对一些不同类型参数做预处理 </summary>
        /// <typeparam name="T">
        /// <see cref="GameObject"/>仅克隆;
        /// </typeparam>
        public T LoadInstance<T>(string name) where T : Object
            => Preprocess<T>(Load<T>(name));

        //异步加载资源
        //开启异步加载的协程
        public void LoadAsync<T>(string name, UnityAction<T> callback) where T : Object
            => MonoMgr.GetInstance().StartCoroutine(IE_LoadAsync(name, callback));
        private IEnumerator IE_LoadAsync<T>(string name, UnityAction<T> callback) where T : Object
        {
            ResourceRequest r = Resources.LoadAsync<T>(name);
            yield return r;
            callback(r.asset as T);
        }

        // 协程异步加载并进行预处理
        public void LoadInstanceAsync<T>(string name, UnityAction<T> callback) where T : Object
            => MonoMgr.GetInstance().StartCoroutine(IE_LoadInstanceAsync(name, callback));
        private IEnumerator IE_LoadInstanceAsync<T>(string name, UnityAction<T> callback) where T : Object
        {
            yield return IE_LoadAsync<T>(name, (asset) => Preprocess<T>(asset));
        }
    }
}