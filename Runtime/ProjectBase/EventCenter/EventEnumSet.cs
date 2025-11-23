using UnityEngine;

namespace Momos.Core.Event
{
    public class EventEnumSet
    {
        public enum E_GameInit
        {
            /// <summary> 更新游戏初始化进度信息 </summary>
            UpdateGameInitProgressMessage,
            /// <summary> 更新游戏新场景进度 </summary>
            UpdateSceneLoading_float,
            /// <summary> 等待游戏加载完成后将一直运行的Mono Update事件 </summary>
            WaitInitEndAddMonoMgr_UAction
        }

        public enum E_InputEventKey
        {
            /// <summary> 水平热键 -1~1的事件监听 </summary>
            E_Input_Horizontal,
            /// <summary> 竖直热键 -1~1的事件监听 </summary>
            E_Input_Vertical,
            // 鼠标
            Mouse,
            // 数字键
            Alpha0,
            Alpha1,
            Alpha2,
            Alpha3,
            Alpha4,
            Alpha5,
            Alpha6,
            Alpha7,
            Alpha8,
            Alpha9,
        }
    }
}