using Momos.Core.Event;
using Momos.Tools.Interface;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Momos.Tools.GameMain
{
    public enum E_PlayerInput
    {
        LeftMouseDown,
        LeftMousePress,
        LeftMouseUp,
        RightMouseDown,
        RightMousePress,
        RightMouseUp
    }

    public abstract class PlayerInputControllerBase : IMonoEvt
    {
        public bool IsInUI => EventSystem.current.IsPointerOverGameObject();

        public virtual void StartDo()
        {
            // 左键
            InputMgr.GetInstance().AddMouseEvent(
                E_PlayerInput.LeftMouseDown,
                DefaultLeftMouseDownEvt,
                0,
                InputInfo.E_InputType.Down);
            InputMgr.GetInstance().AddMouseEvent(
                E_PlayerInput.LeftMousePress,
                DefaultLeftMousePressEvt,
                0,
                InputInfo.E_InputType.Always);
            InputMgr.GetInstance().AddMouseEvent(
                E_PlayerInput.LeftMouseUp,
                DefaultLeftMouseUpEvt,
                0,
                InputInfo.E_InputType.Up);
            // 右键
            InputMgr.GetInstance().AddMouseEvent(
                E_PlayerInput.RightMouseDown,
                DefaultRightMouseDownEvt,
                1,
                InputInfo.E_InputType.Down);
            InputMgr.GetInstance().AddMouseEvent(
                E_PlayerInput.RightMousePress,
                DefaultRightMousePressEvt,
                1,
                InputInfo.E_InputType.Always);
            InputMgr.GetInstance().AddMouseEvent(
                E_PlayerInput.RightMouseUp,
                DefaultRightMouseUpEvt,
                1,
                InputInfo.E_InputType.Up);
        }

        public virtual void UpdateDo() { }

        protected abstract void DefaultLeftMouseDownEvt();
        protected abstract void DefaultLeftMousePressEvt();
        protected abstract void DefaultLeftMouseUpEvt();

        protected abstract void DefaultRightMouseDownEvt();
        protected abstract void DefaultRightMousePressEvt();
        protected abstract void DefaultRightMouseUpEvt();

    }
}