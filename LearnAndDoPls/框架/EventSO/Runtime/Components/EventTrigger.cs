using UnityEngine;
using UnityEngine.Events;
using CDTU.EventSystem.Data;

namespace CDTU.EventSystem.Components
{
    /// <summary>
    /// 事件触发器组件
    /// 用于在GameObject上监听和触发事件
    /// </summary>
    public class EventTrigger : MonoBehaviour
    {
        [SerializeField] private EventInfo[] events;
        [SerializeField] private bool logEvents = true;

        /// <summary>
        /// Unity事件，当事件被触发时调用
        /// </summary>
        [System.Serializable]
        public class EventUnityEvent : UnityEvent<EventConfig, IEventPayload> { }

        [SerializeField] private EventUnityEvent onEventTriggered;

        private void Awake()
        {
            RegisterEvents();
        }

        private void OnDestroy()
        {
            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            if (events == null) return;

            foreach (var eventInfo in events)
            {
                if (eventInfo.EventConfig == null || !eventInfo.AutoRegister) continue;

                EventManager.Instance.RegisterEvent(eventInfo.EventConfig, () =>
                {
                    var payload = new EmptyEventPayload(gameObject);
                    HandleEventTriggered(eventInfo.EventConfig, payload);
                });
            }
        }

        private void UnregisterEvents()
        {
            if (events == null) return;

            foreach (var eventInfo in events)
            {
                if (eventInfo.EventConfig == null || !eventInfo.AutoUnregister) continue;

                EventManager.Instance.UnregisterEvent(eventInfo.EventConfig, () =>
                {
                    var payload = new EmptyEventPayload(gameObject);
                    HandleEventTriggered(eventInfo.EventConfig, payload);
                });
            }
        }

        private void HandleEventTriggered(EventConfig config, IEventPayload payload)
        {
            if (logEvents)
            {
                Debug.Log($"Event triggered: {config.DisplayName} from {payload.Sender.name}");
            }

            onEventTriggered?.Invoke(config, payload);
        }

        /// <summary>
        /// 手动触发指定事件
        /// </summary>
        public void TriggerEvent(EventConfig config)
        {
            if (config == null)
            {
                Debug.LogError("Cannot trigger null event config");
                return;
            }

            var payload = new EmptyEventPayload(gameObject);
            EventManager.Instance.TriggerEvent(config);
            HandleEventTriggered(config, payload);
        }

        /// <summary>
        /// 手动触发带参数的事件
        /// </summary>
        public void TriggerEvent<T>(EventConfig config, T value)
        {
            if (config == null)
            {
                Debug.LogError("Cannot trigger null event config");
                return;
            }

            var payload = new ValueEventPayload<T>(gameObject, value);
            EventManager.Instance.TriggerEvent(config, value);
            HandleEventTriggered(config, payload);
        }
    }
}
