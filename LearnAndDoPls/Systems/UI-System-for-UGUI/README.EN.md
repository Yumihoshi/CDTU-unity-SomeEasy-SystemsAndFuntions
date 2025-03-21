# UI-System-for-UGUI Written by @Yuan-Zzzz

A UGUI-based Unity UI framework, developed iteratively by Yuan-Zzzz during game project development, aimed at improving UI development efficiency and simplifying UI management processes.

[中文](README.md)

## System Features

- **Stack-based UI Management**: Each layer uses a stack to manage UI panels, automatically handling opening, closing, and hierarchy relationships
- **Layer Management**: Multi-level UI management through UILayer, including Scene Layer, Background Layer, Normal Layer, Info Layer, Top Layer, and Tip Layer
- **Lifecycle Management**: Complete UI lifecycle (OnOpen, OnResume, OnPause, OnClose, OnUpdate)
- **Data-Driven**: Supports IUIData interface for data transmission, decoupling UI and data
- **Editor Extensions**: Provides visual UI management tools for quick creation and editing of UI panels
- **Editor Component Binding**: Visual UI component selection and binding tools, automatic binding code generation
- **Exception Handling**: Comprehensive error checking and exception handling with clear error messages

## Core Functions

### 1. UI Controller Automation
- Visual UI panel creation tools
- One-click generation of UI prefabs and controller code
- Automatic creation of standard lifecycle method templates
- Support for custom templates and code generation rules

### 2. UI Component Binding Automation
- Visual component selection and binding
- Support for complex component type selection and validation
- Auto-generation of type-safe component references and binding code
- Support for nested UI component binding and path validation

### 3. Stack-based UI Management
- Layer-based UI stack management
- Automatic handling of UI panel hierarchies
- Support for pausing and resuming same-layer UIs
- Support for getting current layer's top panel
- Built-in panel state checking and conflict resolution

## System Architecture

```
UI-System-for-UGUI/
├── Runtime/                   # Runtime core code
│   ├── UIView.cs             # UI view base class
│   ├── UIViewController.cs   # UI controller component
│   ├── UISystem.cs          # UI system management class
│   ├── UILayer.cs           # UI layer enum definition
│   ├── UIConfig.cs          # UI system configuration
│   └── IUIData.cs           # UI data interface
├── Editor/                   # Editor extension code
│   ├── UIViewEditor.cs      # UI view editor
│   └── UIViewManager.cs     # UI manager window
└── Extension/               # Extension code
    └── UIViewExtension.cs   # UI view extension methods
```

## Usage

### 1. Create UI Panel

1. Open the UI manager window: `Menu -> Framework -> UI -> UIViewManager`
2. Click the "Create New UI Panel" button
3. Enter the UI panel name (e.g., `LoginPanel`) and confirm
4. The system will automatically create a prefab and corresponding script

### 2. Edit UI Panel

1. Select the UI panel to edit in the UI manager
2. Use the control list panel to view and select UI components
3. Click the "+" button next to components to add them to the binding list
4. Select the component type to bind in the binding list
5. Click "Generate UI Binding Code" to generate binding code

### 3. Open UI Panel

```csharp
// Open a normal UI panel
UISystem.Instance.OpenPanel<LoginPanel, NoneUIData>(NoneUIData.noneUIData, UILayer.NormalLayer);

// Open a UI panel that requires data
public class PlayerInfoData : IUIData
{
    public string playerName;
    public int level;
    public float hp;
    
    public bool IsValid()
    {
        return !string.IsNullOrEmpty(playerName);
    }
}

var playerData = new PlayerInfoData { 
    playerName = "Player1", 
    level = 10, 
    hp = 100 
};
UISystem.Instance.OpenPanel<PlayerInfoPanel, PlayerInfoData>(playerData, UILayer.InfoLayer);

// Open a scene UI
UISystem.Instance.OpenViewInScene<CharacterPreviewPanel, CharacterData>(characterData);
```

### 4. Write UI Panel Logic

```csharp
public partial class LoginPanel : UIView
{
    // Automatically generated binding fields
    private Button loginButton;
    private InputField usernameInput;
    private InputField passwordInput;
    
    private void BindUI()
    {
        // Automatically generated binding code
        loginButton = GetOrAddComponentInChildren<Button>("LoginButton");
        usernameInput = GetOrAddComponentInChildren<InputField>("UsernameInput");
        passwordInput = GetOrAddComponentInChildren<InputField>("PasswordInput");
    }
    
    public override void OnOpen()
    {
        // Bind UI components
        BindUI();
        // Add event listeners
        loginButton.onClick.AddListener(OnLoginButtonClick);
    }
    
    public override void OnResume()
    {
        // Called when the panel resumes from a paused state
    }
    
    public override void OnPause()
    {
        // Called when the panel is covered by another panel
    }
    
    public override void OnClose()
    {
        // Remove event listeners
        loginButton.onClick.RemoveListener(OnLoginButtonClick);
    }
    
    public override void OnUpdate()
    {
        // Per-frame update logic (triggered by UIViewController)
    }
    
    private void OnLoginButtonClick()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;
        // Login logic...
    }
}
```

### 5. Close UI Panel

```csharp
// Close the top panel
UISystem.Instance.CloseTopPanel(UILayer.NormalLayer);

// Close a scene UI
sceneUIView.CloseInSceneLayer();

// Close all panels
UISystem.Instance.CloseAll();
```

## Best Practices

1. **UI Layer Management**
   - Scene Layer: UI elements within the scene
   - Background Layer: Full-screen backgrounds, main menu backgrounds
   - Normal Layer: Main function panels
   - Info Layer: Information display, status panels
   - Top Layer: Pop-ups, dialog boxes
   - Tip Layer: Hints, notifications, tooltips

2. **Data-Driven**
   - Create an IUIData implementation class for each UI panel that needs data
   - Use IsValid() method to validate data
   - Use NoneUIData.noneUIData for UI panels without data

3. **UI Lifecycle**
   - OnOpen: Initialize UI, bind events
   - OnResume: Restore UI state, resubscribe to events
   - OnPause: Pause UI state, pause some functionality
   - OnClose: Clean up resources, remove event subscriptions
   - OnUpdate: UI logic that needs to be updated every frame

## Notes

1. UI prefabs must include the UIViewController component
2. Ensure correct path configuration in UIConfig
3. UI scripts and binding code will be automatically generated to the configured directories
4. Access UI system functionality through UISystem.Instance singleton
5. Must call SetData() to pass data before calling OnOpen()