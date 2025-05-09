using UnityEngine;

public class EventEffectReceiver : MonoBehaviour
{
    public EventEvent eventlEvent;

    private void OnEnable() => eventlEvent.Register(OnEventCast);
    private void OnDisable() => eventlEvent.Unregister(OnEventCast);
    private void OnDestroy() => eventlEvent?.UnregisterListener(this);

    private void OnEventCast(EventInfo info)
    {
        Debug.Log($"事件触发：{info.EventName}, 来自 {info.Caster.name}");
        // 播放特效、加伤害、震屏等
    }
}