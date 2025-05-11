using UnityEngine;

namespace CDTU.EventSystem.Data
{
    /// <summary>
    /// 事件信息类，用于在编辑器中配置和序列化事件数据
    /// </summary>
    [System.Serializable]
    public class EventInfo
    {
        [Tooltip("事件配置")]
        public EventConfig EventConfig;

        [Tooltip("是否在Awake时自动注册")]
        public bool AutoRegister = true;

        [Tooltip("是否在OnDestroy时自动注销")]
        public bool AutoUnregister = true;
    }
}
