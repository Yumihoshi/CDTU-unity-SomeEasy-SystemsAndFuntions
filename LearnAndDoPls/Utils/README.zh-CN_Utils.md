# Unity 实用工具集合

[English](README.EN_Utils.md) | 🌏 中文

这是一个为 Unity 项目开发的实用工具集合，提供了多个常用的工具类和函数，帮助简化开发流程，提高代码质量。

## 📚 功能模块

### 🎯 单例模式 (Singleton)
提供了一个通用的单例模式基类，特点：
- 自动创建实例（如果不存在）
- 场景切换时保持单例存活（DontDestroyOnLoad）
- 防止重复实例化
- 线程安全

使用示例：
```csharp
public class GameManager : Singleton<GameManager> {
    protected override void Awake() {
        base.Awake();
        // 你的初始化代码
    }
    
    public void GameLogic() {
        // 游戏逻辑
    }
}

// 在其他地方使用
GameManager.Instance.GameLogic();
```

### 🎮 对象池 (ObjectPool)
高效的对象池系统，用于减少运行时实例化/销毁对象的性能开销：
- 支持任意 Unity Object 类型
- 自动管理对象激活状态
- 支持预热和动态扩容
- 内置安全检查机制

使用示例：
```csharp
// 创建对象池
public GameObject bulletPrefab;
private ObjectPool<GameObject> bulletPool;

void Start() {
    // 初始化对象池（预制体，初始大小，父物体）
    bulletPool = new ObjectPool<GameObject>(bulletPrefab, 20, transform);
}

// 从池中获取对象
GameObject bullet = bulletPool.Get();

// 使用完后返回池中
bulletPool.Release(bullet);
```

## 💡 最佳实践

### 单例模式
1. 只将真正需要全局访问的管理器类设为单例
2. 在 Awake 中进行初始化
3. 注意避免单例之间的循环依赖

### 对象池
1. 根据对象的使用频率合理设置初始池大小
2. 在游戏开始时预热对象池
3. 及时回收不再使用的对象
4. 使用父物体组织池化对象

## 🔧 安装使用

1. 将 `Utils` 文件夹复制到你的项目的 `Assets` 文件夹中
2. 添加相应的命名空间引用：
```csharp
using Utils; // 对象池
using Managers; // 单例基类
```

## ⚙️ 要求

- Unity 2019.4 或更高版本
- .NET Standard 2.0 或更高版本

## 📝 许可证

本项目采用 MIT 许可证 - 详情请查看 LICENSE 文件
