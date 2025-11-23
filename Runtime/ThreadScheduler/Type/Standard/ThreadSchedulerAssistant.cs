using Momos.Tools.ThreadTools.Timer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Momos.Tools.ThreadTools {
    public class ThreadSchedulerAssistant {
        public void Log(string message, int key) {
            Debug.Log(message);
        }

        public void LogWorkCancel(Type type, DelayWork delayWork, string message) {
            Debug.Log(message);
        }

        public void LogError(Type type, Exception e) {
            Debug.LogError(e);
        }
    }
}