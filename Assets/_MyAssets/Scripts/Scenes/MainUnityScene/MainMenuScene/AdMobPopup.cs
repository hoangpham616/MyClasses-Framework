using UnityEngine;
using UnityEngine.EventSystems;
using MyClasses;
using MyClasses.UI;

namespace MyApp
{
    public class AdMobPopup : MyUGUIPopup
    {
        #region ----- Variable -----

        private MyUGUIButton _btnClose;
        private MyUGUIButton _btnShowBanner;
        private MyUGUIButton _btnLoadInterstitial;
        private MyUGUIButton _btnShowInterstitial;
        private MyUGUIButton _btnLoadRewardedVideo;
        private MyUGUIButton _btnShowRewardedVideo;

        #endregion

        #region ----- Constructor -----

        public AdMobPopup(EPopupID id, string prefabName, bool isFloat = false, bool isRepeatable = false)
            : base(id, prefabName, isFloat, isRepeatable)
        {
        }

        #endregion

        #region ----- MyUGUIPopup Implementation -----

        public override void OnUGUIInit()
        {
            this.LogInfo("OnUGUIInit", null, ELogColor.DARK_UI);

            base.OnUGUIInit();

            _btnClose = MyUtilities.FindObject(GameObject, "Container/ButtonClose").GetComponent<MyUGUIButton>();
            _btnShowBanner = MyUtilities.FindObject(GameObject, "Container/Buttons/ButtonShowBanner").GetComponent<MyUGUIButton>();
            _btnLoadInterstitial = MyUtilities.FindObject(GameObject, "Container/Buttons/ButtonLoadInterstitial").GetComponent<MyUGUIButton>();
            _btnShowInterstitial = MyUtilities.FindObject(GameObject, "Container/Buttons/ButtonShowInterstitial").GetComponent<MyUGUIButton>();
            _btnLoadRewardedVideo = MyUtilities.FindObject(GameObject, "Container/Buttons/ButtonLoadRewardedVideo").GetComponent<MyUGUIButton>();
            _btnShowRewardedVideo = MyUtilities.FindObject(GameObject, "Container/Buttons/ButtonShowRewardedVideo").GetComponent<MyUGUIButton>();
        }

        public override void OnUGUIEnter()
        {
            this.LogInfo("OnUGUIEnter", "popup id = " + MyUGUIManager.Instance.CurrentPopup.ID.ToString(), ELogColor.DARK_UI);

            base.OnUGUIEnter();

            _btnClose.OnEventPointerClick.AddListener(_OnClickClose);
            _btnShowBanner.OnEventPointerClick.AddListener(_OnClickShowBanner);
            _btnLoadInterstitial.OnEventPointerClick.AddListener(_OnClickLoadInterstitial);
            _btnShowInterstitial.OnEventPointerClick.AddListener(_OnClickShowInterstitial);
            _btnLoadRewardedVideo.OnEventPointerClick.AddListener(_OnClickLoadRewardedVideo);
            _btnShowRewardedVideo.OnEventPointerClick.AddListener(_OnClickShowRewardedVideo);

#if USE_MY_ADMOB
            MyAdMobManager.Instance.Initialize(() =>
            {
                this.LogInfo("OnUGUIEnter", "AdMob was initialized", ELogColor.DARK_SDK);
            });
#endif
        }

        public override bool OnUGUIVisible()
        {
            if (base.OnUGUIVisible())
            {
                this.LogInfo("OnUGUIVisible", null, ELogColor.DARK_UI);
                return true;
            }
            return false;
        }

        public override void OnUGUIUpdate(float deltaTime)
        {
        }

        public override void OnUGUIExit()
        {
            this.LogInfo("OnUGUIExit", null, ELogColor.DARK_UI);

            base.OnUGUIExit();

            _btnClose.OnEventPointerClick.RemoveAllListeners();
            _btnShowBanner.OnEventPointerClick.RemoveAllListeners();
            _btnLoadInterstitial.OnEventPointerClick.RemoveAllListeners();
            _btnShowInterstitial.OnEventPointerClick.RemoveAllListeners();
            _btnLoadRewardedVideo.OnEventPointerClick.RemoveAllListeners();
            _btnShowRewardedVideo.OnEventPointerClick.RemoveAllListeners();
        }

        public override bool OnUGUIInvisible()
        {
            if (base.OnUGUIInvisible())
            {
                this.LogInfo("OnUGUIInvisible", null, ELogColor.DARK_UI);
                return true;
            }
            return false;
        }

        public override void OnUGUIBackKey()
        {
            MyUGUIManager.Instance.Back();
        }

        #endregion

        #region ----- Button Event -----

        private void _OnClickClose(PointerEventData arg0)
        {
            Hide();
        }

        private void _OnClickShowBanner(PointerEventData arg0)
        {
            this.LogInfo("_OnClickShowBanner", null, ELogColor.UI);

#if USE_MY_ADMOB
            MyAdMobManager.Instance.ShowBanner();
#else
            this.LogError("_OnClickShowBanner", "please add \"USE_MY_ADMOB\" to Define Symbols for using this feature");
#endif
        }

        private void _OnClickLoadInterstitial(PointerEventData arg0)
        {
            this.LogInfo("_OnClickLoadInterstitial", null, ELogColor.UI);

#if USE_MY_ADMOB
            if (!MyAdMobManager.Instance.IsInterstitialAdLoaded() && !MyAdMobManager.Instance.IsInterstitialAdLoading())
            {
                MyAdMobManager.Instance.LoadInterstitialAd(null, () =>
                {
                    this.LogInfo("_OnClickLoadInterstitial().LoadInterstitialAd", "onLoadedCallback", ELogColor.UI);
                }, () =>
                {
                    this.LogInfo("_OnClickLoadInterstitial().LoadInterstitialAd", "onFailedToLoadCallback", ELogColor.UI);
                });
            }
            else
            {
                Debug.Log("AdMobPopup._OnClickLoadInterstitial(): interstitial is loading");
            }
#else
            this.LogError("_OnClickLoadInterstitial", "please add \"USE_MY_ADMOB\" to Define Symbols for using this feature");
#endif
        }

        private void _OnClickShowInterstitial(PointerEventData arg0)
        {
            this.LogInfo("_OnClickShowInterstitial", null, ELogColor.UI);

#if USE_MY_ADMOB
            if (!MyAdMobManager.Instance.IsInterstitialAdLoaded() || MyAdMobManager.Instance.IsInterstitialAdLoading())
            {
                MyUGUIManager.Instance.ShowToastMessage("Interstitial hasn't loaded yet");
            }
            else
            {
                MyAdMobManager.Instance.ShowInterstitialAd(() =>
                {
                    this.LogInfo("_OnClickShowInterstitial().ShowInterstitialAd", "onOpeningCallback", ELogColor.SDK);
                }, () =>
                {
                    this.LogInfo("_OnClickShowInterstitial().ShowInterstitialAd", "onClosedCallback", ELogColor.SDK);
                });
            }
#else
            this.LogError("_OnClickShowInterstitial", "please add \"USE_MY_ADMOB\" to Define Symbols for using this feature");
#endif
        }

        private void _OnClickLoadRewardedVideo(PointerEventData arg0)
        {
            this.LogInfo("_OnClickLoadRewardedVideo", null, ELogColor.UI);

#if USE_MY_ADMOB
            if (!MyAdMobManager.Instance.IsRewardedAdLoaded() && !MyAdMobManager.Instance.IsRewardedAdLoading())
            {
                MyAdMobManager.Instance.LoadRewardedAd(null, () =>
                {
                    this.LogInfo("_OnClickLoadRewardedVideo().LoadRewardedAd", "onLoadedCallback", ELogColor.SDK);
                }, () =>
                {
                    this.LogInfo("_OnClickLoadRewardedVideo().LoadRewardedAd", "onFailedToLoadCallback", ELogColor.SDK);
                });
            }
            else
            {
                this.LogInfo("_OnClickLoadRewardedVideo", "rewarded video is loading", ELogColor.SDK);
            }
#else
            this.LogError("_OnClickLoadRewardedVideo", "please add \"USE_MY_ADMOB\" to Define Symbols for using this feature");
#endif
        }

        private void _OnClickShowRewardedVideo(PointerEventData arg0)
        {
            this.LogInfo("_OnClickShowRewardedVideo", null, ELogColor.UI);

#if USE_MY_ADMOB
            if (!MyAdMobManager.Instance.IsRewardedAdLoaded() || MyAdMobManager.Instance.IsRewardedAdLoading())
            {
                MyUGUIManager.Instance.ShowToastMessage("Rewarded Video hasn't loaded yet");
            }
            else
            {
                MyAdMobManager.Instance.ShowRewardedAd(() =>
                {
                    this.LogInfo("_OnClickShowRewardedVideo().ShowRewardedAd", "onOpeningCallback", ELogColor.SDK);
                }, () =>
                {
                    this.LogInfo("_OnClickShowRewardedVideo().ShowRewardedAd", "onUserEarnedRewardCallback", ELogColor.SDK);
                }, () =>
                {
                    this.LogInfo("_OnClickShowRewardedVideo().ShowRewardedAd", "onFailedToShowCallback", ELogColor.SDK);
                }, () =>
                {
                    this.LogInfo("_OnClickShowRewardedVideo().ShowRewardedAd", "onSkippedCallback", ELogColor.SDK);
                }, () =>
                {
                    this.LogInfo("_OnClickShowRewardedVideo().ShowRewardedAd", "onClosedCallback", ELogColor.SDK);
                });
            }
#else
            this.LogError("_OnClickShowRewardedVideo", "please add \"USE_MY_ADMOB\" to Define Symbols for using this feature");
#endif
        }

        #endregion

        #region ----- Public Method -----



        #endregion

        #region ----- Private Method -----



        #endregion
    }
}