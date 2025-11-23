using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Momos.Core.Pool
{
    public interface PoolContainerBase<K, T>
    {
        /// <summary> </summary>
        /// <param name="key"> 无key对应的值返回-1 </param>
        /// <returns></returns>
        public abstract int GetCount(K key);
        public abstract bool TryGet(K key, out T item);
        public abstract void Push(K key, T item, bool isThreadSafe);
    }
}