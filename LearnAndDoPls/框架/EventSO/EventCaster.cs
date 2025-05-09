using UnityEngine;

public class EventCaster : MonoBehaviour
{
    public EventConfig eventConfig;

    public void CastEvent()
    {
        var info = new EventInfo
        {
            EventName = eventConfig.EventName,
            Power = eventConfig.Power,
            Caster = this.gameObject
        };

        eventl.EventOnCast?.Raise(info);
    }
}
