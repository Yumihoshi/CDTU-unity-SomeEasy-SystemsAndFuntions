# Unity Simple Systems & Functions

🌏 English | [CN 中文](README.CN.md)

A collection of simple, reusable systems and functions for Unity projects. This repository contains ready-to-use modules that can help streamline your Unity game development.

## Available Systems


### 1. [SaveSettingsSystem][https://github.com/whatevertogo/Unity-SaveSystem]

#### For the future i will add 2 more to make the systems which are 2.for saving the data of the game(not long to see it) and 3.saving the big data of the game(a little difficult,so maye a long time)

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

### 2. [DialogueSystem][https://github.com/whatevertogo/Unity-DialogueSystem]

A modular, easy-to-understand dialogue system for Unity. The system uses a Control-Controller,InterfaceYouNeed  pattern that makes it highly customizable while remaining simple to use. Features include:

- Linear dialogues
- Branching dialogues with multiple options
- Voice-synchronized dialogues
- Easy prefab creation for dialogue sequences
- you can build a your Dialogue Ways for you own situation

### 3. [UI System for UGUI](LearnAndDoPls/Systems/UI-System-for-UGUI/README.EN.md)

A clean, structured UI management system built for Unity's UGUI. Features include:

- View-based UI architecture
- UI layer management
- Easy view transitions
- View lifecycle management

These codes are written by [@Yuan-Zzzz](https://github.com/Yuan-Zzzz) from the original [UI-System-for-UGUI](https://github.com/Yuan-Zzzz/UI-System-for-UGUI) project and i fix some bugs. I added some comments and made it more readable.


### 4.[HexGridSystem][https://github.com/whatevertogo/HexGridSystem]

This project is a hexagonal grid system that can be used to create hexagonal maps in the Unity game engine. It provides a set of C# scripts, including HexCell, HexCoordinates, HexGrid, HexGridHighlight, HexMesh, and HexMetrics, which help developers quickly build hexagonal grid maps.

Key Features
HexCell: Defines the basic properties and behaviors of hexagonal grid cells
HexCoordinates: Implements a cubic coordinate system for easy positioning and calculation of hexagonal grids
HexGrid: Manages the creation, layout, and updating of hexagonal grids
HexGridHighlight: Provides highlighting and selection functionality for grid cells
HexMesh: Dynamically generates mesh models for hexagonal grids with customizable appearances
HexMetrics: Defines hexagonal geometric parameters and constants to ensure grid consistency

### 5.[Timer](https://github.com/whatevertogo/Timer)

- A "Timer System" library implemented in C#, designed for timing management in game/Unity scenarios (the submission mentions removing Unity .meta files and the example TimerExample, and the file/interface naming and implementation are related to Unity/game timing).
Main components (based on the initial submission and subsequent notes):
ITimerSystem / ITimerEntity interfaces: define the contracts for the timing system and timer entities.
TimerEntity: implementation of a timer entity, used to encapsulate the state/callback of a single timer.
TimerSystemManager: a centralized system for managing timers, responsible for scheduling, refreshing, and shutdown.
TimerExtensions: convenient extension methods, possibly used to simplify timer calls/binding.
TimerObjectPool: an object pool implementation to reduce the overhead of frequent allocations.
TimerCallback: callback encapsulation/type definition.
TimerExample: example usage/demo.

### 6.[支持多Logic和单View绑定的基于UGUI的UI系统](https://github.com/whatevertogo/UnityUGUI-UISystem)

This is a lightweight UI framework example for Unity, including a UI manager, a base view class, sample views, and an implementation that separates UI logic. It also uses editor tools to automatically generate corresponding templates. It is suitable as a reference for UI architecture and quick integration templates for small games or projects.


## [Utils](LearnAndDoPls/Utils/README.EN_Utils.md)

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

## [Utility Skills](LearnAndDoPls/SomeSkills/README.EN_SomeSkills.md)

A collection of practical C# implementation examples:

- EventManager - A simple event system for communication between components
- GameInput - Input handling utilities
- PlayerController - Basic player movement and control
- TriggerObject - Easily handle trigger-based interactions

## [Art](Art/README.EN_Art.md)

Visual enhancement tools and shaders:

- Custom shader collection
- Material utilities
- Rendering optimization tools
- Asset management system

Written by [@Yumihoshi](https://github.com/Yumihoshi) from the original [Art Asset Management](https://github.com/Yumihoshi/Art-Asset-Management) project.



Happy Coding!

