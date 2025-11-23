using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Momos.Core
{
    public interface ISingletonMono
    {
        /// <summary> true:设置为过场景不销毁 </summary>
        public bool IsDontDestroy { get; }
    }
}