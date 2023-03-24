/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyAdMobManager (version 1.26)
 * Require:     GoogleMobileAds-v7.1.0
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
        private int _bannerOpeningDelayFrameCallback = 0;
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
        private int _interstitialOpeningDelayFrameCallback = 0;
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
        private int _rewardedOpeningDelayFrameCallback = 0;
        [SerializeField]
        private int _rewardedFailedToShowDelayFrameCallback = 0;
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
        private Action _onBannerOpeningCallback;
        private Action _onBannerClosedCallback;

        private InterstitialAd _interstitialAd;
        private Action _onInterstitialAdLoadedCallback;
        private Action _onInterstitialAdFailedToLoadCallback;
        private Action _onInterstitialAdOpeningCallback;
        private Action _onInterstitialAdClosedCallback;

        private RewardedAd _rewardedAd;
        private Action _onRewardedAdLoadedCallback;
        private Action _onRewardedAdFailedToLoadCallback;
        private Action _onRewardedAdOpeningCallback;
        private Action _onRewardedAdFailedToShowCallback;
        private Action _onRewardedAdSkippedCallback;
        private Action _onRewardedAdUserEarnedRewardCallback;
        private Action _onRewardedAdClosedCallback;

        private AppOpenAd _appOpenAd;
        private Action _onAppOpenAdShowCallback;
        private Action _onAppOpenAdFailedToShowCallback;
        private Action _onAppOpenAdRecordImpressionCallback;
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

        #region ----- Implement MonoBehaviour -----

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
            if (_bannerOpeningDelayFrameCallback > 0)
            {
                _bannerOpeningDelayFrameCallback -= 1;
                if (_bannerOpeningDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnBannerOpening(): callback");
#endif

                    if (_onBannerOpeningCallback != null)
                    {
                        _onBannerOpeningCallback();
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
            if (_interstitialOpeningDelayFrameCallback > 0)
            {
                _interstitialOpeningDelayFrameCallback -= 1;
                if (_interstitialOpeningDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialOpening(): callback");
#endif

                    if (_onInterstitialAdOpeningCallback != null)
                    {
                        _onInterstitialAdOpeningCallback();
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
            if (_rewardedOpeningDelayFrameCallback > 0)
            {
                _rewardedOpeningDelayFrameCallback -= 1;
                if (_rewardedOpeningDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdOpening(): callback");
#endif

                    if (_onRewardedAdOpeningCallback != null)
                    {
                        _onRewardedAdOpeningCallback();
                    }
                }
            }
            else if (_rewardedFailedToShowDelayFrameCallback > 0)
            {
                _rewardedFailedToShowDelayFrameCallback -= 1;
                if (_rewardedFailedToShowDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdFaiedToShow(): callback");
#endif

                    if (_onRewardedAdFailedToShowCallback != null)
                    {
                        _onRewardedAdFailedToShowCallback();
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
            UnityAds.SetConsentMetaData("gdpr.consent", true);
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
            _onBannerOpeningCallback = onOpeningCallback;
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
            _banner.OnAdLoaded += _OnBannerLoaded;
            _banner.OnAdFailedToLoad += _OnBannerFaiedToLoad;
            _banner.OnAdOpening += _OnBannerOpening;
            _banner.OnAdClosed += _OnBannerClosed;
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
            _onBannerOpeningCallback = null;
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
            return _interstitialAd != null && _interstitialAd.IsLoaded();
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

            _interstitialAd = new InterstitialAd(adUnitId);
            _interstitialAd.LoadAd(new AdRequest.Builder().Build());
            _interstitialAd.OnAdLoaded += _OnInterstitialLoaded;
            _interstitialAd.OnAdFailedToLoad += _OnInterstitialFaiedToLoad;
            _interstitialAd.OnAdOpening += _OnInterstitialOpening;
            _interstitialAd.OnAdClosed += _OnInterstitialClosed;

#if UNITY_EDITOR
            _OnInterstitialLoaded(null, null);
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
                _onInterstitialAdOpeningCallback = onOpeningCallback;
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
            return _rewardedAd != null && _rewardedAd.IsLoaded();
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

            _rewardedAd = new RewardedAd(adUnitId);
            _rewardedAd.LoadAd(new AdRequest.Builder().Build());
            _rewardedAd.OnAdLoaded += _OnRewardedAdLoaded;
            _rewardedAd.OnAdFailedToLoad += _OnRewardedAdFaiedToLoad;
            _rewardedAd.OnAdOpening += _OnRewardedAdOpening;
            _rewardedAd.OnAdFailedToShow += _OnRewardedAdFaiedToShow;
            _rewardedAd.OnUserEarnedReward += _OnRewardedAdUserEarnedReward;
            _rewardedAd.OnAdClosed += _OnRewardedAdClosed;

#if UNITY_EDITOR
            _OnRewardedAdLoaded(null, null);
#endif
        }

        /// <summary>
        /// Show a rewarded ad.
        /// </summary>
        public void ShowRewardedAd(Action onOpeningCallback = null, Action onUserEarnedRewardCallback = null, Action onFailedToShowCallback = null, Action onSkippedCallback = null, Action onClosedCallback = null)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] ShowRewardedAd(): isLoaded=" + IsRewardedAdLoaded());
#endif

            if (IsRewardedAdLoaded())
            {
                _onRewardedAdLoadedCallback = null;
                _onRewardedAdFailedToLoadCallback = null;
                _onRewardedAdOpeningCallback = onOpeningCallback;
                _onRewardedAdFailedToShowCallback = onFailedToShowCallback;
                _onRewardedAdSkippedCallback = onSkippedCallback;
                _onRewardedAdUserEarnedRewardCallback = onUserEarnedRewardCallback;
                _onRewardedAdClosedCallback = onClosedCallback;

                _isHasReward = false;

                _rewardedAd.Show();
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
            PlayerPrefs.Save();
#elif UNITY_IOS
            PlayerPrefs.SetString("MyAdMobManager_AppOpenAdId", string.IsNullOrEmpty(id) ? mIosDefaultAppOpenAdId : id);
            PlayerPrefs.Save();
#endif
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

            _onAppOpenAdShowCallback = null;
            _onAppOpenAdFailedToShowCallback = null;
            _onAppOpenAdRecordImpressionCallback = null;
            _onAppOpenAdClosedCallback = null;

            _isLoadingAppOpen = true;
            _isShowingAppOpen = false;
            
            AppOpenAd.LoadAd(adUnitId, screenOrientation, new AdRequest.Builder().Build(), ((appOpenAd, error) =>
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
                _onAppOpenAdShowCallback = onShowCallback;
                _onAppOpenAdFailedToShowCallback = onFailedToShowCallback;
                _onAppOpenAdRecordImpressionCallback = onRecordImpressionCallback;
                _onAppOpenAdClosedCallback = onClosedCallback;

                _appOpenAd.OnAdDidPresentFullScreenContent += _OnAppOpenDidPresentFullScreenContent;
                _appOpenAd.OnAdFailedToPresentFullScreenContent += _OnAppOpenFailedToPresentFullScreenContent;
                _appOpenAd.OnAdDidRecordImpression += _OnAppOpenDidRecordImpression;
                _appOpenAd.OnAdDidDismissFullScreenContent += _OnAppOpenDidDismissFullScreenContent;

                _appOpenAd.Show();
            }
        }

        #endregion

        #region ----- Banner Event -----

        private void _OnBannerLoaded(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnBannerLoaded()");
#endif

            _bannerLoadedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnBannerFaiedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnBannerFaiedToLoad(): message=" + args.ToString());
#endif

            _bannerFailedToLoadDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnBannerOpening(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnBannerOpening()");
#endif

            _bannerOpeningDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnBannerClosed(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnBannerClosed()");
#endif

            _bannerClosedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        #endregion

        #region ----- Interstitial Event -----

        private void _OnInterstitialLoaded(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialLoaded()");
#endif

            _interstitialLoadedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnInterstitialFaiedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialFaiedToLoad(): message=" + args.ToString());
#endif

            _interstitialFailedToLoadDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnInterstitialOpening(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialOpening()");
#endif

            _interstitialOpeningDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnInterstitialClosed(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialClosed()");
#endif

            _interstitialClosedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        #endregion

        #region ----- Rewarded Video Event -----

        private void _OnRewardedAdLoaded(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdLoaded()");
#endif

            _rewardedLoadedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnRewardedAdFaiedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdFaiedToLoad(): message=" + args.LoadAdError.GetMessage());
#endif

            _rewardedFailedToLoadDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnRewardedAdOpening(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdOpening()");
#endif

            _rewardedOpeningDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnRewardedAdFaiedToShow(object sender, AdErrorEventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdFaiedToShow(): message=" + args.AdError.GetMessage());
#endif

            _rewardedFailedToShowDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnRewardedAdUserEarnedReward(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdUserEarnedReward()");
#endif

            _isHasReward = true;
        }

        private void _OnRewardedAdClosed(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdClosed()");
#endif

            _rewardedClosedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        #endregion

        #region ----- App Open Event -----

        private void _OnAppOpenDidPresentFullScreenContent(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnAppOpenDidPresentFullScreenContent()");
#endif

            _isShowingAppOpen = true;

            if (_onAppOpenAdShowCallback != null)
            {
                _onAppOpenAdShowCallback();
            }
        }

        private void _OnAppOpenFailedToPresentFullScreenContent(object sender, AdErrorEventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnAppOpenFailedToPresentFullScreenContent(): message=" + args.ToString());
#endif

            if (_onAppOpenAdFailedToShowCallback != null)
            {
                _onAppOpenAdFailedToShowCallback();
            }
        }

        private void _OnAppOpenDidRecordImpression(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnAppOpenDidRecordImpression()");
#endif

            if (_onAppOpenAdRecordImpressionCallback != null)
            {
                _onAppOpenAdRecordImpressionCallback();
            }
        }

        private void _OnAppOpenDidDismissFullScreenContent(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnAppOpenDidDismissFullScreenContent()");
#endif

            _appOpenAd = null;
            _isShowingAppOpen = false;
            _lastAppOpenHideTimestamp = MyLocalTime.CurrentUnixTime;

            if (_onAppOpenAdClosedCallback != null)
            {
                _onAppOpenAdClosedCallback();
            }
        }

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