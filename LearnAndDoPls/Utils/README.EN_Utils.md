# Unity Utility Functions

🌏 English | [CN 中文](README.zh-CN_Utils.md)

A collection of useful utility functions and helper classes for Unity projects.

## Features

### 1. Singleton
- Generic singleton implementation
- Thread-safe singleton pattern
- MonoBehaviour singleton support

### 2. Extensions
- Transform extensions for easy manipulation
- GameObject extensions for common operations
- Vector3 helper functions
- String utility methods

### 3. Math Utilities
- Common mathematical calculations
- Interpolation functions
- Random number generation helpers
- Geometry calculations

### 4. File Operations
- File reading/writing utilities
- JSON serialization helpers
- Path manipulation functions

## Usage Examples

### Singleton Implementation
```csharp
public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
        // Your initialization code
    }
}
```

### Extension Methods
```csharp
// Transform position setting
transform.SetPositionX(5f);

// GameObject finding with type
var player = gameObject.FindComponentInChildren<Player>();
```

## Installation

1. Copy the Utils folder into your Unity project's Assets folder
2. Import the necessary namespaces in your scripts

## Dependencies

- Unity 2019.4 or higher

## License

This project is licensed under the MIT License - see the LICENSE file for details
