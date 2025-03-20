# Unity简易系统与功能集合

CN 中文 | [🌏 English](README.md)

这是一个为Unity项目设计的简易可复用系统与功能的集合。该仓库包含了即插即用的模块，可以帮助您简化Unity游戏开发流程。

## 可用系统

### 1.[对话系统(DialogueSystem)](LearnAndDoPls/Dialogue/README.CN_DialogueSystem.md)

一个模块化、易于理解的Unity对话系统。该系统采用Control-Controller模式设计，使其既高度可定制又简单易用。功能特点包括：

- 线性对话
- 带多选项的分支对话
- 语音同步对话
- 对话序列的预制体创建功能

### 2.[对象池(ObjectPool)](LearnAndDoPls/ObjectPool/README.CN_ObjectPool.md)

一个高效的对象池系统，可减少实例化/销毁的开销，提高性能。特别适用于频繁生成的对象，如：

- 投射物
- 粒子效果
- 敌人
- 可收集物品

### 3.[UGUI的UI系统](LearnAndDoPls/UI-System-for-UGUI/README.md) 谢谢老老老老老老老社长的提供 [@Yuan-Zzzz](https://github.com/Yuan-Zzzz)

一个为Unity的UGUI设计的整洁、结构化的UI管理系统。功能特点包括：

- 基于视图的UI架构
- UI层级管理
- 简易的视图转换
- 视图生命周期管理

所有代码均由[@Yuan-Zzzz](https://github.com/Yuan-Zzzz)编写，我只是添加了一些注释，使其更易读。

### 4.[实用技能(SomeSkills)](LearnAndDoPls/SomeSkills/README.CN_SomeSkills.md)

这些脚本展示了一些c#的skills：

- EventManager - 用于组件间通信的简单事件系统
- GameInput - 输入处理工具
- PlayerController - 基本的玩家移动和控制
- TriggerObject - 轻松处理基于触发器的交互

### 5.[美术(Art)](Art/README.CN_Art.md)

Shader

由[@Yumihoshi](https://github.com/Yumihoshi)写的来自原版[艺术资产管理](https://github.com/Yumihoshi/Art-Asset-Management)项目。

## 使用方法

1. 只需将您需要的文件夹复制到Unity项目中
2. 查看每个系统文件夹中的单独README文件，获取详细使用说明
3. 按照规范组织和管理项目资源

## 贡献

@whatevertogo 欢迎任何人指出我代码的不足之处，帮助我提高。

祝编码愉快！