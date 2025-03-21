# Unity Utility Collection

🌏 [中文](README.zh-CN_Utils.md) | English

A collection of utility tools developed for Unity projects, providing multiple commonly used utility classes and functions to simplify development process and improve code quality.

## 📚 Features

### 🎯 Singleton
A generic singleton base class with features:
- Automatic instance creation (if not exists)
- Persistent across scenes (DontDestroyOnLoad)
- Prevention of duplicate instantiation
- Thread-safe implementation

Usage example:
```csharp
public class GameManager : Singleton<GameManager> {
    protected override void Awake() {
        base.Awake();
        // Your initialization code
    }
    
    public void GameLogic() {
        // Game logic
    }
}

// Usage elsewhere
GameManager.Instance.GameLogic();
```

### 🎮 ObjectPool
Efficient object pooling system to reduce runtime instantiation/destruction overhead:
- Supports any Unity Object type
- Automatic object activation state management
- Pre-warming and dynamic expansion support
- Built-in safety checks

Usage example:
```csharp
// Create object pool
public GameObject bulletPrefab;
private ObjectPool<GameObject> bulletPool;

void Start() {
    // Initialize pool (prefab, initial size, parent transform)
    bulletPool = new ObjectPool<GameObject>(bulletPrefab, 20, transform);
}

// Get object from pool
GameObject bullet = bulletPool.Get();

// Return to pool when done
bulletPool.Release(bullet);
```

## 💡 Best Practices

### Singleton
1. Only make manager classes that truly need global access singletons
2. Initialize in Awake
3. Avoid circular dependencies between singletons

### Object Pool
1. Set appropriate initial pool size based on object usage frequency
2. Pre-warm pools at game start
3. Always return objects when no longer needed
4. Use parent transforms to organize pooled objects

## 🔧 Installation

1. Copy the `Utils` folder into your project's `Assets` folder
2. Add the appropriate namespace references:
```csharp
using Utils; // For ObjectPool
using Managers; // For Singleton base class
```

## ⚙️ Requirements

- Unity 2019.4 or higher
- .NET Standard 2.0 or higher

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details
