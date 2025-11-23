using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Momos.Core.Event
{
    // 类状态机: 对某一输入状态有对应的方案
    public class InputAreaMgr : BaseManager<InputAreaMgr>
    {
        /// <summary> 
        /// 输入域: 根据玩家界面状态设置对应的输入域,
        /// 每个输入域都有自己逻辑,不同的输入域的同一按键可能有不同的逻辑 如: 在聊天时回车发送消息、在剧情时回车下一条剧情,
        /// </summary>
        public enum E_InputArea
        {
            Global = 0, // 全局
            Setting,    // 设置(某种游戏暂停)
            Playing,    // 游玩过程
            UI,         // 大UI展开,阻断一些逻辑
            OnShift,    // Shift按压中
            OnCtrl,     // Ctrl按压中
            OnAtl,      // Atl按压中
                        // ……
        }

        private bool[] isActiveOfAreas;
        public InputAreaMgr()
        {
            isActiveOfAreas = new bool[Enum.GetValues(typeof(E_InputArea)).Length];
            for (int i = 0; i < isActiveOfAreas.Length; i++)
                isActiveOfAreas[i] = false;
        }

        private bool AreaIsActive(E_InputArea area)
            => isActiveOfAreas[(int)area];

        public void SetAreaActive(E_InputArea area, bool value)
            => isActiveOfAreas[(int)area] = value;

        /// <summary> 外部询问一些域是否生效 </summary>
        public bool VerAreaIsActive(params E_InputArea[] areas)
        {
            bool isActive = true;
            // 全局域时刻生效
            if (areas.Length == 1 && areas[0] == E_InputArea.Global)
                return true;
            // 所有域生效才通过
            foreach (E_InputArea area in areas)
                if (!AreaIsActive(area))
                {
                    isActive = false;
                    break;
                }
            return isActive;
        }
    }
}