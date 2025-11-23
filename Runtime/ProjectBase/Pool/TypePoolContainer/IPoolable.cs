using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Momos.Core.Pool {
    /// <summary> 对象池支持的类需要有一个重置数据的接口 </summary>
    public interface IPoolable {
        /// <summary> 填入缓存池中时进行的初始化, 但初次构造时应当调用一次 </summary>
        public void ToInitial();
    }

    public static class PoolableExpansion {
        /// <summary> 放入缓存池 </summary>
        public static void PushPool(this IPoolable obj) => TypePoolMgr.GetInstance().Push(obj);
    }
}