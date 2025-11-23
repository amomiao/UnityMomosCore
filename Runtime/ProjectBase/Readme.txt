提供了一个Project/Asset:
	[CreateAssetMenu(fileName = "ScriptObjectSet", menuName = "CustomScriptObject/ScriptObjectSet", order = 0)]

Base: 单例基类
	BaseManager<T> 纯数据类T。
	SingletonAutoMono<T> 创建一个物体挂载组件T 作为单例，并且过场景不移除。
	SingletonMono<T> 手动挂载在物体上的组件T 作为单例。
	命名空间: Momos.Core
	单例模式基类,使用GetInstance()得到单例对象。

EventCenter: 事件中心
	EventCenter 事件中心, 接收任意枚举的任意枚举值,作为事件的Key。
		返回枚举是否有事件: 
			public bool IsExistEvent(Enum eventKeyEnum)
		添加事件监听: 无参/一参/二参重载
			public void AddEventListener(Enum key, UnityAction action)
			public void AddEventListener<T>(Enum key, UnityAction<T> action)
			public void AddEventListener<T1,T2>(Enum key, UnityAction<T1,T2> action)
		移除事件监听: 无参/一参/二参重载
			public void RemoveEventListener(Enum key, UnityAction action)
			public void RemoveEventListener<T>(Enum key, UnityAction<T> action)
			public void RemoveEventListener<T1,T2>(Enum key, UnityAction<T1,T2> action)
		事件触发: 无参/一参/二参重载
			public void EventTrigger(Enum key)
			public void EventTrigger<T>(Enum key, T info)
			public void EventTrigger<T1, T2>(Enum key, T1 info1, T2 info2)
		清除所有事件:
			public void Clear()
***	EventEnumSet 事件枚举集,可以被改动。
	命名空间: Momos.Core.Event

Input: 输入
	InputAreaMgr 输入域控制,输入在某一域内时才能触发。
		设置area域是否激活: 
			public void SetAreaActive(E_InputArea area, bool value)
		验证参数域是否全激活: 
			public bool VerAreaIsActive(params E_InputArea[] areas)
	InputInfo 键入数据信息
	InputMgr 键入管理器
		开启/关闭运行: 
			public void IsRun(bool isStart)
		添加键盘键入事件: // 会注册事件
			public void AddKeyEvent(...)
		添加鼠标键入事件: 
			public void AddMouseEvent(...) 
		更改事件对应键盘位: // 不会注册事件
			public void ChangeKeyboardInfo(...)	
		更改事件对应鼠标位: 
			public void ChangeMouseInfo(...)
		移除事件: 
			public void RemoveInputInfopublic void RemoveInputInfo(Enum eventEnum)
		输出绑定信息: 
			public override string ToString()
		下一次发生键入事件时,得到'InputInfo'执行回调: 
			public void GetInputInfo(UnityAction<InputInfo> callBack)
	命名空间: Momos.Core.Event

Mono: 运行时
	MonoController 一个继承了Mono的组件，作为统一运行时的载体。
	MonoMgr 统一运行时管理器
		构造函数创建一个物体挂载MonoController
			public MonoMgr()
		添加统一Update事件
			public void AddUpdateListener(UnityAction fun)
		移除统一Update事件
			public void RemoveUpdateListener(UnityAction fun)
		启动协程
			public Coroutine StartCoroutine(IEnumerator routine)
			public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
			public Coroutine StartCoroutine(string methodName)
		停止协程
			public void StopCoroutine(IEnumerator routine)
			public void StopCoroutine(IEnumerator routine)
			public void StopCoroutine(string methodName)
		停止MonoController上的所有协程
			public void StopAllCoroutines()
	命名空间: Momos.Core.Event

Music: 音乐
	MusicMgr 音乐(背景音乐/音效)管理器
		播放背景音乐: 背景音乐需要放在 'Resources/Music/BK/'下
			public void PlayBkMusic(string name)
		暂停背景音乐
			public void PauseBKMusic()
		停止背景音乐
			public void StopBKMusic()
		调整背景音乐音量
			public void ChangeBKValue(float v)
		播放音效
			public void PlaySound(string name, bool isLoop, bool immediate = false, UnityAction<AudioSource> callBack = null)
		改变音效声音大小
			public void ChangeSoundValue(float value)
		停止音效(参数)
			public void StopSound(AudioSource source)
		清除音效
			public void ClearSound()
	命名空间: Momos.Core.Event

Pool: 对象池
	PoolMgr GameObject对象池管理器,存入的都是GameObject.
		获取,如果没有会使用name从Resources中加载。
			public void GetObj(string name, UnityAction<GameObject> callBack)
		存入
			public void PushObj(string name, GameObject obj)
		清除
			 public void Clear()
	命名空间: Momos.Core.Pool

Res: 资源加载
	EditorResMgr 编辑器资源读取,声明一个rootPath,读入以"Assets/GameData/"为根目录。
		加载单个资源
			public T LoadEditorRes<T>(string path)
		加载单个ScriptableObject资源
			public T LoadScriptableObject<T>(string path)
		加载一个文件夹下的全部 某种ScriptableObject资源
			public T[] LoadScriptableObjects<T>(string path)
		加载精灵
			public Sprite LoadSprite(string path, string spriteName)
		加载图集所有图片
			public Dictionary<string, Sprite> LoadSprites(string path)
		将字符串数据写入txt存入StreamingAssets文件夹
			public void String2TxtInStreamingAssets(string name, string content, string prefixPath = "")
		将StreamingAssets文件夹中的txt读入String
			public string Txt2StringInStreamingAssets(string name, string prefixPath = "")
	ResMgr 从Resources加载资源
		同步加载
			public T Load<T>(string name)
		同步加载并进行预处理
			public T LoadInstance<T>(string name)
		协程异步加载
			public void LoadAsync<T>(string name, UnityAction<T> callback)
		协程异步加载并进行预处理
			public void LoadInstanceAsync<T>(string name, UnityAction<T> callback)
	ScriptObjectAssetMgr 加载可编程物体
		构造函数加载一个可编程物体集合, 读取都是从此集合中进行。
			public ScriptObjectAssetMgr()
		使用泛型加载: 逻辑中指定加载类型时使用
			public T Load<T>(string name = "")
		使用类型加载: 逻辑中可能加载任意可编程物体时使用
			public ScriptableObject Load(Type type, string name = "")
	ScriptObjectSetAsset 一个可编程物体集合
	命名空间: Momos.Core.Asset

Scene 场景管理
	SceneMgr
		同步切换场景
        	public void LoadScene(string name, UnityAction callBack = null)
        异步切换场景: 调用EventEnumSet.E_GameInit.UpdateSceneLoading_float 事件获得加载进度
        	public void LoadSceneAsyn(string name, UnityAction callBack = null)
	命名空间: Momos.Core.Event

UI 管理UI,极小项目可用,否则使用ZUI
	BasePanel 面板基类
		展示
			public virtual void ShowMe()
		隐藏
			public virtual void HideMe()
		获取控件
			public T GetControl<T>(string controlName)
		按钮事件 [pro v]: 按钮的统一事件,需要判断名称
			protected virtual void OnClick(string btnName)
		开关事件 [pro v]: 开关的统一事件,需要判断名称
			protected virtual void OnValueChanged(string toggleName, bool value)
	UIManager 管理UI
		添加自定义事件监听
			public static void AddCustomEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callBack)
		构造函数: 获取画布物体'Canvas,EventSystem'或加载'UI/Canvas,UI/EventSystem',并获得'Bot/Mid/Top/System'四层
			public UIManager()
		得到层:
			public Transform GetLayerFather(E_UI_Layer layer)
		展示面板: 获得或从'UI/{name}'加载
			public void ShowPanel<T>(string panelName, E_UI_Layer layer = E_UI_Layer.Mid, UnityAction<T> callBack = null)
		隐藏面板:
			public void HidePanel(string panelName)
		得到面板: 会null
			public T GetPanel<T>(string name)
	命名空间: Momos.Core.UI

UWQ UnityWWWRequest
	UWQResMgr
		利用UnityWebRequest去加载资源：类型只能是string、byte[]、Texture、AssetBundle,要自己加上协议 http、ftp、file.
			public void LoadRes<T>(string path, UnityAction<T> callBack, UnityAction failCallBack)
	命名空间: Momos.Core.Net

ProjectFrame 项目框架类
***	初始化了一些框架类,可以作为程序最入口。