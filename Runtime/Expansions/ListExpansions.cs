using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Momos.Tools.Expansions {
    public static class ListExpansions {
        /// <summary> 快速移除: [警告]会破坏顺序 </summary>
        public static void QuickRemoveAt<T>(this List<T> list, int index) {
            if (index < list.Count) {
                list[index] = list[^1];
                list.RemoveAt(list.Count - 1);
            }
        }

        /// <summary> 使用Key快速移除: [警告]会破坏顺序 </summary>
        public static void QuickRemove<T,K>(this List<T> list, K key, Func<T,K,bool> isKeyFunc) {
            for (int i = 0; i < list.Count; i++) {
                if (isKeyFunc.Invoke(list[i], key)) {
                    QuickRemoveAt(list, i);
                    break;
                }
            }
        }
    }
}