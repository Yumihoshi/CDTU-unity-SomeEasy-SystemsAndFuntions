# Unity简易系统与功能集合

CN 中文 | [🌏 English](README.md)

这是一个为Unity项目设计的简易可复用系统与功能的集合。该仓库包含了即插即用的模块，可以帮助您简化Unity游戏开发流程。

## 可用系统

### 1. [设置管理系统(SaveSettingsSystem)](https://github.com/whatevertogo/Unity-SaveSystem)

#### 将来我可能会添加另外两个系统，一个保存游戏数据(不久的将来)，另一个保存游戏的大数据（稍微有点难，所以可能需要很长时间）

#### 如果真的要用可以用已经及其完善的[FlexiArchiveSYstem](https://github.com/wenen-creator/FlexiArchiveSystem),写的极好,就是职责分离的太开了,太晕了.

#### 有一些例子也被添加其中(LearnAndDoPls/Systems/SaveSystem/SaveSettingsSystem/PlayerPrefsForSettings/SettingsExamples)

#### 但是请不要用这个保存你的游戏数据，因为这个方式保存游戏数据(数据稍微大点)并不好。你可以用它来保存游戏的设置，比如音频、图形等设置。

#### 如果你也有一些良好的代码优秀的例子，请添加进去，阿里嘎多阔塞伊马斯.

一个灵活的Unity设置管理系统，用于处理游戏设置并将其保存到PlayerPrefs中。功能特点包括：

- 通过ScriptableObject进行通用设置管理
- 自动序列化和持久化
- 事件驱动架构
- 支持多种设置类型（音频、图形等）
- 类型安全的设置访问
- 内置错误处理和验证

但是请不要用这个保存你的游戏数据，因为这个方式保存游戏数据(数据稍微大点)并不好。你可以用它来保存游戏的设置，比如音频、图形等设置。

### 2. [对话系统(DialogueSystem)](https://github.com/whatevertogo/Unity-DialogueSystem)

一个基于MVC架构的模块化Unity对话系统。该系统通过Control-Controller,可选接口 模式和接口设计实现了高度的扩展性和维护性。功能特点包括：

- MVC架构设计，实现了逻辑、数据和视图的分离
- 基于接口的对话控制器设计（IBranchingDialogue和IVoiceDialogue）
- 支持多种对话形式：
  - 线性对话：基础的顺序对话展示
  - 分支对话：支持多选项分支和动态切换
  - 语音对话：支持与文本同步的语音播放
  - 自定义对话
- 完整的生命周期事件系统
- 打字机效果和自动播放支持
- ScriptableObject based DialogueSO数据管理
- 编辑器友好的配置界面

### 3. [UGUI的UI系统](LearnAndDoPls/Systems/UI-System-for-UGUI/README.md)

一个为Unity的UGUI设计的整洁、结构化的UI管理系统。功能特点包括：

- 基于视图的UI架构
- UI层级管理
- 简易的视图转换
- 视图生命周期管理
- 对话面板自动化生成
- 组件引用的自动绑定
- 完整的UI事件系统

代码由[@Yuan-Zzzz](https://github.com/Yuan-Zzzz)编写，我修复了一些bug并添加了更多注释使其更易读。

### 4.[HexGridSystem](https://github.com/whatevertogo/HexGridSystem)

这个项目是一个六边形网格系统，可用于在Unity游戏引擎中创建六边形地图。它提供了一组C#脚本，包括HexCell、HexCoordinates、HexGrid、HexGridHighlight、HexMesh和HexMetrics等，可以帮助开发者快速构建六边形网格地图。

主要功能点
HexCell: 定义六边形网格单元的基本属性和行为
HexCoordinates: 实现立方体坐标系统，便于六边形网格的定位和计算
HexGrid: 管理六边形网格的创建、布局和更新
HexGridHighlight: 提供网格单元的高亮和选择功能
HexMesh: 动态生成六边形网格的网格模型，支持定制外观
HexMetrics: 定义六边形几何参数和常量，确保网格一致性

### 5.[Timer](https://github.com/whatevertogo/Timer)

- 一个用 C# 实现的 “定时器系统”库，用于游戏/Unity 场景中的时间管理（提交说明中提到移除了 Unity 的 .meta 文件和示例 TimerExample，文件/接口命名和实现都与 Unity 或游戏计时相关）。
主要组件（基于初始提交和后续说明）：
ITimerSystem / ITimerEntity 接口：定义计时系统和定时器实体的契约。
TimerEntity：定时器实体实现，用于封装单个定时器的状态和回调。
TimerSystemManager：集中管理定时器的系统，负责调度、刷新和关闭。
TimerExtensions：便捷的扩展方法，简化定时器调用和绑定。
TimerObjectPool：对象池实现，用于减少频繁分配的开销。
TimerCallback：回调封装/类型定义。
TimerExample：示例用法/演示。

### 6.[支持多Logic和单View绑定的基于UGUI的UI系统](https://github.com/whatevertogo/UnityUGUI-UISystem)

一个 轻量级 Unity UI 框架示例，包括 UI 管理器、基础视图类、示例视图，以及实现了 UI 逻辑与视图分离的机制。
使用编辑器工具自动生成对应模板。
适合作为 UI 架构参考，以及小型游戏或项目的快速集成模板。

## [工具库(Utils)](LearnAndDoPls/Utils/README.zh-CN_Utils.md)

一个全面的实用工具和辅助类集合：

#### 对象池系统
- 高效的对象重用系统
- 自动池扩展
- 内存管理优化
- 特别适用于频繁生成的对象：
  - 投射物
  - 粒子效果
  - 敌人
  - 可收集物品

#### 核心工具

- 单例模式实现
- 扩展方法集合

#### 事件总线
-我习惯于用来不同系统之间的解耦

## [实用技能(Utility Skills)](LearnAndDoPls/SomeSkills/README.CN_SomeSkills.md)

这些脚本展示了一些C#的实用技能：

- GameInput - 输入处理工具
- PlayerController - 基本的玩家移动和控制
- TriggerObject - 轻松处理基于触发器的交互

## [美术资源(Art)](Art/README.CN_Art.md)

视觉增强工具和着色器：

- 自定义着色器集合
- 材质工具
- 渲染优化工具
- 资产管理系统

由[@Yumihoshi](https://github.com/Yumihoshi)编写，来自原版[艺术资产管理](https://github.com/Yumihoshi/Art-Asset-Management)项目。



## 贡献

@whatevertogo 欢迎任何人指出我代码的不足之处，帮助我提高。

祝编码愉快！
