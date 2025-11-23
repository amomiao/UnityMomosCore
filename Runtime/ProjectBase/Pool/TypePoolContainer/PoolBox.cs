using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Momos.Core.Pool {
    public abstract class PoolBoxBase<T> where T : class {
        public abstract int Count { get; }

        public void Input(T obj) {
            InputToContainer(obj);
        }

        public bool TryOutput(out T item) {
            item = null;
            if (Count > 0 && TryOutputFromContainer(out item)) {
                return true;
            }
            return false;
        }

        protected abstract void InputToContainer(T item);

        /// <returns> <paramref name="item"/>是否得到了赋值 </returns>
        protected abstract bool TryOutputFromContainer(out T item);
    }

    public class PoolBox<T> : PoolBoxBase<T> where T : class {
        Queue<T> container = new Queue<T>();
        public override int Count => container.Count;

        protected override void InputToContainer(T item) {
            container.Enqueue(item);
        }

        protected override bool TryOutputFromContainer(out T item) {
            item = container.Dequeue();
            return true;
        }
    }

    public class ThreadSafePoolBox<T> : PoolBoxBase<T> where T : class {
        ConcurrentQueue<T> container = new ConcurrentQueue<T>();
        public override int Count => container.Count;

        protected override void InputToContainer(T item) {
            container.Enqueue(item);
        }

        protected override bool TryOutputFromContainer(out T item) {
            return container.TryDequeue(out item);
        }
    }

}