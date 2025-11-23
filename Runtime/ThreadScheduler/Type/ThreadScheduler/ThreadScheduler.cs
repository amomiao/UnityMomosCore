using Momos.Core;
using System;
using System.Threading.Tasks;

namespace Momos.Tools.ThreadTools
{
    public class ThreadScheduler : SingletonAutoMono<ThreadScheduler>,ISingletonMono
    {
        public MainThreadWork MainWork { get; private set; }
        public SubTreadWork SubWork { get; private set; }
        public bool IsDontDestroy => false;

        #region 生命周期方法
        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            MainWork = new MainThreadWork();
            SubWork = new SubTreadWork();
            MainWork.Start();
            SubWork.Start();
        }

        private void Update()
        {
            // base.OnUpdate();
            MainWork?.UpdateWorkMainThread();
            SubWork?.UpdateWorkMainThread();
        }

        private void OnDestroy()
        {
            MainWork?.OnDispose();
            SubWork?.OnDispose();
        }

        #endregion 生命周期方法

        /// <summary> 在'主线程'运行作业 </summary>
        public void MainRun(Action evt)
        {
            MainWork.EnqueEvent(evt);
        }

        /// <summary> 在'子线程'运行作业 </summary>
        public async void SubRunAsync(Action evt)
        {
            await SubRun(evt);
        }

        /// <summary> 在'子线程'运行作业 </summary>
        public Task<int> SubRun(Action evt)
        {
            return SubWork.EnqueEvent(evt);
        }
    }
}