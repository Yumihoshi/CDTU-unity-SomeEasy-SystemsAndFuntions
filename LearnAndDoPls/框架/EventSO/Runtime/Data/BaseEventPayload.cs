using UnityEngine;
using CDTU.EventSystem.Core;

namespace CDTU.EventSystem.Data
{
    /// <summary>
    /// 基础事件数据类，实现 IEventPayload 接口
    /// 所有自定义事件数据类都应该继承此类
    /// </summary>
    public class BaseEventPayload : IEventPayload
    {
        private readonly GameObject sender;
        private readonly float timestamp;

        public GameObject Sender => sender;
        public float Timestamp => timestamp;

        public BaseEventPayload(GameObject sender)
        {
            this.sender = sender;
            this.timestamp = Time.time;
        }
    }

    /// <summary>
    /// 空事件数据，用于不需要传递数据的事件
    /// </summary>
    public class EmptyEventPayload : BaseEventPayload
    {
        public EmptyEventPayload(GameObject sender) : base(sender) { }
    }

    /// <summary>
    /// 值类型事件数据，用于传递单个值的事件
    /// </summary>
    public class ValueEventPayload<T> : BaseEventPayload
    {
        public T Value { get; }

        public ValueEventPayload(GameObject sender, T value) : base(sender)
        {
            Value = value;
        }
    }
}
