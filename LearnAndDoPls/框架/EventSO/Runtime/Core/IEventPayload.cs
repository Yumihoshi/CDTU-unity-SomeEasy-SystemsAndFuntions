using UnityEngine;

namespace CDTU.EventSystem.Core
{
    /// <summary>
    /// 事件数据接口，定义事件传递的数据结构
    /// </summary>
    public interface IEventPayload
    {
        /// <summary>
        /// 获取事件发送者
        /// </summary>
        GameObject Sender { get; }

        /// <summary>
        /// 获取事件时间戳
        /// </summary>
        float Timestamp { get; }
    }
}
