using UnityEngine;
using System;

public class VoidEventListener : MonoBehaviour
{
    public VoidEvent Event;
    public Action Response;

    private void OnEnable()
    {
        Event?.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event?.UnregisterListener(this);
    }

    private void OnDestroy()
    {
        Event?.UnregisterListener(this); // 确保在对象销毁时注销事件
    }

    public void OnEventRaised()
    {
        Response?.Invoke(); // 调用委托
    }
}