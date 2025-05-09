using UnityEngine;

[CreateAssetMenu(menuName = "Configs/EventConfig")]
public class EventConfig : ScriptableObject
{
    public string EventName;
    //添加你想要的
    public EventEvent EventOnCast; // 拖入 EventEvent
}