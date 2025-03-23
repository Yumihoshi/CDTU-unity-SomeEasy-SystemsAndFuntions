using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using SaveSystem;

public class GraphicsManager : BaseSettingsManager<GraphicsSettings>
{
    [SerializeField] private bool dontDestroyOnLoad = false;
    
    [Header("图形设置数据")]
    [SerializeField] private GraphicsSettingsSO settingsSO;
    public GraphicsSettings graphicsSettings;

    [Header("UI控制组件")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Dropdown qualityDropdown;
    [SerializeField] private Slider frameRateSlider;
    [SerializeField] private Text frameRateText;

    private Resolution[] availableResolutions;

    protected override void Awake()
    {
        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
        
        // 初始化分辨率选项要在InitializeSettings之前，因为设置可能需要使用分辨率列表
        InitializeResolutionOptions();
        
        base.Awake(); // 调用基类的Awake，它会初始化设置并自动注册
    }

    protected override void InitializeSettings()
    {
        if (settingsSO == null)
        {
            AllSettingsManager.Logger.LogError($"{nameof(GraphicsManager)}: settingsSO is not assigned!");
            return;
        }
        
        graphicsSettings = new GraphicsSettings(settingsSO);
        graphicsSettings.OnGraphicsChanged += HandleGraphicsChanged;

        // 绑定UI事件
        BindUIControls();
        UpdateUI();
    }
    
    protected override void OnDestroy()
    {
        if (graphicsSettings != null)
        {
            graphicsSettings.OnGraphicsChanged -= HandleGraphicsChanged;
        }
        base.OnDestroy(); // 调用基类的OnDestroy，它会自动注销
    }
    
    private void BindUIControls()
    {
        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.AddListener(value => graphicsSettings.FullscreenMode = value);

        if (resolutionDropdown != null)
            resolutionDropdown.onValueChanged.AddListener(value => graphicsSettings.ResolutionIndex = value);

        if (qualityDropdown != null)
            qualityDropdown.onValueChanged.AddListener(value => graphicsSettings.QualityLevel = value);

        if (frameRateSlider != null)
            frameRateSlider.onValueChanged.AddListener(value => {
                int frameRate = Mathf.RoundToInt(value);
                graphicsSettings.TargetFrameRate = frameRate;
                if (frameRateText != null)
                    frameRateText.text = frameRate.ToString();
            });
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