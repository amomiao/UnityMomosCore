using Momos.Tools.ThreadTools.Timer;
using System.Collections.Concurrent;
using UnityEngine;

namespace Momos.Tools.ThreadTools
{
    public abstract class WorkComponentBase<T> : IThreador where T : class
    {
        /// <summary> 
        /// 线程安全'工作'队列: 记录运行中的工作
        ///     允许多个写入写出,无需锁或同步。
        /// </summary>
        protected readonly ConcurrentQueue<T> workQue = new ConcurrentQueue<T>();
        /// <summary> 工具助手 </summary>
        protected ThreadSchedulerAssistant assistant = new ThreadSchedulerAssistant();

        /// <summary> 线程启动 </summary>
        public abstract void Start();

        /// <summary> 交给主线程去运行的逻辑 </summary>
        public abstract void UpdateWorkMainThread();

        /// <summary> 处理已经完成的工作, 必须包含移除逻辑, 不移除重开一个新方法。 </summary>
        protected abstract void HandlerWorkFinish();

        /// <summary> 释放线程, 详细注释在接口里 </summary>
        public abstract void OnDispose();
    }
}