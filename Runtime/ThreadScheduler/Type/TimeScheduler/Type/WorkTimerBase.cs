using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Momos.Tools.ThreadTools.Timer {
    public class WorkTimerBase : WorkThreadComponentBase<DelayWork> {
        private long tid;   // tid计数器
        private readonly DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        private readonly Queue<DelayWork> waitCallBackWorkQue = new Queue<DelayWork>();
        private readonly Queue<DelayWork> finishWorkQue = new Queue<DelayWork>();

        /// <summary> 
        /// 线程安全'延时工作'字典:
        ///     允许多个写入写出,无需锁或同步。
        /// </summary>
        protected readonly ConcurrentDictionary<long, DelayWork> delayWorkDic = new ConcurrentDictionary<long, DelayWork>();

        /// <summary> 请求一个'TaskID' </summary>
        protected long RequestID => tid++;
        /// <summary> 当前时间的时间戳 </summary>
        protected long NowTimeMs => (long)(DateTime.UtcNow - startTime).TotalMilliseconds;

        public WorkTimerBase() {
            this.tid = 0;
        }

        /// <summary> 主进程调用更新(仅允许主线程执行) </summary>
        public override void UpdateWorkMainThread() {
            // 主线程工作完成队列中是否存在已经完成的工作
            if (workQue.Count > 0) {
                workQue.TryDequeue(out DelayWork work);
                try {
                    if (!work.IsAsync) {
                        work.InvokeCallBack();
                    }
                    else // async任务: 设置为Completed
                    {
                        work.SetAsyncCancelTokenResult(true);
                    }

                    // 如果工作是完成的, 那么做一些'废弃工作'(仅预留,无逻辑)
                    if (work.IsCompleted)
                        work.OnDispose();
                }
                catch (Exception e) {
                    assistant.LogError(this.GetType(), e);
                }
            }
        }

        #region Task.Run 运行逻辑
        /// <summary> 工作(子线程执行) </summary>
        protected override void Do() {
            Update();
            base.Do();
            HandlerOnceLoopFinish();
        }
        #region Do Funciton
        /// <summary> 更新 </summary>
        private void Update() {
            long nowTimeMs = NowTimeMs;
            DelayWork work;
            foreach (var item in delayWorkDic) {
                work = item.Value;
                // 当前时间小于目标时间, 继续运行
                if (work.VerTimeMsIsRun(nowTimeMs)) {
                    continue;
                }
                work.IncreLoopIndex();
                work.ReduceLoopCount();
                if (work.IsCompleted)
                    finishWorkQue.Enqueue(work);
                else {
                    work.RefrushTargetTimeMs();
                    waitCallBackWorkQue.Enqueue(work);
                }
            }
        }
        /// <summary> 处理已经完成的工作(进行整个'工作循环'中的最后一次任务) </summary>
        protected override void HandlerWorkFinish() {
            // 完成工作的逻辑
            void ProcessFisishWork(DelayWork work) {
                delayWorkDic.TryRemove(work.Tid, out DelayWork dw);
                if (dw != null)
                    workQue.Enqueue(dw);
            }

            while (finishWorkQue.Count > 0) {
                ProcessFisishWork(finishWorkQue.Dequeue());
            }
        }
        /// <summary> 处理循环计时器完成的工作(进行整个'工作循环'中的一次任务) </summary>
        private void HandlerOnceLoopFinish() {
            while (waitCallBackWorkQue.Count > 0)
                workQue.Enqueue(waitCallBackWorkQue.Dequeue());
        }
        #endregion Do Funciton
        #endregion Task.Run 运行逻辑

    }
}