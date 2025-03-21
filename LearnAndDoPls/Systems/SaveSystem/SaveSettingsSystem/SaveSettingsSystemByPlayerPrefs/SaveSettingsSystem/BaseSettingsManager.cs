using UnityEngine;
using Managers;
using System;

namespace SaveSystem
{
    /// <summary>
    /// 所有设置管理器的抽象基类
    /// 需要加入Utils的Singleton
    /// </summary>
    public abstract class BaseSettingsManager<TSettings> : Singleton<BaseSettingsManager<TSettings>>, ISaveSettings
        where TSettings : class, ISaveSettings
    {
        protected TSettings settings;
        
        /// <summary>
        /// 设置变更时触发的事件
        /// </summary>
        public event EventHandler SettingsChanged;

        protected virtual void Start()
        {
            InitializeSettings();
            Load();
            
            // 订阅设置的变更事件
            if (settings != null && settings is ISaveSettings saveSettings)
            {
                saveSettings.SettingsChanged += HandleSettingsChanged;
            }
        }
        
        /// <summary>
        /// 处理设置变更事件
        /// </summary>
        protected virtual void HandleSettingsChanged(object sender, EventArgs e)
        {
            // 将设置变更事件向上传播
            SettingsChanged?.Invoke(this, e);
        }

        protected abstract void InitializeSettings();
        
        public virtual void Save()
        {
            if (settings != null)
            {
                settings.Save();
            }
        }

        public virtual void Load()
        {
            if (settings != null)
            {
                settings.Load();
            }
        }
        
        /// <summary>
        /// 重置设置到默认值
        /// </summary>
        public virtual void ResetToDefault()
        {
            if (settings != null)
            {
                settings.ResetToDefault();
                // 不需要在这里触发事件，因为settings.ResetToDefault()应该会触发其自身的SettingsChanged事件
            }
        }

        protected virtual void OnDestroy()
        {
            // 取消订阅事件，防止内存泄漏
            if (settings != null && settings is ISaveSettings saveSettings)
            {
                saveSettings.SettingsChanged -= HandleSettingsChanged;
            }
        }
    }
}