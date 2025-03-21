using UnityEngine;
using System;

namespace SaveSystem
{
    /// <summary>
    /// 所有设置管理器的抽象基类
    /// </summary>
    public abstract class BaseSettingsManager<TSettings> : MonoBehaviour, ISaveSettings
        where TSettings : class, ISaveSettings
    {
        protected TSettings settings;

        /// <summary>
        /// 设置变更时触发的事件
        /// </summary>
        public virtual event EventHandler SettingsChanged;

        protected virtual void Awake()
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

        /// <summary>
        /// 设定类的初始化方法，因为是一个抽象类所以大家都要实现
        /// </summary>
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
            }
        }

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (settings != null && settings is ISaveSettings saveSettings)
            {
                saveSettings.SettingsChanged -= HandleSettingsChanged;
            }
        }
    }
}