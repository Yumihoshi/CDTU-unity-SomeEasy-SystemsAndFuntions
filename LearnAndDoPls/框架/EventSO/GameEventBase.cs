using UnityEngine;

public abstract class GameEventBase<T> : ScriptableObject
{
    public delegate void EventRaised(T value);
    private event EventRaised onEventRaised;

    public void Raise(T value) => onEventRaised?.Invoke(value);
    public void Register(EventRaised callback) => onEventRaised += callback;
    public void Unregister(EventRaised callback) => onEventRaised -= callback;
}
