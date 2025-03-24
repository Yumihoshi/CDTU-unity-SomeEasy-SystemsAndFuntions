using UnityEngine;

namespace Utils
{
    /// <summary>
    /// 一个简易的日志记录器，用于在Unity编辑器和开发版本中输出日志，保证不会再发布版本中输出
    /// </summary>
    public static class Logger
    {
        [System.Diagnostics.Conditional("UNITY_EDITOR")]//
        [System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
        public static void Log(string message)
        {
            Debug.Log(message);
        }

        public static void LogError(string message)
        {
            Debug.LogError(message);
        }
    }
}