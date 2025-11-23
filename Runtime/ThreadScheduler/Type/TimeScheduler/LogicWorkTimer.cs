using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Momos.Tools.ThreadTools.Timer
{
    /// <summary> 逻辑层计时器 </summary>
    public class LogicWorkTimer : WorkTimerBase
    {
        /// <summary> 延时执行 </summary>
        /// <param name="delayMs"> 延时(ms) </param>
        /// <param name="cb"> 完成回调(callback) </param>
        /// <param name="ccb"> 取消回调(cancel callback) </param>
        /// <param name="loopCount"> 循环次数 </param>
        /// <returns> 返回任务的<see cref="DelayWork.tid"/> </returns>
        public long AddDelay(uint delayMs, Action cb, Action ccb, int loopCount = 1)
        {
            DelayWork dw = new DelayWork(RequestID, NowTimeMs, delayMs, cb, ccb, loopCount);
            delayWorkDic.TryAdd(dw.Tid, dw);
            return dw.Tid;
        }

        /// <summary> 添加一个'async延迟工作' </summary>
        /// <param name="delayMs"></param>
        /// <param name="taskCompletionSource"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(uint delayMs,TaskCompletionSource<bool> taskCompletionSource)
        {
            DelayWork dw = new DelayWork(RequestID, NowTimeMs, delayMs, null, null, 1, taskCompletionSource);
            delayWorkDic.TryAdd(dw.Tid, dw);
            // 等待异步任务结果: 任意时刻调用
            /// <see cref="TaskCompletionSource.SetResult(TResult)"/>、
            /// <see cref="TaskCompletionSource.SetCanceled"/>、
            /// <see cref="TaskCompletionSource.SetException(Exception)"/>
            /// 都会使得状态发生变化 'await'结束
            await taskCompletionSource.Task;
            // 任务完成 后处理
            if (!taskCompletionSource.Task.Result)  // 中途被取消, 移除计时器
                RemoveDelay(dw.Tid);
            return true;
        }

        /// <summary> 移除一个'延时执行' </summary>
        /// <param name="tid"> 给予<see cref="DelayWork.tid"/> </param>
        /// <returns> 成功移除返回true </returns>
        public bool RemoveDelay(long tid)
        {
            delayWorkDic.TryRemove(tid, out DelayWork dw);
            if (dw != null)
            {
                assistant.LogWorkCancel(GetType(), dw, "逻辑层延迟任务取消");
                dw.InvokeCancelCallBack();
                dw.OnDispose();
                return true;
            }
            return false;
        }
    }
}