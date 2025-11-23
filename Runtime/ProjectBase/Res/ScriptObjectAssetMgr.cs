using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Momos.Core.Asset
{
    public class ScriptObjectAssetMgr : BaseManager<ScriptObjectAssetMgr>
    {
        private ScriptObjectSetAsset asset;

        public ScriptObjectAssetMgr()
        {
            asset = GetAsset();
        }

        private ScriptObjectSetAsset GetAsset()
        {
            // 不能使用ResMgr去Load, 若空会递归爆栈
            return Resources.Load<ScriptObjectSetAsset>("ScriptObjectSet");
        }

        public T Load<T>(string name = "") where T : ScriptableObject
        { 
            if(asset != null)
                return asset.GetScriptableObejct<T>(name);
            return null;
        }

        public ScriptableObject Load(Type type, string name = "")
        {
            if (asset != null)
                asset.GetScriptableObejct(type, name);
            return null;
        }
    }
}