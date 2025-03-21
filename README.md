# Unity Simple Systems & Functions

🌏 English | [CN 中文](README.zh-CN.md)

A collection of simple, reusable systems and functions for Unity projects. This repository contains ready-to-use modules that can help streamline your Unity game development.

## Available Systems

### 1. [SaveSettingsSystem](LearnAndDoPls/Systems/SaveSystem/SaveSettingsSystem/SaveSettingsSystemByPlayerPrefs/README.md)

#### For the future i will add 2 more to make the systems which are 2.for saving the data of the game(not long to see it) and 3.saving the big data of the game(a little difficult,so maye a long time)

#### And some examples in(LearnAndDoPls/Systems/SaveSystem/SaveSettingsSystem/PlayerPrefsForSettings/SettingsExamples)

#### But you would better not use it for saving the data of the game, because it is not a good way to save the data of the game. You can use it for saving the settings of the game, such as audio, graphics, etc.

#### if you have some examples can add in it please add it.

A flexible settings management system for Unity that handles game settings and saves them to PlayerPrefs. Features include:

- Generic settings management through ScriptableObjects
- Automatic serialization and persistence
- Event-driven architecture
- Support for multiple setting types (Audio, Graphics, etc.)
- Type-safe settings access
- Built-in error handling and validation

#### But you would better not use it for saving the data of the game, because it is not a good way to save the data of the game. You can use it for saving the settings of the game, such as audio, graphics, etc.

### 2. [DialogueSystem](LearnAndDoPls/Systems/DialogueSystem/README.CN_DialogueSystem.md)

A modular, easy-to-understand dialogue system for Unity. The system uses a Control-Controller pattern that makes it highly customizable while remaining simple to use. Features include:

- Linear dialogues
- Branching dialogues with multiple options
- Voice-synchronized dialogues
- Easy prefab creation for dialogue sequences

### 3. [UI System for UGUI](LearnAndDoPls/Systems/UI-System-for-UGUI/README.EN.md)

A clean, structured UI management system built for Unity's UGUI. Features include:

- View-based UI architecture
- UI layer management
- Easy view transitions
- View lifecycle management

These codes are written by [@Yuan-Zzzz](https://github.com/Yuan-Zzzz) from the original [UI-System-for-UGUI](https://github.com/Yuan-Zzzz/UI-System-for-UGUI) project and i fix some bugs. I added some comments and made it more readable.

## 4. [Utils](LearnAndDoPls/Utils/README.EN_Utils.md)

A comprehensive collection of utility functions and helper classes:

#### Object Pooling System
- Efficient object reuse system
- Automatic pool expansion
- Memory management optimization
- Perfect for frequently spawned objects:
  - Projectiles
  - Particle effects
  - Enemies
  - Collectables

#### Core Utilities
- Singleton implementations
- Extension methods
- Common helper functions
- Math utilities
- File operations helpers

## 5. [Utility Skills](LearnAndDoPls/SomeSkills/README.EN_SomeSkills.md)

A collection of practical C# implementation examples:

- EventManager - A simple event system for communication between components
- GameInput - Input handling utilities
- PlayerController - Basic player movement and control
- TriggerObject - Easily handle trigger-based interactions

## 6. [Art](Art/README.EN_Art.md)

Visual enhancement tools and shaders:

- Custom shader collection
- Material utilities
- Rendering optimization tools
- Asset management system

Written by [@Yumihoshi](https://github.com/Yumihoshi) from the original [Art Asset Management](https://github.com/Yumihoshi/Art-Asset-Management) project.

## Performance Considerations

### Memory Management

- Use object pooling for frequently created/destroyed objects
- Implement proper cleanup in OnDisable/OnDestroy
- Avoid allocations during gameplay
- Cache component references

### CPU Optimization

- Use coroutines for time-delayed operations
- Implement efficient update patterns
- Utilize job system for heavy computations
- Profile and optimize bottlenecks

### Best Practices

1. Component Organization
   - Keep components focused and single-purpose
   - Use proper separation of concerns
   - Implement interfaces for better abstraction

2. Code Structure
   - Follow Unity's execution order
   - Use ScriptableObjects for configuration
   - Implement proper error handling
   - Write clear, documented code

3. Scene Management
   - Organize hierarchies efficiently
   - Use prefabs consistently
   - Implement proper scene loading patterns


### Debug Tips

- Enable development builds for detailed logs
- Use Unity's Profiler for performance analysis
- Implement debug logging in critical sections
- Utilize Unity's Debug.DrawLine for physics visualization

## How to Use

1. Simply copy the System-folders you need into your Unity project
2. Check the individual README files in each system folder for detailed usage instructions
3. Follow the conventions to organize and manage project assets

## Contribution

@whatevertogo I welcome anyone to point out the shortcomings of my code to help me improve.

Happy Coding!

