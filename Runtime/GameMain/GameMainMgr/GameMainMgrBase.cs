using Momos.Core.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Momos.Tools.Interface;

namespace Momos.Tools.GameMain
{
    public abstract class GameMainMgrBase : MonoBehaviour
    {
        // 执行时刻是最早的一批Start()
        // 先执行这些协程初始化
        private Queue<IGameInit> initObjQue = new Queue<IGameInit>();
        // 然后执行所有的StartDo
        // 最后跟随UpdateDo
        private Queue<IMonoEvt> monoEvtQue = new Queue<IMonoEvt>();

        protected abstract void GetInitObjs(ref Queue<IGameInit> initObjQue);
        protected abstract void GetMonoEvts(ref Queue<IMonoEvt> monoEvtQue);

        protected virtual void PreStartDo() { }
        protected virtual void LastStartDo() { }

        private IEnumerator IE_RunGameStart(UnityAction callback)
        {
            while (initObjQue.Count > 0)
                yield return initObjQue.Dequeue().IE_Init();
            callback?.Invoke();
        }

        protected virtual void UpdateDo()
        {
            foreach (var evt in monoEvtQue)
                evt.UpdateDo();
        }

        private void Start()
        {
            // 获得处理对象
            GetInitObjs(ref initObjQue);
            GetMonoEvts(ref monoEvtQue);
            // 管理器自己要载入的事件
            PreStartDo();
            // 运行初始化
            StartCoroutine(IE_RunGameStart(() =>
            {
                foreach (var evt in monoEvtQue)
                    evt.StartDo();
                LastStartDo();
                // 将MonoUpdate事件交给Mono控制器
                MonoMgr.GetInstance().AddUpdateListener(UpdateDo);
            }));
        }


    }
}