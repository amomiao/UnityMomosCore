using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace Momos.Core.Event
{
    /// <summary>
    /// 1.可以提供给外部添加帧更新事件的方法
    /// 2.可以提供给外部添加 协程的方法
    /// </summary>
    public class MonoMgr : BaseManager<MonoMgr>
    {
        private static GameObject ControllerObj;
        private MonoController controller;

        public MonoMgr()
        {
            // 保证场景上只有一个MonoObj
            if (ControllerObj == null)
            {
                ControllerObj = new GameObject("MonoController");
                // GameObject.Destroy(ControllerObj);
            }
            if (ControllerObj.GetComponent<MonoController>() != null)
                GameObject.Destroy(ControllerObj.GetComponent<MonoController>());
            // 保证场景上只有一个MonoControllerComponent
            controller = ControllerObj.AddComponent<MonoController>();
        }

        /// <summary> 给外部提供的 添加帧更新事件的函数 </summary>
        /// <param name="fun"></param>
        public void AddUpdateListener(UnityAction fun)
        {
            controller.AddUpdateListener(fun);
        }

        /// <summary> 提供给外部 用于移除帧更新事件函数 </summary>
        /// <param name="fun"></param>
        public void RemoveUpdateListener(UnityAction fun)
        {
            controller.RemoveUpdateListener(fun);
        }

        /// <summary> 提供给外部 在统一的MonoMgr脚本上开协程 </summary>
        /// <param name="routine"></param>
        /// <returns></returns>
        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return controller.StartCoroutine(routine);
        }

        /// <summary> 提供给外部 在统一的MonoMgr脚本上开协程 </summary>
        /// <param name="methodName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
        {
            return controller.StartCoroutine(methodName, value);
        }

        /// <summary> 提供给外部 在统一的MonoMgr脚本上开协程 </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public Coroutine StartCoroutine(string methodName)
        {
            return controller.StartCoroutine(methodName);
        }

        /// <summary> 提供给外部 在统一的MonoMgr脚本上关协程 </summary>
        /// <param name="methodName"></param>
        public void StopCoroutine(IEnumerator routine)
        {
            controller.StopCoroutine(routine);
        }

        /// <summary> 提供给外部 在统一的MonoMgr脚本上关协程 </summary>
        /// <param name="methodName"></param>
        public void StopCoroutine(Coroutine routine)
        {
            controller.StopCoroutine(routine);
        }

        /// <summary> 提供给外部 在统一的MonoMgr脚本上关协程 </summary>
        /// <param name="methodName"></param>
        public void StopCoroutine(string methodName)
        {
            controller.StopCoroutine(methodName);
        }

        /// <summary> 提供给外部 在统一的MonoMgr脚本上关闭_所有_协程 </summary>
        public void StopAllCoroutines()
        {
            controller.StopAllCoroutines();
        }

    }
}