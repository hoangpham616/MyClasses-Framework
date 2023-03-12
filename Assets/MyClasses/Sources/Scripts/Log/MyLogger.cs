/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyLogger (version 1.0)
 */

using UnityEngine;

namespace MyClasses
{
    public static class MyLogger
    {
        #region ----- Define -----

        private static readonly string CLASS_METHOD_LOG = "[{0}] {1}(): {2}";
        private static readonly string CLASS_METHOD_LOG_DARK_SDK = "<color=#05AEAB>[{0}] {1}(): {2}</color>";
        private static readonly string CLASS_METHOD_LOG_SDK = "<color=#6AC7C6>[{0}] {1}(): {2}</color>";
        private static readonly string CLASS_METHOD_LOG_DARK_NETWORK = "<color=#5151DD>[{0}] {1}(): {2}</color>";
        private static readonly string CLASS_METHOD_LOG_NETWORK = "<color=#A1A1FF>[{0}] {1}(): {2}</color>";
        private static readonly string CLASS_METHOD_LOG_DARK_UI = "<color=#FF33FB>[{0}] {1}(): {2}</color>";
        private static readonly string CLASS_METHOD_LOG_UI = "<color=#FF9CFD>[{0}] {1}(): {2}</color>";
        private static readonly string CLASS_METHOD_LOG_DARK_GAMEPLAY = "<color=#0DD131>[{0}] {1}(): {2}</color>";
        private static readonly string CLASS_METHOD_LOG_GAMEPLAY = "<color=#7CDD8D>[{0}] {1}(): {2}</color>";

        private static readonly string CLASS_LOG = "[{0}] {1}";
        private static readonly string CLASS_LOG_DARK_SDK = "<color=#05AEAB>[{0}] {1}</color>";
        private static readonly string CLASS_LOG_SDK = "<color=#6AC7C6>[{0}] {1}</color>";
        private static readonly string CLASS_LOG_DARK_NETWORK = "<color=#5151DD>[{0}] {1}</color>";
        private static readonly string CLASS_LOG_NETWORK = "<color=#A1A1FF>[{0}] {1}</color>";
        private static readonly string CLASS_LOG_DARK_UI = "<color=#FF33FB>[{0}] {1}</color>";
        private static readonly string CLASS_LOG_UI = "<color=#FF9CFD>[{0}] {1}</color>";
        private static readonly string CLASS_LOG_DARK_GAMEPLAY = "<color=#0DD131>[{0}] {1}</color>";
        private static readonly string CLASS_LOG_GAMEPLAY = "<color=#7CDD8D>[{0}] {1}</color>";

        #endregion

        #region ----- Public Function -----

        /// <summary>
        /// Print info log.
        /// </summary>
        public static void Info(string className, string methodName, string log, ELogColor color = ELogColor.DEFAULT)
        {
#if !(DISABLE_MY_LOGGER_INFO || DISABLE_MY_LOGGER_ALL)
            Debug.Log(_FormatString(className, methodName, log, color));
#endif
        }

        /// <summary>
        /// Print info log.
        /// </summary>
        public static void Info(string className, string log, ELogColor color = ELogColor.DEFAULT)
        {
#if !(DISABLE_MY_LOGGER_INFO || DISABLE_MY_LOGGER_ALL)
            Debug.Log(_FormatString(className, log, color));
#endif
        }

        /// <summary>
        /// Print warning log.
        /// </summary>
        public static void Warning(string className, string methodName, string log, ELogColor color = ELogColor.DEFAULT)
        {
#if !(DISABLE_MY_LOGGER_WARNING || DISABLE_MY_LOGGER_ALL)
            Debug.LogWarning(_FormatString(className, methodName, log, color));
#endif
        }

        /// <summary>
        /// Print warning log.
        /// </summary>
        public static void Warning(string className, string log, ELogColor color = ELogColor.DEFAULT)
        {
#if !(DISABLE_MY_LOGGER_WARNING || DISABLE_MY_LOGGER_ALL)
            Debug.LogWarning(_FormatString(className, log, color));
#endif
        }

        /// <summary>
        /// Print error log.
        /// </summary>
        public static void Error(string className, string methodName, string log, ELogColor color = ELogColor.DEFAULT)
        {
#if !(DISABLE_MY_LOGGER_ERROR || DISABLE_MY_LOGGER_ALL)
            Debug.LogError(_FormatString(className, methodName, log, color));
#endif
        }

        /// <summary>
        /// Print error log.
        /// </summary>
        public static void Error(string className, string log, ELogColor color = ELogColor.DEFAULT)
        {
#if !(DISABLE_MY_LOGGER_ERROR || DISABLE_MY_LOGGER_ALL)
            Debug.LogError(_FormatString(className, log, color));
#endif
        }

        #endregion

        #region ----- Private Function -----

        /// <summary>
        /// Format string.
        /// </summary>
        private static string _FormatString(string className, string methodName, string log, ELogColor color)
        {
            switch (color)
            {
                case ELogColor.DARK_SDK:
                    return string.Format(CLASS_METHOD_LOG_DARK_SDK, className, methodName, log);

                case ELogColor.SDK:
                    return string.Format(CLASS_METHOD_LOG_SDK, className, methodName, log);

                case ELogColor.DARK_NETWORK:
                    return string.Format(CLASS_METHOD_LOG_DARK_NETWORK, className, methodName, log);

                case ELogColor.NETWORK:
                    return string.Format(CLASS_METHOD_LOG_NETWORK, className, methodName, log);

                case ELogColor.DARK_UI:
                    return string.Format(CLASS_METHOD_LOG_DARK_UI, className, methodName, log);

                case ELogColor.UI:
                    return string.Format(CLASS_METHOD_LOG_UI, className, methodName, log);

                case ELogColor.DARK_GAMEPLAY:
                    return string.Format(CLASS_METHOD_LOG_DARK_GAMEPLAY, className, methodName, log);

                case ELogColor.GAMEPLAY:
                    return string.Format(CLASS_METHOD_LOG_GAMEPLAY, className, methodName, log);

                default:
                    return string.Format(CLASS_METHOD_LOG, className, methodName, log);
            }
        }

        /// <summary>
        /// Format string.
        /// </summary>
        private static string _FormatString(string className, string log, ELogColor color)
        {
            switch (color)
            {
                case ELogColor.DARK_SDK:
                    return string.Format(CLASS_LOG_DARK_SDK, className, log);

                case ELogColor.SDK:
                    return string.Format(CLASS_LOG_SDK, className, log);

                case ELogColor.DARK_NETWORK:
                    return string.Format(CLASS_LOG_DARK_NETWORK, className, log);

                case ELogColor.NETWORK:
                    return string.Format(CLASS_LOG_NETWORK, className, log);

                case ELogColor.DARK_UI:
                    return string.Format(CLASS_LOG_DARK_UI, className, log);

                case ELogColor.UI:
                    return string.Format(CLASS_LOG_UI, className, log);

                case ELogColor.DARK_GAMEPLAY:
                    return string.Format(CLASS_LOG_DARK_GAMEPLAY, className, log);

                case ELogColor.GAMEPLAY:
                    return string.Format(CLASS_LOG_GAMEPLAY, className, log);

                default:
                    return string.Format(CLASS_LOG, className, log);
            }
        }

        #endregion
    }

    #region ----- Enumeration -----

    public enum ELogColor
    {
        DEFAULT,
        DARK_SDK,
        SDK,
        DARK_NETWORK,
        NETWORK,
        DARK_UI,
        UI,
        DARK_GAMEPLAY,
        GAMEPLAY
    }

    #endregion
}