using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Momos.Tools.ProjectViewDrawor
{
    [Serializable]
    public class HighlightFolderItem
    {
        public string localPath;
        public Color color = new Color(0.9f, 0.9f, 0.6f, 0.3f);
        public bool isDrawColor;
        public bool isExist = false;
        public string guid;

        /// <summary> 值类型使用方法写入 </summary>
        public void SetColor(Color color)
        { 
            this.color = color;
        }
    }

    public class ProjectViewItemConfigAsset : ScriptableObject
    {
        public List<HighlightFolderItem> highlightFloderItemList = new List<HighlightFolderItem>();
    }
}