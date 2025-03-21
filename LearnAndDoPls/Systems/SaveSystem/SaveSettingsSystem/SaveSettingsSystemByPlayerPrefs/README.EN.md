# SaveSettingsSystem User Guide

SaveSettingsSystem is a settings management system for Unity that provides a unified way to handle various game settings (such as audio, graphics, etc.) and save them to PlayerPrefs.

![How to Use](images/image.png)
![Design Concept](images/image1.png)
![Generic Constraints and Dependencies](images/image2.png)
![AudioSettings Example](images/image3.png)

## Core Features

1. Unified settings management interface
2. Automatic serialization and persistence
3. Type-safe settings access
4. Event-driven settings updates
5. Support for default values and reset functionality
6. UI binding support

## System Architecture

### 1. Core Interfaces and Base Classes

#### ISaveSettings Interface
```csharp
public interface ISaveSettings
{
    event EventHandler SettingsChanged;
    void Save();
    void Load();
    void ResetToDefault();
}
```
- Defines basic settings operations: save, load, reset
- Provides settings change event notification mechanism

#### BaseSettings<TData, TSettingsSO>

- Abstract base class for all concrete settings classes
- Implements common serialization and persistence logic
- Provides settings change event handling
- Type parameters:
  - TData: Settings data type (must be serializable)
  - TSettingsSO: ScriptableObject settings type

#### BaseSettingsManager<TSettings>

- Abstract base class for managing specific settings instances
- Implements singleton pattern
- Handles UI binding and event propagation

### 2. Implementation Examples

#### Audio Settings System

##### AudioSettingsSO (Data Container)
```csharp
[CreateAssetMenu(fileName = "AudioVolumeSettingsSO", menuName = "Settings/Audio SettingsSO")]
public class AudioSettingsSO : ScriptableObject
{
    public float masterVolume = 1f;
    public float bgmVolume = 1f;
    public float sfxVolume = 1f;
}
```

##### AudioSettings (Settings Logic)

- Inherits from BaseSettings<AudioVolumeData, AudioSettingsSO>
- Implements volume control logic
- Provides actual volume calculation methods

##### AudioManager (Manager)

- Inherits from BaseSettingsManager<AudioSettings>
- Manages audio sources and audio clips
- Handles UI interaction and volume updates

#### Graphics Settings System

##### GraphicsSettingsSO (Data Container)
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

##### GraphicsSettings (Settings Logic)

- Inherits from BaseSettings<GraphicsData, GraphicsSettingsSO>
- Implements graphics settings logic
- Provides resolution and quality setting methods

##### GraphicsManager (Manager)

- Inherits from BaseSettingsManager<GraphicsSettings>
- Manages resolution options
- Handles UI interaction and graphics settings updates

### Scene-Specific Settings Support

The system now supports both global and scene-specific settings management, particularly useful for the AudioManager:

#### Configurable DontDestroyOnLoad
```csharp
public class AudioManager : BaseSettingsManager<AudioSettings>
{
    [SerializeField] private bool dontDestroyOnLoad = false; // Toggle for DontDestroyOnLoad behavior
    
    // ...configuration logic...
}
```

This feature allows you to:
- Set up scene-specific audio settings by disabling DontDestroyOnLoad
- Maintain global settings across scenes by enabling DontDestroyOnLoad
- Avoid settings conflicts between different scenes
- Support specialized audio configurations for specific scenes

#### Best Practices for Scene-Specific Settings

1. **Global Settings**
   - Enable dontDestroyOnLoad in the inspector for managers that need to persist
   - Use this for main menu, global BGM, etc.

2. **Scene-Specific Settings**
   - Disable dontDestroyOnLoad for scene-specific managers
   - Useful for level-specific audio, specialized configurations
   - Managers will be destroyed when leaving the scene

3. **Configuration Tips**
   - Consider scene requirements when deciding persistence
   - Document which scenes use global vs local settings
   - Test scene transitions to ensure proper behavior

## Usage Flow

### 1. Create Settings Data Container
```csharp
// 1. Create ScriptableObject asset
[CreateAssetMenu(fileName = "YourSettingsSO", menuName = "Settings/Your Settings")]
public class YourSettingsSO : ScriptableObject
{
    public float someValue = 1f;
    // Add your settings fields
}
```

### 2. Implement Settings Class
```csharp
public class YourSettings : BaseSettings<YourData, YourSettingsSO>
{
    public YourSettings(YourSettingsSO settings) : base(settings, "YourSettings")
    {
    }
    
    // Implement required methods
    protected override YourData GetDataFromSettings() { ... }
    protected override void ApplyDataToSettings(YourData data) { ... }
    public override void ResetToDefault() { ... }
}
```

### 3. Create Manager
```csharp
public class YourManager : BaseSettingsManager<YourSettings>
{
    [SerializeField] private YourSettingsSO settingsSO;
    
    protected override void InitializeSettings()
    {
        settings = new YourSettings(settingsSO);
        // Add event listeners and initialization logic
    }
}
```

## Data Flow

1. **User Interaction Trigger**
   - UI components (sliders, toggles, etc.) trigger value changes
   - Manager receives these changes and passes them to corresponding settings class

2. **Settings Update**
   - Settings class validates and updates internal data
   - Triggers specific change events (like OnVolumeChanged)
   - Triggers general SettingsChanged event

3. **Data Persistence**
   - BaseSettings serializes data to JSON
   - Saves to local storage via PlayerPrefs
   - Notifies listeners after successful save

4. **Event Notification Chain**
   - Settings class triggers its own change events
   - Manager receives and processes these events
   - UI updates to reflect new settings values

## Best Practices

1. **Type Safety**
   - Use strongly typed settings data classes
   - Ensure data classes are serializable

2. **Event Handling**
   - Properly subscribe and unsubscribe from events
   - Clean up event listeners in OnDestroy

3. **Value Validation**
   - Perform range checks before setting values
   - Use Mathf.Clamp and similar methods to ensure valid values

4. **UI Binding**
   - Initialize UI components in Start
   - Use UnityEvent to simplify UI binding

5. **Error Handling**
   - Gracefully handle load failures
   - Provide sensible default values
   - Use try-catch to catch potential exceptions

## Usage Examples

```csharp
// Load settings at game startup
## Example Usage

### 1. Basic Settings Management

```csharp
// Load settings at game startup
void Start()
{
    // Ensure settings are loaded before use
    settingsManager.Load();
    
    // Subscribe to settings change event
    if (settingsManager.Settings is ISaveSettings settings)
    {
        settings.SettingsChanged += OnSettingsChanged;
    }
}

void OnDestroy()
{
    // Clean up event subscriptions
    if (settingsManager.Settings is ISaveSettings settings)
    {
        settings.SettingsChanged -= OnSettingsChanged;
    }
}

// Handle settings changes
private void OnSettingsChanged(object sender, EventArgs e)
{
    // Update UI or other related logic
    UpdateUI();
    // Automatically save settings
    settingsManager.Save();
}

// Manually trigger settings save
public void OnSettingChanged()
{
    settingsManager.Save();
}

// Reset to default settings
public void ResetSettings()
{
    settingsManager.ResetToDefault();
    UpdateUI(); // Ensure UI reflects new settings
}
```

### 2. UI Binding Example

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
        // Volume slider
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        
        // Fullscreen toggle
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
        
        // Quality settings
        qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
    }
    
    private void LoadAndApplySettings()
    {
        // Load settings and update UI
        audioManager.Load();
        graphicsManager.Load();
        
        // Update UI display
        UpdateUIValues();
    }
    
    private void UpdateUIValues()
    {
        // Update UI controls with current settings
        masterVolumeSlider.value = audioManager.Settings.MasterVolume;
        fullscreenToggle.isOn = graphicsManager.Settings.FullscreenMode;
        qualityDropdown.value = graphicsManager.Settings.QualityLevel;
    }
    
    // UI event handling
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

### 3. Custom Settings Example

```csharp
// Custom settings data
[System.Serializable]
public class GameplayData
{
    public float gameDifficulty = 1f;
    public bool tutorialEnabled = true;
    public string lastSelectedCharacter = "Default";
}

// Custom settings SO
[CreateAssetMenu(fileName = "GameplaySettingsSO", menuName = "Settings/Gameplay Settings")]
public class GameplaySettingsSO : ScriptableObject
{
    public float gameDifficulty = 1f;
    public bool tutorialEnabled = true;
    public string lastSelectedCharacter = "Default";
}

// Custom settings class
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

### 4. Error Handling Example

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
            Debug.LogError($"Failed to initialize settings: {e.Message}");
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
            Debug.LogWarning($"Failed to load settings, using default values: {e.Message}");
            settings.ResetToDefault();
        }
    }
    
    private void HandleInitializationError()
    {
        // Create emergency settings
        var emergencySettings = ScriptableObject.CreateInstance<GameplaySettingsSO>();
        settings = new GameplaySettings(emergencySettings);
        settings.ResetToDefault();
        
        // Notify user
        Debug.LogWarning("Using emergency settings configuration");
    }
    
    public void SaveWithBackup()
    {
        try
        {
            // Create backup before saving
            var currentData = settings.GetDataFromSettings();
            string backupKey = $"{settings.SettingsKey}_backup";
            Save_Load_SettingsSystem_Functions.SaveByPlayerPrefs(backupKey, currentData);
            
            // Perform actual save
            settings.Save();
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save settings: {e.Message}");
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
                Debug.Log("Settings restored from backup");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to restore from backup: {e.Message}");
            settings.ResetToDefault();
        }
    }
}
```

### 5. Settings Migration Example

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
                    Debug.LogWarning($"Unknown settings version: {oldVersion}, resetting to default values");
                    manager.Settings.ResetToDefault();
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to migrate settings: {e.Message}");
            manager.Settings.ResetToDefault();
        }
    }
    
    private static void MigrateFromV1ToV2(BaseSettingsManager<GameplaySettings> manager)
    {
        // Example migration logic
        var settings = manager.Settings;
        // Perform migration operations...
        Debug.Log("Settings successfully migrated to V2");
    }
}
```

### Usage Recommendations

1. **Performance Optimization**
   - Avoid frequent saves, consider using debouncing
   - Batch process large settings changes
   - Serialize data only when necessary

2. **Security**
   - Validate loaded data
   - Implement settings backup mechanism
   - Handle version migrations

3. **Maintainability**
   - Use constants to manage settings keys
   - Implement detailed logging
   - Keep settings classes single-responsibility

4. **User Experience**
   - Provide settings preview functionality
   - Implement undo/redo functionality
   - Add settings import/export functionality