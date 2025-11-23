using Momos.Core.Apps;
using Momos.Core.Asset;
using Momos.Core.Event;
using Momos.Core.Logical;
using Momos.Core.Net;
using Momos.Core.Pool;
using Momos.Tools.ThreadTools;
using UnityEngine;
namespace Momos.Tools.GameMain
{
    /// <summary> 对于取消了Redomain的项目,重新加载单例。 </summary>
    public abstract class LoadingMgrBase : MonoBehaviour
    {
        private void Awake()
        {
            IfNullReloadMgr();
        }

        protected virtual void IfNullReloadMgr()
        {
            // ResMgr
            ResMgr.GetInstance().ReloadIfNotNull();
            HttpClientMgr.GetInstance().ReloadIfNotNull();
            // MonoMgr
            MonoMgr.GetInstance().ReloadIfNotNull();
            // LogicMgr: 由于依赖关系, LogicMgr最好在MonoMgr后调用
            LogicMgr.GetInstance().ReloadIfNotNull();
            // PoolMgr
            ObjPoolMgr.GetInstance().ReloadIfNotNull();
            TypePoolMgr.GetInstance().ReloadIfNotNull();
            // ScriptObjectAssetMgr
            ScriptObjectAssetMgr.GetInstance().ReloadIfNotNull();
            // Input
            InputMgr.GetInstance().ReloadIfNotNull();
            InputAreaMgr.GetInstance().ReloadIfNotNull();
            // Event
            EventCenter.GetInstance().ReloadIfNotNull();
            // UI Module
            //UIModule.GetInstance().Reload();
            MusicMgr.GetInstance().ReloadIfNotNull();
            SceneMgr.GetInstance().ReloadIfNotNull();
            // Other
            //Debuger.Reload();
            ApplicationMgr.GetInstance().ReloadIfNotNull();
        }
    }
}