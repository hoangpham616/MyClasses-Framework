/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyMonoSingleton (version 1.1)
 */

using UnityEngine;

namespace MyClasses
{
    public class MyMonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static object _singletonLock = new object();
        protected static T _instance;

        /// <summary>
        /// Return the only one instance of inherited class.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_singletonLock)
                    {
                        _instance = (T)FindObjectOfType(typeof(T));
                        if (_instance)
                        {
                            if (FindObjectsOfType(typeof(T)).Length > 1)
                            {
                                Debug.LogError("[" + typeof(T).Name + "] There is more than one instance of this class.");
                            }
                        }
                        else
                        {
                            GameObject obj = new GameObject(typeof(T).Name);
                            _instance = obj.AddComponent<T>();
                            DontDestroyOnLoad(obj);
                        }
                    }
                }
                return _instance;
            }
        }
    }
}