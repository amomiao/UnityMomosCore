using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Momos.Tools.ThreadTools.Timer
{
    public class DelayTimeTask
    {
        // isCompleted  isSuccess
        // false        false       未完成
        // false        true        [不存在的情况]
        // true         false       取消
        // true         true        完成

        private long tid;
        private bool isCompleted;
        private bool isSuccess;
        private int loopCount;
        private int invokeCount;

        public bool IsWaitOrRun => isCompleted == false && isSuccess == false;
        public bool IsSuccess => isSuccess;

        public DelayTimeTask(long tid, Action callBack, Action cancelCallBack, int loopCount)
        {
            this.tid = tid;
            isCompleted = false;
            isSuccess = false;
            this.loopCount = loopCount;
            this.invokeCount = 0;

            if (callBack == null)
                callBack = AddInvokeCount;
            else
                callBack += AddInvokeCount;
            
            if (cancelCallBack == null)
                cancelCallBack = SetCancel;
            else
                cancelCallBack += SetCancel;
        }

        public void Cancel()
        {
            if(IsWaitOrRun)
                TimeScheduler.GetInstance().CancelDelayDo(tid);
        }

        private void AddInvokeCount()
        {
            invokeCount++;
            if (invokeCount >= loopCount)
                SetCompleted();
        }

        private void SetCompleted()
        {
            isCompleted = true;
            isSuccess = true;
        }

        private void SetCancel()
        { 
            isCompleted = true;
            isSuccess = false;
        }
    }

    public class AsyncTimeTask
    {
        // isCompleted  isSuccess
        // false        false       未完成
        // false        true        [不存在的情况]
        // true         false       取消
        // true         true        完成

        private long tid;
        private bool isCompleted;
        private bool isSuccess;
        private int loopCount;
        private int invokeCount;

        public bool IsWaitOrRun => isCompleted == false && isSuccess == false;
        public bool IsSuccess => isSuccess;

        public AsyncTimeTask(long tid, Action callBack, Action cancelCallBack, int loopCount)
        {
            this.tid = tid;
            isCompleted = false;
            isSuccess = false;
            this.loopCount = loopCount;
            this.invokeCount = 0;

            if (callBack == null)
                callBack = AddInvokeCount;
            else
                callBack += AddInvokeCount;

            if (cancelCallBack == null)
                cancelCallBack = SetCancel;
            else
                cancelCallBack += SetCancel;
        }

        public void Cancel()
        {
            if (IsWaitOrRun)
                TimeScheduler.GetInstance().CancelDelayDo(tid);
        }

        private void AddInvokeCount()
        {
            invokeCount++;
            if (invokeCount >= loopCount)
                SetCompleted();
        }

        private void SetCompleted()
        {
            isCompleted = true;
            isSuccess = true;
        }

        private void SetCancel()
        {
            isCompleted = true;
            isSuccess = false;
        }
    }
}