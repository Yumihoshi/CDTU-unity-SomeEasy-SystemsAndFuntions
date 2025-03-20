# Unity 实用工具函数

[English](README.EN_Utils.md) | 🌏 中文

Unity项目中常用的实用工具函数和辅助类集合。

## 功能特性

### 1. 单例模式
- 通用单例模式实现
- 线程安全的单例模式
- MonoBehaviour单例支持

### 2. 扩展方法
- Transform扩展方法，便于操作
- GameObject扩展方法，常用操作
- Vector3辅助函数
- 字符串工具方法

### 3. 数学工具
- 常用数学计算
- 插值函数
- 随机数生成助手
- 几何计算工具

### 4. 文件操作
- 文件读写工具
- JSON序列化助手
- 路径操作函数

## 使用示例

### 单例实现
```csharp
public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
        // 在这里写初始化代码
    }
}
```

### 扩展方法
```csharp
// 设置Transform的X坐标
transform.SetPositionX(5f);

// 按类型查找子物体组件
var player = gameObject.FindComponentInChildren<Player>();
```

## 安装方法

1. 将Utils文件夹复制到Unity项目的Assets文件夹中
2. 在你的脚本中导入所需的命名空间

## 依赖要求

- Unity 2019.4 或更高版本

## 许可证

本项目采用MIT许可证 - 详情请查看LICENSE文件
