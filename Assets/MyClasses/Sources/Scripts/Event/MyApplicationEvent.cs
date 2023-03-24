/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyApplicationEvent (version 1.1)
 */

using UnityEngine;
using System;

namespace MyClasses
{
    public class MyApplicationEvent
    {
        #region ----- Internal Class -----

        public class ApplicationInstance : MonoBehaviour
        {
            #region ----- Property -----

            public Action<bool> OnFocus;
            public Action<bool> OnPause;

            #endregion

            #region ----- Implement MonoBehaviour -----

            /// <summary>
            /// OnApplicationFocus.
            /// </summary>
            void OnApplicationFocus(bool hasFocus)
            {
                if (OnFocus != null)
                {
                    OnFocus(hasFocus);
                }
            }

            /// <summary>
            /// OnApplicationPause.
            /// </summary>
            void OnApplicationPause(bool isPause)
            {
                if (OnPause != null)
                {
                    OnPause(isPause);
                }
            }

            #endregion
        }

        #endregion

        #region ----- Variable -----

        private static GameObject _applicationObject;
        private static ApplicationInstance _applicationEventInstance;

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Add a OnApplicationFocus event.
        /// </summary>
        public static void AddApplicationFocus(Action<bool> evt)
        {
            _Initialize();
            _applicationEventInstance.OnFocus += evt;
        }

        /// <summary>
        /// Remove a OnApplicationFocus event.
        /// </summary>
        public static void RemoveApplicationFocus(Action<bool> evt)
        {
            _Initialize();
            _applicationEventInstance.OnFocus -= evt;
        }

        /// <summary>
        /// Add a OnApplicationPause event.
        /// </summary>
        public static void AddApplicationPause(Action<bool> evt)
        {
            _Initialize();
            _applicationEventInstance.OnPause += evt;
        }

        /// <summary>
        /// Remove a OnApplicationPause event.
        /// </summary>
        public static void RemoveApplicationPause(Action<bool> evt)
        {
            _Initialize();
            _applicationEventInstance.OnPause -= evt;
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Initialize.
        /// </summary>
        private static void _Initialize()
        {
            if (_applicationObject == null)
            {
                string objName = typeof(MyApplicationEvent).Name;

                _applicationObject = MyUtilities.FindObjectInRoot(objName);

                if (_applicationObject == null)
                {
                    _applicationObject = new GameObject(objName);
                }

                GameObject.DontDestroyOnLoad(_applicationObject);
            }

            if (_applicationEventInstance == null)
            {
                _applicationEventInstance = _applicationObject.GetComponent<ApplicationInstance>();

                if (_applicationEventInstance == null)
                {
                    _applicationEventInstance = _applicationObject.AddComponent(typeof(ApplicationInstance)) as ApplicationInstance;
                }
            }
        }

        #endregion
    }
}