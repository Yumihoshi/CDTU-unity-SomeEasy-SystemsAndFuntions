using UnityEngine;

[CreateAssetMenu(menuName = "Configs/EventConfig")]
public class EventConfig : ScriptableObject
{
    public string EventName;
    public int Power;
    public EventEvent EventOnCast; // 拖入 EventEvent
}