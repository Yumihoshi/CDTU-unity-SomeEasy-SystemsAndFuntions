using System;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    public class GraphicsSettings : BaseSettings<GraphicsSettings.GraphicsData, GraphicsSettingsSO>
    {
        [Serializable]
        public class GraphicsData
        {
            public bool fullscreenMode = true;
            public int resolutionIndex = 0;
            public int qualityLevel = 1;
            public int targetFrameRate = 60;
        }

        // 图形设置变化的特定事件
        public event Action OnGraphicsChanged;

        public GraphicsSettings(GraphicsSettingsSO settings) : base(settings, "GraphicsSettings")
        {
        }

        #region 图形设置属性
        public bool FullscreenMode
        {
            get => settingsSO.fullscreenMode;
            set
            {
                if (settingsSO.fullscreenMode != value)
                {
                    settingsSO.fullscreenMode = value;
                    OnGraphicsChanged?.Invoke();
                    NotifySettingsChanged();
                }
            }
        }

        public int ResolutionIndex
        {
            get => settingsSO.resolutionIndex;
            set
            {
                if (settingsSO.resolutionIndex != value)
                {
                    settingsSO.resolutionIndex = value;
                    OnGraphicsChanged?.Invoke();
                    NotifySettingsChanged();
                }
            }
        }

        public int QualityLevel
        {
            get => settingsSO.qualityLevel;
            set
            {
                int clampedValue = Mathf.Clamp(value, 0, 2);
                if (settingsSO.qualityLevel != clampedValue)
                {
                    settingsSO.qualityLevel = clampedValue;
                    OnGraphicsChanged?.Invoke();
                    NotifySettingsChanged();
                }
            }
        }

        public int TargetFrameRate
        {
            get => settingsSO.targetFrameRate;
            set
            {
                int clampedValue = Mathf.Clamp(value, 30, 144);
                if (settingsSO.targetFrameRate != clampedValue)
                {
                    settingsSO.targetFrameRate = clampedValue;
                    OnGraphicsChanged?.Invoke();
                    NotifySettingsChanged();
                }
            }
        }
        #endregion

        #region 实用方法
        // 应用图形设置到Unity
        public void ApplyGraphicsSettings()
        {
            // 设置全屏模式
            Screen.fullScreen = FullscreenMode;

            // 设置画质等级
            QualitySettings.SetQualityLevel(QualityLevel, true);

            // 设置目标帧率
            Application.targetFrameRate = TargetFrameRate;

            // 如果需要设置分辨率，可以先获取所有支持的分辨率
            if (Screen.resolutions.Length > ResolutionIndex)
            {
                Resolution resolution = Screen.resolutions[ResolutionIndex];
                Screen.SetResolution(resolution.width, resolution.height, FullscreenMode);
            }
        }
        #endregion

        #region BaseSettings实现
        protected override GraphicsData GetDataFromSettings()
        {
            return new GraphicsData
            {
                fullscreenMode = FullscreenMode,
                resolutionIndex = ResolutionIndex,
                qualityLevel = QualityLevel,
                targetFrameRate = TargetFrameRate
            };
        }

        protected override void ApplyDataToSettings(GraphicsData data)
        {
            bool changed = false;
            
            changed |= (settingsSO.fullscreenMode != data.fullscreenMode);
            changed |= (settingsSO.resolutionIndex != data.resolutionIndex);
            changed |= (settingsSO.qualityLevel != data.qualityLevel);
            changed |= (settingsSO.targetFrameRate != data.targetFrameRate);

            settingsSO.fullscreenMode = data.fullscreenMode;
            settingsSO.resolutionIndex = data.resolutionIndex;
            settingsSO.qualityLevel = data.qualityLevel;
            settingsSO.targetFrameRate = data.targetFrameRate;

            if (changed)
            {
                OnGraphicsChanged?.Invoke();
                NotifySettingsChanged();
            }
        }

        public override void ResetToDefault()
        {
            FullscreenMode = true;
            ResolutionIndex = 0;
            QualityLevel = 1;
            TargetFrameRate = 60;
        }
        #endregion
    }
}