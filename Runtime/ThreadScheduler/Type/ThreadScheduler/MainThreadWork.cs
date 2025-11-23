using System;

namespace Momos.Tools.ThreadTools
{
    public class MainThreadWork : WorkComponentBase<Action>
    {
        /// <summary> 添加'工作'到子线程队列 </summary>
        /// <returns></returns>
        public void EnqueEvent(Action evt)
        {
            if (evt != null)
                workQue.Enqueue(evt);
        }

        public override void Start() { }

        public override void UpdateWorkMainThread()
        {
            if (workQue.Count > 0)
                HandlerWorkFinish();
        }

        protected override void HandlerWorkFinish()
        {
            if (workQue.TryDequeue(out Action evt) && evt != null)
                evt.Invoke();
        }

        public override void OnDispose()
        {
            while (workQue.Count > 0)
                workQue.TryDequeue(out _);
        }
    }
}