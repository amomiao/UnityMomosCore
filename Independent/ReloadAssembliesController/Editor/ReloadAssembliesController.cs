using UnityEditor;
using UnityEngine;

namespace Momos.Tools.EditorTools.Projects {
    [InitializeOnLoad]
    internal class ReloadAssembliesController {
        private const string MENU_KEY = "Tools/LockingReloadAssemblies";
        private const string KEY = "LockReloadAssemblies";
        private static bool IsLockReloadAssemblies;

        static ReloadAssembliesController() {
            IsLockReloadAssemblies = EditorPrefs.GetBool(KEY, false);
            Menu.SetChecked(MENU_KEY, IsLockReloadAssemblies);
            if (IsLockReloadAssemblies) {
                EditorApplication.LockReloadAssemblies();
            }
            else {
                EditorApplication.UnlockReloadAssemblies();
            }
            EditorApplication.playModeStateChanged += LogPlayModeState;
        }

        private static void LogPlayModeState(PlayModeStateChange state) {
            if (state == PlayModeStateChange.EnteredPlayMode && EditorPrefs.GetBool(KEY, false)) {
                EditorApplication.isPlaying = false;
                Debug.LogWarning("重新加载程序集已被锁定。");
                EditorUtility.DisplayDialog("警告", "已锁定重新加载程序集，请注意！！！", "确定");
            }
        }

        [MenuItem(MENU_KEY, priority = int.MaxValue)]
        private static void SetLockReloadAssemblies() {
            if (EditorPrefs.GetBool(KEY, false)) {
                Debug.Log("重新加载程序集已解锁。");
                EditorApplication.UnlockReloadAssemblies();
                EditorPrefs.SetBool(KEY, false);
                Menu.SetChecked(MENU_KEY, false);
            }
            else {
                if (EditorUtility.DisplayDialog("提示", "是否锁定 重新加载程序集 \n\n锁定以后无法重新加载程序集,\n也不会触发脚本编译。", "继续锁定", "取消")) {
                    Debug.Log("重新加载程序集已锁定。");
                    EditorApplication.LockReloadAssemblies();
                    EditorPrefs.SetBool(KEY, true);
                    Menu.SetChecked(MENU_KEY, true);
                }
            }
        }
    }
}