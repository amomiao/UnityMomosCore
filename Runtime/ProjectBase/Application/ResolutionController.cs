using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Momos.Core.Apps
{
    public class ResolutionController
    {
        public int NowResolutionIndex
        {
            get 
            {
                for (int i = 0; i < Resolutions.Length; i++)
                {
                    if (Resolutions[i].width == Screen.width && Resolutions[i].height == Screen.height)
                        return i;
                }
                return -1;
            }
        }
        public float DPI => Screen.dpi;
        public bool IsFullScene => Screen.fullScreenMode == FullScreenMode.FullScreenWindow;
        public Resolution NowResolution => Screen.currentResolution;
        public Resolution[] Resolutions { get; private set; }

        public ResolutionController()
        {
            Resolutions = Screen.resolutions.
                // 不是整数的刷新率过滤掉
                Where((res)=>res.refreshRateRatio.value % 1 == 0).
                // 将从小到大的分辨率反转
                Reverse().ToArray();
        }

        /// <summary> 调整分辨率(交给UI使用的) </summary>
        public void AdjustResolutionForUI(int index) => AdjustResolution(Resolutions[index]);

        /// <summary> 是否全屏 </summary>
        public void AdjustResolution(bool isFullScreen)
        {
            if (isFullScreen)
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            else
                Screen.fullScreenMode = FullScreenMode.Windowed;
        }

        /// <summary> 分辨率、是否全屏、刷新率 </summary>
        public void AdjustResolution(Resolution resolution)
        { 
            if(!resolution.Equals(NowResolution))
                Screen.SetResolution(resolution.width, resolution.height, IsFullScene, (int)resolution.refreshRateRatio.value);
        }

        /// <summary> 调整刷新率 </summary>
        public void AdjustRefreshRate(int refreshRate)
            => Screen.SetResolution(NowResolution.width, NowResolution.height, IsFullScene, refreshRate);
    }
}