using System.Threading;
using System.Threading.Tasks;

namespace Momos.Tools.ThreadTools
{
    public abstract class WorkThreadComponentBase<T> : WorkComponentBase<T> where T : class
    {
        /// <summary> 
        /// 管理线程的'取消令牌',
        /// '取消令牌'是<see cref="CancellationTokenSource.Token"/>
        /// </summary>
        protected CancellationTokenSource cts;

        public override void Start()
        {
            cts = new CancellationTokenSource();
            Task task = Task.Run(() => UpdateWorkSubThread(cts.Token));
        }

        /// <summary> 子线程更新 </summary>
        protected virtual void UpdateWorkSubThread(CancellationToken token)
        {
            // 非'取消'则运行
            while (!token.IsCancellationRequested)
            {
                // 更新子线程
                Do();
                Task.Delay(20, token).Wait(token);
            }
            cts.Cancel();
        }

        /// <summary> 工作(子线程执行) </summary>
        protected virtual void Do()
        {
            HandlerWorkFinish();
        }

        public override void OnDispose()
        {
            cts?.Cancel();
        }
    }
}