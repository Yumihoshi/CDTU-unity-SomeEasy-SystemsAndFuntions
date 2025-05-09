using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/VoidEvent")]
public class VoidEvent : ScriptableObject
{
    private readonly List<VoidEventListener> listeners = new List<VoidEventListener>();

    public void Raise()
    {
        var currentListeners = new List<VoidEventListener>(listeners);
        for (int i = 0; i < currentListeners.Count; i++)
        {
            currentListeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(VoidEventListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void UnregisterListener(VoidEventListener listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }
}