using Momos.Core.Apps.Flags;
using System.Collections.Concurrent;
using System.Threading;

namespace Momos.Core.ThreadTools {
    public interface IThreadUnitRequest {
        public void Execute();
    }

    public class ThreadProcessUnit<U> where U : IThreadUnitRequest {
        public static bool IsRunning => ThreadFlag.IsRunning;

        readonly ConcurrentQueue<U> queue = new();
        readonly Thread workerThread;

        public int Count => queue.Count;
        public long PCount { get; private set; }

        public ThreadProcessUnit() {
            workerThread = new Thread(WorkerLoop) {
                IsBackground = true
            };
            workerThread.Start();
            PCount = 0;
        }

        public void Enqueue(U unit) => queue.Enqueue(unit);

        void WorkerLoop() {
            while (IsRunning) {
                while (queue.TryDequeue(out var req)) {
                    req.Execute();
                    PCount++;
                }
                Thread.Sleep(1);
            }
        }
    }
}