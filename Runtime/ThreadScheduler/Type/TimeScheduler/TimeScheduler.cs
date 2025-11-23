using Momos.Core;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Momos.Tools.ThreadTools.Timer
{
    /// <summary> 工作调度 </summary>
    public class TimeScheduler : SingletonAutoMono<TimeScheduler>,ISingletonMono
    {
        public bool IsDontDestroy => false;
        public LogicWorkTimer LogicTimer { get; private set; }
        public RenderWorkTimer RenderTimer { get; private set; }

        #region 生命周期方法
        private void Awake()
        {
            Init();    
        }

        private void Init()
        { 
            LogicTimer = new LogicWorkTimer();
            RenderTimer = new RenderWorkTimer();
            LogicTimer.Start();
            RenderTimer.Start();
        }

        private void Update()
        {
            // base.OnUpdate();
            LogicTimer?.UpdateWorkMainThread();
            RenderTimer?.UpdateWorkMainThread();
        }

        private void OnDestroy()
        {
            LogicTimer?.OnDispose();
            RenderTimer?.OnDispose();
        }
        #endregion 生命周期方法

        // 对于'延时一帧':
        // 没有cancelCallBack, 没有返回值, 因为一帧时间内的取消是不合理的。
        // 没有loopCount, 因为'延时'被设置为了一帧, 如果允许计数将会是每帧执行。
        /// <summary> (逻辑层)'延时1帧'执行(20ms=>60fps) </summary>
        public void DelayFrameDo(Action callBack) => DelayDo(20, callBack, null);
        /// <summary> (逻辑层)'延时1帧'async执行(20ms=>60fps) </summary>
        public async Task<bool> AsyncFrameDo() => await AsyncDo(20,new TaskCompletionSource<bool>());
        /// <summary> (渲染层)'延时1帧'执行(20ms=>60fps) </summary>
        public void DelayFrameDo(GameObject obj, Action callBack) => DelayDo(obj, 20, callBack, null);

        /// <summary> (逻辑层)延时执行 </summary>
        /// <param name="delayTimeMs"> 延时(ms) </param>
        /// <param name="callBack"> 完成回调(callback) </param>
        /// <param name="cancelCallBack"> 取消回调(cancel callback) </param>
        /// <param name="loopCount"> 循环次数 </param>
        /// <returns> 返回任务的<see cref="DelayWork.tid"/> </returns>
        public long DelayDo(uint delayTimeMs, Action callBack, Action cancelCallBack = null, int loopCount = 1)
        {
            return LogicTimer.AddDelay(delayTimeMs,callBack,cancelCallBack,loopCount);
        }

        /// <summary> (逻辑层)async延时执行 </summary>
        public async Task<bool> AsyncDo(uint delayTimeMs, TaskCompletionSource<bool> taskCompletionSource = null)
        { 
            taskCompletionSource ??= new TaskCompletionSource<bool>();
            return await LogicTimer.AddAsync(delayTimeMs,taskCompletionSource);
        }

        /// <summary> (渲染层)延时执行 </summary>
        /// <param name="obj"> 附加到的GameObject </param>
        /// <param name="delayTimeMs"> 延时(ms) </param>
        /// <param name="callBack"> 完成回调(callback) </param>
        /// <param name="cancelCallBack"> 取消回调(cancel callback) </param>
        /// <param name="loopCount"> 循环次数 </param>
        /// <returns> 返回任务的<see cref="DelayWork.tid"/> </returns>
        public long DelayDo(GameObject obj, uint delayTimeMs, Action callBack, Action cancelCallBack, int loopCount = 1)
        {
            return RenderTimer.AddDelay(obj, delayTimeMs, callBack, cancelCallBack, loopCount);
        }

        /// <summary> (逻辑层)移除一个'延时执行' </summary>
        /// <param name="tid"> 给予<see cref="DelayWork.tid"/> </param>
        /// <returns> 成功移除返回true </returns>
        public bool CancelDelayDo(long tid) => LogicTimer.RemoveDelay(tid);

        /// <summary> (逻辑层)移除一个'async延时执行' </summary>
        /// <returns> 成功移除返回true </returns>
        public void CancelAsyncDo(TaskCompletionSource<bool> taskCompletionSource) => taskCompletionSource.SetResult(false);

        /// <summary> (渲染层)移除一个'延时执行'在对应物体 </summary>
        public void CancelDelayDo(GameObject obj, long tid) => RenderTimer.RemoveObjBindTimer(obj, tid);

        /// <summary> (渲染层)移除一个物体上所有的'延时执行' </summary>
        public void CancelObjDelayDo(GameObject obj) => RenderTimer.RemoveObjDelay(obj, true);
        
    }
}