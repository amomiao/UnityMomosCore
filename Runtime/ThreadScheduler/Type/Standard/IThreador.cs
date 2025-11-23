using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Momos.Tools.ThreadTools
{
    /// <summary> 线程容器 </summary>
    public interface IThreador
    {
        /// <summary> 
        /// 释放: 
        /// 线程对象必须得到释放, 并且要处理'运行时的停止'和'意外停止':
        /// 运行时停止: 通过API正常停止;
        /// 意外停止: Alt F4等, 表现为<see cref="GameObject">的OnDestroy() 中包含 <see cref="OnDispose"/>
        /// </summary>
        public void OnDispose();
    }
}