/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUtilities.Tween (version 1.3)
 */

using UnityEngine;
using System;

namespace MyClasses
{
    public static partial class MyUtilities
    {

        #region ----- Color -----

        /// <summary>
        /// Change the value of a color over time.
        /// </summary>
        /// <param name="key">if the key name is the same, the following function call will replace the previous function call.</param>
        public static void TweenColor(string key, Color from, Color to, float duration, float timeStep, Action<Color> onUpdateCallback, Action onCompleteCallback)
        {
            float startTime = Time.time;
            MyCoroutiner.SetInterval(key, duration, timeStep, () => 
            {
                if (onUpdateCallback != null)
                {
                    float timeElapsed = Time.time - startTime;
                    float t = Mathf.Clamp01(timeElapsed / duration);
                    Color lerpedColor = Color.Lerp(from, to, t);
                    onUpdateCallback(lerpedColor);
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
        /// Change the value of a color over time.
        /// </summary>
        public static void TweenColor(Color from, Color to, float duration, float timeStep, Action<Color> onUpdateCallback, Action onCompleteCallback)
        {
            float startTime = Time.time;
            MyCoroutiner.SetInterval(duration, timeStep, () => 
            {
                if (onUpdateCallback != null)
                {
                    float timeElapsed = Time.time - startTime;
                    float t = Mathf.Clamp01(timeElapsed / duration);
                    Color lerpedColor = Color.Lerp(from, to, t);
                    onUpdateCallback(lerpedColor);
                }
            }, () =>
            {
                if (onCompleteCallback != null)
                {
                    onCompleteCallback();
                }
            });
        }

        #endregion

        #region ----- Number -----

        /// <summary>
        /// Change the value of a number over time.
        /// </summary>
        /// <param name="key">if the key name is the same, the following function call will replace the previous function call.</param>
        public static void TweenNumber(string key, int from, int to, float duration, float timeStep, Action<int> onUpdateCallback, Action onCompleteCallback)
        {
            float startTime = Time.time;
            int distance = to - from;
            MyCoroutiner.SetInterval(key, duration, timeStep, () => 
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
        public static void TweenNumber(int from, int to, float duration, float timeStep, Action<int> onUpdateCallback, Action onCompleteCallback)
        {
            float startTime = Time.time;
            int distance = to - from;
            MyCoroutiner.SetInterval(duration, timeStep, () => 
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
        /// <param name="key">if the key name is the same, the following function call will replace the previous function call.</param>
        public static void TweenNumber(string key, float from, float to, float duration, float timeStep, Action<float> onUpdateCallback, Action onCompleteCallback)
        {
            float startTime = Time.time;
            float distance = to - from;
            MyCoroutiner.SetInterval(key, duration, timeStep, () => 
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
        public static void TweenNumber(float from, float to, float duration, float timeStep, Action<float> onUpdateCallback, Action onCompleteCallback)
        {
            float startTime = Time.time;
            float distance = to - from;
            MyCoroutiner.SetInterval(duration, timeStep, () => 
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
        /// <param name="key">if the key name is the same, the following function call will replace the previous function call.</param>
        public static void TweenNumber(string key, long from, long to, float duration, float timeStep, Action<long> onUpdateCallback, Action onCompleteCallback)
        {
            float startTime = Time.time;
            long distance = to - from;
            MyCoroutiner.SetInterval(key, duration, timeStep, () => 
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

        /// <summary>
        /// Change the value of a number over time.
        /// </summary>
        public static void TweenNumber(long from, long to, float duration, float timeStep, Action<long> onUpdateCallback, Action onCompleteCallback)
        {
            float startTime = Time.time;
            long distance = to - from;
            MyCoroutiner.SetInterval(duration, timeStep, () => 
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

        #endregion
    }
}