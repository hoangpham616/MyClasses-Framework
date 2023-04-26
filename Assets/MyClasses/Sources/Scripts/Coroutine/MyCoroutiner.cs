/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyCoroutiner (version 1.8)
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
        public static void ExecuteAfterEndOfFrame(Action action)
        {
            Start(_DelayActionUntilEndOfFrame(action));
        }

        /// <summary>
        /// Execute a function after a delay.
        /// </summary>
        public static void ExecuteAfterDelayTime(float delaySecond, Action action)
        {
            if (delaySecond > 0)
            {
                Start(_DelayActionByTime(delaySecond, action));
            }
            else
            {
                if (action != null)
                {
                    action();
                }
            }
        }

        /// <summary>
        /// Execute a function after a delay.
        /// </summary>
        public static void ExecuteAfterDelayTime(string key, float delaySecond, Action action)
        {
            if (delaySecond > 0)
            {
                Start(key, _DelayActionByTime(delaySecond, action));
            }
            else
            {
                if (action != null)
                {
                    action();
                }
            }
        }

        /// <summary>
        /// Execute a function after a delay.
        /// </summary>
        public static void ExecuteAfterDelayFrame(int delayFrame, Action action)
        {
            if (delayFrame > 0)
            {
                Start(_DelayActionByFrame(delayFrame, action));
            }
            else
            {
                if (action != null)
                {
                    action();
                }
            }
        }

        /// <summary>
        /// Execute a function after a delay.
        /// </summary>
        public static void ExecuteAfterDelayFrame(string key, int delayFrame, Action action)
        {
            if (delayFrame > 0)
            {
                Start(key, _DelayActionByFrame(delayFrame, action));
            }
            else
            {
                if (action != null)
                {
                    action();
                }
            }
        }

        /// <summary>
        /// Execute a function iteratively in the interval.
        /// </summary>
        public static void SetInterval(float duration, int delayFrame, Action onUpdateCallback, Action onCompleteCallback)
        {
            Start(_DoActionRepeatedly(duration, delayFrame, onUpdateCallback, onCompleteCallback));
        }

        /// <summary>
        /// Executing a function iteratively in the interval.
        /// </summary>
        public static void SetInterval(string key, float duration, int delayFrame, Action onUpdateCallback, Action onCompleteCallback)
        {
            Start(key, _DoActionRepeatedly(duration, delayFrame, onUpdateCallback, onCompleteCallback));
        }

        /// <summary>
        /// Executing a function iteratively in the interval.
        /// </summary>
        public static void SetInterval(float duration, float delaySecond, Action onUpdateCallback, Action onCompleteCallback)
        {
            Start(_DoActionRepeatedly(duration, delaySecond, onUpdateCallback, onCompleteCallback));
        }

        /// <summary>
        /// Executing a function iteratively in the interval.
        /// </summary>
        public static void SetInterval(string key, float duration, float delaySecond, Action onUpdateCallback, Action onCompleteCallback)
        {
            Start(key, _DoActionRepeatedly(duration, delaySecond, onUpdateCallback, onCompleteCallback));
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
        private static IEnumerator _DelayActionUntilEndOfFrame(Action action)
        {
            yield return new WaitForEndOfFrame();

            if (action != null)
            {
                action();
            }
        }

        /// <summary>
        /// Delay a action.
        /// </summary>
        private static IEnumerator _DelayActionByTime(float delaySecond, Action action)
        {
            yield return new WaitForSeconds(delaySecond);

            if (action != null)
            {
                action();
            }
        }

        /// <summary>
        /// Delay a action.
        /// </summary>
        private static IEnumerator _DelayActionByFrame(int delayFrame, Action action)
        {
            for (int i = 0; i < delayFrame; i++)
            {
                yield return null;
            }

            if (action != null)
            {
                action();
            }
        }

        /// <summary>
        /// Do a action repeatedly.
        /// </summary>
        private static IEnumerator _DoActionRepeatedly(float duration, int delayFrame, Action onUpdateCallback, Action onCompleteCallback)
        {
            int countFrame = 0;
            float deadline = Time.time + duration;
            while (Time.time < deadline)
            {
                yield return null;
                countFrame += 1;

                if (countFrame == delayFrame)
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
        private static IEnumerator _DoActionRepeatedly(float duration, float delaySecond, Action onUpdateCallback, Action onCompleteCallback)
        {
            float deadline = Time.time + duration;
            while (Time.time < deadline)
            {
                float remainingSecond = deadline - Time.time;
                if (delaySecond < remainingSecond)
                {
                    yield return new WaitForSeconds(delaySecond);
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