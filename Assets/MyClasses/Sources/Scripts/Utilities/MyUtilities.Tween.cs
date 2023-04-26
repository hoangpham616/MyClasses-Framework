/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUtilities.Tween (version 1.1)
 */

using UnityEngine;
using System;

namespace MyClasses
{
    public static partial class MyUtilities
    {
        /// <summary>
        /// Change the value of a number over time.
        /// </summary>
        public static void TweenNumber(int from, int to, float duration, float delaySecond, Action<int> onUpdateCallback, Action onCompleteCallback)
        {
            float startTime = Time.time;
            int distance = to - from;
            MyCoroutiner.SetInterval(duration, delaySecond, () => 
            {
                if (onUpdateCallback != null)
                {
                    int cur = from + (int)(((Time.time - startTime) / duration) * distance);
                    onUpdateCallback(cur);
                }
            }, () =>
            {
                if (onCompleteCallback != null)
                {
                    onCompleteCallback();
                }
            });
        }

        /// <summary>
        /// Change the value of a number over time.
        /// </summary>
        public static void TweenNumber(float from, float to, float duration, float delaySecond, Action<float> onUpdateCallback, Action onCompleteCallback)
        {
            float startTime = Time.time;
            float distance = to - from;
            MyCoroutiner.SetInterval(duration, delaySecond, () => 
            {
                if (onUpdateCallback != null)
                {
                    float cur = from + (((Time.time - startTime) / duration) * distance);
                    onUpdateCallback(cur);
                }
            }, () =>
            {
                if (onCompleteCallback != null)
                {
                    onCompleteCallback();
                }
            });
        }

        /// <summary>
        /// Change the value of a number over time.
        /// </summary>
        public static void TweenNumber(long from, long to, float duration, float delaySecond, Action<long> onUpdateCallback, Action onCompleteCallback)
        {
            float startTime = Time.time;
            long distance = to - from;
            MyCoroutiner.SetInterval(duration, delaySecond, () => 
            {
                if (onUpdateCallback != null)
                {
                    long cur = from + (long)(((Time.time - startTime) / duration) * distance);
                    onUpdateCallback(cur);
                }
            }, () =>
            {
                if (onCompleteCallback != null)
                {
                    onCompleteCallback();
                }
            });
        }
    }
}