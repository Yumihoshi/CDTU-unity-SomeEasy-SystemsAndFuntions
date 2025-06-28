using UnityEngine;

namespace CDTU.Utils
{
    /// <summary>
    /// 一个简易的日志记录器，用于在Unity编辑器和开发版本中输出日志，保证不会再发布版本中输出
    /// </summary>
    public static class Logger
    {
        // 控制输出日志的级别
        public static void Log(string message)
        {
#if UNITY_EDITOR
            Debug.Log("[Editor Log] " + message);  // 编辑器版本输出详细日志
#elif DEVELOPMENT_BUILD
        Debug.Log("[Dev Log] " + message);  // 开发版本输出简洁日志
#endif
        }

        public static void LogWarning(string message)
        {
#if UNITY_EDITOR
            Debug.LogWarning("[Editor Warning] " + message);  // 编辑器版本输出详细警告
#elif DEVELOPMENT_BUILD
        Debug.LogWarning("[Dev Warning] " + message);  // 开发版本输出简洁警告
#endif
        }

        public static void LogError(string message)
        {
#if UNITY_EDITOR
            Debug.LogError("[Editor Error] " + message);  // 编辑器版本输出详细错误
#elif DEVELOPMENT_BUILD
        Debug.LogError("[Dev Error] " + message);  // 开发版本输出简洁错误
#endif
        }
    }
}
