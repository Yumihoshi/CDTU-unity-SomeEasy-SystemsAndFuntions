using System;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    public class AudioSettings : BaseSettings<AudioSettings.AudioVolumeData, AudioSettingsSO>
    {
        [Serializable]
        public class AudioVolumeData
        {
            public float masterVolume = 1f;
            public float bgmVolume = 1f;
            public float sfxVolume = 1f;
        }

        public AudioSettings(AudioSettingsSO settings) : base(settings, "AudioSettings")
        {
        }

        // 音量变化的特定事件，与基类的OnSettingsChanged不同
        public event Action OnVolumeChanged;

        #region 音量控制逻辑
        public float MasterVolume
        {
            get => settingsSO.masterVolume;
            set
            {
                float clampedValue = Mathf.Clamp01(value);
                if (!Mathf.Approximately(settingsSO.masterVolume, clampedValue))
                {
                    settingsSO.masterVolume = clampedValue;
                    VolumeChanged(); // 使用统一的事件通知方法
                }
            }
        }

        public float BGMVolume
        {
            get => settingsSO.bgmVolume;
            set
            {
                float clampedValue = Mathf.Clamp01(value);
                if (!Mathf.Approximately(settingsSO.bgmVolume, clampedValue))
                {
                    settingsSO.bgmVolume = clampedValue;
                    VolumeChanged(); // 使用统一的事件通知方法
                }
            }
        }

        public float SFXVolume
        {
            get => settingsSO.sfxVolume;
            set
            {
                float clampedValue = Mathf.Clamp01(value);
                if (!Mathf.Approximately(settingsSO.sfxVolume, clampedValue))
                {
                    settingsSO.sfxVolume = clampedValue;
                    VolumeChanged(); // 使用统一的事件通知方法
                }
            }
        }

        // 新增：统一的事件通知方法
        private void VolumeChanged()
        {
            // 先触发特定事件，再触发通用事件
            OnVolumeChanged?.Invoke();
            NotifySettingsChanged();
        }

        // 计算实际的BGM音量（考虑主音量）
        public float GetActualBGMVolume()
        {
            return Mathf.Clamp01(MasterVolume * BGMVolume);
        }

        // 计算实际的音效音量（考虑主音量）
        public float GetActualSFXVolume()
        {
            return Mathf.Clamp01(MasterVolume * SFXVolume);
        }

        // 重置所有音量到默认值
        public override void ResetToDefault()
        {
            // 直接设置值，然后统一触发事件
            settingsSO.masterVolume = 1f;
            settingsSO.bgmVolume = 1f;
            settingsSO.sfxVolume = 1f;
            VolumeChanged();
        }
        #endregion

        #region BaseSettings实现
        protected override AudioVolumeData GetDataFromSettings()
        {
            return new AudioVolumeData
            {
                masterVolume = MasterVolume,
                bgmVolume = BGMVolume,
                sfxVolume = SFXVolume
            };
        }

        protected override void ApplyDataToSettings(AudioVolumeData data)
        {
            // 这里避免触发多次事件，先记录旧状态
            bool changed = false;
            changed |= !Mathf.Approximately(settingsSO.masterVolume, data.masterVolume);
            changed |= !Mathf.Approximately(settingsSO.bgmVolume, data.bgmVolume);
            changed |= !Mathf.Approximately(settingsSO.sfxVolume, data.sfxVolume);

            // 直接设置值，不通过属性，避免触发多次事件
            settingsSO.masterVolume = data.masterVolume;
            settingsSO.bgmVolume = data.bgmVolume;
            settingsSO.sfxVolume = data.sfxVolume;

            // 如果有修改再触发事件
            if (changed)
            {
                OnVolumeChanged?.Invoke();
                NotifySettingsChanged();
            }
        }
        #endregion
    }
}