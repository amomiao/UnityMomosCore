using Momos.Core.Event;
using UnityEngine;
using UnityEngine.Events;

namespace Momos.Core.Logical {
    public class LogicMgr : BaseManager<LogicMgr> {
        private int logicSpanMs = 66;
        private long accountTime = 0;
        private long nextFrameTime;
        private UnityAction action;

        public bool IsStop { get; set; }
        /// <summary> 累计帧数 </summary>
        public long AccountFrame { get; private set; }
        /// <summary> 逻辑帧间隔(毫秒) </summary>
        public int LogicSpanMs { get; private set; }
        /// <summary> 逻辑帧间隔(秒) </summary>
        public float DeltaTimeSec { get; private set; }
        /// <summary> Mono时间在两个逻辑帧之间的补间(Tweens)插值进度t </summary>
        public float RenderT { get; private set; }

        public LogicMgr() {
            // 设置初始值
            accountTime = 0;
            IsStop = false;
            AccountFrame = 0;
            LogicSpanMs = logicSpanMs;
            DeltaTimeSec = logicSpanMs / 1000f;
            // 根据初始值计算
            UpdateNextFrameTime();
            // 进行逻辑
            MonoMgr.GetInstance().AddUpdateListener(() => {
                TryUpdate(Time.deltaTime);
            });
        }

        /// <summary> 给外部提供的 添加帧更新事件的函数 </summary>
        /// <param name="fun"> 给一个帧间隔时间(秒)的参数的方法 </param>
        public void AddLogicUpdateListener(UnityAction fun) {
            action += fun;
        }

        /// <summary> 提供给外部 用于移除帧更新事件函数 </summary>
        /// <param name="fun"> 给一个帧间隔时间(秒)的参数的方法 </param>
        public void RemoveLogicUpdateListener(UnityAction fun) {
            action -= fun;
        }

        /// <summary> 每渲染帧尝试触发一次逻辑帧 </summary>
        public bool TryUpdate(float increTimeSec) {
            if (IsStop)
                return false;

            accountTime += Mathf.RoundToInt(increTimeSec * 1000);
            if (accountTime >= nextFrameTime) {
                LogicUpdate();
            }
            // 更新渲染帧补间插值
            RenderT = accountTime % LogicSpanMs / (float)LogicSpanMs;
            return false;
        }

        /// <summary> 更新下一逻辑帧时间 </summary>
        private void UpdateNextFrameTime() {
            this.nextFrameTime = (AccountFrame + 1) * logicSpanMs;
        }

        /// <summary> 触发逻辑帧更新事件 </summary>
        private void LogicUpdate() {
            AccountFrame++; // 先加再执行事件
            action?.Invoke();
            UpdateNextFrameTime();
        }
    }
}