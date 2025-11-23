namespace Momos.Core.Logical
{
    /// <summary> 拓展方法指向逻辑帧运行时 </summary>
    public interface ILogicRuntime { }

    public static class ILogicRuntimeExpansion
    {
        public static long GetAccountFrame(this ILogicRuntime obj) => LogicMgr.GetInstance().AccountFrame;
        public static float GetRenderT(this ILogicRuntime obj) => LogicMgr.GetInstance().RenderT;
        public static int GetFrameSpanTimeMS(this ILogicRuntime obj) => LogicMgr.GetInstance().LogicSpanMs;
        public static float GetDeltaTimeSec(this ILogicRuntime obj) => LogicMgr.GetInstance().DeltaTimeSec;
    }
}