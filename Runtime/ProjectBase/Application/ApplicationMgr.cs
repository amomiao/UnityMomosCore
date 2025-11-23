using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Momos.Core.Apps {
    public class ApplicationMgr : BaseManager<ApplicationMgr> {
        /// <summary> 分辨率调整器 </summary>
        public ResolutionController ResolutionController;

        public ApplicationMgr() {
            ResolutionController = new ResolutionController(); ;
        }

        #region Runtime
        public void QuitGame() => Application.Quit();
        #endregion Runtime

        #region 外部访问
        public void OpenURL(string url) => Application.OpenURL(url);
        public void OpenExplorer(string path) => System.Diagnostics.Process.Start("explorer.exe", path);
        #endregion 外部访问
    }
}