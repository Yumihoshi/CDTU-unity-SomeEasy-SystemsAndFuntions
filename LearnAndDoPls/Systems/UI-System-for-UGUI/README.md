# UI-System-for-UGUI

作者: @Yuan-Zzzz

一个基于UGUI的Unity UI框架，由Yuan-Zzzz在游戏项目开发过程中迭代开发，旨在提高UI开发效率，简化UI管理流程。

[English](README.EN.md)

## 系统特点

- **栈式UI管理**：每个层级使用栈管理UI面板，自动处理UI面板的打开、关闭和层级关系
- **层级管理**：通过UILayer实现多层级UI管理，包括场景层、背景层、普通层、信息层、顶层和提示层
- **生命周期管理**：完整的UI生命周期（OnOpen、OnResume、OnPause、OnClose、OnUpdate）
- **数据驱动**：支持IUIData接口进行数据传递，实现UI与数据的解耦
- **编辑器扩展**：提供可视化UI管理工具，快速创建和编辑UI面板
- **编辑器组件绑定**：可视化的UI组件选择和绑定工具，自动生成绑定代码
- **异常处理机制**：完善的错误检查和异常处理，提供清晰的错误提示

## 核心功能

### 1. UI控制器的自动化生成

- 可视化UI面板创建工具
- 一键生成UI预制体和控制器代码
- 自动创建标准生命周期方法模板
- 支持自定义模板和代码生成规则

### 2. UI组件绑定流程自动化

- 可视化组件选择与绑定
- 支持复杂组件类型选择和验证
- 自动生成类型安全的组件引用和绑定代码
- 支持嵌套UI组件绑定和组件路径检查

### 3. 栈式UI管理

- 分层级的UI栈管理
- 自动处理UI面板的层级关系
- 支持同层级UI的暂停和恢复
- 支持获取当前层级的顶部面板
- 内置面板状态检查和冲突处理

## 系统架构

```
UI-System-for-UGUI/
├── Runtime/                   # 运行时核心代码
│   ├── UIView.cs             # UI视图基类
│   ├── UIViewController.cs   # UI控制器组件
│   ├── UISystem.cs          # UI系统管理类
│   ├── UILayer.cs           # UI层级枚举定义
│   ├── UIConfig.cs          # UI系统配置
│   └── IUIData.cs           # UI数据接口
├── Editor/                   # 编辑器扩展代码
│   ├── UIViewEditor.cs      # UI视图编辑器
│   └── UIViewManager.cs     # UI管理器窗口
└── Extension/               # 扩展功能代码
    └── UIViewExtension.cs   # UI视图扩展方法
```

## 使用方法

### 1. 创建UI面板

1. 打开UI管理器窗口：`菜单 -> Framework -> UI -> UIViewManager`
2. 点击"创建新的UI面板"按钮
3. 输入UI面板名称（如`LoginPanel`）并确认
4. 系统将自动创建预制体和对应脚本

### 2. 编辑UI面板

1. 在UI管理器中选择要编辑的UI面板
2. 使用控件列表面板查看和选择UI组件
3. 点击组件旁的"+"按钮将其添加到绑定列表
4. 在绑定列表中选择需要绑定的组件类型
5. 点击"生成UI绑定代码"生成绑定代码

### 3. 打开UI面板

```csharp
// 打开普通UI面板
UISystem.Instance.OpenPanel<LoginPanel, NoneUIData>(NoneUIData.noneUIData, UILayer.NormalLayer);

// 打开需要数据的UI面板
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

// 打开场景UI
UISystem.Instance.OpenViewInScene<CharacterPreviewPanel, CharacterData>(characterData);
```

### 4. 编写UI面板逻辑

```csharp
public partial class LoginPanel : UIView
{
    // 自动生成的控件绑定部分
    private Button loginButton;
    private InputField usernameInput;
    private InputField passwordInput;
    
    private void BindUI()
    {
        // 自动生成的组件绑定代码
        loginButton = GetOrAddComponentInChildren<Button>("LoginButton");
        usernameInput = GetOrAddComponentInChildren<InputField>("UsernameInput");
        passwordInput = GetOrAddComponentInChildren<InputField>("PasswordInput");
    }
    
    public override void OnOpen()
    {
        // 绑定UI组件
        BindUI();
        // 添加事件监听
        loginButton.onClick.AddListener(OnLoginButtonClick);
    }
    
    public override void OnResume()
    {
        // 当面板从暂停状态恢复时调用
    }
    
    public override void OnPause()
    {
        // 当面板被其他面板覆盖时调用
    }
    
    public override void OnClose()
    {
        // 移除事件监听
        loginButton.onClick.RemoveListener(OnLoginButtonClick);
    }
    
    public override void OnUpdate()
    {
        // 每帧更新逻辑（由UIViewController触发）
    }
    
    private void OnLoginButtonClick()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;
        // 登录逻辑...
    }
}
```

### 5. 关闭UI面板

```csharp
// 关闭顶层面板
UISystem.Instance.CloseTopPanel(UILayer.NormalLayer);

// 关闭场景UI
sceneUIView.CloseInSceneLayer();

// 关闭所有面板
UISystem.Instance.CloseAll();
```

## 最佳实践

1. **UI层级管理**
   - 场景层(SceneLayer)：场景内的UI元素
   - 背景层(BackgroundLayer)：全屏背景、主菜单背景
   - 普通层(NormalLayer)：主要功能面板
   - 信息层(InfoLayer)：信息展示、状态面板
   - 顶层(TopLayer)：弹窗、对话框
   - 提示层(TipLayer)：提示、通知、工具提示

2. **数据驱动**
   - 为每个需要数据的UI面板创建对应的IUIData实现类
   - 使用IsValid()方法验证数据有效性
   - 使用NoneUIData.noneUIData作为无数据UI面板的参数

3. **UI生命周期**
   - OnOpen：初始化UI、绑定事件
   - OnResume：恢复UI状态、重新订阅事件
   - OnPause：暂停UI状态、暂停部分功能
   - OnClose：清理资源、移除事件订阅
   - OnUpdate：需要每帧更新的UI逻辑

## 注意事项

1. UI预制体需要包含UIViewController组件
2. 确保正确设置了UIConfig中的路径配置
3. UI脚本和绑定代码会自动生成到配置的目录中
4. 使用UISystem.Instance单例访问UI系统功能
5. 在调用OnOpen()之前必须先调用SetData()传入数据
