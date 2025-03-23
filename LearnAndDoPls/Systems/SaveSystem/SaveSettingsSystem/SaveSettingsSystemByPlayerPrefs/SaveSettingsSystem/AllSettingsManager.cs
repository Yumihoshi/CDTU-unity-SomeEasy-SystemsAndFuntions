using UnityEngine;
using System.Collections.Generic;
using Managers;

namespace SaveSettingsSystem
{
    /// <summary>
    /// 总的设置管理器，用于管理所有子设置管理器
    /// </summary>
    public class AllSettingsManager : SingletonDD<AllSettingsManager>
    {
        public static bool HasInstance => Instance != null;
        /// <summary>
        /// 所有注册的设置管理器
        /// 使用哈希表来避免重复注册，因为哈希值都是唯一的
        /// 这样可以提高性能，因为哈希表的查找速度比列表快
        /// </summary>
        /// <typeparam name="ISaveSettings"></typeparam>
        /// <returns></returns>
        private HashSet<ISaveSettings> allSettings = new HashSet<ISaveSettings>();//使用哈希表
        /// <summary>
        /// 所有注册的设置管理器
        /// </summary>
        public IReadOnlyCollection<ISaveSettings> AllSettings => allSettings;

        /// <summary>
        /// 通过用于缓存注册的管理器
        /// </summary>
        private Dictionary<System.Type, MonoBehaviour> managerCache = new Dictionary<System.Type, MonoBehaviour>();

        #region 简易日志分类记录器
        /// <summary>
        /// 简易日志分类记录器
        /// </summary>
        public static class SettingsLogger
        {
            [System.Diagnostics.Conditional("UNITY_EDITOR")]//
            [System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
            public static void Log(string message)
            {
                Debug.Log($"[Settings] {message}");
            }
            
            public static void LogError(string message)
            {
                Debug.LogError($"[Settings] {message}");
            }
        }
        #endregion

        public void RegisterManager(ISaveSettings manager)
        {
            if (manager == null || !(manager is MonoBehaviour monoBehaviour)) return;
            
            var type = monoBehaviour.GetType();
            if (!managerCache.ContainsKey(type))
            {
                managerCache[type] = monoBehaviour;
                allSettings.Add(manager);
                SettingsLogger.Log($"注册管理器: {type.Name}");
            }
        }

        public void UnregisterManager(ISaveSettings manager)
        {
            if (manager == null || !(manager is MonoBehaviour monoBehaviour)) return;
            
            var type = monoBehaviour.GetType();
            if (managerCache.ContainsKey(type))
            {
                allSettings.Remove(manager);
                managerCache.Remove(type);
                SettingsLogger.Log($"注销管理器: {type.Name}");
            }
        }

        public T GetManager<T>() where T : MonoBehaviour
        {
            return managerCache.TryGetValue(typeof(T), out var manager) ? manager as T : null;
        }

        private void SafeExecute(System.Action action, string operationName)
        {
            try
            {
                action();
            }
            catch (System.Exception e)
            {
                SettingsLogger.LogError($"{operationName}失败: {e.Message}");
            }
        }

        public void SaveAllSettings()
        {
            foreach (var settings in allSettings)
            {
                SafeExecute(() => settings.Save(), "保存设置");
            }
        }

        public void LoadAllSettings()
        {
            foreach (var settings in allSettings)
            {
                SafeExecute(() => 
                {
                    try
                    {
                        settings.Load();
                    }
                    catch
                    {
                        settings.ResetToDefault();
                        throw;
                    }
                }, "加载设置");
            }
        }

        public void ResetAllSettings()
        {
            foreach (var settings in allSettings)
            {
                settings.ResetToDefault();
            }
            SaveAllSettings();
        }

        private void OnApplicationQuit()
        {
            SaveAllSettings();
        }
    }
}