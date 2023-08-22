using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace AHL.Core.General.Utils
{
    public static class AHLDebug
    {
        #region Parameters

        [Conditional("ENABLE_LOGS")]
        public static void Log(object message)
        {
            Debug.Log(message);
        }
        
        [Conditional("ENABLE_LOGS")]
        public static void Log(object message, Object context)
        {
            Debug.Log(message, context);
        }

        [Conditional("ENABLE_LOGS")]
        public static void LogFormat(string format, params object[] args)
        {
            Debug.LogFormat(format, args);
        }

        [Conditional("ENABLE_LOGS")]
        public static void LogErrorFormat(string format, params object[] args)
        {
            Debug.LogErrorFormat(format, args);
        }
        
        [Conditional("ENABLE_LOGS")]
        public static void LogException(Exception exception)
        {
            Debug.LogException(exception);
        }
        
        [Conditional("ENABLE_LOGS")]
        public static void LogError(object message, Object context = null)
        {
            Debug.LogError(message, context);
        }
        
        [Conditional("ENABLE_LOGS")]
        public static void LogWarning(string warning)
        {
            Debug.LogWarning(warning);
        }
        
        [Conditional("ENABLE_LOGS")]
        public static void LogWarningFormat(string format, params object[] args)
        {
            Debug.LogWarningFormat(format, args);
        }
        
        #endregion Public Methods
    }
}

