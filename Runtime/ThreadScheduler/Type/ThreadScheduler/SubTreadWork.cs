using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Momos.Tools.ThreadTools
{
    public class ThreadWorkItem
    {
        private Action callBack;
        private TaskCompletionSource<int> awaitTask;

        public bool IsNotNullCallBack => callBack != null;

        public ThreadWorkItem(Action callBack, TaskCompletionSource<int> taskCompletion)
        {
            this.callBack = callBack;
            awaitTask = taskCompletion;
        }

        public void InvokeCallBack()
        {
            callBack?.Invoke();
            awaitTask.SetResult(1);
        }
    }

    public class SubTreadWork : WorkThreadComponentBase<ThreadWorkItem>
    {
        /// <summary> 添加'工作'到子线程队列 </summary>
        /// <returns></returns>
        public Task<int> EnqueEvent(Action evt)
        {
            TaskCompletionSource<int> taskCompletion = new TaskCompletionSource<int>();
            workQue.Enqueue(new ThreadWorkItem(evt, taskCompletion));
            return taskCompletion.Task;
        }

        /// <summary> 不在主线程做任何事 </summary>
        public override void UpdateWorkMainThread() { }

        #region Task.Run 运行逻辑
        /// <summary> 工作(子线程执行) </summary>
        protected override void Do()
        {
            while (workQue.Count > 0)
            {
                base.Do();
            }
        }
        /// <summary> 处理已经完成的工作(进行整个'工作循环'中的最后一次任务) </summary>
        protected override void HandlerWorkFinish()
        {
            if (workQue.TryDequeue(out ThreadWorkItem item))
            {
                if (item != null && item.IsNotNullCallBack)
                    item.InvokeCallBack();
            }
        }
        #endregion Task.Run 运行逻辑

        public override void OnDispose()
        {
            base.OnDispose();
            while (workQue.Count > 0)
                workQue.TryDequeue(out _);
        }
    }
}