using UnityEngine;
using System;

namespace CDTU.EventSystem.Data
{
    /// <summary>
    /// 事件配置数据类，用于定义和配置游戏中的事件。
    /// 遵循Unity的数据驱动设计模式，将事件的配置数据与逻辑分离。
    /// </summary>
    [CreateAssetMenu(fileName = "New Event Config", menuName = "CDTU/Configs/EventConfig")]
    public class EventConfig : ScriptableObject
    {
        [Header("事件基础信息")]
        [Tooltip("事件的唯一标识名称")]
        [SerializeField] private string eventId = string.Empty;
        
        [Tooltip("事件的显示名称")]
        [SerializeField] private string displayName = string.Empty;
        
        [Tooltip("事件的描述信息")]
        [SerializeField, TextArea(3, 10)] 
        private string description = string.Empty;
        
        [Header("事件参数配置")]
        [Tooltip("事件参数类型")]
        [SerializeField] private EventParameterType parameterType = EventParameterType.None;
        
        [Header("事件运行时配置")]
        [Tooltip("是否在触发时记录日志")]
        [SerializeField] private bool enableLogging = true;
        
        [Tooltip("是否允许在Editor中测试")]
        [SerializeField] private bool allowEditorTesting = true;

        // 公共属性
        public string EventId => string.IsNullOrEmpty(eventId) ? name : eventId;
        public string DisplayName => string.IsNullOrEmpty(displayName) ? EventId : displayName;
        public string Description => description;
        public EventParameterType ParameterType => parameterType;
        public bool EnableLogging => enableLogging;
        public bool AllowEditorTesting => allowEditorTesting;
        
        /// <summary>
        /// 事件参数类型枚举
        /// </summary>
        public enum EventParameterType
        {
            None,
            Integer,
            Float,
            String,
            Boolean,
            Vector3,
            GameObject,
            Custom
        }

        private void OnValidate()
        {
            // 确保显示名称不为空
            if (string.IsNullOrEmpty(displayName))
            {
                displayName = name;
            }

            // 确保事件ID格式正确
            if (!string.IsNullOrEmpty(eventId) && !IsValidEventId(eventId))
            {
                Debug.LogWarning($"事件ID '{eventId}' 格式不正确。事件ID只能包含字母、数字和下划线。");
                eventId = MakeValidEventId(eventId);
            }
        }

        /// <summary>
        /// 验证事件配置是否有效
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(EventId) && IsValidEventId(EventId);
        }

        /// <summary>
        /// 检查事件ID是否符合命名规范
        /// </summary>
        private bool IsValidEventId(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            return System.Text.RegularExpressions.Regex.IsMatch(id, @"^[a-zA-Z_][a-zA-Z0-9_]*$");
        }

        /// <summary>
        /// 将无效的事件ID转换为有效格式
        /// </summary>
        private string MakeValidEventId(string id)
        {
            if (string.IsNullOrEmpty(id)) return name;
            // 将非法字符替换为下划线
            return System.Text.RegularExpressions.Regex.Replace(id, @"[^a-zA-Z0-9_]", "_");
        }

#if UNITY_EDITOR
        private void Reset()
        {
            eventId = name;
            displayName = name;
            description = "请添加事件描述";
            parameterType = EventParameterType.None;
            enableLogging = true;
            allowEditorTesting = true;
        }
#endif
    }
}