# 实用技能(SomeSkills)

一系列可用于任何Unity项目的实用脚本集合。这些组件被设计为模块化和易于集成到您的游戏中。
全是示例，只是提供思想，具体实现可以根据需要进行修改和扩展。

## 可用组件

### 事件管理器(EventManager)

一个集中化的事件管理系统，使用对象池来优化事件参数对象。

**主要特点：**
- 对事件参数使用对象池以减少垃圾回收
- 自动重置和回收事件参数
- 统一管理所有游戏事件
- 单例模式便于访问

**使用示例：**
```csharp
// 订阅事件
EventManager.Instance.OnTriggerObjectSelected += HandleObjectSelected;

// 触发事件
EventManager.Instance.TriggerObjectSelected(selectedObject);

// 事件处理器
private void HandleObjectSelected(object sender, EventManager.TriggerObjectSelectedEventArgs args)
{
    TriggerObject selectedObject = args.SelectedObject;
    // 对选中的对象进行操作
}
```

### 游戏输入(GameInput)

一个灵活的输入处理系统，将输入检测与游戏逻辑分离。

**主要特点：**
- 同时处理鼠标和键盘输入
- 可配置的输入设置
- 常用输入动作的事件
- 易于扩展以满足自定义输入需求

**使用示例：**
```csharp
// 获取引用
private GameInput _gameInput;

// 初始化
_gameInput = GetComponent<GameInput>();
_gameInput.OnClick += HandleClick;

// 处理输入事件
private void HandleClick(object sender, EventArgs e)
{
    // 响应点击事件
}
```

### 玩家控制器(PlayerController)

一个模块化的玩家控制器，处理游戏世界中的移动和与对象的交互。

**主要特点：**
- 具有可配置速度和旋转的角色移动
- 对象选择和交互系统
- 与EventManager和GameInput系统集成
- 可定制的移动行为

**使用示例：**
```csharp
// 在Inspector中配置
// - 设置移动速度
// - 设置旋转速度
// - 连接到导航系统
// - 设置交互参数

// 在代码中获取引用
PlayerController playerController = GetComponent<PlayerController>();

// 使用方法
playerController.MoveToPosition(targetPosition);
```

### 触发对象(TriggerObject)

所有游戏中交互对象的抽象基类。

**主要特点：**
- 为所有交互对象定义基本功能
- 提供交互范围和状态控制
- 为选择和交互事件提供虚拟方法
- 易于扩展以创建特定的交互对象

**使用示例：**
```csharp
// 创建派生类
public class Collectible : TriggerObject
{
    public override void Interact()
    {
        // 自定义交互行为
        CollectItem();
        base.Interact();
    }
    
    public override void OnSelected()
    {
        // 高亮对象
        ShowHighlight();
        base.OnSelected();
    }
    
    public override void OnDeselected()
    {
        // 移除高亮
        HideHighlight();
        base.OnDeselected();
    }
}
```

## 集成方式

这些组件被设计为可以一起工作，但也可以单独使用：

1. **EventManager** 提供通信骨干
2. **GameInput** 捕获玩家输入并触发事件
3. **PlayerController** 消费输入事件并处理玩家动作
4. **TriggerObject** 为游戏对象定义交互接口

## 最佳实践

1. **使用EventManager** 处理所有游戏事件以保持清晰的架构
2. **扩展TriggerObject** 来创建游戏中所有可交互对象
3. **在Inspector中配置PlayerController** 以适应不同的角色类型
4. **如需额外的输入处理，可以自定义GameInput**

## 系统需求

- Unity 2019.4 或更新版本
- 不需要额外的包依赖

## 示例场景

要查看这些组件的实际运行效果，请查看仓库中的示例场景。