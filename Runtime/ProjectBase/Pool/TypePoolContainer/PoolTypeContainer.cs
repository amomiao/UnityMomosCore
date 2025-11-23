using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace Momos.Core.Pool {
    public class PoolTypeContainer : PoolContainerBase<Type, IPoolable> {
        protected ConcurrentDictionary<Type, PoolBoxBase<IPoolable>> typePoolDic = new ConcurrentDictionary<Type, PoolBoxBase<IPoolable>>();

        public int GetCount(Type key) {
            if (typePoolDic.ContainsKey(key))
                return typePoolDic[key].Count;
            return -1;
        }

        public bool TryGet(Type key, out IPoolable item) {
            if (GetCount(key) > 0 && typePoolDic[key].TryOutput(out item)) {
                return true;
            }
            item = null;
            return false;
        }

        public void Push(Type key, IPoolable item, bool isThreadSafe = false) {
            if (!typePoolDic.ContainsKey(key)) {
                if (isThreadSafe) {
                    typePoolDic.TryAdd(key, new ThreadSafePoolBox<IPoolable>());
                }
                else {
                    typePoolDic.TryAdd(key, new PoolBox<IPoolable>());
                }
            }
            typePoolDic[key].Input(item);
        }

        public void Clear() {
            typePoolDic.Clear();
        }
    }
}