# Unity简易系统与功能集合
# 成工CDTU出品，匠心之作

CN 中文 | [🌏 English](README.md)

这是一个为Unity项目设计的简易可复用系统与功能的集合。该仓库包含了即插即用的模块，可以帮助您简化Unity游戏开发流程。

## 可用系统

### 1. [设置管理系统(SaveSettingsSystem)](LearnAndDoPls/Systems/SaveSystem/SaveSettingsSystem/SaveSettingsSystemByPlayerPrefs/README.CN.md)

#### 将来我可能会添加另外两个系统，一个保存游戏数据(不久的将来)，另一个保存游戏的大数据（稍微有点难，所以可能需要很长时间）

#### 有一些例子也被添加其中

#### 如果你也有一些良好的代码优秀的例子，请添加进去，阿里嘎多阔塞伊马斯.

一个灵活的Unity设置管理系统，用于处理游戏设置并将其保存到PlayerPrefs中。功能特点包括：

- 通过ScriptableObject进行通用设置管理
- 自动序列化和持久化
- 事件驱动架构
- 支持多种设置类型（音频、图形等）
- 类型安全的设置访问
- 内置错误处理和验证

但是请不要用这个保存你的游戏数据，因为这个方式保存游戏数据(数据稍微大点)并不好。你可以用它来保存游戏的设置，比如音频、图形等设置。

### 2. [对话系统(DialogueSystem)](LearnAndDoPls/Systems/DialogueSystem/README.CN_DialogueSystem.md)

一个模块化、易于理解的Unity对话系统。该系统采用Control-Controller模式设计，使其既高度可定制又简单易用。功能特点包括：

- 线性对话
- 带多选项的分支对话
- 语音同步对话
- 对话序列的预制体创建功能

### 3. [UGUI的UI系统](LearnAndDoPls/Systems/UI-System-for-UGUI/README.md)

一个为Unity的UGUI设计的整洁、结构化的UI管理系统。功能特点包括：

- 基于视图的UI架构
- UI层级管理
- 简易的视图转换
- 视图生命周期管理

代码由[@Yuan-Zzzz](https://github.com/Yuan-Zzzz)编写，我修复了一些bug并添加了更多注释使其更易读。

## 4. [工具库(Utils)](LearnAndDoPls/Utils/README.zh-CN_Utils.md)

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
- 扩展方法
- 常用辅助函数
- 数学工具
- 文件操作助手

## 5. [实用技能(Utility Skills)](LearnAndDoPls/SomeSkills/README.CN_SomeSkills.md)

这些脚本展示了一些C#的实用技能：

- EventManager - 用于组件间通信的简单事件系统
- GameInput - 输入处理工具
- PlayerController - 基本的玩家移动和控制
- TriggerObject - 轻松处理基于触发器的交互

## 6. [美术资源(Art)](Art/README.CN_Art.md)

视觉增强工具和着色器：

- 自定义着色器集合
- 材质工具
- 渲染优化工具
- 资产管理系统

由[@Yumihoshi](https://github.com/Yumihoshi)编写，来自原版[艺术资产管理](https://github.com/Yumihoshi/Art-Asset-Management)项目。

## 性能考虑

### 内存管理

- 对频繁创建/销毁的对象使用对象池
- 在OnDisable/OnDestroy中正确清理
- 避免游戏运行时的内存分配
- 缓存组件引用

### CPU优化

- 使用协程处理延时操作
- 实现高效的更新模式
- 利用Job系统处理重计算
- 分析和优化性能瓶颈

### 最佳实践

1. 组件组织
   - 保持组件功能单一明确
   - 使用适当的关注点分离
   - 实现接口以获得更好的抽象

2. 代码结构
   - 遵循Unity的执行顺序
   - 使用ScriptableObject进行配置
   - 实现适当的错误处理
   - 编写清晰、有文档的代码

3. 场景管理
   - 高效组织层级结构
   - 一致使用预制体
   - 实现适当的场景加载模式

## 故障排除

### 常见问题

1. 对象池问题
   ```
   问题：对象未返回池中
   解决：确保在OnDisable中正确实现池返回逻辑，检查对象是否正确禁用
   ```

2. 内存泄漏
   ```
   问题：内存使用量持续增长
   解决：使用Memory Profiler进行分析，检查事件订阅是否正确解除，确保所有资源都被正确释放
   ```

3. 性能下降
   ```
   问题：随时间推移帧率下降
   解决：使用Profiler识别性能瓶颈，实现对象池，优化Update循环，减少GC调用
   ```

4. UI系统问题
   ```
   问题：UI元素无响应
   解决：检查Canvas设置，验证EventSystem存在并正确配置，确保RayCastTarget设置正确
   ```

5. 设置系统问题
   ```
   问题：设置未能正确保存或加载
   解决：检查PlayerPrefs的键值是否正确，验证序列化数据格式，确保文件权限正确
   ```

### 调试技巧

- 启用开发版本构建以获取详细日志
- 使用Unity的Profiler进行性能分析
- 在关键代码处实现条件调试日志
- 使用Unity的Debug.DrawLine进行物理和路径可视化
- 善用断点和日志来追踪数据流
- 实现DEBUG预处理指令来控制调试功能

### 问题预防

- 遵循代码规范和最佳实践
- 定期进行性能检查和优化
- 保持良好的错误处理和日志记录
- 做好版本控制和备份
- 进行充分的单元测试和集成测试

## 使用方法

1. 只需将您需要的System文件夹复制到Unity项目中
2. 查看每个系统文件夹中的单独README文件，获取详细使用说明
3. 按照规范组织和管理项目资源

## 贡献

@whatevertogo 欢迎任何人指出我代码的不足之处，帮助我提高。

祝编码愉快！
