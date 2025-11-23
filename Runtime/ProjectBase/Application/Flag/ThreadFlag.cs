using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Momos.Core.Apps.Flags {
    public static class ThreadFlag {
        public static volatile bool IsRunning = true;

        // 任意平台运行启动时调用
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init() {
            IsRunning = true;
            // 任意平台运行结束时调用
            Application.quitting += () => IsRunning = false;
        }
    }
}