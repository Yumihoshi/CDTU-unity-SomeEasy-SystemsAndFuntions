using Managers;
using System;

namespace SaveSystem
{
    /// <summary>
    /// 所有设置管理器的抽象基类
    /// Todo-需要加入Utils的Singleton并继承它，并实现ISaveSettings接口
    /// Todo-You need to include the Singleton from the Utils namespace and inherit from it, while also implementing the ISaveSettings interface.
    /// If you have done it，you can delete these todo-notes
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

            // 订阅设置的变更事件，利用了接口的(我认为叫引用)的特性
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
                // 不需要在这里触发事件，因为settings.ResetToDefault()应该会触发其自身的SettingsChanged事件
            }
        }


        /// <summary>
        /// 取消订阅事件
        /// </summary>
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