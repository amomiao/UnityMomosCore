using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using static Momos.Core.Event.InputAreaMgr;

namespace Momos.Core.Event
{
    public class InputMgr : BaseManager<InputMgr>
    {
        /// <summary>
        /// Key:任何枚举类型的枚举值,对应着事件中心的相应事件;
        /// Value:触发键信息,包含申请对象的类型。
        /// </summary>
        private Dictionary<Enum, InputInfo> inputDic = new Dictionary<Enum, InputInfo>();
        //是否开启了输入系统检测
        private bool isStart;
        //用于在改建时获取输入信息的委托 只有当update中获取到信息的时候 再通过委托传递给外部
        private UnityAction<InputInfo> getInputInfoCallBack;
        //是否开始检测输入信息
        private bool isBeginCheckInput = false;

        //当前遍历时取出的输入信息
        InputInfo nowInputInfo;

        public InputMgr()
        {
            MonoMgr.GetInstance().AddUpdateListener(InputUpdate);
            isStart = true;
        }

        /// <summary> 开启或者关闭 输入管理模块的检测 </summary>
        public void IsRun(bool isStart) => this.isStart = isStart;

        // 会顺带将事件注册到事件中心
        /// <summary> 初始键位触发监听 </summary>
        /// <param name="eventEnum"> 触发事件的枚举 </param>
        public void AddKeyEvent(Enum eventEnum, UnityAction evt, KeyCode key, InputInfo.E_InputType inputType, Type type = null, E_InputArea area = E_InputArea.Global)
        {
            ChangeKeyboardInfo(eventEnum, key, inputType, type, area);
            if (!EventCenter.GetInstance().IsExistEvent(eventEnum))
                EventCenter.GetInstance().AddEventListener(eventEnum, evt);
        }
        /// <summary> 初始鼠标触发监听 </summary>
        public void AddMouseEvent(Enum eventEnum, UnityAction evt, int mouseID, InputInfo.E_InputType inputType, Type type = null)
        {
            ChangeMouseInfo(eventEnum, mouseID, inputType, type);
            if (!EventCenter.GetInstance().IsExistEvent(eventEnum))
                EventCenter.GetInstance().AddEventListener(eventEnum, evt);
        }

        // 不会顺带将事件注册到事件中心
        /// <summary> 提供给外部改键或初始化的方法(键盘) </summary>
        public void ChangeKeyboardInfo(Enum eventEnum, KeyCode key, InputInfo.E_InputType inputType, Type type = null, E_InputArea area = E_InputArea.Global)
        {
            //初始化
            if (!inputDic.ContainsKey(eventEnum))
            {
                inputDic.Add(eventEnum, new InputInfo(inputType, key, type));
            }
            else//改键
            {
                //如果之前是鼠标 我们必须要修改它的按键类型
                inputDic[eventEnum].keyOrMouse = InputInfo.E_KeyOrMouse.Key;
                inputDic[eventEnum].key = key;
                inputDic[eventEnum].applicantType = type;
            }
        }
        /// <summary> 提供给外部改键或初始化的方法(鼠标) </summary>
        public void ChangeMouseInfo(Enum eventEnum, int mouseID, InputInfo.E_InputType inputType, Type type = null)
        {
            //初始化
            if (!inputDic.ContainsKey(eventEnum))
            {
                inputDic.Add(eventEnum, new InputInfo(inputType, mouseID, type));
            }
            else//改键
            {
                //如果之前是鼠标 我们必须要修改它的按键类型
                inputDic[eventEnum].keyOrMouse = InputInfo.E_KeyOrMouse.Mouse;
                inputDic[eventEnum].mouseID = mouseID;
                inputDic[eventEnum].inputType = inputType;
                inputDic[eventEnum].applicantType = type;
            }
        }

        /// <summary> 移除指定行为的输入监听 </summary>
        public void RemoveInputInfo(Enum eventEnum)
        {
            if (inputDic.ContainsKey(eventEnum))
                inputDic.Remove(eventEnum);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("长信息 InputMgr报告日志:\n");
            foreach (KeyValuePair<Enum, InputInfo> keyValuePair in inputDic)
            {
                if (keyValuePair.Value.applicantType != null)
                    sb.AppendLine($"枚举事件:{keyValuePair.Key} 由Type:{keyValuePair.Value.applicantType.Name}申请");
                else
                    sb.AppendLine($"枚举事件:{keyValuePair.Key} 的申请者未记录");
            }
            return sb.ToString();
        }

        #region CheckInput
        /// <summary> 获取下一次的输入信息 </summary>
        public void GetInputInfo(UnityAction<InputInfo> callBack)
        {
            getInputInfoCallBack = callBack;
            MonoMgr.GetInstance().StartCoroutine(BeginCheckInput());
        }
        private IEnumerator BeginCheckInput()
        {
            //等一帧
            yield return 0;
            //一帧后才会被置成true
            isBeginCheckInput = true;
        }
        /// <summary> 检测一次输入 </summary>
        private void CheckInput()
        {
            //当一个键按下时 然后遍历所有按键信息 得到是谁被按下了
            if (Input.anyKeyDown)
            {
                InputInfo inputInfo = null;
                //我们需要去遍历监听所有键位的按下 来得到对应输入的信息
                //键盘
                Array keyCodes = Enum.GetValues(typeof(KeyCode));
                foreach (KeyCode inputKey in keyCodes)
                {
                    //判断到底是谁被按下了 那么就可以得到对应的输入的键盘信息
                    if (Input.GetKeyDown(inputKey))
                    {
                        inputInfo = new InputInfo(InputInfo.E_InputType.Down, inputKey);
                        break;
                    }
                }
                //鼠标
                for (int i = 0; i < 3; i++)
                {
                    if (Input.GetMouseButtonDown(i))
                    {
                        inputInfo = new InputInfo(InputInfo.E_InputType.Down, i);
                        break;
                    }
                }
                //把获取到的信息传递给外部
                getInputInfoCallBack.Invoke(inputInfo);
                getInputInfoCallBack = null;
                //检测一次后就停止检测了
                isBeginCheckInput = false;
            }
        }
        #endregion CheckInput

        private void InputUpdate()
        {
            //当委托不为空时 证明想要获取到输入的信息 传递给外部
            if (isBeginCheckInput)
                CheckInput();
            //如果外部没有开启检测功能 就不要检测
            if (!isStart)
                return;
            foreach (Enum eventType in inputDic.Keys)
            {
                nowInputInfo = inputDic[eventType];
                //如果是键盘输入
                if (nowInputInfo.keyOrMouse == InputInfo.E_KeyOrMouse.Key)
                {
                    //是抬起还是按下还是长按
                    switch (nowInputInfo.inputType)
                    {
                        case InputInfo.E_InputType.Down:
                            if (Input.GetKeyDown(nowInputInfo.key))
                                EventCenter.GetInstance().EventTrigger(eventType);
                            break;
                        case InputInfo.E_InputType.Up:
                            if (Input.GetKeyUp(nowInputInfo.key))
                                EventCenter.GetInstance().EventTrigger(eventType);
                            break;
                        case InputInfo.E_InputType.Always:
                            if (Input.GetKey(nowInputInfo.key))
                                EventCenter.GetInstance().EventTrigger(eventType);
                            break;
                        default:
                            break;
                    }
                }
                //如果是鼠标输入
                else
                {
                    switch (nowInputInfo.inputType)
                    {
                        case InputInfo.E_InputType.Down:
                            if (Input.GetMouseButtonDown(nowInputInfo.mouseID))
                                EventCenter.GetInstance().EventTrigger(eventType);
                            break;
                        case InputInfo.E_InputType.Up:
                            if (Input.GetMouseButtonUp(nowInputInfo.mouseID))
                                EventCenter.GetInstance().EventTrigger(eventType);
                            break;
                        case InputInfo.E_InputType.Always:
                            if (Input.GetMouseButton(nowInputInfo.mouseID))
                                EventCenter.GetInstance().EventTrigger(eventType);
                            break;
                        default:
                            break;
                    }
                }
            }

            //EventCenter.GetInstance().EventTrigger(EventEnumSet.E_InputEventKey.E_Input_Horizontal, Input.GetAxis("Horizontal"));
            //EventCenter.GetInstance().EventTrigger(EventEnumSet.E_InputEventKey.E_Input_Vertical, Input.GetAxis("Vertical"));
        }
    }
}