using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Momos.Tools.ThreadTools.Timer
{
    public static class TimeSchedulerExpansion 
    {
        public static void DelayFrameDo(this GameObject obj, Action callBack)
            => TimeScheduler.GetInstance().DelayFrameDo(obj, callBack);

        public static long DelayDo(this GameObject obj, uint delayTimeMs, Action callBack, Action cancelCallBack, int loopCount = 1)
            => TimeScheduler.GetInstance().DelayDo(obj, delayTimeMs, callBack, cancelCallBack, loopCount);

        public static void CancelDelayDo(this GameObject obj) => TimeScheduler.GetInstance().CancelObjDelayDo(obj);
    }
}