/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyAdMobManager (version 1.27)
 * Require:     GoogleMobileAds-v8.0.0
 * Require:     GoogleMobileAds-v7.1.0 (USE_MY_ADMOB_7_1_0)
 */

#pragma warning disable 0162
#pragma warning disable 0414

#if USE_MY_ADMOB
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System;
using System.Collections.Generic;
using GoogleMobileAds.Api;

#if USE_MY_ADMOB_APPLOVIN
using GoogleMobileAds.Api.Mediation.AppLovin;
#endif

#if USE_MY_ADMOB_MOPUB
using GoogleMobileAds.Api.Mediation.MoPub;
#endif

#if USE_MY_ADMOB_UNITY_ADS
using GoogleMobileAds.Api.Mediation.UnityAds;
#endif

#if USE_MY_ADMOB_VUNGLE
using GoogleMobileAds.Api.Mediation.Vungle;
#endif

namespace MyClasses
{
    public class MyAdMobManager : MonoBehaviour
    {
        #region ----- Define -----

        private const int CALLBACK_DELAY_FRAME = 3;

        #endregion

        #region ----- Variable -----

        [SerializeField]
        private bool _isInitialized = false;
        [SerializeField]
        private bool _isEnableTest = true;
        [SerializeField]
        private bool _isUseGoogleTestAdsId = true;
        [SerializeField]
        private string _testDeviceId = "19CEAA9BB2BC2D8C16D6D202AE2A6C1E";

        [SerializeField]
        private string _androidDefaultBannerId = string.Empty;
        [SerializeField]
        private string _iosDefaultBannerId = string.Empty;
        [SerializeField]
        private int _countBannerRequest = 0;
        [SerializeField]
        private long _lastBannerRequestTimestamp = 0;
        [SerializeField]
        private long _lastBannerShowTimestamp = 0;
        [SerializeField]
        private bool _isLoadingBanner = false;
        [SerializeField]
        private bool _isShowingBanner = false;
        [SerializeField]
        private int _bannerLoadedDelayFrameCallback = 0;
        [SerializeField]
        private int _bannerFailedToLoadDelayFrameCallback = 0;
        [SerializeField]
        private int _bannerOpenedDelayFrameCallback = 0;
        [SerializeField]
        private int _bannerClosedDelayFrameCallback = 0;

        [SerializeField]
        private string _androidDefaultInterstitialAdId = string.Empty;
        [SerializeField]
        private string _iosDefaultInterstitialAdId = string.Empty;
        [SerializeField]
        private long _lastInterstitialHideTimestamp = 0;
        [SerializeField]
        private bool _isLoadingInterstitial = false;
        [SerializeField]
        private int _interstitialLoadedDelayFrameCallback = 0;
        [SerializeField]
        private int _interstitialFailedToLoadDelayFrameCallback = 0;
        [SerializeField]
        private int _interstitialOpenedDelayFrameCallback = 0;
        [SerializeField]
        private int _interstitialClosedDelayFrameCallback = 0;

        [SerializeField]
        private string _androidDefaultRewardedAdId = string.Empty;
        [SerializeField]
        private string _iosDefaultRewardedAdId = string.Empty;
        [SerializeField]
        private long _lastRewardedHideTimestamp = 0;
        [SerializeField]
        private bool _isLoadingRewardred = false;
        [SerializeField]
        private bool _isHasReward = false;
        [SerializeField]
        private int _rewardedLoadedDelayFrameCallback = 0;
        [SerializeField]
        private int _rewardedFailedToLoadDelayFrameCallback = 0;
        [SerializeField]
        private int _rewardedOpenedDelayFrameCallback = 0;
        [SerializeField]
        private int _rewardedFailedToOpenDelayFrameCallback = 0;
        [SerializeField]
        private int _rewardedClosedDelayFrameCallback = 0;

        [SerializeField]
        private string _androidDefaultAppOpenAdId = string.Empty;
        [SerializeField]
        private string mIosDefaultAppOpenAdId = string.Empty;
        [SerializeField]
        private bool _isLoadingAppOpen = false;
        [SerializeField]
        private bool _isShowingAppOpen = false;
        [SerializeField]
        private long _lastAppOpenRequestTimestamp = 0;
        [SerializeField]
        private long _lastAppOpenHideTimestamp = 0;

        private BannerView _banner;
        private AdRequest _bannerRequest;
        private Action _onBannerLoadedCallback;
        private Action _onBannerFailedToLoadCallback;
        private Action _onBannerOpenedCallback;
        private Action _onBannerClosedCallback;

        private InterstitialAd _interstitialAd;
        private AdRequest _interstitialAdRequest;
        private Action _onInterstitialAdLoadedCallback;
        private Action _onInterstitialAdFailedToLoadCallback;
        private Action _onInterstitialAdOpenedCallback;
        private Action _onInterstitialAdClosedCallback;

        private RewardedAd _rewardedAd;
        private AdRequest _rewardedAdRequest;
        private Action _onRewardedAdLoadedCallback;
        private Action _onRewardedAdFailedToLoadCallback;
        private Action _onRewardedAdOpenedCallback;
        private Action _onRewardedAdFailedToOpenCallback;
        private Action _onRewardedAdSkippedCallback;
        private Action _onRewardedAdUserEarnedRewardCallback;
        private Action _onRewardedAdClosedCallback;

        private AppOpenAd _appOpenAd;
        private AdRequest _appOpenAdRequest;
        private Action _onAppOpenAdOpenedCallback;
        private Action _onAppOpenAdFailedToOpenCallback;
        private Action _onAppOpenAdImpressionRecordedCallback;
        private Action _onAppOpenAdClosedCallback;

        public bool IsInitialized
        {
            get { return _isInitialized; }
        }

        public long CountBannerRequest
        {
            get { return _countBannerRequest; }
        }

        public long LastBannerRequestTimestamp
        {
            get { return _lastBannerRequestTimestamp; }
        }

        public long LastBannerShowTimestamp
        {
            get { return _lastBannerShowTimestamp; }
            set { _lastBannerShowTimestamp = value; }
        }

        public long LastInterstitialHideTimestamp
        {
            get { return _lastInterstitialHideTimestamp; }
            set { _lastInterstitialHideTimestamp = value; }
        }

        public long LastRewardedHideTimestamp
        {
            get { return _lastRewardedHideTimestamp; }
            set { _lastRewardedHideTimestamp = value; }
        }

        public long LastAppOpenRequestTimestamp
        {
            get { return _lastAppOpenRequestTimestamp; }
        }

        public long LastAppOpenHideTimestamp
        {
            get { return _lastAppOpenHideTimestamp; }
            set { _lastAppOpenHideTimestamp = value; }
        }

        #endregion

        #region ----- Singleton -----

        private static object _singletonLock = new object();
        private static MyAdMobManager _instance;

        public static MyAdMobManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_singletonLock)
                    {
                        _instance = (MyAdMobManager)FindObjectOfType(typeof(MyAdMobManager));
                        if (_instance == null)
                        {
                            GameObject obj = new GameObject(typeof(MyAdMobManager).Name);
                            _instance = obj.AddComponent<MyAdMobManager>();
                        }
                        DontDestroyOnLoad(_instance);
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region ----- MonoBehaviour Implementation -----

        /// <summary>
        /// Update.
        /// </summary>
        void Update()
        {
            // banner
            if (_bannerLoadedDelayFrameCallback > 0)
            {
                _bannerLoadedDelayFrameCallback -= 1;
                if (_bannerLoadedDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
                    Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnBannerLoaded(): callback");
#endif

                    _isLoadingBanner = false;
                    _isShowingBanner = true;
                    _lastBannerShowTimestamp = MyLocalTime.CurrentUnixTime;

                    if (_onBannerLoadedCallback != null)
                    {
                        _onBannerLoadedCallback();
                    }
                }
            }
            else if (_bannerFailedToLoadDelayFrameCallback > 0)
            {
                _bannerFailedToLoadDelayFrameCallback -= 1;
                if (_bannerFailedToLoadDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
                    Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnBannerFaiedToLoad(): callback");
#endif

                    _isLoadingBanner = false;

                    if (_onBannerFailedToLoadCallback != null)
                    {
                        _onBannerFailedToLoadCallback();
                    }
                }
            }
            if (_bannerOpenedDelayFrameCallback > 0)
            {
                _bannerOpenedDelayFrameCallback -= 1;
                if (_bannerOpenedDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
                    Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnBannerOpened(): callback");
#endif

                    if (_onBannerOpenedCallback != null)
                    {
                        _onBannerOpenedCallback();
                    }
                }
            }
            if (_bannerClosedDelayFrameCallback > 0)
            {
                _bannerClosedDelayFrameCallback -= 1;
                if (_bannerClosedDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
                    Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnBannerClosed(): callback");
#endif

                    if (_onBannerClosedCallback != null)
                    {
                        _onBannerClosedCallback();
                    }
                }
            }

            // interstitial
            if (_interstitialLoadedDelayFrameCallback > 0)
            {
                _interstitialLoadedDelayFrameCallback -= 1;
                if (_interstitialLoadedDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
                    Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialLoaded(): callback");
#endif

                    _isLoadingInterstitial = false;

                    if (_onInterstitialAdLoadedCallback != null)
                    {
                        _onInterstitialAdLoadedCallback();
                    }
                }
            }
            else if (_interstitialFailedToLoadDelayFrameCallback > 0)
            {
                _interstitialFailedToLoadDelayFrameCallback -= 1;
                if (_interstitialFailedToLoadDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
                    Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialFaiedToLoad(): callback");
#endif

                    _isLoadingInterstitial = false;

                    if (_onInterstitialAdFailedToLoadCallback != null)
                    {
                        _onInterstitialAdFailedToLoadCallback();
                    }
                }
            }
            if (_interstitialOpenedDelayFrameCallback > 0)
            {
                _interstitialOpenedDelayFrameCallback -= 1;
                if (_interstitialOpenedDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
                    Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialOpened(): callback");
#endif

                    if (_onInterstitialAdOpenedCallback != null)
                    {
                        _onInterstitialAdOpenedCallback();
                    }
                }
            }
            if (_interstitialClosedDelayFrameCallback > 0)
            {
                _interstitialClosedDelayFrameCallback -= 1;
                if (_interstitialClosedDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
                    Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialClosed(): callback");
#endif

                    _isLoadingInterstitial = false;
                    _lastInterstitialHideTimestamp = MyLocalTime.CurrentUnixTime;

                    if (_onInterstitialAdClosedCallback != null)
                    {
                        _onInterstitialAdClosedCallback();
                    }
                }
            }

            // rewarded
            if (_rewardedLoadedDelayFrameCallback > 0)
            {
                _rewardedLoadedDelayFrameCallback -= 1;
                if (_rewardedLoadedDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
                    Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdLoaded(): callback");
#endif

                    _isLoadingRewardred = false;

                    if (_onRewardedAdLoadedCallback != null)
                    {
                        _onRewardedAdLoadedCallback();
                    }
                }
            }
            else if (_rewardedFailedToLoadDelayFrameCallback > 0)
            {
                _rewardedFailedToLoadDelayFrameCallback -= 1;
                if (_rewardedFailedToLoadDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
                    Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdFaiedToLoad(): callback");
#endif

                    _isLoadingRewardred = false;

                    if (_onRewardedAdFailedToLoadCallback != null)
                    {
                        _onRewardedAdFailedToLoadCallback();
                    }
                }
            }
            if (_rewardedOpenedDelayFrameCallback > 0)
            {
                _rewardedOpenedDelayFrameCallback -= 1;
                if (_rewardedOpenedDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
                    Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdOpened(): callback");
#endif

                    if (_onRewardedAdOpenedCallback != null)
                    {
                        _onRewardedAdOpenedCallback();
                    }
                }
            }
            else if (_rewardedFailedToOpenDelayFrameCallback > 0)
            {
                _rewardedFailedToOpenDelayFrameCallback -= 1;
                if (_rewardedFailedToOpenDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
                    Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdFaiedToOpen(): callback");
#endif

                    if (_onRewardedAdFailedToOpenCallback != null)
                    {
                        _onRewardedAdFailedToOpenCallback();
                    }
                }
            }
            if (_rewardedClosedDelayFrameCallback > 0)
            {
                _rewardedClosedDelayFrameCallback -= 1;
                if (_rewardedClosedDelayFrameCallback == 0)
                {
                    _isLoadingRewardred = false;
                    _lastRewardedHideTimestamp = MyLocalTime.CurrentUnixTime;

                    if (_isHasReward)
                    {
#if DEBUG_MY_ADMOB
                    Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdUserEarnedReward(): callback");
#endif

                        if (_onRewardedAdUserEarnedRewardCallback != null)
                        {
                            _onRewardedAdUserEarnedRewardCallback();
                        }
                    }
                    else
                    {
                        if (_onRewardedAdSkippedCallback != null)
                        {
                            _onRewardedAdSkippedCallback();
                        }
                    }

#if DEBUG_MY_ADMOB
                    Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdClosed(): callback");
#endif

                    if (_onRewardedAdClosedCallback != null)
                    {
                        _onRewardedAdClosedCallback();
                    }
                }
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Initialize.
        /// </summary>
        public void Initialize(Action onCompleteCallback = null)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] Initialize()");
#endif

            if (_isEnableTest && _testDeviceId.Length > 0)
            {
                if (_isUseGoogleTestAdsId)
                {
                    _androidDefaultBannerId = "ca-app-pub-3940256099942544/6300978111";
                    _androidDefaultInterstitialAdId = "ca-app-pub-3940256099942544/1033173712";
                    _androidDefaultRewardedAdId = "ca-app-pub-3940256099942544/5224354917";
                    _androidDefaultAppOpenAdId = "ca-app-pub-3940256099942544/3419835294";

                    _iosDefaultBannerId = "ca-app-pub-3940256099942544/2934735716";
                    _iosDefaultInterstitialAdId = "ca-app-pub-3940256099942544/4411468910";
                    _iosDefaultRewardedAdId = "ca-app-pub-3940256099942544/1712485313";
                    mIosDefaultAppOpenAdId = "ca-app-pub-3940256099942544/5662855259";

#if UNITY_ANDROID
                    PlayerPrefs.SetString("MyAdMobManager_BannerId", _androidDefaultBannerId);
                    PlayerPrefs.SetString("MyAdMobManager_InterstitialAdId", _androidDefaultInterstitialAdId);
                    PlayerPrefs.SetString("MyAdMobManager_RewardedAdId", _androidDefaultRewardedAdId);
#elif UNITY_IOS
                    PlayerPrefs.SetString("MyAdMobManager_BannerId", _iosDefaultBannerId);
                    PlayerPrefs.SetString("MyAdMobManager_InterstitialAdId", _iosDefaultInterstitialAdId);
                    PlayerPrefs.SetString("MyAdMobManager_RewardedAdId", _iosDefaultRewardedAdId);
#endif
                }

                List<string> deviceIds = new List<string>();
                deviceIds.AddRange(_testDeviceId.Split(';'));
                RequestConfiguration requestConfiguration = new RequestConfiguration.Builder()
                    .SetTestDeviceIds(deviceIds)
#if USE_MY_ADMOB_UNDER_13
                    .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.True)
#endif
                    .build();
                MobileAds.SetRequestConfiguration(requestConfiguration);
            }

            MobileAds.Initialize(initStatus =>
            {
#if DEBUG_MY_ADMOB
                Debug.Log("[" + typeof(MyAdMobManager).Name + "] Initialize(): completed");
#endif

                _isInitialized = true;

                if (onCompleteCallback != null)
                {
                    onCompleteCallback();
                }
            });

#if USE_MY_ADMOB_APPLOVIN
            AppLovin.Initialize();
            AppLovin.SetHasUserConsent(true);
#endif

#if USE_MY_ADMOB_MOPUB
            MoPub.InitializeSdk("8c09b6f2cb324838acf2fdad6899f5a8");
#endif

#if USE_MY_ADMOB_UNITY_ADS
            // UnityAds.SetGDPRConsentMetaData(true); // Unity Ads < 3.3.0
            GoogleMobileAds.Mediation.UnityAds.Api.UnityAds.SetConsentMetaData("gdpr.consent", true);
#endif

#if USE_MY_ADMOB_VUNGLE
            Vungle.UpdateConsentStatus(VungleConsent.ACCEPTED);
#endif
        }

        /// <summary>
        /// Set a new banner id.
        /// </summary>
        public void SetDefaultBannerId(string id)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] SetDefaultBannerId(): id=" + id);
#endif

            if (_isEnableTest && _isUseGoogleTestAdsId)
            {
                return;
            }

#if UNITY_ANDROID
            PlayerPrefs.SetString("MyAdMobManager_BannerId", string.IsNullOrEmpty(id) ? _androidDefaultBannerId : id);
            PlayerPrefs.Save();
#elif UNITY_IOS
            PlayerPrefs.SetString("MyAdMobManager_BannerId", string.IsNullOrEmpty(id) ? _iosDefaultBannerId : id);
            PlayerPrefs.Save();
#endif
        }

        /// <summary>
        /// Check if a interstitial ad is loading.
        /// </summary>
        public bool IsBannerLoading()
        {
            return _isLoadingBanner;
        }

        /// <summary>
        /// Check if a interstitial ad is showing.
        /// </summary>
        public bool IsBannerShowing()
        {
            return _isShowingBanner;
        }

        /// <summary>
        /// Show a banner.
        /// </summary>
        public void ShowBanner(string adUnitId = null, AdSize size = null, AdPosition position = AdPosition.Bottom, Action onLoadedCallback = null, Action onFailedToLoadCallback = null, Action onOpeningCallback = null, Action onClosedCallback = null)
        {
            if (_isEnableTest && _isUseGoogleTestAdsId)
            {
#if UNITY_ANDROID
                adUnitId = _androidDefaultBannerId;
#elif UNITY_IOS
                adUnitId = _iosDefaultBannerId;
#endif
            }

            if (adUnitId == null)
            {
#if UNITY_ANDROID
                adUnitId = PlayerPrefs.GetString("MyAdMobManager_BannerId", _androidDefaultBannerId);
#elif UNITY_IOS
                adUnitId = PlayerPrefs.GetString("MyAdMobManager_BannerId", _iosDefaultBannerId);
#endif
            }

            if (size == null)
            {
                size = AdSize.Banner;
            }

#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] ShowBanner(): adUnitId=" + adUnitId + " | size=" + size.AdType + " | position=" + position);
#endif

            _onBannerLoadedCallback = onLoadedCallback;
            _onBannerFailedToLoadCallback = onFailedToLoadCallback;
            _onBannerOpenedCallback = onOpeningCallback;
            _onBannerClosedCallback = onClosedCallback;

            _countBannerRequest += 1;

            _lastBannerRequestTimestamp = MyLocalTime.CurrentUnixTime;
#if UNITY_EDITOR
            _lastBannerShowTimestamp = MyLocalTime.CurrentUnixTime;
#else
            _isLoadingBanner = true;
#endif

            if (_bannerRequest == null)
            {
                _bannerRequest = new AdRequest.Builder().Build();
            }
            if (_banner != null)
            {
                _banner.Destroy();
            }
            _banner = new BannerView(adUnitId, size, position);
            _banner.LoadAd(_bannerRequest);
#if USE_MY_ADMOB_7_1_0
            _banner.OnAdLoaded += _OnBannerLoaded_7_1_0;
            _banner.OnAdFailedToLoad += _OnBannerFaiedToLoad_7_1_0;
            _banner.OnAdOpening += _OnBannerOpening_7_1_0;
            _banner.OnAdClosed += _OnBannerClosed_7_1_0;
#else
            _banner.OnBannerAdLoaded += _OnBannerLoaded;
            _banner.OnBannerAdLoadFailed += _OnBannerFaiedToLoad;
            _banner.OnAdFullScreenContentOpened += _OnBannerOpened;
            _banner.OnAdFullScreenContentClosed += _OnBannerClosed;
#endif
        }

        /// <summary>
        /// Hide the banner.
        /// </summary>
        public void HideBanner()
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] HideBanner()");
#endif

            _onBannerLoadedCallback = null;
            _onBannerFailedToLoadCallback = null;
            _onBannerOpenedCallback = null;
            _onBannerClosedCallback = null;

            if (_banner != null)
            {
                _banner.Hide();
            }
        }

        /// <summary>
        /// Set a new interstitial ad id.
        /// </summary>
        public void SetDefaultInterstitialAdId(string id)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] SetDefaultInterstitialAdId(): id=" + id);
#endif

            if (_isEnableTest && _isUseGoogleTestAdsId)
            {
                return;
            }

#if UNITY_ANDROID
            PlayerPrefs.SetString("MyAdMobManager_InterstitialAdId", string.IsNullOrEmpty(id) ? _androidDefaultInterstitialAdId : id);
            PlayerPrefs.Save();
#elif UNITY_IOS
            PlayerPrefs.SetString("MyAdMobManager_InterstitialAdId", string.IsNullOrEmpty(id) ? _iosDefaultInterstitialAdId : id);
            PlayerPrefs.Save();
#endif
        }

        /// <summary>
        /// Check if a interstitial ad is loading.
        /// </summary>
        public bool IsInterstitialAdLoading()
        {
            return _isLoadingInterstitial;
        }

        /// <summary>
        /// Check if an interstitial ad is ready.
        /// </summary>
        public bool IsInterstitialAdLoaded()
        {
#if USE_MY_ADMOB_7_1_0
            return _interstitialAd != null && _interstitialAd.IsLoaded();
#else
            return _interstitialAd != null && _interstitialAd.CanShowAd();
#endif
        }

        /// <summary>
        /// Load an interstitial ad.
        /// </summary>
        public void LoadInterstitialAd(string adUnitId = null, Action onLoadedCallback = null, Action onFailedToLoadCallback = null)
        {
            if (_isEnableTest && _isUseGoogleTestAdsId)
            {
#if UNITY_ANDROID
                adUnitId = _androidDefaultInterstitialAdId;
#elif UNITY_IOS
                adUnitId = _iosDefaultInterstitialAdId;
#endif
            }

            if (adUnitId == null)
            {
#if UNITY_ANDROID
                adUnitId = PlayerPrefs.GetString("MyAdMobManager_InterstitialAdId", _androidDefaultInterstitialAdId);
#elif UNITY_IOS
                adUnitId = PlayerPrefs.GetString("MyAdMobManager_InterstitialAdId", _iosDefaultInterstitialAdId);
#endif
            }

#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] LoadInterstitialAd(): adUnitId=" + adUnitId);
#endif

            _onInterstitialAdLoadedCallback = onLoadedCallback;
            _onInterstitialAdFailedToLoadCallback = onFailedToLoadCallback;

            _isLoadingInterstitial = true;

            if (_interstitialAdRequest == null)
            {
                _interstitialAdRequest = new AdRequest.Builder().Build();
            }

#if USE_MY_ADMOB_7_1_0
            _interstitialAd = new InterstitialAd(adUnitId);
            _interstitialAd.LoadAd(_interstitialAdRequest);
            _interstitialAd.OnAdLoaded += _OnInterstitialLoaded_7_1_0;
            _interstitialAd.OnAdFailedToLoad += _OnInterstitialFaiedToLoad_7_1_0;
            _interstitialAd.OnAdOpening += _OnInterstitialOpening_7_1_0;
            _interstitialAd.OnAdClosed += _OnInterstitialClosed_7_1_0;

#if UNITY_EDITOR
            _OnInterstitialLoaded_7_1_0(null, null);
#endif
#else
            if (_interstitialAd != null)
            {
                _interstitialAd.Destroy();
                _interstitialAd = null;
            }

            InterstitialAd.Load(adUnitId, _interstitialAdRequest, (InterstitialAd ad, LoadAdError error) =>
            {
                _isLoadingInterstitial = false;

                if (error != null || ad == null)
                {
#if DEBUG_MY_ADMOB
                    Debug.LogError("[" + typeof(MyAdMobManager).Name + "] LoadInterstitialAd(): error=" + error.GetMessage());
#endif

                    _OnInterstitialFaiedToLoad(error);
                }
                else
                {
#if DEBUG_MY_ADMOB
                    Debug.Log("[" + typeof(MyAdMobManager).Name + "] LoadInterstitialAd(): succeed");
#endif

                    _interstitialAd = ad;
                    _interstitialAd.OnAdFullScreenContentOpened += _OnInterstitialOpened;
                    _interstitialAd.OnAdFullScreenContentClosed += _OnInterstitialClosed;

                    _OnInterstitialLoaded();
                }
            });

#if UNITY_EDITOR
            _OnInterstitialLoaded();
#endif
#endif
        }

        /// <summary>
        /// Show an interstitial ad.
        /// </summary>
        public void ShowInterstitialAd(Action onOpeningCallback = null, Action onClosedCallback = null)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] ShowInterstitialAd(): isLoaded=" + IsInterstitialAdLoaded());
#endif

            if (IsInterstitialAdLoaded())
            {
                _onInterstitialAdLoadedCallback = null;
                _onInterstitialAdFailedToLoadCallback = null;
                _onInterstitialAdOpenedCallback = onOpeningCallback;
                _onInterstitialAdClosedCallback = onClosedCallback;

                _lastInterstitialHideTimestamp = MyLocalTime.CurrentUnixTime;

                _interstitialAd.Show();
            }
        }

        /// <summary>
        /// Set a new rewarded ad id.
        /// </summary>
        public void SetDefaultRewardedAdId(string id)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] SetDefaultRewardedAdId(): id=" + id);
#endif

            if (_isEnableTest && _isUseGoogleTestAdsId)
            {
                return;
            }

#if UNITY_ANDROID
            PlayerPrefs.SetString("MyAdMobManager_RewardedAdId", string.IsNullOrEmpty(id) ? _androidDefaultRewardedAdId : id);
            PlayerPrefs.Save();
#elif UNITY_IOS
            PlayerPrefs.SetString("MyAdMobManager_RewardedAdId", string.IsNullOrEmpty(id) ? _iosDefaultRewardedAdId : id);
            PlayerPrefs.Save();
#endif
        }

        /// <summary>
        /// Check if a rewarded ad is loading.
        /// </summary>
        public bool IsRewardedAdLoading()
        {
            return _isLoadingRewardred;
        }

        /// <summary>
        /// Check if a rewarded ad is ready.
        /// </summary>
        public bool IsRewardedAdLoaded()
        {
#if USE_MY_ADMOB_7_1_0
            return _rewardedAd != null && _rewardedAd.IsLoaded();
#else
            return _rewardedAd != null && _rewardedAd.CanShowAd();
#endif
        }

        /// <summary>
        /// Load a rewarded ad.
        /// </summary>
        public void LoadRewardedAd(string adUnitId = null, Action onLoadedCallback = null, Action onFailedToLoadCallback = null)
        {
            if (_isEnableTest && _isUseGoogleTestAdsId)
            {
#if UNITY_ANDROID
                adUnitId = _androidDefaultRewardedAdId;
#elif UNITY_IOS
                adUnitId = _iosDefaultRewardedAdId;
#endif
            }

            if (adUnitId == null)
            {
#if UNITY_ANDROID
                adUnitId = PlayerPrefs.GetString("MyAdMobManager_RewardedAdId", _androidDefaultRewardedAdId);
#elif UNITY_IOS
                adUnitId = PlayerPrefs.GetString("MyAdMobManager_RewardedAdId", _iosDefaultRewardedAdId);
#endif
            }

#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] LoadRewardedAd(): adUnitId=" + adUnitId);
#endif
            _onRewardedAdLoadedCallback = onLoadedCallback;
            _onRewardedAdFailedToLoadCallback = onFailedToLoadCallback;

            _isLoadingRewardred = true;

            if (_rewardedAdRequest == null)
            {
                _rewardedAdRequest = new AdRequest.Builder().Build();
            }

#if USE_MY_ADMOB_7_1_0
            _rewardedAd = new RewardedAd(adUnitId);
            _rewardedAd.LoadAd(_rewardedAdRequest);
            _rewardedAd.OnAdLoaded += _OnRewardedAdLoaded_7_1_0;
            _rewardedAd.OnAdFailedToLoad += _OnRewardedAdFaiedToLoad_7_1_0;
            _rewardedAd.OnAdOpening += _OnRewardedAdOpening_7_1_0;
            _rewardedAd.OnAdFailedToShow += _OnRewardedAdFaiedToShow_7_1_0;
            _rewardedAd.OnUserEarnedReward += _OnRewardedAdUserEarnedReward_7_1_0;
            _rewardedAd.OnAdClosed += _OnRewardedAdClosed_7_1_0;

#if UNITY_EDITOR
            _OnRewardedAdLoaded_7_1_0(null, null);
#endif
#else
            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }

            RewardedAd.Load(adUnitId, _rewardedAdRequest, (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
#if DEBUG_MY_ADMOB
                    Debug.LogError("[" + typeof(MyAdMobManager).Name + "] LoadRewardedAd(): error=" + error.GetMessage());
#endif

                    _OnRewardedAdFaiedToLoad(error);
                }
                else
                {
#if DEBUG_MY_ADMOB
                    Debug.Log("[" + typeof(MyAdMobManager).Name + "] LoadRewardedAd(): succeed");
#endif

                    _rewardedAd = ad;
                    _rewardedAd.OnAdFullScreenContentOpened += _OnRewardedAdOpened;
                    _rewardedAd.OnAdFullScreenContentFailed += _OnRewardedAdFaiedToOpen;
                    _rewardedAd.OnAdFullScreenContentClosed += _OnRewardedAdClosed;

                    _OnRewardedAdLoaded(ad.GetResponseInfo());
                }
            });
#endif
        }

        /// <summary>
        /// Show a rewarded ad.
        /// </summary>
        public void ShowRewardedAd(Action onOpenedCallback = null, Action onUserEarnedRewardCallback = null, Action onFailedToOpenCallback = null, Action onSkippedCallback = null, Action onClosedCallback = null)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] ShowRewardedAd(): isLoaded=" + IsRewardedAdLoaded());
#endif

            if (IsRewardedAdLoaded())
            {
                _onRewardedAdLoadedCallback = null;
                _onRewardedAdFailedToLoadCallback = null;
                _onRewardedAdOpenedCallback = onOpenedCallback;
                _onRewardedAdFailedToOpenCallback = onFailedToOpenCallback;
                _onRewardedAdSkippedCallback = onSkippedCallback;
                _onRewardedAdUserEarnedRewardCallback = onUserEarnedRewardCallback;
                _onRewardedAdClosedCallback = onClosedCallback;

                _isHasReward = false;

#if USE_MY_ADMOB_7_1_0
                _rewardedAd.Show();
#else
                _rewardedAd.Show((Reward reward) =>
                {
                    _OnRewardedAdUserEarnedReward();
                });
#endif
            }
        }

        /// <summary>
        /// Set a new app open ad id.
        /// </summary>
        public void SetDefaultAppOpenAdId(string id)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] SetDefaultAppOpenAdId(): id=" + id);
#endif

            if (_isEnableTest && _isUseGoogleTestAdsId)
            {
                return;
            }
            
#if UNITY_ANDROID
            PlayerPrefs.SetString("MyAdMobManager_AppOpenAdId", string.IsNullOrEmpty(id) ? _androidDefaultAppOpenAdId : id);
#elif UNITY_IOS
            PlayerPrefs.SetString("MyAdMobManager_AppOpenAdId", string.IsNullOrEmpty(id) ? mIosDefaultAppOpenAdId : id);
#endif
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Check if an app open ad is loading.
        /// </summary>
        public bool IsAppOpenAdLoading()
        {
            return _isLoadingAppOpen;
        }

        /// <summary>
        /// Check if an app open ad is ready.
        /// </summary>
        public bool IsAppOpenAdLoaded()
        {
            return _appOpenAd != null && MyLocalTime.CurrentUnixTime - _lastAppOpenRequestTimestamp < 14400000;
        }

        /// <summary>
        /// Check if an app open ad is showing.
        /// </summary>
        public bool IsAppOpenAdShowing()
        {
            return _appOpenAd != null && _isShowingAppOpen;
        }

        /// <summary>
        /// Load an app open  ad.
        /// </summary>
        public void LoadAppOpenAd(ScreenOrientation screenOrientation = ScreenOrientation.Portrait, string adUnitId = null, Action onLoadedCallback = null, Action onFailedToLoadCallback = null)
        {
            if (_isEnableTest && _isUseGoogleTestAdsId)
            {
#if UNITY_ANDROID
                adUnitId = _androidDefaultAppOpenAdId;
#elif UNITY_IOS
                adUnitId = mIosDefaultAppOpenAdId;
#endif
            }

            if (adUnitId == null)
            {
#if UNITY_ANDROID
                adUnitId = PlayerPrefs.GetString("MyAdMobManager_AppOpenAdId", _androidDefaultAppOpenAdId);
#elif UNITY_IOS
                adUnitId = PlayerPrefs.GetString("MyAdMobManager_AppOpenAdId", mIosDefaultAppOpenAdId);
#endif
            }

#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] LoadAppOpenAd(): adUnitId=" + adUnitId + " | screenOrientation=" + screenOrientation);
#endif

            _appOpenAd = null;

            _onAppOpenAdOpenedCallback = null;
            _onAppOpenAdFailedToOpenCallback = null;
            _onAppOpenAdImpressionRecordedCallback = null;
            _onAppOpenAdClosedCallback = null;

            _isLoadingAppOpen = true;
            _isShowingAppOpen = false;

            if (_appOpenAdRequest == null)
            {
                _appOpenAdRequest = new AdRequest.Builder().Build();
            }
            
#if USE_MY_ADMOB_7_1_0
            AppOpenAd.LoadAd(adUnitId, screenOrientation, _appOpenAdRequest, ((appOpenAd, error) =>
            {
                _isLoadingAppOpen = false;

                if (error != null)
                {
#if DEBUG_MY_ADMOB
                    Debug.LogError("[" + typeof(MyAdMobManager).Name + "] LoadAppOpenAd(): message=" + error.LoadAdError.GetMessage());
#endif

                    if (onFailedToLoadCallback != null)
                    {
                        onFailedToLoadCallback();
                    }
                }
                else
                {
#if DEBUG_MY_ADMOB
                    Debug.Log("[" + typeof(MyAdMobManager).Name + "] LoadAppOpenAd(): succeed");
#endif

                    _appOpenAd = appOpenAd;
                    _lastAppOpenRequestTimestamp = MyLocalTime.CurrentUnixTime;

                    if (onLoadedCallback != null)
                    {
                        onLoadedCallback();
                    }
                }
            }));
#else
            if (_appOpenAd != null)
            {
                _appOpenAd.Destroy();
                _appOpenAd = null;
            }

            AppOpenAd.Load(adUnitId, screenOrientation, _appOpenAdRequest, (AppOpenAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
#if DEBUG_MY_ADMOB
                    Debug.LogError("[" + typeof(MyAdMobManager).Name + "] LoadAppOpenAd(): error=" + error.GetMessage());
#endif

                    if (onFailedToLoadCallback != null)
                    {
                        onFailedToLoadCallback();
                    }
                }
                else
                {
#if DEBUG_MY_ADMOB
                    Debug.Log("[" + typeof(MyAdMobManager).Name + "] LoadAppOpenAd(): succeed");
#endif

                    _lastAppOpenRequestTimestamp = MyLocalTime.CurrentUnixTime;

                    _appOpenAd = ad;
                    _appOpenAd.OnAdFullScreenContentOpened += _onAppOpenAdOpenedCallback;
                    _appOpenAd.OnAdFullScreenContentFailed += _OnAppOpenAdFailedToOpen;
                    _appOpenAd.OnAdImpressionRecorded += _OnAppOpenAdImpressionRecorded;
                    _appOpenAd.OnAdFullScreenContentClosed += _OnAppOpenAdClosed;

                    if (onLoadedCallback != null)
                    {
                        onLoadedCallback();
                    }
                }
            });
#endif
        }

        /// <summary>
        /// Show an app open ad.
        /// </summary>
        public void ShowAppOpenAd(Action onShowCallback = null, Action onFailedToShowCallback = null, Action onRecordImpressionCallback = null, Action onClosedCallback = null)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] ShowAppOpenAd(): isLoaded=" + IsAppOpenAdLoaded());
#endif

            if (IsAppOpenAdLoaded())
            {
                _onAppOpenAdOpenedCallback = onShowCallback;
                _onAppOpenAdFailedToOpenCallback = onFailedToShowCallback;
                _onAppOpenAdImpressionRecordedCallback = onRecordImpressionCallback;
                _onAppOpenAdClosedCallback = onClosedCallback;

#if USE_MY_ADMOB_7_1_0
                _appOpenAd.OnAdDidPresentFullScreenContent += _OnAppOpenDidPresentFullScreenContent_7_1_0;
                _appOpenAd.OnAdFailedToPresentFullScreenContent += _OnAppOpenFailedToPresentFullScreenContent_7_1_0;
                _appOpenAd.OnAdDidRecordImpression += _OnAppOpenDidRecordImpression_7_1_0;
                _appOpenAd.OnAdDidDismissFullScreenContent += _OnAppOpenDidDismissFullScreenContent_7_1_0;
#endif

                _appOpenAd.Show();
            }
        }

        #endregion

        #region ----- Banner Event -----

        private void _OnBannerLoaded()
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnBannerLoaded()");
#endif

            _bannerLoadedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnBannerFaiedToLoad(LoadAdError error)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnBannerFaiedToLoad(): error=" + error.GetMessage());
#endif

            _bannerFailedToLoadDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnBannerOpened()
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnBannerOpened()");
#endif

            _bannerOpenedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnBannerClosed()
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnBannerClosed()");
#endif

            _bannerClosedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

#if USE_MY_ADMOB_7_1_0
        private void _OnBannerLoaded_7_1_0(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnBannerLoaded_7_1_0()");
#endif

            _bannerLoadedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnBannerFaiedToLoad_7_1_0(object sender, AdFailedToLoadEventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnBannerFaiedToLoad_7_1_0(): message=" + args.ToString());
#endif

            _bannerFailedToLoadDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnBannerOpening_7_1_0(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnBannerOpening_7_1_0()");
#endif

            _bannerOpenedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnBannerClosed_7_1_0(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnBannerClosed_7_1_0()");
#endif

            _bannerClosedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }
#endif

        #endregion

        #region ----- Interstitial Event -----

        private void _OnInterstitialLoaded()
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialLoaded()");
#endif

            _interstitialLoadedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnInterstitialFaiedToLoad(LoadAdError error)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialFaiedToLoad(): error=" + error.GetMessage());
#endif

            _interstitialFailedToLoadDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnInterstitialOpened()
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialOpened()");
#endif

            _interstitialOpenedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnInterstitialClosed()
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialClosed()");
#endif

            _interstitialClosedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

#if USE_MY_ADMOB_7_1_0
        private void _OnInterstitialLoaded_7_1_0(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialLoaded_7_1_0()");
#endif

            _interstitialLoadedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnInterstitialFaiedToLoad_7_1_0(object sender, AdFailedToLoadEventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialFaiedToLoad_7_1_0(): message=" + args.ToString());
#endif

            _interstitialFailedToLoadDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnInterstitialOpening_7_1_0(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialOpening_7_1_0()");
#endif

            _interstitialOpenedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnInterstitialClosed_7_1_0(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialClosed_7_1_0()");
#endif

            _interstitialClosedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }
#endif

        #endregion

        #region ----- Rewarded Video Event -----

        private void _OnRewardedAdLoaded(ResponseInfo info)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdLoaded(): info=" + info);
#endif

            _rewardedLoadedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnRewardedAdFaiedToLoad(LoadAdError error)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdFaiedToLoad(): error=" + error);
#endif

            _rewardedFailedToLoadDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnRewardedAdOpened()
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdOpened()");
#endif

            _rewardedOpenedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnRewardedAdFaiedToOpen(AdError error)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdFaiedToOpen(): error=" + error);
#endif

            _rewardedFailedToOpenDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnRewardedAdUserEarnedReward()
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdUserEarnedReward()");
#endif

            _isHasReward = true;
        }

        private void _OnRewardedAdClosed()
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdClosed()");
#endif

            _rewardedClosedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

#if USE_MY_ADMOB_7_1_0
        private void _OnRewardedAdLoaded_7_1_0(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdLoaded_7_1_0()");
#endif

            _rewardedLoadedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnRewardedAdFaiedToLoad_7_1_0(object sender, AdFailedToLoadEventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdFaiedToLoad_7_1_0(): message=" + args.LoadAdError.GetMessage());
#endif

            _rewardedFailedToLoadDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnRewardedAdOpening_7_1_0(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdOpening_7_1_0()");
#endif

            _rewardedOpenedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnRewardedAdFaiedToShow_7_1_0(object sender, AdErrorEventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdFaiedToShow_7_1_0(): message=" + args.AdError.GetMessage());
#endif

            _rewardedFailedToOpenDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnRewardedAdUserEarnedReward_7_1_0(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdUserEarnedReward_7_1_0()");
#endif

            _isHasReward = true;
        }

        private void _OnRewardedAdClosed_7_1_0(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdClosed_7_1_0()");
#endif

            _rewardedClosedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }
#endif

        #endregion

        #region ----- App Open Event -----

        private void _OnAppOpenOpened(ResponseInfo info)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnAppOpenOpened(): info=" + info);
#endif

            _isShowingAppOpen = true;

            if (_onAppOpenAdOpenedCallback != null)
            {
                _onAppOpenAdOpenedCallback();
            }
        }

        private void _OnAppOpenAdFailedToOpen(AdError error)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnAppOpenAdFailedToOpen(): error=" + error);
#endif

            if (_onAppOpenAdFailedToOpenCallback != null)
            {
                _onAppOpenAdFailedToOpenCallback();
            }
        }

        private void _OnAppOpenAdImpressionRecorded()
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnAppOpenAdImpressionRecorded()");
#endif

            if (_onAppOpenAdImpressionRecordedCallback != null)
            {
                _onAppOpenAdImpressionRecordedCallback();
            }
        }

        private void _OnAppOpenAdClosed()
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnAppOpenAdClosed()");
#endif

            _appOpenAd = null;
            _isShowingAppOpen = false;
            _lastAppOpenHideTimestamp = MyLocalTime.CurrentUnixTime;

            if (_onAppOpenAdClosedCallback != null)
            {
                _onAppOpenAdClosedCallback();
            }
        }

#if USE_MY_ADMOB_7_1_0
        private void _OnAppOpenDidPresentFullScreenContent_7_1_0(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnAppOpenDidPresentFullScreenContent_7_1_0()");
#endif

            _isShowingAppOpen = true;

            if (_onAppOpenAdOpenedCallback != null)
            {
                _onAppOpenAdOpenedCallback();
            }
        }

        private void _OnAppOpenFailedToPresentFullScreenContent_7_1_0(object sender, AdErrorEventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnAppOpenFailedToPresentFullScreenContent_7_1_0(): message=" + args.ToString());
#endif

            if (_onAppOpenAdFailedToOpenCallback != null)
            {
                _onAppOpenAdFailedToOpenCallback();
            }
        }

        private void _OnAppOpenDidRecordImpression_7_1_0(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnAppOpenDidRecordImpression_7_1_0()");
#endif

            if (_onAppOpenAdImpressionRecordedCallback != null)
            {
                _onAppOpenAdImpressionRecordedCallback();
            }
        }

        private void _OnAppOpenDidDismissFullScreenContent_7_1_0(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnAppOpenDidDismissFullScreenContent_7_1_0()");
#endif

            _appOpenAd = null;
            _isShowingAppOpen = false;
            _lastAppOpenHideTimestamp = MyLocalTime.CurrentUnixTime;

            if (_onAppOpenAdClosedCallback != null)
            {
                _onAppOpenAdClosedCallback();
            }
        }
#endif

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyAdMobManager))]
    public class MyAdMobManagerEditor : Editor
    {
        private MyAdMobManager _script;
        private SerializedProperty _isEnableTest;
        private SerializedProperty _isUseGoogleTestAdsId;
        private SerializedProperty _testDeviceId;
        private SerializedProperty _androidDefaultBannerId;
        private SerializedProperty _iosDefaultBannerId;
        private SerializedProperty _lastBannerShowTimestamp;
        private SerializedProperty _androidDefaultInterstitialAdId;
        private SerializedProperty _iosDefaultInterstitialAdId;
        private SerializedProperty _lastInterstitialHideTimestamp;
        private SerializedProperty _androidDefaultRewardedAdId;
        private SerializedProperty _iosDefaultRewardedAdId;
        private SerializedProperty _lastRewardedHideTimestamp;
        private SerializedProperty _androidDefaultAppOpenAdId;
        private SerializedProperty mIosDefaultAppOpenAdId;
        private SerializedProperty _lastAppOpenHideTimestamp;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            _script = (MyAdMobManager)target;
            _isEnableTest = serializedObject.FindProperty("_isEnableTest");
            _isUseGoogleTestAdsId = serializedObject.FindProperty("_isUseGoogleTestAdsId");
            _testDeviceId = serializedObject.FindProperty("_testDeviceId");
            _androidDefaultBannerId = serializedObject.FindProperty("_androidDefaultBannerId");
            _iosDefaultBannerId = serializedObject.FindProperty("_iosDefaultBannerId");
            _lastBannerShowTimestamp = serializedObject.FindProperty("_lastBannerShowTimestamp");
            _androidDefaultInterstitialAdId = serializedObject.FindProperty("_androidDefaultInterstitialAdId");
            _iosDefaultInterstitialAdId = serializedObject.FindProperty("_iosDefaultInterstitialAdId");
            _lastInterstitialHideTimestamp = serializedObject.FindProperty("_lastInterstitialHideTimestamp");
            _androidDefaultRewardedAdId = serializedObject.FindProperty("_androidDefaultRewardedAdId");
            _iosDefaultRewardedAdId = serializedObject.FindProperty("_iosDefaultRewardedAdId");
            _lastRewardedHideTimestamp = serializedObject.FindProperty("_lastRewardedHideTimestamp");
            _androidDefaultAppOpenAdId = serializedObject.FindProperty("_androidDefaultAppOpenAdId");
            mIosDefaultAppOpenAdId = serializedObject.FindProperty("mIosDefaultAppOpenAdId");
            _lastAppOpenHideTimestamp = serializedObject.FindProperty("_lastAppOpenHideTimestamp");
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(_script), typeof(MyAdMobManager), false);

            serializedObject.Update();

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Test", EditorStyles.boldLabel);
            _isEnableTest.boolValue = EditorGUILayout.Toggle("   Enable", _isEnableTest.boolValue);
            _isUseGoogleTestAdsId.boolValue = EditorGUILayout.Toggle("   Use Google Ads Id", _isUseGoogleTestAdsId.boolValue);
            _testDeviceId.stringValue = EditorGUILayout.TextField("   Device Ids (separate by \";\")", _testDeviceId.stringValue);

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Banner", EditorStyles.boldLabel);
            _androidDefaultBannerId.stringValue = EditorGUILayout.TextField("   Android Default ID", _androidDefaultBannerId.stringValue);
            _iosDefaultBannerId.stringValue = EditorGUILayout.TextField("   IOS Default ID", _iosDefaultBannerId.stringValue);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("   Last Show Timestamp", GUILayout.Width(EditorGUIUtility.labelWidth));
                EditorGUILayout.LabelField(_lastBannerShowTimestamp.longValue.ToString());
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Interstitial Ad", EditorStyles.boldLabel);
            _androidDefaultInterstitialAdId.stringValue = EditorGUILayout.TextField("   Android Default ID", _androidDefaultInterstitialAdId.stringValue);
            _iosDefaultInterstitialAdId.stringValue = EditorGUILayout.TextField("   IOS Default ID", _iosDefaultInterstitialAdId.stringValue);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("   Last Hide Timestamp", GUILayout.Width(EditorGUIUtility.labelWidth));
                EditorGUILayout.LabelField(_lastInterstitialHideTimestamp.longValue.ToString());
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Rewarded Ad", EditorStyles.boldLabel);
            _androidDefaultRewardedAdId.stringValue = EditorGUILayout.TextField("   Android Default ID", _androidDefaultRewardedAdId.stringValue);
            _iosDefaultRewardedAdId.stringValue = EditorGUILayout.TextField("   IOS Default ID", _iosDefaultRewardedAdId.stringValue);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("   Last Hide Timestamp", GUILayout.Width(EditorGUIUtility.labelWidth));
                EditorGUILayout.LabelField(_lastRewardedHideTimestamp.longValue.ToString());
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("App Open Ad", EditorStyles.boldLabel);
            _androidDefaultAppOpenAdId.stringValue = EditorGUILayout.TextField("   Android Default ID", _androidDefaultAppOpenAdId.stringValue);
            mIosDefaultAppOpenAdId.stringValue = EditorGUILayout.TextField("   IOS Default ID", mIosDefaultAppOpenAdId.stringValue);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("   Last Hide Timestamp", GUILayout.Width(EditorGUIUtility.labelWidth));
                EditorGUILayout.LabelField(_lastAppOpenHideTimestamp.longValue.ToString());
            }
            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}

#endif