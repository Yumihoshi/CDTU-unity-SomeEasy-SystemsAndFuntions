using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using SaveSystem;

public class GraphicsManager : BaseSettingsManager<GraphicsSettings>
{
    [Header("图形设置数据")]
    [SerializeField] private GraphicsSettingsSO settingsSO;

     private GraphicsSettings graphicsSettings;
    
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
        InitializeSettings(); // 移到这里，确保在初始化分辨率选项后再初始化设置
    }
    
    protected override void InitializeSettings()
    {
        graphicsSettings = new GraphicsSettings(settingsSO);
        graphicsSettings.OnGraphicsChanged += HandleGraphicsChanged;
        
        // 绑定UI事件
        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.AddListener(value => graphicsSettings.FullscreenMode = value);
            
        if (resolutionDropdown != null)
            resolutionDropdown.onValueChanged.AddListener(value => graphicsSettings.ResolutionIndex = value);
            
        if (qualityDropdown != null)
            qualityDropdown.onValueChanged.AddListener(value => graphicsSettings.QualityLevel = value);
            
        if (frameRateSlider != null)
            frameRateSlider.onValueChanged.AddListener(value => graphicsSettings.TargetFrameRate = Mathf.RoundToInt(value));
        
        UpdateUI();
    }
    
    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (graphicsSettings != null)
        {
            graphicsSettings.OnGraphicsChanged -= HandleGraphicsChanged;
        }
    }
    
    private void HandleGraphicsChanged()
    {
        UpdateUI();
        ApplyGraphicsSettings();
    }
    
    public override void ResetToDefault()
    {
        ((BaseSettings<GraphicsSettings.GraphicsData, GraphicsSettingsSO>)graphicsSettings).ResetToDefault();
        Save();
        UpdateUI();
        ApplyGraphicsSettings();
    }
    
    /// <summary>
    /// 应用图形设置到系统
    /// </summary>
    private void ApplyGraphicsSettings()
    {
        graphicsSettings.ApplyGraphicsSettings();
    }
    
    /// <summary>
    /// 更新UI控件
    /// </summary>
    private void UpdateUI()
    {
        if (fullscreenToggle != null)
            fullscreenToggle.isOn = graphicsSettings.FullscreenMode;
            
        if (resolutionDropdown != null && resolutionDropdown.options.Count > graphicsSettings.ResolutionIndex)
            resolutionDropdown.value = graphicsSettings.ResolutionIndex;
            
        if (qualityDropdown != null)
            qualityDropdown.value = graphicsSettings.QualityLevel;
            
        if (frameRateSlider != null)
            frameRateSlider.value = graphicsSettings.TargetFrameRate;
            
        if (frameRateText != null)
            frameRateText.text = graphicsSettings.TargetFrameRate.ToString();
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
                float refreshRate = (float)resolution.refreshRateRatio.numerator / resolution.refreshRateRatio.denominator;
                string option = $"{resolution.width} x {resolution.height} @{refreshRate:F2}Hz";
                options.Add(option);
            }
            
            resolutionDropdown.AddOptions(options);
        }
    }
}