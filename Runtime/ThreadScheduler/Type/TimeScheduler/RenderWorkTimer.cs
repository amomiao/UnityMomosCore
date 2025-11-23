using System;
using System.Collections.Generic;
using UnityEngine;

namespace Momos.Tools.ThreadTools.Timer
{
    /// <summary> 渲染层计时器 </summary>
    public class RenderWorkTimer : WorkTimerBase
    {
        /// <summary>
        /// key: GameObject的HashCode
        /// Value: DelayWork的tid
        /// </summary>
        protected Dictionary<int,List<long>> objHashDic = new Dictionary<int, List<long>>();
        // private int listInitCapacity = 5;

        /// <summary> 延时执行 </summary>
        /// <param name="obj"> 绑定对象 </param>
        /// <param name="delayMs"> 延时(ms) </param>
        /// <param name="cb"> 完成回调(callback) </param>
        /// <param name="ccb"> 取消回调(cancel callback) </param>
        /// <param name="loopCount"> 循环次数 </param>
        public long AddDelay(GameObject obj, uint delayMs, Action cb, Action ccb, int loopCount = 1)
        {
            DelayWork dw = new DelayWork(obj, RequestID, NowTimeMs, delayMs, cb, ccb, loopCount);
            delayWorkDic.TryAdd(dw.Tid, dw);
            int hashCode = dw.HashCode;
            if (!objHashDic.ContainsKey(hashCode))
                objHashDic.Add(hashCode, new List<long>());
            objHashDic[hashCode].Add(dw.Tid);
            return dw.Tid;
        }

        /// <summary> 移除指定'对象'身上的指定'延迟工作' </summary>
        public void RemoveObjBindTimer(GameObject obj, long tid)
        { 
            if(obj != null)
                RemoveObjBindTimer(obj.GetHashCode(), tid);
        }
        /// <summary> 移除指定'对象'身上的指定'延迟工作' </summary>
        private void RemoveObjBindTimer(int hashCode, long tid)
        {
            if (objHashDic.TryGetValue(hashCode, out List<long> list))
            {
                if (list.Count > 0) // 移除Value的一部分
                {
                    list.Remove(tid);
                    delayWorkDic.TryRemove(tid, out _);
                }
                if (list.Count == 0)    // 移除KeyValuePair
                    objHashDic.Remove(hashCode);
            }
        }

        /// <summary> 移除一个物体上对应的所有'延时执行' </summary>
        /// <param name="obj"> 通过GameObject获得hashCode </param>
        public void RemoveObjDelay(GameObject obj, bool isCancelCallback = true)
        {
            if (obj != null)
                RemoveObjDelay(obj.GetHashCode(), isCancelCallback);
        }
        /// <summary> 移除一个物体上对应的所有'延时执行' </summary>
        /// <param name="hashCode"> 通过GameObject获得hashCode </param>
        /// <param name="isCancelCallback"> 是否执行'取消回调' </param>
        private void RemoveObjDelay(int hashCode,bool isCancelCallback)
        { 
            if (objHashDic.TryGetValue(hashCode, out List<long> list))
            {
                foreach (long tid in list)
                {
                    // 移除工作
                    if (delayWorkDic.TryRemove(tid, out DelayWork dw) && isCancelCallback)
                        dw?.InvokeCancelCallBack();
                }
                list.Clear();
                objHashDic.Remove(hashCode);
            }
        }

        public override void UpdateWorkMainThread()
        {
            if (workQue.Count == 0)
                return;
            if (workQue.TryDequeue(out DelayWork dw))
            {
                // 1.计时器绑定对象 已经被销毁
                // 2.计时器绑定对象 已经被隐藏(selfAction == false)
                if (!dw.IsRunRenderWork)
                {
                    assistant.LogWorkCancel(GetType(), dw, "渲染层延迟任务取消,因为物体已被摧毁或隐藏");
                    RemoveObjDelay(dw.HashCode, false);
                    return;
                }
                try
                {
                    dw.InvokeCallBack();
                    if (dw.IsCompleted)
                        RemoveObjBindTimer(dw.HashCode,dw.Tid);
                }
                catch (Exception e)
                {
                    assistant.LogError(this.GetType(), e);
                }
            }
        }
    }
}