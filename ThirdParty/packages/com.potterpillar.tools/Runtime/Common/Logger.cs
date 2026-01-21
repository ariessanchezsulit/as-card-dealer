#region Header

// Logger.cs
// Aries Sanchez Sulit
// 2022-11-17 at 11:38 PM

#endregion

using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace PP.Tools.Common
{
    /// <summary>
    ///     Wraps around UnityEngine.Debug.LogX(..) methods decorated with the [Conditional("ENABLE_LOGGING")] attribute.
    ///     Use this instead to avoid string allocations on logs that are going to be ignored / omitted anyway.
    /// </summary>
    public static class Logger
    {
        [Conditional("ENABLE_LOGGING")]
        public static void Log(string message)
        {
            Debug.Log(message);
        }

        [Conditional("ENABLE_LOGGING")]
        public static void Log(Object context, string message)
        {
            Debug.Log(message, context);
        }

        [Conditional("ENABLE_LOGGING")]
        public static void Log(params string[] messages)
        {
            Debug.Log(messages);
        }

        [Conditional("ENABLE_LOGGING")]
        public static void Log(Object context, params string[] messages)
        {
            Debug.Log(messages, context);
        }

        [Conditional("ENABLE_LOGGING")]
        public static void LogWarning(string warning)
        {
            Debug.LogWarning(warning);
        }

        [Conditional("ENABLE_LOGGING")]
        public static void LogWarning(Object context, string warning)
        {
            Debug.LogWarning(warning, context);
        }

        [Conditional("ENABLE_LOGGING")]
        public static void LogError(string error)
        {
            Debug.LogError(error);
        }

        [Conditional("ENABLE_LOGGING")]
        public static void LogError(Object context, string error)
        {
            Debug.LogError(error, context);
        }

        [Conditional("ENABLE_LOGGING")]
        public static void LogError(Exception exception)
        {
            Debug.LogError(exception);
        }

        [Conditional("ENABLE_LOGGING")]
        public static void LogError(Object context, Exception exception)
        {
            Debug.LogError(exception, context);
        }

        [Conditional("ENABLE_LOGGING")]
        public static void LogNullReferenceError(string name, string source = null)
        {
            LogError($"NullReferenceError: {name}{(string.IsNullOrWhiteSpace(source) ? "" : $" on {source}")}");
        }

        [Conditional("ENABLE_LOGGING")]
        public static void LogIf(bool condition, string message)
        {
            if (condition) Debug.Log(message);
        }

        [Conditional("ENABLE_LOGGING")]
        public static void LogIf(Object context, bool condition, string message)
        {
            if (condition) Debug.Log(message, context);
        }

        [Conditional("ENABLE_LOGGING")]
        public static void LogWarningIf(bool condition, string warning)
        {
            if (condition)
            {
                Debug.LogWarning(warning);
            }
        }

        [Conditional("ENABLE_LOGGING")]
        public static void LogWarningIf(Object context, bool condition, string warning)
        {
            if (condition)
            {
                Debug.LogWarning(warning, context);
            }
        }

        [Conditional("ENABLE_LOGGING")]
        public static void LogErrorIf(bool condition, string error)
        {
            if (condition)
            {
                Debug.LogError(error);
            }
        }

        [Conditional("ENABLE_LOGGING")]
        public static void LogErrorIf(Object context, bool condition, string error)
        {
            if (condition)
            {
                Debug.LogError(error, context);
            }
        }

        [Conditional("ENABLE_LOGGING")]
        public static void LogFormat(string format, params object[] parameters)
        {
            Debug.LogFormat(format, parameters);
        }
    }
}
