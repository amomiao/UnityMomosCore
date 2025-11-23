using Momos.Core.Asset;
using Momos.Core.Event;
using Momos.Core.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Momos.Core
{
    /// <summary>
    /// 负责项目框架的交换器
    /// </summary>
    public class ProjectFrame : MonoBehaviour
    {
        public static ProjectFrame _instance;
        private void Awake()
        {
            _instance = this;
            ProjectFrameInit();
        }

        #region ProjectFrameInit 项目框架初始化
        /// <summary>
        /// 项目框架初始化
        /// </summary>
        private void ProjectFrameInit()
        {
            EventCenter.GetInstance();
            MonoMgr.GetInstance();
            //InputMgr依赖于MonoMgr
            InputMgr.GetInstance();
            ResMgr.GetInstance();
            //MusicMgr依赖于ResMgr
            MusicMgr.GetInstance();
            //PoolMgr依赖于ResMgr
            ObjPoolMgr.GetInstance();
            //UIManager依赖于ResMgr
            // UIManager.GetInstance();
            SceneMgr.GetInstance();
        }
        #endregion
    }
}