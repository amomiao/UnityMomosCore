进入Core的原则:
1.脚本只读:除了对于所有项目适用的优化修改，不允许改写脚本去单独适应一个项目。
2.一次写: 若有写入操作,只在项目创建时触发一次。

/Editor: 待废弃,不要往里面加东
/Independent:'独立工具' 放置仅单向依赖Unity的程序集
/IndependentEditor:唯一'独立工具'允许引用的程序集,放置一些编辑器相关类型。
	勾选了"AutoReferenced", 允许全局的引用。
	主程序集的编译会受到'IndependentEditor'的影响, 而不会受到其他'Independent'的影响。
/Runtime: 运行时相关逻辑

// 以下内容自由修改:
// Editor下文件夹

// Independent下文件夹
PackManager 包管理

// Runtime下文件夹
ProjectBase 框架
ThreadScheduler 线程管理器