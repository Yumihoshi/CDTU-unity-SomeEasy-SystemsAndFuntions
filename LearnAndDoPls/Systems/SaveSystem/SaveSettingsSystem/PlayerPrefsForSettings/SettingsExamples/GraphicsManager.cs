using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using SaveSystem;
using GraphicsSettingsSystem = SaveSystem.GraphicsSettings;

public class GraphicsManager : BaseSettingsManager<GraphicsSettingsSystem>
{
    [Header("图形设置数据")]
    [SerializeField] private GraphicsSettingsSO settingsSO;
    
    [Header("UI控制组件")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Dropdown qualityDropdown;
    [SerializeField] private Slider frameRateSlider;
    [SerializeField] private Text frameRateText;
    
    private Resolution[] availableResolutions;
    
    protected override void Awake()
    {
        base.Awake();
        InitializeResolutionOptions();
    }
    
    protected override void InitializeSettings()
    {
        settings = new GraphicsSettingsSystem(settingsSO);
        settings.OnGraphicsChanged += HandleGraphicsChanged;
        
        // 绑定UI事件
        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.AddListener(value => settings.FullscreenMode = value);
            
        if (resolutionDropdown != null)
            resolutionDropdown.onValueChanged.AddListener(value => settings.ResolutionIndex = value);
            
        if (qualityDropdown != null)
            qualityDropdown.onValueChanged.AddListener(value => settings.QualityLevel = value);
            
        if (frameRateSlider != null)
            frameRateSlider.onValueChanged.AddListener(value => settings.TargetFrameRate = Mathf.RoundToInt(value));
        
        UpdateUI();
    }
    
    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (settings != null)
        {
            settings.OnGraphicsChanged -= HandleGraphicsChanged;
        }
    }
    
    private void HandleGraphicsChanged()
    {
        UpdateUI();
        ApplyGraphicsSettings();
    }
    
    public override void ResetToDefault()
    {
        ((BaseSettings<GraphicsSettings.GraphicsData, GraphicsSettingsSO>)settings).ResetToDefault();
        Save();
        UpdateUI();
        ApplyGraphicsSettings();
    }
    
    /// <summary>
    /// 应用图形设置到系统
    /// </summary>
    private void ApplyGraphicsSettings()
    {
        settings.ApplyGraphicsSettings();
    }
    
    /// <summary>
    /// 更新UI控件
    /// </summary>
    private void UpdateUI()
    {
        if (fullscreenToggle != null)
            fullscreenToggle.isOn = settings.FullscreenMode;
            
        if (resolutionDropdown != null && resolutionDropdown.options.Count > settings.ResolutionIndex)
            resolutionDropdown.value = settings.ResolutionIndex;
            
        if (qualityDropdown != null)
            qualityDropdown.value = settings.QualityLevel;
            
        if (frameRateSlider != null)
            frameRateSlider.value = settings.TargetFrameRate;
            
        if (frameRateText != null)
            frameRateText.text = settings.TargetFrameRate.ToString();
    }
    
    /// <summary>
    /// 初始化分辨率选项
    /// </summary>
    private void InitializeResolutionOptions()
    {
        if (resolutionDropdown != null)
        {
            availableResolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();
            
            List<string> options = new List<string>();
            for (int i = 0; i < availableResolutions.Length; i++)
            {
                Resolution resolution = availableResolutions[i];
                string option = $"{resolution.width} x {resolution.height} @{resolution.refreshRate}Hz";
                options.Add(option);
            }
            
            resolutionDropdown.AddOptions(options);
        }
    }
}