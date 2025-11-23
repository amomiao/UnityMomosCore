using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Momos.Core.Event
{
    #region IEventInfo: 无参 1参 2参,更多参数时应当考虑写一个容器Type
    public interface IEventInfo { }
    public class EventInfo : IEventInfo
    {
        public UnityAction actions;
        public EventInfo(UnityAction action)
        {
            actions += action;
        }
    }
    public class EventInfo<T> : IEventInfo
    {
        public UnityAction<T> actions;
        public EventInfo(UnityAction<T> action)
        {
            actions += action;
        }
    }
    public class EventInfo<T1, T2> : IEventInfo
    {
        public UnityAction<T1, T2> actions;
        public EventInfo(UnityAction<T1, T2> action)
        {
            actions += action;
        }
    }
    #endregion IEventInfo

    /// <summary> 事件中心 容器 </summary>
    /// <typeparam name="Key"> 当前只会为Enum,热更新可能需要更换string等 </typeparam>
    public class EventCenterBox<Key> where Key : Enum
    {
        //key —— 事件的名字（比如：怪物死亡，玩家死亡，通关 等等）
        //value —— 对应的是 监听这个事件 对应的委托函数们
        private Dictionary<Key, IEventInfo> eventDic = new Dictionary<Key, IEventInfo>();

        public bool IsExistEvent(Key eventKeyEnum) => eventDic.ContainsKey(eventKeyEnum) && eventDic[eventKeyEnum] != null;

        #region  AddEventListener添加事件监听
        /// <summary> 监听不需要参数传递的事件 </summary>
        public void AddEventListener(Key name, UnityAction action)
        {
            //有没有对应的事件监听
            //有的情况
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo).actions += action;
            //没有的情况
            else
                eventDic.Add(name, new EventInfo(action));
        }
        /// <summary> 添加事件监听 </summary>
        /// <param name="name">事件的名字</param>
        /// <param name="action">准备用来处理事件 的委托函数</param>
        public void AddEventListener<T>(Key name, UnityAction<T> action)
        {
            //有没有对应的事件监听
            //有的情况
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo<T>).actions += action;
            //没有的情况
            else
                eventDic.Add(name, new EventInfo<T>(action));
        }
        /// <summary> 添加事件监听 </summary>
        /// <param name="name">事件的名字</param>
        /// <param name="action">准备用来处理事件 的委托函数</param>
        public void AddEventListener<T1,T2>(Key name, UnityAction<T1,T2> action)
        {
            //有没有对应的事件监听
            //有的情况
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo<T1,T2>).actions += action;
            //没有的情况
            else
                eventDic.Add(name, new EventInfo<T1,T2>(action));
        }
        #endregion  AddEventListener添加事件监听

        #region RemoveEventListener移除事件监听
        /// <summary> 移除不需要参数的事件 </summary>
        public void RemoveEventListener(Key name, UnityAction action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo).actions -= action;
        }

        /// <summary> 移除对应的1参事件监听 </summary>
        /// <param name="name">事件的名字</param>
        /// <param name="action">对应之前添加的委托函数</param>
        public void RemoveEventListener<T>(Key name, UnityAction<T> action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo<T>).actions -= action;
        }
        
        /// <summary> 移除对应的2参事件监听 </summary>
        /// <param name="name">事件的名字</param>
        /// <param name="action">对应之前添加的委托函数</param>
        public void RemoveEventListener<T1,T2>(Key name, UnityAction<T1,T2> action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo<T1,T2>).actions -= action;
        }
        #endregion RemoveEventListener移除事件监听

        #region EventTrigger事件触发
        /// <summary> 无参事件触发 </summary>
        public void EventTrigger(Key name)
        {
            if (eventDic.ContainsKey(name))
            {
                if ((eventDic[name] as EventInfo).actions != null)
                    (eventDic[name] as EventInfo).actions.Invoke();
            }
        }
        /// <summary> 1参事件触发 </summary>
        /// <param name="name">哪一个名字的事件触发了</param>
        public void EventTrigger<T>(Key name, T info)
        {
            if (eventDic.ContainsKey(name))
            {
                if ((eventDic[name] as EventInfo<T>).actions != null)
                    (eventDic[name] as EventInfo<T>).actions.Invoke(info);
            }
        }
        /// <summary> 2参事件触发 </summary>
        /// <param name="name">哪一个名字的事件触发了</param>
        public void EventTrigger<T1,T2>(Key name, T1 info1,T2 info2)
        {
            if (eventDic.ContainsKey(name))
            {
                if ((eventDic[name] as EventInfo<T1,T2>).actions != null)
                    (eventDic[name] as EventInfo<T1, T2>).actions.Invoke(info1,info2);
            }
        }
        #endregion EventTrigger事件触发

        /// <summary> 清空事件中心,主要用在场景切换时 </summary>
        public void Clear() => eventDic.Clear();
    }

    public class EventCenter : BaseManager<EventCenter>
    {
        private Dictionary<Type, EventCenterBox<Enum>> generalControl;

        public EventCenter()
        {
            generalControl = new Dictionary<Type, EventCenterBox<Enum>>();
        }

        public bool IsExistEvent(Enum eventKeyEnum)
           => generalControl.ContainsKey(eventKeyEnum.GetType()) && generalControl[eventKeyEnum.GetType()].IsExistEvent(eventKeyEnum);

        #region  AddEventListener添加事件监听
        /// <summary> 监听不需要参数传递的事件 </summary>
        public void AddEventListener(Enum key, UnityAction action)
        {
            if (!generalControl.ContainsKey(key.GetType()))
                generalControl.Add(key.GetType(), new EventCenterBox<Enum>());
            generalControl[key.GetType()].AddEventListener(key, action);
        }
        /// <summary> 添加1参事件监听 </summary>
        /// <param name="name">事件的名字</param>
        /// <param name="action">准备用来处理事件 的委托函数</param>
        public void AddEventListener<T>(Enum key, UnityAction<T> action)
        {
            if (!generalControl.ContainsKey(key.GetType()))
                generalControl.Add(key.GetType(), new EventCenterBox<Enum>());
            generalControl[key.GetType()].AddEventListener(key, action);
        }
        /// <summary> 添加2参事件监听 </summary>
        /// <param name="name">事件的名字</param>
        /// <param name="action">准备用来处理事件 的委托函数</param>
        public void AddEventListener<T1,T2>(Enum key, UnityAction<T1,T2> action)
        {
            if (!generalControl.ContainsKey(key.GetType()))
                generalControl.Add(key.GetType(), new EventCenterBox<Enum>());
            generalControl[key.GetType()].AddEventListener(key, action);
        }
        #endregion  AddEventListener添加事件监听

        #region RemoveEventListener移除事件监听
        /// <summary> 移除不需要参数的事件 </summary>
        public void RemoveEventListener(Enum key, UnityAction action) => generalControl[key.GetType()].RemoveEventListener(key, action);
        /// <summary> 移除对应的1参事件监听 </summary>
        /// <param name="name">事件的名字</param>
        /// <param name="action">对应之前添加的委托函数</param>
        public void RemoveEventListener<T>(Enum key, UnityAction<T> action) => generalControl[key.GetType()].RemoveEventListener(key, action);
        /// <summary> 移除对应的2参事件监听 </summary>
        /// <param name="name">事件的名字</param>
        /// <param name="action">对应之前添加的委托函数</param>
        public void RemoveEventListener<T1,T2>(Enum key, UnityAction<T1,T2> action) => generalControl[key.GetType()].RemoveEventListener(key, action);
        #endregion RemoveEventListener移除事件监听

        #region EventTrigger事件触发
        /// <summary> 事件触发（不需要参数的 </summary>
        public void EventTrigger(Enum key)
        {
            if (generalControl.ContainsKey(key.GetType()))
                generalControl[key.GetType()].EventTrigger(key);
        }
        /// <summary> 1参事件触发 </summary>
        /// <param name="name">哪一个名字的事件触发了</param>
        public void EventTrigger<T>(Enum key, T info)
        {
            if (generalControl.ContainsKey(key.GetType()))
                generalControl[key.GetType()].EventTrigger(key, info);
        }
        /// <summary> 2参事件触发 </summary>
        /// <param name="name">哪一个名字的事件触发了</param>
        public void EventTrigger<T1, T2>(Enum key, T1 info1, T2 info2)
        {
            if (generalControl.ContainsKey(key.GetType()))
                generalControl[key.GetType()].EventTrigger(key, info1, info2);
        }
        #endregion EventTrigger事件触发

        /// <summary> 清空事件中心,主要用在场景切换时 </summary>
        public void Clear()
        {
            foreach (EventCenterBox<Enum> box in generalControl.Values)
                box.Clear();
            generalControl.Clear();
        }
    }
}
