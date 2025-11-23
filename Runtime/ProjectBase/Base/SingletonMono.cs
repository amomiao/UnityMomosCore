using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Momos.Core
{
    /// <summary> 挂载式 继承Mono的单例模式基类 </summary>
    public abstract class SingletonMono<T> : MonoBehaviour, ISingletonMono where T : MonoBehaviour
    {
        private static T _instance;
        public static T GetInstance() => _instance;

        public abstract bool IsDontDestroy { get; }

        protected virtual void Awake()
        {
            //已经存在一个对应的单例模式对象了 不需要在有一个了
            if (_instance != null)
            {
                Destroy(this);
                return;
            }
            _instance = this as T;
            //我们挂载继承该单例模式基类的脚本后 依附的对象过场景时就不会被移除了
            //就可以保证在游戏的整个生命周期中都存在 
            if(IsDontDestroy)
                DontDestroyOnLoad(this.gameObject);
        }
    }
}