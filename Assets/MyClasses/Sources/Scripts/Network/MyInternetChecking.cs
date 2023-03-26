/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyInternetChecking (version 1.1)
 */

#pragma warning disable 0414

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System;
using System.Collections;

namespace MyClasses
{
    public class MyInternetChecking : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        private string[] _globalDNSs = new string[] { "8.8.8.8", "208.67.222.222", "8.26.56.26", "4.2.2.1" };   // International DNS
        [SerializeField]
        private string[] _localDNSs = new string[] { "203.162.4.191", "203.113.131.1", "210.245.24.20" };       // Vietnam DNS
        [SerializeField]
        private float _checkTime = 0;

        private int _globalIndex = 0;
        private int _localIndex = 0;
        private bool _isGlobalCheck = true;
        private bool _isConnecting = false;
        private float _lastTimePingAllSuccess = -1;
        private Action<bool> _onPingCallback = null;

        #endregion

        #region ----- Property -----

        public float CheckingTime
        {
            get { return _checkTime; }
        }

        public bool IsConnecting
        {
            get { return _checkTime > 0 && _isConnecting && Application.internetReachability != NetworkReachability.NotReachable; }
        }

        #endregion

        #region ----- Singleton -----

        private static object _singletonLock = new object();
        private static MyInternetChecking _instance;

        public static MyInternetChecking Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_singletonLock)
                    {
                        _instance = (MyInternetChecking)FindObjectOfType(typeof(MyInternetChecking));
                        if (_instance == null)
                        {
                            GameObject obj = new GameObject(typeof(MyInternetChecking).Name);
                            _instance = obj.AddComponent<MyInternetChecking>();
                            DontDestroyOnLoad(obj);
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region ----- MonoBehaviour Implementation -----

        /// <summary>
        /// Start.
        /// </summary>
        void Start()
        {
            StartCoroutine(_Ping());
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Check internet connection immediately.
        /// </summary>
        public void CheckConnection(Action<bool> callback, float timeout = 5f)
        {
            if (callback != null)
            {
                StartCoroutine(_PingAll(callback, timeout));
            }
        }

        /// <summary>
        /// Set ping frequency.
        /// </summary>
        /// <param name="onFirstResultCallback">callback when the first ping done</param>
        public void SetFrequency(float checkTime, Action<bool> onFirstResultCallback = null)
        {
            _checkTime = checkTime >= 0.025f ? checkTime : 0.025f;

            if (onFirstResultCallback != null)
            {
                _onPingCallback = onFirstResultCallback;
            }
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Ping to check internet connection.
        /// </summary>
        private IEnumerator _Ping()
        {
#if UNITY_WEBGL
            _isConnecting = true;

            if (_onPingCallback != null)
            {
                _onPingCallback(_isConnecting);
                _onPingCallback = null;
            }

            yield return null;
#else
            float timeout = 1;

            while (true)
            {
                if (_checkTime <= 0)
                {
                    yield return null;
                    continue;
                }

                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    _isConnecting = false;

                    if (_onPingCallback != null)
                    {
                        _onPingCallback(_isConnecting);
                        _onPingCallback = null;
                    }

                    yield return new WaitForSecondsRealtime(_checkTime);
                }
                else
                {
                    Ping ping = new Ping(_isGlobalCheck ? _globalDNSs[_globalIndex] : _localDNSs[_localIndex]);
                    float deadline = Time.time + timeout;
                    while (!ping.isDone && Time.time < deadline)
                    {
                        yield return null;
                    }

                    _isConnecting = ping.isDone && ping.time >= 0;
                    if (!_isConnecting)
                    {
                        if (_isGlobalCheck)
                        {
                            _globalIndex = (_globalIndex + 1) % _globalDNSs.Length;
                        }
                        else
                        {
                            _localIndex = (_localIndex + 1) % _localDNSs.Length;
                        }
                        _isGlobalCheck = !_isGlobalCheck;

                        timeout = Mathf.Clamp(timeout + 0.25f, 1, 3);
                    }
                    else
                    {
                        timeout = 1;
                    }

                    if (_lastTimePingAllSuccess > 0 && Time.time - _lastTimePingAllSuccess < 3)
                    {
                        _lastTimePingAllSuccess = -1;
                        _isConnecting = true;
                    }

                    if (_onPingCallback != null)
                    {
                        _onPingCallback(_isConnecting);
                        _onPingCallback = null;
                    }

                    yield return new WaitForSecondsRealtime(_isConnecting || _checkTime < 0.25f ? _checkTime : _checkTime / 10f);
                }
            }
#endif
        }

        /// <summary>
        /// Ping to check internet connection.
        /// </summary>
        private IEnumerator _PingAll(Action<bool> callback, float timeout = 5f)
        {
#if UNITY_WEBGL
            yield return null;
            
            _isConnecting = true;

            callback(_isConnecting);
#else
            bool isConnect = false;

            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                Ping[] pings = new Ping[_localDNSs.Length + _globalDNSs.Length];
                for (int i = 0; i < _localDNSs.Length; i++)
                {
                    pings[i] = new Ping(_localDNSs[i]);
                }
                for (int i = 0; i < _globalDNSs.Length; i++)
                {
                    pings[_localDNSs.Length + i] = new Ping(_globalDNSs[i]);
                }

                float deadline = Time.time + timeout;
                while (!isConnect && Time.time < deadline)
                {
                    for (int i = 0; i < pings.Length; i++)
                    {
                        if (pings[i].isDone && pings[i].time >= 0)
                        {
                            _lastTimePingAllSuccess = Time.time;
                            _isConnecting = true;
                            isConnect = true;
                            callback(true);

                            break;
                        }
                    }

                    if (!isConnect)
                    {
                        yield return null;
                    }
                }
            }

            if (!isConnect)
            {
                _lastTimePingAllSuccess = -1;
                _isConnecting = false;
                callback(false);
            }
#endif
        }

        #endregion

        #region ----- Enumeration -----

        public enum EFrequency
        {
            Never = 0,
            Always = 1,
            Regularly = 4,
            Normally = 8,
            Occasionally = 12,
            Rarely = 20
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyInternetChecking))]
    public class MyInternetCheckingEditor : Editor
    {
        private MyInternetChecking _script;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            _script = (MyInternetChecking)target;
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(_script), typeof(MyInternetChecking), false);

            SerializedProperty globalProperty = serializedObject.FindProperty("_globalDNSs");
            SerializedProperty localProperty = serializedObject.FindProperty("_localDNSs");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(globalProperty, new GUIContent("Glogal DNS"), true);
            EditorGUILayout.PropertyField(localProperty, new GUIContent("Local DNS"), true);
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }

#endif
}