# Utility Skills

A collection of useful scripts that can be used in any Unity project. These components are designed to be modular and easy to integrate into your game.

## Available Components

### EventManager

A centralized event management system that uses object pooling to optimize event parameter objects.

**Key Features:**
- Object pooling for event arguments to reduce garbage collection
- Automatic reset and recycling of event parameters
- Unified management of all game events
- Singleton pattern for easy access

**Example Usage:**
```csharp
// Subscribe to an event
EventManager.Instance.OnTriggerObjectSelected += HandleObjectSelected;

// Trigger an event
EventManager.Instance.TriggerObjectSelected(selectedObject);

// Event handler
private void HandleObjectSelected(object sender, EventManager.TriggerObjectSelectedEventArgs args)
{
    TriggerObject selectedObject = args.SelectedObject;
    // Do something with the selected object
}
```

### GameInput

A flexible input handling system that abstracts input detection from the game logic.

**Key Features:**
- Handles both mouse and keyboard inputs
- Configurable input settings
- Events for common input actions
- Easy to extend for custom input requirements

**Example Usage:**
```csharp
// Get a reference
private GameInput _gameInput;

// Initialize
_gameInput = GetComponent<GameInput>();
_gameInput.OnClick += HandleClick;

// Handle input events
private void HandleClick(object sender, EventArgs e)
{
    // Respond to click event
}
```

### PlayerController

A modular player controller that handles movement and interaction with objects in the game world.

**Key Features:**
- Character movement with configurable speed and rotation
- Object selection and interaction system
- Integration with the EventManager and GameInput systems
- Customizable movement behaviors

**Example Usage:**
```csharp
// Configure in the Inspector
// - Set movement speed
// - Set rotation speed
// - Connect to navigation system
// - Set interaction parameters

// Get reference in code
PlayerController playerController = GetComponent<PlayerController>();

// Use methods
playerController.MoveToPosition(targetPosition);
```

### TriggerObject

An abstract base class for all interactive objects in your game.

**Key Features:**
- Defines the basic functionality for all interactive objects
- Provides interaction range and state control
- Virtual methods for selection and interaction events
- Easy to extend for specific interactive objects

**Example Usage:**
```csharp
// Create a derived class
public class Collectible : TriggerObject
{
    public override void Interact()
    {
        // Custom interaction behavior
        CollectItem();
        base.Interact();
    }
    
    public override void OnSelected()
    {
        // Highlight the object
        ShowHighlight();
        base.OnSelected();
    }
    
    public override void OnDeselected()
    {
        // Remove highlight
        HideHighlight();
        base.OnDeselected();
    }
}
```

## Integration

These components are designed to work together but can also be used individually:

1. **EventManager** provides the communication backbone
2. **GameInput** captures player input and raises events
3. **PlayerController** consumes input events and handles player actions
4. **TriggerObject** defines the interaction interface for game objects

## Best Practices

1. **Use EventManager** for all game events to maintain a clean architecture
2. **Extend TriggerObject** for all interactive objects in your game
3. **Configure PlayerController** in the inspector for different character types
4. **Customize GameInput** if you need additional input handling

## Requirements

- Unity 2019.4 or newer
- No additional packages required

## Example Scene

To see these components in action, check the example scenes in the repository.