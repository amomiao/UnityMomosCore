API: 
	1.Class ThreadScheduler: MonoAutoSingletion
	// 源于父类
	GetInstance(): 获得单例
	// 成员
	public void MainRun(Action evt): 在'主线程'运行作业
	public async void SubRunAsync(Action evt): 在'子线程'运行作业
	public Task<int> SubRun(Action evt): 在'子线程'运行作业

	2.Class TimeScheduler: MonoAutoSingletion
	// 源于父类
	GetInstance(): 获得单例
	// 成员
	// 延时一帧: 只是延迟一帧, 无取消操作
	public void DelayFrameDo(Action callBack)
	public async Task<bool> AsyncFrameDo()
	public void DelayFrameDo(GameObject obj, Action callBack)
	// 延时: 
	public long DelayDo(uint delayTimeMs, Action callBack, Action cancelCallBack, int loopCount = 1)
	public async Task<bool> AsyncDo(uint delayTimeMs, TaskCompletionSource<bool> taskCompletionSource = null)
	public long DelayDo(GameObject obj, uint delayTimeMs, Action callBack, Action cancelCallBack, int loopCount = 1)
	// 取消延时
	public bool CancelDelayDo(long tid)
	public void CancelAsyncDo(TaskCompletionSource<bool> taskCompletionSource)
	public void CancelDelayDo(GameObject obj, long tid)
	public void CancelObjDelayDo(GameObject obj)

	3.TimeSchedulerExpansion拓展方法
	public static void DelayFrameDo(this GameObject obj, Action callBack)
	public static long DelayDo(this GameObject obj, uint delayTimeMs, Action callBack, Action cancelCallBack, int loopCount = 1)
	public static void CancelDelayDo(this GameObject obj)

	4.ThreadSchedulerAssistant: 负责日志输出

Type:
	1.WorkComponentBase 作业逻辑的最基类
		MainThreadWork
		WorkThreadComponentBase
	2.WorkThreadComponentBase 线程逻辑的最基类
		SubTreadWork
		WorkTimerBase	
	3.WorkTimerBase 计时线程逻辑的基类
		LogicWorkTimer
		RenderWorkTimer