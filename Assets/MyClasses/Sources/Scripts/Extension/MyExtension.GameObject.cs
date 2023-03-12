/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyExtension.GameObject (version 1.2)
 */

using UnityEngine;

namespace MyClasses
{
    public static partial class MyExtension
    {
        /// <summary>
        /// Add a component into specified game object if the component doesn't exist on this game object.
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject != null)
            {
                T component = gameObject.GetComponent<T>();
                if (component == null)
                {
                    component = gameObject.AddComponent<T>();
                }
                return component;
            }

            return null;
        }

        /// <summary>
        /// Return the first child of the specified object.
        /// </summary>
        public static GameObject GetFirstChild(this GameObject gameObject)
        {
            if (gameObject != null)
            {
                int countChild = gameObject.transform.childCount;
                if (countChild > 0)
                {
                    return gameObject.transform.GetChild(0).gameObject;
                }
            }
            return null;
        }

        /// <summary>
        /// Return the last child of the specified object.
        /// </summary>
        public static GameObject GetLastChild(this GameObject gameObject)
        {
            if (gameObject != null)
            {
                int countChild = gameObject.transform.childCount;
                if (countChild > 0)
                {
                    return gameObject.transform.GetChild(countChild - 1).gameObject;
                }
            }
            return null;
        }

        /// <summary>
        /// Log info.
        /// </summary>
        public static void LogInfo(this GameObject gameObject, string methodName, string log, ELogColor color = ELogColor.DEFAULT)
        {
            MyLogger.Info(gameObject.GetType().Name, methodName, log, color);
        }

        /// <summary>
        /// Log info.
        /// </summary>
        public static void LogInfo(this GameObject gameObject, string log, ELogColor color = ELogColor.DEFAULT)
        {
            MyLogger.Info(gameObject.GetType().Name, log, color);
        }

        /// <summary>
        /// Log warning.
        /// </summary>
        public static void LogWarning(this GameObject gameObject, string methodName, string log, ELogColor color = ELogColor.DEFAULT)
        {
            MyLogger.Warning(gameObject.GetType().Name, methodName, log, color);
        }

        /// <summary>
        /// Log warning.
        /// </summary>
        public static void LogWarning(this GameObject gameObject, string log, ELogColor color = ELogColor.DEFAULT)
        {
            MyLogger.Warning(gameObject.GetType().Name, log, color);
        }

        /// <summary>
        /// Log error.
        /// </summary>
        public static void LogError(this GameObject gameObject, string methodName, string log, ELogColor color = ELogColor.DEFAULT)
        {
            MyLogger.Error(gameObject.GetType().Name, methodName, log, color);
        }

        /// <summary>
        /// Log error.
        /// </summary>
        public static void LogError(this GameObject gameObject, string log, ELogColor color = ELogColor.DEFAULT)
        {
            MyLogger.Error(gameObject.GetType().Name, log, color);
        }
    }
}
