/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyCoroutiner (version 1.10)
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyClasses
{
    public class MyCoroutiner
    {
        #region ----- Internal Class -----

        public class CoroutineInstance : MonoBehaviour
        {
        }

        #endregion

        #region ----- Variable -----

        private static GameObject _coroutineObject;
        private static CoroutineInstance _coroutineInstance;
        private static Dictionary<string, IEnumerator> _dictionaryRoutine;

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Execute a function after a delay.
        /// </summary>
        public static void ExecuteAfterEndOfFrame(Action onCallback)
        {
            Start(_DelayActionUntilEndOfFrame(onCallback));
        }

        /// <summary>
        /// Execute a function after a delay.
        /// </summary>
        public static void ExecuteAfterDelayFrame(int delayFrame, Action onCallback)
        {
            if (delayFrame > 0)
            {
                Start(_DoExecuteAfterDelayFrame(delayFrame, onCallback));
            }
            else
            {
                if (onCallback != null)
                {
                    onCallback();
                }
            }
        }

        /// <summary>
        /// Execute a function after a delay.
        /// </summary>
        /// <param name="key">if the key name is the same, the following function call will replace the previous function call.</param>
        public static void ExecuteAfterDelayFrame(string key, int delayFrame, Action onCallback)
        {
            if (delayFrame > 0)
            {
                Start(key, _DoExecuteAfterDelayFrame(delayFrame, onCallback));
            }
            else
            {
                if (onCallback != null)
                {
                    onCallback();
                }
            }
        }

        /// <summary>
        /// Execute a function after a delay.
        /// </summary>
        public static void ExecuteAfterDelayTime(float delaySecond, Action onCallback)
        {
            if (delaySecond > 0)
            {
                Start(_DoExecuteAfterDelayTime(delaySecond, onCallback));
            }
            else
            {
                if (onCallback != null)
                {
                    onCallback();
                }
            }
        }

        /// <summary>
        /// Execute a function after a delay.
        /// </summary>
        /// <param name="key">if the key name is the same, the following function call will replace the previous function call.</param>
        public static void ExecuteAfterDelayTime(string key, float delaySecond, Action onCallback)
        {
            if (delaySecond > 0)
            {
                Start(key, _DoExecuteAfterDelayTime(delaySecond, onCallback));
            }
            else
            {
                if (onCallback != null)
                {
                    onCallback();
                }
            }
        }

        /// <summary>
        /// Execute a function by frame.
        /// </summary>
        /// <param name="onUpdateCallback">called at each frame with the index attached. Index 0 is the frame at the time of function call.</param>
        public static void ExecuteFrameByFrame(int totalFrame, Action<int> onUpdateCallback)
        {
            Start(_DoExecuteFrameByFrame(totalFrame, onUpdateCallback));
        }

        /// <summary>
        /// Execute a function by frame.
        /// </summary>
        /// <param name="key">if the key name is the same, the following function call will replace the previous function call.</param>
        /// <param name="onUpdateCallback">called at each frame with the index attached. Index 0 is the frame at the time of function call.</param>
        public static void ExecuteFrameByFrame(string key, int totalFrame, Action<int> onUpdateCallback)
        {
            Start(key, _DoExecuteFrameByFrame(totalFrame, onUpdateCallback));
        }

        /// <summary>
        /// Execute a function iteratively in the interval.
        /// </summary>
        public static void SetInterval(float duration, int frameStep, Action onUpdateCallback, Action onCompleteCallback)
        {
            Start(_DoActionRepeatedly(duration, frameStep, onUpdateCallback, onCompleteCallback));
        }

        /// <summary>
        /// Executing a function iteratively in the interval.
        /// </summary>
        /// <param name="key">if the key name is the same, the following function call will replace the previous function call.</param>
        public static void SetInterval(string key, float duration, int frameStep, Action onUpdateCallback, Action onCompleteCallback)
        {
            Start(key, _DoActionRepeatedly(duration, frameStep, onUpdateCallback, onCompleteCallback));
        }

        /// <summary>
        /// Executing a function iteratively in the interval.
        /// </summary>
        public static void SetInterval(float duration, float timeStep, Action onUpdateCallback, Action onCompleteCallback)
        {
            Start(_DoActionRepeatedly(duration, timeStep, onUpdateCallback, onCompleteCallback));
        }

        /// <summary>
        /// Executing a function iteratively in the interval.
        /// </summary>
        /// <param name="key">if the key name is the same, the following function call will replace the previous function call.</param>
        public static void SetInterval(string key, float duration, float timeStep, Action onUpdateCallback, Action onCompleteCallback)
        {
            Start(key, _DoActionRepeatedly(duration, timeStep, onUpdateCallback, onCompleteCallback));
        }
 
        /// <summary>
        /// Start a coroutine.
        /// </summary>
        public static void Start(IEnumerator routine)
        {
            _Initialize();

            _coroutineInstance.StartCoroutine(routine);
        }

        /// <summary>
        /// Start a coroutine.
        /// </summary>
        /// <param name="key">if the key name is the same, the following function call will replace the previous function call.</param>
        public static void Start(string key, IEnumerator routine)
        {
            _Initialize();

            if (_dictionaryRoutine.ContainsKey(key))
            {
                if (_dictionaryRoutine[key] != null)
                {
                    _coroutineInstance.StopCoroutine(_dictionaryRoutine[key]);
                }
                _dictionaryRoutine.Remove(key);
            }

            _coroutineInstance.StartCoroutine(routine);
            _dictionaryRoutine.Add(key, routine);
        }

        /// <summary>
        /// Stop a coroutine.
        /// </summary>
        public static void Stop(string key)
        {
            _Initialize();

            if (_dictionaryRoutine.ContainsKey(key))
            {
                if (_dictionaryRoutine[key] != null)
                {
                    _coroutineInstance.StopCoroutine(_dictionaryRoutine[key]);
                }
                _dictionaryRoutine.Remove(key);
            }
        }

        /// <summary>
        /// Stop all coroutines.
        /// </summary>
        public static void StopAll()
        {
            _Initialize();

            _coroutineInstance.StopAllCoroutines();
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Initialize.
        /// </summary>
        private static void _Initialize()
        {
            if (_coroutineObject == null)
            {
                string objName = typeof(MyCoroutiner).Name;

                _coroutineObject = MyUtilities.FindObjectInRoot(objName);

                if (_coroutineObject == null)
                {
                    _coroutineObject = new GameObject(objName);
                }

                GameObject.DontDestroyOnLoad(_coroutineObject);

                _dictionaryRoutine = new Dictionary<string, IEnumerator>();
            }

            if (_coroutineInstance == null)
            {
                _coroutineInstance = _coroutineObject.GetComponent<CoroutineInstance>();

                if (_coroutineInstance == null)
                {
                    _coroutineInstance = _coroutineObject.AddComponent(typeof(CoroutineInstance)) as CoroutineInstance;
                }
            }
        }

        /// <summary>
        /// Delay a action.
        /// </summary>
        private static IEnumerator _DelayActionUntilEndOfFrame(Action onCallback)
        {
            yield return new WaitForEndOfFrame();

            if (onCallback != null)
            {
                onCallback();
            }
        }

        /// <summary>
        /// Delay a action.
        /// </summary>
        private static IEnumerator _DoExecuteAfterDelayFrame(int delayFrame, Action onCallback)
        {
            for (int i = 0; i < delayFrame; i++)
            {
                yield return null;
            }

            if (onCallback != null)
            {
                onCallback();
            }
        }

        /// <summary>
        /// Delay a action.
        /// </summary>
        private static IEnumerator _DoExecuteAfterDelayTime(float delaySecond, Action onCallback)
        {
            yield return new WaitForSeconds(delaySecond);

            if (onCallback != null)
            {
                onCallback();
            }
        }

        /// <summary>
        /// Delay a action.
        /// </summary>
        private static IEnumerator _DoExecuteFrameByFrame(int totalFrame, Action<int> onCallback)
        {
            for (int i = 0; i < totalFrame; i++)
            {
                if (onCallback != null)
                {
                    onCallback(i);
                }
                yield return null;
            }
        }

        /// <summary>
        /// Do a action repeatedly.
        /// </summary>
        private static IEnumerator _DoActionRepeatedly(float duration, int frameStep, Action onUpdateCallback, Action onCompleteCallback)
        {
            int countFrame = 0;
            float deadline = Time.time + duration;
            while (Time.time < deadline)
            {
                yield return null;
                countFrame += 1;

                if (countFrame == frameStep)
                {
                    countFrame = 0;
                    if (onUpdateCallback != null)
                    {
                        onUpdateCallback();
                    }
                }
            }

            if (onCompleteCallback != null)
            {
                onCompleteCallback();
            }
        }

        /// <summary>
        /// Do a action repeatedly.
        /// </summary>
        private static IEnumerator _DoActionRepeatedly(float duration, float timeStep, Action onUpdateCallback, Action onCompleteCallback)
        {
            float deadline = Time.time + duration;
            while (Time.time < deadline)
            {
                float remainingSecond = deadline - Time.time;
                if (timeStep < remainingSecond)
                {
                    yield return new WaitForSeconds(timeStep);
                    if (onUpdateCallback != null)
                    {
                        onUpdateCallback();
                    }
                }
                else
                {
                    yield return new WaitForSeconds(remainingSecond);
                }
            }

            if (onCompleteCallback != null)
            {
                onCompleteCallback();
            }
        }

        #endregion
    }
}