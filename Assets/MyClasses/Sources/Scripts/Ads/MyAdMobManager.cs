/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyAdMobManager (version 1.25)
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
using GoogleMobileAds.Api;
using System.Collections.Generic;

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
        #region ----- Variable -----

        private const int CALLBACK_DELAY_FRAME = 3;

        [SerializeField]
        private bool mIsInitialized = false;
        [SerializeField]
        private bool mTestEnable = true;
        [SerializeField]
        private bool mTestUseGoogleAdsId = true;
        [SerializeField]
        private string mTestDeviceId = "19CEAA9BB2BC2D8C16D6D202AE2A6C1E";

        [SerializeField]
        private string mAndroidDefaultBannerId = string.Empty;
        [SerializeField]
        private string mIosDefaultBannerId = string.Empty;
        [SerializeField]
        private int mCountBannerRequest = 0;
        [SerializeField]
        private long mLastBannerRequestTimestamp = 0;
        [SerializeField]
        private long mLastBannerShowTimestamp = 0;
        [SerializeField]
        private bool mIsLoadingBanner = false;
        [SerializeField]
        private bool mIsShowingBanner = false;
        [SerializeField]
        private int mBannerLoadedDelayFrameCallback = 0;
        [SerializeField]
        private int mBannerFailedToLoadDelayFrameCallback = 0;
        [SerializeField]
        private int mBannerOpeningDelayFrameCallback = 0;
        [SerializeField]
        private int mBannerClosedDelayFrameCallback = 0;

        [SerializeField]
        private string mAndroidDefaultInterstitialAdId = string.Empty;
        [SerializeField]
        private string mIosDefaultInterstitialAdId = string.Empty;
        [SerializeField]
        private long mLastInterstitialHideTimestamp = 0;
        [SerializeField]
        private bool mIsLoadingInterstitial = false;
        [SerializeField]
        private int mInterstitialLoadedDelayFrameCallback = 0;
        [SerializeField]
        private int mInterstitialFailedToLoadDelayFrameCallback = 0;
        [SerializeField]
        private int mInterstitialOpeningDelayFrameCallback = 0;
        [SerializeField]
        private int mInterstitialClosedDelayFrameCallback = 0;

        [SerializeField]
        private string mAndroidDefaultRewardedAdId = string.Empty;
        [SerializeField]
        private string mIosDefaultRewardedAdId = string.Empty;
        [SerializeField]
        private long mLastRewardedHideTimestamp = 0;
        [SerializeField]
        private bool mIsLoadingRewardred = false;
        [SerializeField]
        private bool mIsHasReward = false;
        [SerializeField]
        private int mRewardedLoadedDelayFrameCallback = 0;
        [SerializeField]
        private int mRewardedFailedToLoadDelayFrameCallback = 0;
        [SerializeField]
        private int mRewardedOpeningDelayFrameCallback = 0;
        [SerializeField]
        private int mRewardedFailedToShowDelayFrameCallback = 0;
        [SerializeField]
        private int mRewardedClosedDelayFrameCallback = 0;

        [SerializeField]
        private string mAndroidDefaultAppOpenAdId = string.Empty;
        [SerializeField]
        private string mIosDefaultAppOpenAdId = string.Empty;
        [SerializeField]
        private bool mIsLoadingAppOpen = false;
        [SerializeField]
        private bool mIsShowingAppOpen = false;
        [SerializeField]
        private long mLastAppOpenRequestTimestamp = 0;
        [SerializeField]
        private long mLastAppOpenHideTimestamp = 0;

        private BannerView mBanner;
        private AdRequest mBannerRequest;
        private Action mOnBannerLoadedCallback;
        private Action mOnBannerFailedToLoadCallback;
        private Action mOnBannerOpeningCallback;
        private Action mOnBannerClosedCallback;

        private InterstitialAd mInterstitialAd;
        private Action mOnInterstitialAdLoadedCallback;
        private Action mOnInterstitialAdFailedToLoadCallback;
        private Action mOnInterstitialAdOpeningCallback;
        private Action mOnInterstitialAdClosedCallback;

        private RewardedAd mRewardedAd;
        private Action mOnRewardedAdLoadedCallback;
        private Action mOnRewardedAdFailedToLoadCallback;
        private Action mOnRewardedAdOpeningCallback;
        private Action mOnRewardedAdFailedToShowCallback;
        private Action mOnRewardedAdSkippedCallback;
        private Action mOnRewardedAdUserEarnedRewardCallback;
        private Action mOnRewardedAdClosedCallback;

        private AppOpenAd mAppOpenAd;
        private Action mOnAppOpenAdShowCallback;
        private Action mOnAppOpenAdFailedToShowCallback;
        private Action mOnAppOpenAdRecordImpressionCallback;
        private Action mOnAppOpenAdClosedCallback;

        public bool IsInitialized
        {
            get { return mIsInitialized; }
        }

        public long CountBannerRequest
        {
            get { return mCountBannerRequest; }
        }

        public long LastBannerRequestTimestamp
        {
            get { return mLastBannerRequestTimestamp; }
        }

        public long LastBannerShowTimestamp
        {
            get { return mLastBannerShowTimestamp; }
            set { mLastBannerShowTimestamp = value; }
        }

        public long LastInterstitialHideTimestamp
        {
            get { return mLastInterstitialHideTimestamp; }
            set { mLastInterstitialHideTimestamp = value; }
        }

        public long LastRewardedHideTimestamp
        {
            get { return mLastRewardedHideTimestamp; }
            set { mLastRewardedHideTimestamp = value; }
        }

        public long LastAppOpenRequestTimestamp
        {
            get { return mLastAppOpenRequestTimestamp; }
        }

        public long LastAppOpenHideTimestamp
        {
            get { return mLastAppOpenHideTimestamp; }
            set { mLastAppOpenHideTimestamp = value; }
        }

        #endregion

        #region ----- Singleton -----

        private static object mSingletonLock = new object();
        private static MyAdMobManager mInstance;

        public static MyAdMobManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (mSingletonLock)
                    {
                        mInstance = (MyAdMobManager)FindObjectOfType(typeof(MyAdMobManager));
                        if (mInstance == null)
                        {
                            GameObject obj = new GameObject(typeof(MyAdMobManager).Name);
                            mInstance = obj.AddComponent<MyAdMobManager>();
                        }
                        DontDestroyOnLoad(mInstance);
                    }
                }
                return mInstance;
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
            if (mBannerLoadedDelayFrameCallback > 0)
            {
                mBannerLoadedDelayFrameCallback -= 1;
                if (mBannerLoadedDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnBannerLoaded(): callback");
#endif

                    mIsLoadingBanner = false;
                    mIsShowingBanner = true;
                    mLastBannerShowTimestamp = MyLocalTime.CurrentUnixTime;

                    if (mOnBannerLoadedCallback != null)
                    {
                        mOnBannerLoadedCallback();
                    }
                }
            }
            else if (mBannerFailedToLoadDelayFrameCallback > 0)
            {
                mBannerFailedToLoadDelayFrameCallback -= 1;
                if (mBannerFailedToLoadDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnBannerFaiedToLoad(): callback");
#endif

                    mIsLoadingBanner = false;

                    if (mOnBannerFailedToLoadCallback != null)
                    {
                        mOnBannerFailedToLoadCallback();
                    }
                }
            }
            if (mBannerOpeningDelayFrameCallback > 0)
            {
                mBannerOpeningDelayFrameCallback -= 1;
                if (mBannerOpeningDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnBannerOpening(): callback");
#endif

                    if (mOnBannerOpeningCallback != null)
                    {
                        mOnBannerOpeningCallback();
                    }
                }
            }
            if (mBannerClosedDelayFrameCallback > 0)
            {
                mBannerClosedDelayFrameCallback -= 1;
                if (mBannerClosedDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnBannerClosed(): callback");
#endif

                    if (mOnBannerClosedCallback != null)
                    {
                        mOnBannerClosedCallback();
                    }
                }
            }

            // interstitial
            if (mInterstitialLoadedDelayFrameCallback > 0)
            {
                mInterstitialLoadedDelayFrameCallback -= 1;
                if (mInterstitialLoadedDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialLoaded(): callback");
#endif

                    mIsLoadingInterstitial = false;

                    if (mOnInterstitialAdLoadedCallback != null)
                    {
                        mOnInterstitialAdLoadedCallback();
                    }
                }
            }
            else if (mInterstitialFailedToLoadDelayFrameCallback > 0)
            {
                mInterstitialFailedToLoadDelayFrameCallback -= 1;
                if (mInterstitialFailedToLoadDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialFaiedToLoad(): callback");
#endif

                    mIsLoadingInterstitial = false;

                    if (mOnInterstitialAdFailedToLoadCallback != null)
                    {
                        mOnInterstitialAdFailedToLoadCallback();
                    }
                }
            }
            if (mInterstitialOpeningDelayFrameCallback > 0)
            {
                mInterstitialOpeningDelayFrameCallback -= 1;
                if (mInterstitialOpeningDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialOpening(): callback");
#endif

                    if (mOnInterstitialAdOpeningCallback != null)
                    {
                        mOnInterstitialAdOpeningCallback();
                    }
                }
            }
            if (mInterstitialClosedDelayFrameCallback > 0)
            {
                mInterstitialClosedDelayFrameCallback -= 1;
                if (mInterstitialClosedDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialClosed(): callback");
#endif

                    mIsLoadingInterstitial = false;
                    mLastInterstitialHideTimestamp = MyLocalTime.CurrentUnixTime;

                    if (mOnInterstitialAdClosedCallback != null)
                    {
                        mOnInterstitialAdClosedCallback();
                    }
                }
            }

            // rewarded
            if (mRewardedLoadedDelayFrameCallback > 0)
            {
                mRewardedLoadedDelayFrameCallback -= 1;
                if (mRewardedLoadedDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdLoaded(): callback");
#endif

                    mIsLoadingRewardred = false;

                    if (mOnRewardedAdLoadedCallback != null)
                    {
                        mOnRewardedAdLoadedCallback();
                    }
                }
            }
            else if (mRewardedFailedToLoadDelayFrameCallback > 0)
            {
                mRewardedFailedToLoadDelayFrameCallback -= 1;
                if (mRewardedFailedToLoadDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdFaiedToLoad(): callback");
#endif

                    mIsLoadingRewardred = false;

                    if (mOnRewardedAdFailedToLoadCallback != null)
                    {
                        mOnRewardedAdFailedToLoadCallback();
                    }
                }
            }
            if (mRewardedOpeningDelayFrameCallback > 0)
            {
                mRewardedOpeningDelayFrameCallback -= 1;
                if (mRewardedOpeningDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdOpening(): callback");
#endif

                    if (mOnRewardedAdOpeningCallback != null)
                    {
                        mOnRewardedAdOpeningCallback();
                    }
                }
            }
            else if (mRewardedFailedToShowDelayFrameCallback > 0)
            {
                mRewardedFailedToShowDelayFrameCallback -= 1;
                if (mRewardedFailedToShowDelayFrameCallback == 0)
                {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdFaiedToShow(): callback");
#endif

                    if (mOnRewardedAdFailedToShowCallback != null)
                    {
                        mOnRewardedAdFailedToShowCallback();
                    }
                }
            }
            if (mRewardedClosedDelayFrameCallback > 0)
            {
                mRewardedClosedDelayFrameCallback -= 1;
                if (mRewardedClosedDelayFrameCallback == 0)
                {
                    mIsLoadingRewardred = false;
                    mLastRewardedHideTimestamp = MyLocalTime.CurrentUnixTime;

                    if (mIsHasReward)
                    {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdUserEarnedReward(): callback");
#endif

                        if (mOnRewardedAdUserEarnedRewardCallback != null)
                        {
                            mOnRewardedAdUserEarnedRewardCallback();
                        }
                    }
                    else
                    {
                        if (mOnRewardedAdSkippedCallback != null)
                        {
                            mOnRewardedAdSkippedCallback();
                        }
                    }

#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdClosed(): callback");
#endif

                    if (mOnRewardedAdClosedCallback != null)
                    {
                        mOnRewardedAdClosedCallback();
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

            if (mTestEnable && mTestDeviceId.Length > 0)
            {
                if (mTestUseGoogleAdsId)
                {
                    mAndroidDefaultBannerId = "ca-app-pub-3940256099942544/6300978111";
                    mAndroidDefaultInterstitialAdId = "ca-app-pub-3940256099942544/1033173712";
                    mAndroidDefaultRewardedAdId = "ca-app-pub-3940256099942544/5224354917";
                    mAndroidDefaultAppOpenAdId = "ca-app-pub-3940256099942544/3419835294";

                    mIosDefaultBannerId = "ca-app-pub-3940256099942544/2934735716";
                    mIosDefaultInterstitialAdId = "ca-app-pub-3940256099942544/4411468910";
                    mIosDefaultRewardedAdId = "ca-app-pub-3940256099942544/1712485313";
                    mIosDefaultAppOpenAdId = "ca-app-pub-3940256099942544/5662855259";

#if UNITY_ANDROID
                    PlayerPrefs.SetString("MyAdMobManager_BannerId", mAndroidDefaultBannerId);
                    PlayerPrefs.SetString("MyAdMobManager_InterstitialAdId", mAndroidDefaultInterstitialAdId);
                    PlayerPrefs.SetString("MyAdMobManager_RewardedAdId", mAndroidDefaultRewardedAdId);
#elif UNITY_IOS
                    PlayerPrefs.SetString("MyAdMobManager_BannerId", mIosDefaultBannerId);
                    PlayerPrefs.SetString("MyAdMobManager_InterstitialAdId", mIosDefaultInterstitialAdId);
                    PlayerPrefs.SetString("MyAdMobManager_RewardedAdId", mIosDefaultRewardedAdId);
#endif
                }

                List<string> deviceIds = new List<string>();
                deviceIds.AddRange(mTestDeviceId.Split(';'));
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

                mIsInitialized = true;

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

            if (mTestEnable && mTestUseGoogleAdsId)
            {
                return;
            }

#if UNITY_ANDROID
            PlayerPrefs.SetString("MyAdMobManager_BannerId", string.IsNullOrEmpty(id) ? mAndroidDefaultBannerId : id);
            PlayerPrefs.Save();
#elif UNITY_IOS
            PlayerPrefs.SetString("MyAdMobManager_BannerId", string.IsNullOrEmpty(id) ? mIosDefaultBannerId : id);
            PlayerPrefs.Save();
#endif
        }

        /// <summary>
        /// Check if a interstitial ad is loading.
        /// </summary>
        public bool IsBannerLoading()
        {
            return mIsLoadingBanner;
        }

        /// <summary>
        /// Check if a interstitial ad is showing.
        /// </summary>
        public bool IsBannerShowing()
        {
            return mIsShowingBanner;
        }

        /// <summary>
        /// Show a banner.
        /// </summary>
        public void ShowBanner(string adUnitId = null, AdSize size = null, AdPosition position = AdPosition.Bottom, Action onLoadedCallback = null, Action onFailedToLoadCallback = null, Action onOpeningCallback = null, Action onClosedCallback = null)
        {
            if (mTestEnable && mTestUseGoogleAdsId)
            {
#if UNITY_ANDROID
                adUnitId = mAndroidDefaultBannerId;
#elif UNITY_IOS
                adUnitId = mIosDefaultBannerId;
#endif
            }

            if (adUnitId == null)
            {
#if UNITY_ANDROID
                adUnitId = PlayerPrefs.GetString("MyAdMobManager_BannerId", mAndroidDefaultBannerId);
#elif UNITY_IOS
                adUnitId = PlayerPrefs.GetString("MyAdMobManager_BannerId", mIosDefaultBannerId);
#endif
            }

            if (size == null)
            {
                size = AdSize.Banner;
            }

#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] ShowBanner(): adUnitId=" + adUnitId + " | size=" + size.AdType + " | position=" + position);
#endif

            mOnBannerLoadedCallback = onLoadedCallback;
            mOnBannerFailedToLoadCallback = onFailedToLoadCallback;
            mOnBannerOpeningCallback = onOpeningCallback;
            mOnBannerClosedCallback = onClosedCallback;

            mCountBannerRequest += 1;

            mLastBannerRequestTimestamp = MyLocalTime.CurrentUnixTime;
#if UNITY_EDITOR
            mLastBannerShowTimestamp = MyLocalTime.CurrentUnixTime;
#else
            mIsLoadingBanner = true;
#endif

            if (mBannerRequest == null)
            {
                mBannerRequest = new AdRequest.Builder().Build();
            }
            if (mBanner != null)
            {
                mBanner.Destroy();
            }
            mBanner = new BannerView(adUnitId, size, position);
            mBanner.LoadAd(mBannerRequest);
            mBanner.OnAdLoaded += _OnBannerLoaded;
            mBanner.OnAdFailedToLoad += _OnBannerFaiedToLoad;
            mBanner.OnAdOpening += _OnBannerOpening;
            mBanner.OnAdClosed += _OnBannerClosed;
        }

        /// <summary>
        /// Hide the banner.
        /// </summary>
        public void HideBanner()
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] HideBanner()");
#endif

            mOnBannerLoadedCallback = null;
            mOnBannerFailedToLoadCallback = null;
            mOnBannerOpeningCallback = null;
            mOnBannerClosedCallback = null;

            if (mBanner != null)
            {
                mBanner.Hide();
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

            if (mTestEnable && mTestUseGoogleAdsId)
            {
                return;
            }

#if UNITY_ANDROID
            PlayerPrefs.SetString("MyAdMobManager_InterstitialAdId", string.IsNullOrEmpty(id) ? mAndroidDefaultInterstitialAdId : id);
            PlayerPrefs.Save();
#elif UNITY_IOS
            PlayerPrefs.SetString("MyAdMobManager_InterstitialAdId", string.IsNullOrEmpty(id) ? mIosDefaultInterstitialAdId : id);
            PlayerPrefs.Save();
#endif
        }

        /// <summary>
        /// Check if a interstitial ad is loading.
        /// </summary>
        public bool IsInterstitialAdLoading()
        {
            return mIsLoadingInterstitial;
        }

        /// <summary>
        /// Check if an interstitial ad is ready.
        /// </summary>
        public bool IsInterstitialAdLoaded()
        {
            return mInterstitialAd != null && mInterstitialAd.IsLoaded();
        }

        /// <summary>
        /// Load an interstitial ad.
        /// </summary>
        public void LoadInterstitialAd(string adUnitId = null, Action onLoadedCallback = null, Action onFailedToLoadCallback = null)
        {
            if (mTestEnable && mTestUseGoogleAdsId)
            {
#if UNITY_ANDROID
                adUnitId = mAndroidDefaultInterstitialAdId;
#elif UNITY_IOS
                adUnitId = mIosDefaultInterstitialAdId;
#endif
            }

            if (adUnitId == null)
            {
#if UNITY_ANDROID
                adUnitId = PlayerPrefs.GetString("MyAdMobManager_InterstitialAdId", mAndroidDefaultInterstitialAdId);
#elif UNITY_IOS
                adUnitId = PlayerPrefs.GetString("MyAdMobManager_InterstitialAdId", mIosDefaultInterstitialAdId);
#endif
            }

#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] LoadInterstitialAd(): adUnitId=" + adUnitId);
#endif

            mOnInterstitialAdLoadedCallback = onLoadedCallback;
            mOnInterstitialAdFailedToLoadCallback = onFailedToLoadCallback;

            mIsLoadingInterstitial = true;

            mInterstitialAd = new InterstitialAd(adUnitId);
            mInterstitialAd.LoadAd(new AdRequest.Builder().Build());
            mInterstitialAd.OnAdLoaded += _OnInterstitialLoaded;
            mInterstitialAd.OnAdFailedToLoad += _OnInterstitialFaiedToLoad;
            mInterstitialAd.OnAdOpening += _OnInterstitialOpening;
            mInterstitialAd.OnAdClosed += _OnInterstitialClosed;

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
                mOnInterstitialAdLoadedCallback = null;
                mOnInterstitialAdFailedToLoadCallback = null;
                mOnInterstitialAdOpeningCallback = onOpeningCallback;
                mOnInterstitialAdClosedCallback = onClosedCallback;

                mLastInterstitialHideTimestamp = MyLocalTime.CurrentUnixTime;

                mInterstitialAd.Show();
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

            if (mTestEnable && mTestUseGoogleAdsId)
            {
                return;
            }

#if UNITY_ANDROID
            PlayerPrefs.SetString("MyAdMobManager_RewardedAdId", string.IsNullOrEmpty(id) ? mAndroidDefaultRewardedAdId : id);
            PlayerPrefs.Save();
#elif UNITY_IOS
            PlayerPrefs.SetString("MyAdMobManager_RewardedAdId", string.IsNullOrEmpty(id) ? mIosDefaultRewardedAdId : id);
            PlayerPrefs.Save();
#endif
        }

        /// <summary>
        /// Check if a rewarded ad is loading.
        /// </summary>
        public bool IsRewardedAdLoading()
        {
            return mIsLoadingRewardred;
        }

        /// <summary>
        /// Check if a rewarded ad is ready.
        /// </summary>
        public bool IsRewardedAdLoaded()
        {
            return mRewardedAd != null && mRewardedAd.IsLoaded();
        }

        /// <summary>
        /// Load a rewarded ad.
        /// </summary>
        public void LoadRewardedAd(string adUnitId = null, Action onLoadedCallback = null, Action onFailedToLoadCallback = null)
        {
            if (mTestEnable && mTestUseGoogleAdsId)
            {
#if UNITY_ANDROID
                adUnitId = mAndroidDefaultRewardedAdId;
#elif UNITY_IOS
                adUnitId = mIosDefaultRewardedAdId;
#endif
            }

            if (adUnitId == null)
            {
#if UNITY_ANDROID
                adUnitId = PlayerPrefs.GetString("MyAdMobManager_RewardedAdId", mAndroidDefaultRewardedAdId);
#elif UNITY_IOS
                adUnitId = PlayerPrefs.GetString("MyAdMobManager_RewardedAdId", mIosDefaultRewardedAdId);
#endif
            }

#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] LoadRewardedAd(): adUnitId=" + adUnitId);
#endif
            mOnRewardedAdLoadedCallback = onLoadedCallback;
            mOnRewardedAdFailedToLoadCallback = onFailedToLoadCallback;

            mIsLoadingRewardred = true;

            mRewardedAd = new RewardedAd(adUnitId);
            mRewardedAd.LoadAd(new AdRequest.Builder().Build());
            mRewardedAd.OnAdLoaded += _OnRewardedAdLoaded;
            mRewardedAd.OnAdFailedToLoad += _OnRewardedAdFaiedToLoad;
            mRewardedAd.OnAdOpening += _OnRewardedAdOpening;
            mRewardedAd.OnAdFailedToShow += _OnRewardedAdFaiedToShow;
            mRewardedAd.OnUserEarnedReward += _OnRewardedAdUserEarnedReward;
            mRewardedAd.OnAdClosed += _OnRewardedAdClosed;

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
                mOnRewardedAdLoadedCallback = null;
                mOnRewardedAdFailedToLoadCallback = null;
                mOnRewardedAdOpeningCallback = onOpeningCallback;
                mOnRewardedAdFailedToShowCallback = onFailedToShowCallback;
                mOnRewardedAdSkippedCallback = onSkippedCallback;
                mOnRewardedAdUserEarnedRewardCallback = onUserEarnedRewardCallback;
                mOnRewardedAdClosedCallback = onClosedCallback;

                mIsHasReward = false;

                mRewardedAd.Show();
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

            if (mTestEnable && mTestUseGoogleAdsId)
            {
                return;
            }
            
#if UNITY_ANDROID
            PlayerPrefs.SetString("MyAdMobManager_AppOpenAdId", string.IsNullOrEmpty(id) ? mAndroidDefaultAppOpenAdId : id);
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
            return mIsLoadingAppOpen;
        }

        /// <summary>
        /// Check if an app open ad is ready.
        /// </summary>
        public bool IsAppOpenAdLoaded()
        {
            return mAppOpenAd != null && MyLocalTime.CurrentUnixTime - mLastAppOpenRequestTimestamp < 14400000;
        }

        /// <summary>
        /// Check if an app open ad is showing.
        /// </summary>
        public bool IsAppOpenAdShowing()
        {
            return mAppOpenAd != null && mIsShowingAppOpen;
        }

        /// <summary>
        /// Load an app open  ad.
        /// </summary>
        public void LoadAppOpenAd(ScreenOrientation screenOrientation = ScreenOrientation.Portrait, string adUnitId = null, Action onLoadedCallback = null, Action onFailedToLoadCallback = null)
        {
            if (mTestEnable && mTestUseGoogleAdsId)
            {
#if UNITY_ANDROID
                adUnitId = mAndroidDefaultAppOpenAdId;
#elif UNITY_IOS
                adUnitId = mIosDefaultAppOpenAdId;
#endif
            }

            if (adUnitId == null)
            {
#if UNITY_ANDROID
                adUnitId = PlayerPrefs.GetString("MyAdMobManager_AppOpenAdId", mAndroidDefaultAppOpenAdId);
#elif UNITY_IOS
                adUnitId = PlayerPrefs.GetString("MyAdMobManager_AppOpenAdId", mIosDefaultAppOpenAdId);
#endif
            }

#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] LoadAppOpenAd(): adUnitId=" + adUnitId + " | screenOrientation=" + screenOrientation);
#endif

            mAppOpenAd = null;

            mOnAppOpenAdShowCallback = null;
            mOnAppOpenAdFailedToShowCallback = null;
            mOnAppOpenAdRecordImpressionCallback = null;
            mOnAppOpenAdClosedCallback = null;

            mIsLoadingAppOpen = true;
            mIsShowingAppOpen = false;
            
            AppOpenAd.LoadAd(adUnitId, screenOrientation, new AdRequest.Builder().Build(), ((appOpenAd, error) =>
            {
                mIsLoadingAppOpen = false;

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

                    mAppOpenAd = appOpenAd;
                    mLastAppOpenRequestTimestamp = MyLocalTime.CurrentUnixTime;

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
                mOnAppOpenAdShowCallback = onShowCallback;
                mOnAppOpenAdFailedToShowCallback = onFailedToShowCallback;
                mOnAppOpenAdRecordImpressionCallback = onRecordImpressionCallback;
                mOnAppOpenAdClosedCallback = onClosedCallback;

                mAppOpenAd.OnAdDidPresentFullScreenContent += _OnAppOpenDidPresentFullScreenContent;
                mAppOpenAd.OnAdFailedToPresentFullScreenContent += _OnAppOpenFailedToPresentFullScreenContent;
                mAppOpenAd.OnAdDidRecordImpression += _OnAppOpenDidRecordImpression;
                mAppOpenAd.OnAdDidDismissFullScreenContent += _OnAppOpenDidDismissFullScreenContent;

                mAppOpenAd.Show();
            }
        }

        #endregion

        #region ----- Banner Event -----

        private void _OnBannerLoaded(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnBannerLoaded()");
#endif

            mBannerLoadedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnBannerFaiedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnBannerFaiedToLoad(): message=" + args.ToString());
#endif

            mBannerFailedToLoadDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnBannerOpening(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnBannerOpening()");
#endif

            mBannerOpeningDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnBannerClosed(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnBannerClosed()");
#endif

            mBannerClosedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        #endregion

        #region ----- Interstitial Event -----

        private void _OnInterstitialLoaded(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialLoaded()");
#endif

            mInterstitialLoadedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnInterstitialFaiedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialFaiedToLoad(): message=" + args.ToString());
#endif

            mInterstitialFailedToLoadDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnInterstitialOpening(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialOpening()");
#endif

            mInterstitialOpeningDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnInterstitialClosed(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnInterstitialClosed()");
#endif

            mInterstitialClosedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        #endregion

        #region ----- Rewarded Video Event -----

        private void _OnRewardedAdLoaded(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdLoaded()");
#endif

            mRewardedLoadedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnRewardedAdFaiedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdFaiedToLoad(): message=" + args.LoadAdError.GetMessage());
#endif

            mRewardedFailedToLoadDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnRewardedAdOpening(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdOpening()");
#endif

            mRewardedOpeningDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnRewardedAdFaiedToShow(object sender, AdErrorEventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdFaiedToShow(): message=" + args.AdError.GetMessage());
#endif

            mRewardedFailedToShowDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        private void _OnRewardedAdUserEarnedReward(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdUserEarnedReward()");
#endif

            mIsHasReward = true;
        }

        private void _OnRewardedAdClosed(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnRewardedAdClosed()");
#endif

            mRewardedClosedDelayFrameCallback = CALLBACK_DELAY_FRAME;
        }

        #endregion

        #region ----- App Open Event -----

        private void _OnAppOpenDidPresentFullScreenContent(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnAppOpenDidPresentFullScreenContent()");
#endif

            mIsShowingAppOpen = true;

            if (mOnAppOpenAdShowCallback != null)
            {
                mOnAppOpenAdShowCallback();
            }
        }

        private void _OnAppOpenFailedToPresentFullScreenContent(object sender, AdErrorEventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.LogError("[" + typeof(MyAdMobManager).Name + "] _OnAppOpenFailedToPresentFullScreenContent(): message=" + args.ToString());
#endif

            if (mOnAppOpenAdFailedToShowCallback != null)
            {
                mOnAppOpenAdFailedToShowCallback();
            }
        }

        private void _OnAppOpenDidRecordImpression(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnAppOpenDidRecordImpression()");
#endif

            if (mOnAppOpenAdRecordImpressionCallback != null)
            {
                mOnAppOpenAdRecordImpressionCallback();
            }
        }

        private void _OnAppOpenDidDismissFullScreenContent(object sender, EventArgs args)
        {
#if DEBUG_MY_ADMOB
            Debug.Log("[" + typeof(MyAdMobManager).Name + "] _OnAppOpenDidDismissFullScreenContent()");
#endif

            mAppOpenAd = null;
            mIsShowingAppOpen = false;
            mLastAppOpenHideTimestamp = MyLocalTime.CurrentUnixTime;

            if (mOnAppOpenAdClosedCallback != null)
            {
                mOnAppOpenAdClosedCallback();
            }
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyAdMobManager))]
    public class MyAdMobManagerEditor : Editor
    {
        private MyAdMobManager mScript;
        private SerializedProperty mTestEnable;
        private SerializedProperty mTestUseGoogleAdsId;
        private SerializedProperty mTestDeviceId;
        private SerializedProperty mAndroidDefaultBannerId;
        private SerializedProperty mIosDefaultBannerId;
        private SerializedProperty mLastBannerShowTimestamp;
        private SerializedProperty mAndroidDefaultInterstitialAdId;
        private SerializedProperty mIosDefaultInterstitialAdId;
        private SerializedProperty mLastInterstitialHideTimestamp;
        private SerializedProperty mAndroidDefaultRewardedAdId;
        private SerializedProperty mIosDefaultRewardedAdId;
        private SerializedProperty mLastRewardedHideTimestamp;
        private SerializedProperty mAndroidDefaultAppOpenAdId;
        private SerializedProperty mIosDefaultAppOpenAdId;
        private SerializedProperty mLastAppOpenHideTimestamp;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyAdMobManager)target;
            mTestEnable = serializedObject.FindProperty("mTestEnable");
            mTestUseGoogleAdsId = serializedObject.FindProperty("mTestUseGoogleAdsId");
            mTestDeviceId = serializedObject.FindProperty("mTestDeviceId");
            mAndroidDefaultBannerId = serializedObject.FindProperty("mAndroidDefaultBannerId");
            mIosDefaultBannerId = serializedObject.FindProperty("mIosDefaultBannerId");
            mLastBannerShowTimestamp = serializedObject.FindProperty("mLastBannerShowTimestamp");
            mAndroidDefaultInterstitialAdId = serializedObject.FindProperty("mAndroidDefaultInterstitialAdId");
            mIosDefaultInterstitialAdId = serializedObject.FindProperty("mIosDefaultInterstitialAdId");
            mLastInterstitialHideTimestamp = serializedObject.FindProperty("mLastInterstitialHideTimestamp");
            mAndroidDefaultRewardedAdId = serializedObject.FindProperty("mAndroidDefaultRewardedAdId");
            mIosDefaultRewardedAdId = serializedObject.FindProperty("mIosDefaultRewardedAdId");
            mLastRewardedHideTimestamp = serializedObject.FindProperty("mLastRewardedHideTimestamp");
            mAndroidDefaultAppOpenAdId = serializedObject.FindProperty("mAndroidDefaultAppOpenAdId");
            mIosDefaultAppOpenAdId = serializedObject.FindProperty("mIosDefaultAppOpenAdId");
            mLastAppOpenHideTimestamp = serializedObject.FindProperty("mLastAppOpenHideTimestamp");
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyAdMobManager), false);

            serializedObject.Update();

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Test", EditorStyles.boldLabel);
            mTestEnable.boolValue = EditorGUILayout.Toggle("   Enable", mTestEnable.boolValue);
            mTestUseGoogleAdsId.boolValue = EditorGUILayout.Toggle("   Use Google Ads Id", mTestUseGoogleAdsId.boolValue);
            mTestDeviceId.stringValue = EditorGUILayout.TextField("   Device Ids (separate by \";\")", mTestDeviceId.stringValue);

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Banner", EditorStyles.boldLabel);
            mAndroidDefaultBannerId.stringValue = EditorGUILayout.TextField("   Android Default ID", mAndroidDefaultBannerId.stringValue);
            mIosDefaultBannerId.stringValue = EditorGUILayout.TextField("   IOS Default ID", mIosDefaultBannerId.stringValue);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("   Last Show Timestamp", GUILayout.Width(EditorGUIUtility.labelWidth));
                EditorGUILayout.LabelField(mLastBannerShowTimestamp.longValue.ToString());
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Interstitial Ad", EditorStyles.boldLabel);
            mAndroidDefaultInterstitialAdId.stringValue = EditorGUILayout.TextField("   Android Default ID", mAndroidDefaultInterstitialAdId.stringValue);
            mIosDefaultInterstitialAdId.stringValue = EditorGUILayout.TextField("   IOS Default ID", mIosDefaultInterstitialAdId.stringValue);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("   Last Hide Timestamp", GUILayout.Width(EditorGUIUtility.labelWidth));
                EditorGUILayout.LabelField(mLastInterstitialHideTimestamp.longValue.ToString());
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Rewarded Ad", EditorStyles.boldLabel);
            mAndroidDefaultRewardedAdId.stringValue = EditorGUILayout.TextField("   Android Default ID", mAndroidDefaultRewardedAdId.stringValue);
            mIosDefaultRewardedAdId.stringValue = EditorGUILayout.TextField("   IOS Default ID", mIosDefaultRewardedAdId.stringValue);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("   Last Hide Timestamp", GUILayout.Width(EditorGUIUtility.labelWidth));
                EditorGUILayout.LabelField(mLastRewardedHideTimestamp.longValue.ToString());
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("App Open Ad", EditorStyles.boldLabel);
            mAndroidDefaultAppOpenAdId.stringValue = EditorGUILayout.TextField("   Android Default ID", mAndroidDefaultAppOpenAdId.stringValue);
            mIosDefaultAppOpenAdId.stringValue = EditorGUILayout.TextField("   IOS Default ID", mIosDefaultAppOpenAdId.stringValue);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("   Last Hide Timestamp", GUILayout.Width(EditorGUIUtility.labelWidth));
                EditorGUILayout.LabelField(mLastAppOpenHideTimestamp.longValue.ToString());
            }
            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}

#endif