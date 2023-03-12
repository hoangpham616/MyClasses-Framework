/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyExtension.List (version 1.1)
 */

using UnityEngine;
using System.Collections.Generic;

namespace MyClasses
{
    public static partial class MyExtension
    {
        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void Log(this List<string> list)
        {
            Debug.Log(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void LogWarning(this List<string> list)
        {
            Debug.LogWarning(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void LogError(this List<string> list)
        {
            Debug.LogError(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void Log(this List<bool> list)
        {
            Debug.Log(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void LogWarning(this List<bool> list)
        {
            Debug.LogWarning(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void LogError(this List<bool> list)
        {
            Debug.LogError(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void Log(this List<byte> list)
        {
            Debug.Log(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void LogWarning(this List<byte> list)
        {
            Debug.LogWarning(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void LogError(this List<byte> list)
        {
            Debug.LogError(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void Log(this List<int> list)
        {
            Debug.Log(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void LogWarning(this List<int> list)
        {
            Debug.LogWarning(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void LogError(this List<int> list)
        {
            Debug.LogError(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void Log(this List<int[]> list)
        {
            Debug.Log(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void LogWarning(this List<int[]> list)
        {
            Debug.LogWarning(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void LogError(this List<int[]> list)
        {
            Debug.LogError(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void Log(this List<float> list)
        {
            Debug.Log(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void LogWarning(this List<float> list)
        {
            Debug.LogWarning(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void LogError(this List<float> list)
        {
            Debug.LogError(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void Log(this List<float[]> list)
        {
            Debug.Log(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void LogWarning(this List<float[]> list)
        {
            Debug.LogWarning(MyUtilities.ToString(list));
        }

        /// <summary>
        /// Print items in list.
        /// </summary>
        public static void LogError(this List<float[]> list)
        {
            Debug.LogError(MyUtilities.ToString(list));
        }
    }
}