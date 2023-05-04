/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyPrivateCoroutiner (version 1.3)
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyClasses
{
    public class MyPrivateCoroutiner
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

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Initialize.
        /// </summary>
        protected static void _Initialize()
        {
            if (_coroutineObject == null)
            {
                string objName = typeof(MyPrivateCoroutiner).Name;

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

        #endregion
    }
}