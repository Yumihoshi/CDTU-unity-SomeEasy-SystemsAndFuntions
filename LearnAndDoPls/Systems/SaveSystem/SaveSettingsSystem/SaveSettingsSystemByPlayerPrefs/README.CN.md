# SaveSettingsSystem 使用说明

SaveSettingsSystem 是一个用于Unity的设置管理系统，它提供了一个统一的方式来处理游戏中的各种设置（如音频、图形等），并能够将这些设置保存到PlayerPrefs中。

![如何使用](images/image.png)
![设计思路](images/image1.png)
![泛型约束和依赖依赖关系](images/image2.png)
![AudioSettings示例](images/image3.png)

## 核心功能

1. 统一的设置管理接口
2. 自动序列化和持久化
3. 类型安全的设置访问
4. 事件驱动的设置更新
5. 支持默认值和重置功能
6. UI绑定支持

## 系统架构

### 1. 核心接口和基类

#### ISaveSettings 接口
```csharp
public interface ISaveSettings
{
    event EventHandler SettingsChanged;
    void Save();
    void Load();
    void ResetToDefault();
}
```
- 定义设置系统的基本操作：保存、加载、重置
- 提供设置变更事件通知机制

#### BaseSettings<TData, TSettingsSO>
- 所有具体设置类的抽象基类
- 实现通用的序列化和持久化逻辑
- 提供设置变更事件处理
- 类型参数：
  - TData: 设置数据类型（必须可序列化）
  - TSettingsSO: ScriptableObject设置类型

#### BaseSettingsManager<TSettings>
- 管理具体设置实例的抽象基类
- 实现单例模式
- 处理UI绑定和事件传递

### 2. 实现示例

#### 音频设置系统

##### AudioSettingsSO（数据容器）
```csharp
[CreateAssetMenu(fileName = "AudioVolumeSettingsSO", menuName = "Settings/Audio SettingsSO")]
public class AudioSettingsSO : ScriptableObject
{
    public float masterVolume = 1f;
    public float bgmVolume = 1f;
    public float sfxVolume = 1f;
}
```

##### AudioSettings（设置逻辑）
- 继承自BaseSettings<AudioVolumeData, AudioSettingsSO>
- 实现音量控制逻辑
- 提供实际音量计算方法

##### AudioManager（管理器）
- 继承自BaseSettingsManager<AudioSettings>
- 管理音频源和音频剪辑
- 处理UI交互和音量更新

#### 图形设置系统

##### GraphicsSettingsSO（数据容器）
```csharp
[CreateAssetMenu(fileName = "GraphicsSettingsSO", menuName = "Settings/Graphics SettingsSO")]
public class GraphicsSettingsSO : ScriptableObject
{
    public bool fullscreenMode = true;
    public int resolutionIndex = 0;
    public int qualityLevel = 1;
    public int targetFrameRate = 60;
}
```

##### GraphicsSettings（设置逻辑）
- 继承自BaseSettings<GraphicsData, GraphicsSettingsSO>
- 实现图形设置逻辑
- 提供分辨率和质量设置方法

##### GraphicsManager（管理器）
- 继承自BaseSettingsManager<GraphicsSettings>
- 管理分辨率选项
- 处理UI交互和图形设置更新

## 使用流程

### 1. 创建设置数据容器
```csharp
// 1. 创建 ScriptableObject 资产
[CreateAssetMenu(fileName = "YourSettingsSO", menuName = "Settings/Your Settings")]
public class YourSettingsSO : ScriptableObject
{
    public float someValue = 1f;
    // 添加你的设置字段
}
```

### 2. 实现设置类
```csharp
public class YourSettings : BaseSettings<YourData, YourSettingsSO>
{
    public YourSettings(YourSettingsSO settings) : base(settings, "YourSettings")
    {
    }
    
    // 实现必要的方法
    protected override YourData GetDataFromSettings() { ... }
    protected override void ApplyDataToSettings(YourData data) { ... }
    public override void ResetToDefault() { ... }
}
```

### 3. 创建管理器
```csharp
public class YourManager : BaseSettingsManager<YourSettings>
{
    [SerializeField] private YourSettingsSO settingsSO;
    
    protected override void InitializeSettings()
    {
        settings = new YourSettings(settingsSO);
        // 添加事件监听等初始化逻辑
    }
}
```

## 数据流向

1. **用户交互触发**
   - UI组件（如滑动条、开关等）触发值变更
   - 管理器接收这些变更并传递给对应的设置类

2. **设置更新**
   - 设置类验证并更新内部数据
   - 触发特定的变更事件（如OnVolumeChanged）
   - 触发通用的SettingsChanged事件

3. **数据持久化**
   - BaseSettings将数据序列化为JSON
   - 通过PlayerPrefs保存到本地存储
   - 保存成功后通知监听者

4. **事件通知链**
   - 设置类触发自身的变更事件
   - 管理器接收并处理这些事件
   - UI更新以反映新的设置值

## 实践想法

1. **类型安全**
   - 使用强类型的设置数据类
   - 确保数据类可以序列化

2. **事件处理**
   - 正确订阅和取消订阅事件
   - 在OnDestroy中清理事件监听

3. **值验证**
   - 在设置值之前进行范围检查
   - 使用Mathf.Clamp等方法确保值的有效性

4. **UI绑定**
   - 在Start中初始化UI组件
   - 使用UnityEvent简化UI绑定

5. **错误处理**
   - 优雅处理加载失败的情况
   - 提供合理的默认值
   - 使用try-catch捕获可能的异常

## 示例用法

### 1. 基础设置管理

```csharp
// 在游戏启动时加载设置
void Start()
{
    // 确保在使用设置之前加载
    settingsManager.Load();
    
    // 订阅设置变更事件
    if (settingsManager.Settings is ISaveSettings settings)
    {
        settings.SettingsChanged += OnSettingsChanged;
    }
}

void OnDestroy()
{
    // 清理事件订阅
    if (settingsManager.Settings is ISaveSettings settings)
    {
        settings.SettingsChanged -= OnSettingsChanged;
    }
}

// 当设置发生变化时的处理
private void OnSettingsChanged(object sender, EventArgs e)
{
    // 更新UI或其他相关逻辑
    UpdateUI();
    // 自动保存设置
    settingsManager.Save();
}

// 手动触发设置保存
public void OnSettingChanged()
{
    settingsManager.Save();
}

// 重置为默认设置
public void ResetSettings()
{
    settingsManager.ResetToDefault();
    UpdateUI(); // 确保UI反映新的设置
}
```

### 2. UI绑定示例

```csharp
public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    
    private AudioManager audioManager;
    private GraphicsManager graphicsManager;
    
    void Start()
    {
        InitializeManagers();
        SetupUIListeners();
        LoadAndApplySettings();
    }
    
    private void InitializeManagers()
    {
        audioManager = AudioManager.Instance;
        graphicsManager = GraphicsManager.Instance;
    }
    
    private void SetupUIListeners()
    {
        // 音量滑块
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        
        // 全屏切换
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
        
        // 质量设置
        qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
    }
    
    private void LoadAndApplySettings()
    {
        // 加载设置并更新UI
        audioManager.Load();
        graphicsManager.Load();
        
        // 更新UI显示
        UpdateUIValues();
    }
    
    private void UpdateUIValues()
    {
        // 使用当前设置更新UI控件
        masterVolumeSlider.value = audioManager.Settings.MasterVolume;
        fullscreenToggle.isOn = graphicsManager.Settings.FullscreenMode;
        qualityDropdown.value = graphicsManager.Settings.QualityLevel;
    }
    
    // UI事件处理
    private void OnMasterVolumeChanged(float value)
    {
        audioManager.Settings.MasterVolume = value;
    }
    
    private void OnFullscreenChanged(bool isFullscreen)
    {
        graphicsManager.Settings.FullscreenMode = isFullscreen;
    }
    
    private void OnQualityChanged(int qualityLevel)
    {
        graphicsManager.Settings.QualityLevel = qualityLevel;
    }
}
```

### 3. 自定义设置示例

```csharp
// 自定义设置数据
[System.Serializable]
public class GameplayData
{
    public float gameDifficulty = 1f;
    public bool tutorialEnabled = true;
    public string lastSelectedCharacter = "Default";
}

// 自定义设置SO
[CreateAssetMenu(fileName = "GameplaySettingsSO", menuName = "Settings/Gameplay Settings")]
public class GameplaySettingsSO : ScriptableObject
{
    public float gameDifficulty = 1f;
    public bool tutorialEnabled = true;
    public string lastSelectedCharacter = "Default";
}

// 自定义设置类
public class GameplaySettings : BaseSettings<GameplayData, GameplaySettingsSO>
{
    public event Action<float> DifficultyChanged;
    
    public GameplaySettings(GameplaySettingsSO settings) : base(settings, "GameplaySettings")
    {
    }
    
    public float GameDifficulty
    {
        get => settingsSO.gameDifficulty;
        set
        {
            float clampedValue = Mathf.Clamp(value, 0.5f, 2f);
            if (!Mathf.Approximately(settingsSO.gameDifficulty, clampedValue))
            {
                settingsSO.gameDifficulty = clampedValue;
                DifficultyChanged?.Invoke(clampedValue);
                NotifySettingsChanged();
            }
        }
    }
    
    protected override GameplayData GetDataFromSettings()
    {
        return new GameplayData
        {
            gameDifficulty = settingsSO.gameDifficulty,
            tutorialEnabled = settingsSO.tutorialEnabled,
            lastSelectedCharacter = settingsSO.lastSelectedCharacter
        };
    }
    
    protected override void ApplyDataToSettings(GameplayData data)
    {
        settingsSO.gameDifficulty = data.gameDifficulty;
        settingsSO.tutorialEnabled = data.tutorialEnabled;
        settingsSO.lastSelectedCharacter = data.lastSelectedCharacter;
    }
    
    public override void ResetToDefault()
    {
        settingsSO.gameDifficulty = 1f;
        settingsSO.tutorialEnabled = true;
        settingsSO.lastSelectedCharacter = "Default";
        NotifySettingsChanged();
    }
}
```

### 4. 错误处理示例

```csharp
public class RobustSettingsManager : BaseSettingsManager<GameplaySettings>
{
    [SerializeField] private GameplaySettingsSO settingsSO;
    
    protected override void InitializeSettings()
    {
        try
        {
            settings = new GameplaySettings(settingsSO);
            LoadSettings();
        }
        catch (Exception e)
        {
            Debug.LogError($"设置初始化失败: {e.Message}");
            HandleInitializationError();
        }
    }
    
    private void LoadSettings()
    {
        try
        {
            settings.Load();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"加载设置失败，使用默认值: {e.Message}");
            settings.ResetToDefault();
        }
    }
    
    private void HandleInitializationError()
    {
        // 创建应急设置
        var emergencySettings = ScriptableObject.CreateInstance<GameplaySettingsSO>();
        settings = new GameplaySettings(emergencySettings);
        settings.ResetToDefault();
        
        // 通知用户
        Debug.LogWarning("使用应急设置配置");
    }
    
    public void SaveWithBackup()
    {
        try
        {
            // 在保存前创建备份
            var currentData = settings.GetDataFromSettings();
            string backupKey = $"{settings.SettingsKey}_backup";
            Save_Load_SettingsSystem_Functions.SaveByPlayerPrefs(backupKey, currentData);
            
            // 执行实际保存
            settings.Save();
        }
        catch (Exception e)
        {
            Debug.LogError($"保存设置失败: {e.Message}");
            TryRestoreFromBackup();
        }
    }
    
    private void TryRestoreFromBackup()
    {
        try
        {
            string backupKey = $"{settings.SettingsKey}_backup";
            var backupData = Save_Load_SettingsSystem_Functions.LoadByPlayerPrefs<GameplayData>(backupKey);
            if (backupData != null)
            {
                settings.ApplyDataToSettings(backupData);
                Debug.Log("已从备份恢复设置");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"从备份恢复失败: {e.Message}");
            settings.ResetToDefault();
        }
    }
}
```

### 5. 设置迁移示例

```csharp
public class SettingsMigrationManager
{
    private const string VERSION_KEY = "SettingsVersion";
    private const int CURRENT_VERSION = 2;
    
    public static void CheckAndMigrateSettings(BaseSettingsManager<GameplaySettings> manager)
    {
        int savedVersion = PlayerPrefs.GetInt(VERSION_KEY, 1);
        if (savedVersion < CURRENT_VERSION)
        {
            MigrateSettings(savedVersion, manager);
            PlayerPrefs.SetInt(VERSION_KEY, CURRENT_VERSION);
            PlayerPrefs.Save();
        }
    }
    
    private static void MigrateSettings(int oldVersion, BaseSettingsManager<GameplaySettings> manager)
    {
        try
        {
            switch (oldVersion)
            {
                case 1:
                    MigrateFromV1ToV2(manager);
                    break;
                default:
                    Debug.LogWarning($"未知的设置版本: {oldVersion}，重置为默认值");
                    manager.Settings.ResetToDefault();
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"设置迁移失败: {e.Message}");
            manager.Settings.ResetToDefault();
        }
    }
    
    private static void MigrateFromV1ToV2(BaseSettingsManager<GameplaySettings> manager)
    {
        // 迁移逻辑示例
        var settings = manager.Settings;
        // 执行迁移操作...
        Debug.Log("设置已成功迁移到V2");
    }
}
```

### 使用建议

1. **性能优化**
   - 避免频繁保存，考虑使用防抖动
   - 大型设置更改时批量处理
   - 仅在必要时序列化数据

2. **安全性**
   - 对加载的数据进行验证
   - 实现设置备份机制
   - 处理版本迁移

3. **可维护性**
   - 使用常量管理设置键
   - 实现详细的日志记录
   - 保持设置类职责单一

4. **用户体验**
   - 提供设置预览功能
   - 实现撤销/重做功能
   - 添加设置导入/导出功能