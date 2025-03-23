using UnityEngine;
using System.Collections.Generic;
using Managers;

namespace SaveSystem
{
    /// <summary>
    /// 总的设置管理器，用于管理所有子设置管理器
    /// </summary>
    public class AllSettingsManager : SingletonDD<AllSettingsManager>
    {
        public static bool HasInstance => Instance != null;
        
        private HashSet<ISaveSettings> allSettings = new HashSet<ISaveSettings>();
        private Dictionary<System.Type, MonoBehaviour> managerCache = new Dictionary<System.Type, MonoBehaviour>();

        /// <summary>
        /// 
        /// </summary>
        public static class Logger
        {
            [System.Diagnostics.Conditional("UNITY_EDITOR")]
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

        public void RegisterManager(ISaveSettings manager)
        {
            if (manager == null || !(manager is MonoBehaviour monoBehaviour)) return;
            
            var type = monoBehaviour.GetType();
            if (!managerCache.ContainsKey(type))
            {
                managerCache[type] = monoBehaviour;
                allSettings.Add(manager);
                Logger.Log($"注册管理器: {type.Name}");
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
                Logger.Log($"注销管理器: {type.Name}");
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
                Logger.LogError($"{operationName}失败: {e.Message}");
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