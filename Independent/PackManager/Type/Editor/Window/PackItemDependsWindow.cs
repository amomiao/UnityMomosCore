using Momos.Tools.EditorTools.Control;
using Momos.Tools.EditorTools.Expansion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Momos.Tools.PackManager
{
    internal class PackItemDependsWindow : EditorWindow
    {
        internal PackMgrConfigAsset config;
        internal PackItem packItem;
        ScrollViewGrid<string> svg;

        private ScrollViewGrid<string> CreateScrollViewGrid()
        {
            return new ScrollViewGrid<string>(18,18,config.packItemList.Select(item => item.packName).ToArray(),
                new ScrollViewGrid<string>.ScrollViewColumnItem((rect, name) => GUI.Label(rect,name),"包",200,1,E_ColumnStyle.Single),
                new ScrollViewGrid<string>.ScrollViewColumnItem(
                    (rect, name) =>
                    {
                        if (packItem.packName == name)
                            GUI.Label(rect, "-");
                        // 不存在依赖 允许添加依赖
                        else if (!packItem.depends1.Contains(name))
                        {
                            if (GUI.Button(rect, "添加"))
                            {
                                packItem.depends1.Add(name);
                                config.SaveInAssetDatabase();
                            }
                        }
                        // 存在依赖 允许移除依赖
                        else
                        {
                            if (GUI.Button(rect, "移除"))
                            {
                                packItem.depends1.Remove(name);
                                config.SaveInAssetDatabase();
                            }
                        }
                    },
                    "操作",100,0,E_ColumnStyle.Full)
            );
        }

        private void OnGUI()
        {
            if (svg == null)
                svg = CreateScrollViewGrid();

            svg.OnGUI(position.OnlySize(), Event.current.mousePosition.y);
        }
    }
}