/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUnityAdsManager (version 1.1)
 */

#pragma warning disable 0162
#pragma warning disable 0414

#if USE_MY_UNITY_ADS

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Advertisements;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyClasses
{
    public class MyUnityAdsManager : MonoBehaviour, IUnityAdsListener
    {
        #region ----- Variable -----

        [SerializeField]
        private string _androidGameId = string.Empty;
        [SerializeField]
        private string _iosGameId = string.Empty;
        [SerializeField]
        private bool _isTestMode = true;

        private Dictionary<string, Action[]> _dictCallbacks = new Dictionary<string, Action[]>();
        private bool _isShowBanner = false;

        #endregion

        #region ----- Singleton -----

        private static object _singletonLock = new object();
        private static MyUnityAdsManager _instance;

        public static MyUnityAdsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_singletonLock)
                    {
                        _instance = (MyUnityAdsManager)FindObjectOfType(typeof(MyUnityAdsManager));
                        if (_instance == null)
                        {
                            GameObject obj = new GameObject(typeof(MyUnityAdsManager).Name);
                            _instance = obj.AddComponent<MyUnityAdsManager>();
                        }
                        DontDestroyOnLoad(_instance);
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region ----- IUnityAdsListener -----

        /// <summary>
        /// OnUnityAdsReady.
        /// </summary>
        public void OnUnityAdsReady(string placementId)
        {
#if DEBUG_MY_UNITY_ADS
            Debug.Log("[" + typeof(MyUnityAdsManager).Name + "] OnUnityAdsReady(): placementId=" + placementId);
#endif
        }

        /// <summary>
        /// OnUnityAdsDidError.
        /// </summary>
        public void OnUnityAdsDidError(string message)
        {
#if DEBUG_MY_UNITY_ADS
            Debug.LogError("[" + typeof(MyUnityAdsManager).Name + "] OnUnityAdsReady(): message=" + message);
#endif
        }

        /// <summary>
        /// OnUnityAdsDidStart.
        /// </summary>
        public void OnUnityAdsDidStart(string placementId)
        {
#if DEBUG_MY_UNITY_ADS
            Debug.Log("[" + typeof(MyUnityAdsManager).Name + "] OnUnityAdsDidStart(): placementId=" + placementId);
#endif

            if (_dictCallbacks.ContainsKey(placementId) && _dictCallbacks[placementId][3] != null)
            {
                _dictCallbacks[placementId][3]();
            }
        }

        /// <summary>
        /// OnUnityAdsDidFinish.
        /// </summary>
        public void OnUnityAdsDidFinish(string placementId, UnityEngine.Advertisements.ShowResult showResult)
        {
#if DEBUG_MY_UNITY_ADS
            Debug.Log("[" + typeof(MyUnityAdsManager).Name + "] OnUnityAdsDidFinish(): placementId=" + placementId + " | showResult=" + showResult.ToString());
#endif

            switch (showResult)
            {
                case ShowResult.Failed:
                    {
                        if (_dictCallbacks.ContainsKey(placementId) && _dictCallbacks[placementId][0] != null)
                        {
                            _dictCallbacks[placementId][0]();
                        }
                    }
                    break;

                case ShowResult.Skipped:
                    {
                        if (_dictCallbacks.ContainsKey(placementId) && _dictCallbacks[placementId][1] != null)
                        {
                            _dictCallbacks[placementId][1]();
                        }
                    }
                    break;

                case ShowResult.Finished:
                    {
                        if (_dictCallbacks.ContainsKey(placementId) && _dictCallbacks[placementId][2] != null)
                        {
                            _dictCallbacks[placementId][2]();
                        }
                    }
                    break;
            }

            if (_dictCallbacks.ContainsKey(placementId))
            {
                _dictCallbacks.Remove(placementId);
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Initialize.
        /// </summary>
        public void Initialize()
        {
#if DEBUG_MY_UNITY_ADS
            Debug.Log("[" + typeof(MyUnityAdsManager).Name + "] Initialize(): _androidGameId=" + _androidGameId + " | _iosGameId=" + _iosGameId + " | _isTestMode=" + _isTestMode);
#endif

#if UNITY_ANDROID
            Advertisement.AddListener(this);
            Advertisement.Initialize(_androidGameId, _isTestMode);
#elif UNITY_IOS
            Advertisement.AddListener(this);
            Advertisement.Initialize(_iosGameId, _isTestMode);
#endif
        }

        /// <summary>
        /// Check if a interstitial video is ready.
        /// </summary>
        public bool IsInterstitialVideoReady(string placementId = "video")
        {
            return Advertisement.IsReady(placementId);
        }

        /// <summary>
        /// Show an interstitial video.
        /// </summary>
        public void ShowInterstitialVideo(Action onStartedCallback = null, Action onFinishedCallback = null, Action onFailedCallback = null, Action onSkippedCallback = null)
        {
            ShowInterstitialVideoByPlacementId("video", onStartedCallback, onFinishedCallback, onFailedCallback, onSkippedCallback);
        }

        /// <summary>
        /// Show an interstitial video.
        /// </summary>
        public void ShowInterstitialVideoByPlacementId(string placementId, Action onStartedCallback = null, Action onFinishedCallback = null, Action onFailedCallback = null, Action onSkippedCallback = null)
        {
#if DEBUG_MY_UNITY_ADS
            Debug.Log("[" + typeof(MyUnityAdsManager).Name + "] ShowInterstitialVideoByPlacementId(): placementId=" + placementId);
#endif

#if UNITY_ANDROID || UNITY_IOS
            _ShowVideo(placementId, onStartedCallback, onFinishedCallback, onFailedCallback, onSkippedCallback);
#endif
        }

        /// <summary>
        /// Check if a rewarded video is ready.
        /// </summary>
        public bool IsRewardedVideoReady(string placementId = "rewardedVideo")
        {
            return Advertisement.IsReady(placementId);
        }

        /// <summary>
        /// Show an rewarded video.
        /// </summary>
        public void ShowRewardedVideo(Action onStartedCallback = null, Action onFinishedCallback = null, Action onFailedCallback = null, Action onSkippedCallback = null)
        {
            ShowRewardedVideoByPlacementId("rewardedVideo", onStartedCallback, onFinishedCallback, onFailedCallback, onSkippedCallback);
        }

        /// <summary>
        /// Show a rewarded video.
        /// </summary>
        public void ShowRewardedVideoByPlacementId(string placementId, Action onStartedCallback = null, Action onFinishedCallback = null, Action onFailedCallback = null, Action onSkippedCallback = null)
        {
#if DEBUG_MY_UNITY_ADS
            Debug.Log("[" + typeof(MyUnityAdsManager).Name + "] ShowRewardedVideoByPlacementId(): placementId=" + placementId);
#endif

#if UNITY_ANDROID || UNITY_IOS
            _ShowVideo(placementId, onStartedCallback, onFinishedCallback, onFailedCallback, onSkippedCallback);
#endif
        }

        /// <summary>
        /// Show a banner.
        /// </summary>
        public void ShowBanner(BannerPosition position, string placementId = "banner")
        {
#if DEBUG_MY_UNITY_ADS
            Debug.Log("[" + typeof(MyUnityAdsManager).Name + "] ShowBanner(): placementId=" + placementId + " | position=" + position.ToString());
#endif

#if UNITY_ANDROID || UNITY_IOS
            _isShowBanner = true;
            StartCoroutine(_DoShowBanner(position, placementId));
#endif
        }

        /// <summary>
        /// Hide the banner.
        /// </summary>
        public void HideBanner()
        {
#if DEBUG_MY_UNITY_ADS
            Debug.Log("[" + typeof(MyUnityAdsManager).Name + "] HideBanner()");
#endif

#if UNITY_ANDROID || UNITY_IOS
            if (_isShowBanner)
            {
                _isShowBanner = false;
                Advertisement.Banner.Hide();
            }
#endif
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Show a video.
        /// </summary>
        private void _ShowVideo(string placementId = "video", Action onStartedCallback = null, Action onFinishedCallback = null, Action onFailedCallback = null, Action onSkippedCallback = null)
        {
#if UNITY_ANDROID || UNITY_IOS
            if (onStartedCallback != null || onFinishedCallback != null || onFailedCallback != null || onSkippedCallback != null)
            {
                _dictCallbacks[placementId] = new Action[4];
                _dictCallbacks[placementId][0] = onFailedCallback;
                _dictCallbacks[placementId][1] = onSkippedCallback;
                _dictCallbacks[placementId][2] = onFinishedCallback;
                _dictCallbacks[placementId][3] = onStartedCallback;
            }
            else if (_dictCallbacks.ContainsKey(placementId))
            {
                _dictCallbacks.Remove(placementId);
            }

            if (Advertisement.IsReady(placementId))
            {
                Advertisement.Show(placementId);
            }
            else
            {
                StartCoroutine(_DoShowVideo(placementId));
            }
#endif
        }

        /// <summary>
        /// Show a video.
        /// </summary>
        private IEnumerator _DoShowVideo(string placementId = "video")
        {
            while (!Advertisement.IsReady(placementId))
            {
                yield return null;
            }

            Advertisement.Show(placementId);
        }

        /// <summary>
        /// Show a banner.
        /// </summary>
        private IEnumerator _DoShowBanner(BannerPosition position, string placementId = "banner")
        {
            while (!Advertisement.IsReady(placementId) && _isShowBanner)
            {
                yield return null;
            }

            if (_isShowBanner)
            {
                Advertisement.Banner.SetPosition(position);
                Advertisement.Banner.Show(placementId);
            }
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyUnityAdsManager))]
    public class MyUnityAdsManagerEditor : Editor
    {
        private MyUnityAdsManager _script;
        private SerializedProperty _androidGameId;
        private SerializedProperty _iosGameId;
        private SerializedProperty _isTestMode;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            _script = (MyUnityAdsManager)target;
            _androidGameId = serializedObject.FindProperty("_androidGameId");
            _iosGameId = serializedObject.FindProperty("_iosGameId");
            _isTestMode = serializedObject.FindProperty("_isTestMode");
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(_script), typeof(MyUnityAdsManager), false);

            serializedObject.Update();

            _iosGameId.stringValue = EditorGUILayout.TextField("IOS Game ID", _iosGameId.stringValue);
            _androidGameId.stringValue = EditorGUILayout.TextField("Android Game ID", _androidGameId.stringValue);
            _isTestMode.boolValue = EditorGUILayout.Toggle("Enable Test Mode", _isTestMode.boolValue);

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}

#endif