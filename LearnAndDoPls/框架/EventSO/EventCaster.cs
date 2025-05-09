using UnityEngine;

public class EventCaster : MonoBehaviour
{
    public EventConfig eventConfig;

    public void CastEvent()
    {
        var info = new EventInfo
        {
            EventName = eventConfig.EventName,
            //todo-添加你想要的
            Caster = this.gameObject
        };

        eventl.EventOnCast?.Raise(info);
    }
}
