using UnityEngine;
using UnityEngine.Events;

namespace Momos.Core.Pool {
    public class TypePoolMgr : BaseManager<TypePoolMgr>, ILoadSceneClear {
        private PoolTypeContainer container = new PoolTypeContainer();

        /// <summary> 
        /// 尝试获取:
        /// 不对<see cref="MonoBehaviour"/>子类提供额外服务, 使用<see cref="ObjPoolMgr.PushComponentObj{T}(string, T)"/>
        /// </summary>
        /// <typeparam name="T"> 获取的类型 </typeparam>
        /// <param name="item"> 获得的数据 </param>
        /// <param name="callBack"> 得到后马上做执行的事件 </param>
        /// <returns> true:成功获得 </returns>
        public bool TryGet<T>(out T item, UnityAction<T> callBack = null) where T : class, IPoolable {
            // 获得对象
            if (!container.TryGet(typeof(T), out IPoolable pi) ||
                // 类匹配
                pi is not T) {
                // 尝试获取失败, 返回
                item = null;
                return false;
            }
            // 获取成功, 执行回调
            item = (T)pi;
            callBack?.Invoke(item);
            return true;
        }

        /// <summary> 
        /// 放置数据:
        /// 不对<see cref="MonoBehaviour"/>子类提供额外服务, 使用<see cref="ObjPoolMgr.PushComponentObj{T}(string, T)"/>
        /// </summary>
        /// <typeparam name="T"> 放置的类型 </typeparam>
        /// <param name="item"> 放置的数据 </param>
        public void Push<T>(T item, bool isThreadSafe = false) where T : class, IPoolable {
            if (item != null) {
                // 数据重置
                item.ToInitial();
                container.Push(item.GetType(), item, isThreadSafe);
            }
        }

        public void Clear() {
            if (container != null) {
                container.Clear();
                container = null;
            }
        }
    }
}