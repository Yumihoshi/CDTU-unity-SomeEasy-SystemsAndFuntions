using UnityEngine;
using System;
using System.Collections.Generic;
using CDTU.EventSystem.Data;

namespace CDTU.EventSystem
{
    /// <summary>
    /// 事件管理器类，负责管理和分发游戏中的事件。
    /// 实现了单例模式以便全局访问，同时支持依赖注入以方便测试。
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        private static EventManager instance;
        
        // 事件委托字典
        private readonly Dictionary<string, Delegate> eventDictionary = new();
        
        // 事件配置缓存
        private readonly Dictionary<string, EventConfig> eventConfigs = new();

        /// <summary>
        /// 获取事件管理器实例
        /// </summary>
        public static EventManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<EventManager>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("EventManager");
                        instance = go.AddComponent<EventManager>();
                        DontDestroyOnLoad(go);
                    }
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 注册一个无参数事件
        /// </summary>
        public void RegisterEvent(EventConfig eventConfig, Action handler)
        {
            if (!ValidateEventConfig(eventConfig)) return;
            
            string eventId = eventConfig.EventId;
            if (!eventDictionary.ContainsKey(eventId))
            {
                eventDictionary[eventId] = null;
                eventConfigs[eventId] = eventConfig;
            }
            eventDictionary[eventId] = (Action)eventDictionary[eventId] + handler;
        }

        /// <summary>
        /// 注册一个带参数的事件
        /// </summary>
        public void RegisterEvent<T>(EventConfig eventConfig, Action<T> handler)
        {
            if (!ValidateEventConfig(eventConfig)) return;
            
            string eventId = eventConfig.EventId;
            if (!eventDictionary.ContainsKey(eventId))
            {
                eventDictionary[eventId] = null;
                eventConfigs[eventId] = eventConfig;
            }
            eventDictionary[eventId] = (Action<T>)eventDictionary[eventId] + handler;
        }

        /// <summary>
        /// 触发无参数事件
        /// </summary>
        public void TriggerEvent(EventConfig eventConfig)
        {
            if (!ValidateEventConfig(eventConfig)) return;
            
            string eventId = eventConfig.EventId;
            if (eventDictionary.TryGetValue(eventId, out Delegate d))
            {
                if (d is Action action)
                {
                    action.Invoke();
                    LogEventTriggered(eventConfig);
                }
                else
                {
                    Debug.LogError($"事件 {eventId} 的参数类型不匹配");
                }
            }
        }

        /// <summary>
        /// 触发带参数的事件
        /// </summary>
        public void TriggerEvent<T>(EventConfig eventConfig, T parameter)
        {
            if (!ValidateEventConfig(eventConfig)) return;
            
            string eventId = eventConfig.EventId;
            if (eventDictionary.TryGetValue(eventId, out Delegate d))
            {
                if (d is Action<T> action)
                {
                    action.Invoke(parameter);
                    LogEventTriggered(eventConfig, parameter);
                }
                else
                {
                    Debug.LogError($"事件 {eventId} 的参数类型不匹配");
                }
            }
        }

        /// <summary>
        /// 注销无参数事件
        /// </summary>
        public void UnregisterEvent(EventConfig eventConfig, Action handler)
        {
            if (!ValidateEventConfig(eventConfig)) return;
            
            string eventId = eventConfig.EventId;
            if (eventDictionary.TryGetValue(eventId, out Delegate d))
            {
                eventDictionary[eventId] = (Action)d - handler;
                CleanupEventIfEmpty(eventId);
            }
        }

        /// <summary>
        /// 注销带参数的事件
        /// </summary>
        public void UnregisterEvent<T>(EventConfig eventConfig, Action<T> handler)
        {
            if (!ValidateEventConfig(eventConfig)) return;
            
            string eventId = eventConfig.EventId;
            if (eventDictionary.TryGetValue(eventId, out Delegate d))
            {
                eventDictionary[eventId] = (Action<T>)d - handler;
                CleanupEventIfEmpty(eventId);
            }
        }

        private bool ValidateEventConfig(EventConfig config)
        {
            if (config == null)
            {
                Debug.LogError("事件配置不能为空");
                return false;
            }

            if (!config.IsValid())
            {
                Debug.LogError($"事件配置 {config.name} 无效");
                return false;
            }

            return true;
        }

        private void CleanupEventIfEmpty(string eventId)
        {
            if (eventDictionary[eventId] == null)
            {
                eventDictionary.Remove(eventId);
                eventConfigs.Remove(eventId);
            }
        }

        private void LogEventTriggered(EventConfig config, object parameter = null)
        {
            if (config.EnableLogging)
            {
                if (parameter != null)
                {
                    Debug.Log($"事件触发: {config.DisplayName} ({config.EventId}), 参数: {parameter}");
                }
                else
                {
                    Debug.Log($"事件触发: {config.DisplayName} ({config.EventId})");
                }
            }
        }
    }
}
