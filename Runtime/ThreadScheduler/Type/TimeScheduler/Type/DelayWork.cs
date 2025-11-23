using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Momos.Tools.ThreadTools.Timer
{
    public class DelayWork
    {
        private readonly long tid;   // id
        private readonly long startTimeMs;   // 开始时间戳
        private long targetTimeMs;  // 结束时间戳
        private long delayTimeMs;   // 延迟时间        
        private Action callBack;// 完成回调
        private Action cancelCallBack;  // 取消回调
        private int loopCount;  // 循环次数: 倒计数,=0时结束运行
        private long loopIndex; // 运行循环次数: 正计数

        private readonly int hashCode;   // attachObj的哈希值
        private GameObject attachObj;   // 附加对象

        private TaskCompletionSource<bool> asyncTaskCompletedToken;    // '异步取消'令牌

        public long Tid => tid;
        public int HashCode => hashCode;

        public bool IsCompleted => loopCount <= 0;
        public bool IsAsync => asyncTaskCompletedToken != null;
        /// <summary> 
        /// 是运行的'渲染层'工作:
        ///     1. 附加物体(<see cref="attachObj"/>)不为空
        ///     2. 处于激活状态
        /// </summary>
        public bool IsRunRenderWork => attachObj != null && !ReferenceEquals(attachObj, null) && attachObj.activeSelf == true;

        /// <summary> 逻辑层的运行 </summary>
        public DelayWork(long tid, long startTimeMs, long delayTimeMs, Action callBack, Action cancelCallBack, int loopCount, TaskCompletionSource<bool> asyncTaskCompletedToken = null)
        {
            this.tid = tid;
            this.startTimeMs = startTimeMs;
            this.delayTimeMs = delayTimeMs;
            this.callBack = callBack;
            this.cancelCallBack = cancelCallBack;
            this.loopCount = loopCount;
            this.asyncTaskCompletedToken = asyncTaskCompletedToken;
            loopIndex = 0;
            RefrushTargetTimeMs();
        }

        /// <summary> 渲染层的运行 </summary>
        public DelayWork(GameObject obj, long tid, long startTimeMs, long delayTimeMs, Action callBack, Action cancelCallBack, int loopCount, TaskCompletionSource<bool> asyncTaskCompletedToken = null):
            this(tid,startTimeMs,delayTimeMs,callBack,cancelCallBack,loopCount,asyncTaskCompletedToken)
        {
            this.hashCode = obj == null ? 0 : obj.GetHashCode();
            this.attachObj = obj;
        }

        public bool VerTimeMsIsRun(long nowTimeMs) => nowTimeMs < targetTimeMs;
        public void IncreLoopIndex() => loopIndex++;
        public void ReduceLoopCount() => loopCount--;
        public void RefrushTargetTimeMs() => this.targetTimeMs = startTimeMs + delayTimeMs * (loopIndex + 1);
        public void InvokeCallBack() => callBack?.Invoke();
        public void InvokeCancelCallBack() => cancelCallBack?.Invoke();
        public void SetAsyncCancelTokenResult(bool result) => asyncTaskCompletedToken?.SetResult(result);

        public void OnDispose()
        { 
            
        }
    }
}