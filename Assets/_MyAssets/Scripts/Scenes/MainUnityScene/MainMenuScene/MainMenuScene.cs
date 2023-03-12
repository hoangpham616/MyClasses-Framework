using UnityEngine.EventSystems;
using MyClasses;
using MyClasses.UI;

namespace MyApp.UI
{
    public class MainMenuScene : MyUGUIScene
    {
        #region ----- Variable -----

        private MyUGUIButton _btnButton;
        private MyUGUIButton _btnRunningMessage;
        private MyUGUIButton _btnFlyingMessage;
        private MyUGUIButton _btnToastMessage;
        private MyUGUIButton _btnLoadingIndicator;
        private MyUGUIButton _btnDialog2Buttons;
        private MyUGUIButton _btnReuasbleListView;
        private MyUGUIButton _btnGameScene;
        private MyUGUIButton _btnLogger;
        private MyUGUIButton _btnCoroutine;
        private MyUGUIButton _btnPool;
        private MyUGUIButton _btnLocalization;
        private MyUGUIButton _btnAdMob;

        #endregion

        #region ----- Constructor -----

        public MainMenuScene(ESceneID id, string prefabName, bool isInitWhenLoadScene, bool isHideHUD = false, float fadeInDuration = 0.5f, float fadeOutDuration = 0.5f)
        : base(id, prefabName, isInitWhenLoadScene, isHideHUD, fadeInDuration, fadeOutDuration)
        {
        }

        #endregion

        #region ----- MyUGUIScene Implementation -----

        public override void OnUGUIInit()
        {
            this.LogInfo("OnUGUIInit", null, ELogColor.DARK_UI);

            base.OnUGUIInit();

            _btnButton = MyUtilities.FindObject(GameObject, "Buttons/ButtonButton").GetComponent<MyUGUIButton>();
            _btnRunningMessage = MyUtilities.FindObject(GameObject, "Buttons/ButtonRunningMessage").GetComponent<MyUGUIButton>();
            _btnFlyingMessage = MyUtilities.FindObject(GameObject, "Buttons/ButtonFlyingMessage").GetComponent<MyUGUIButton>();
            _btnToastMessage = MyUtilities.FindObject(GameObject, "Buttons/ButtonToastMessage").GetComponent<MyUGUIButton>();
            _btnLoadingIndicator = MyUtilities.FindObject(GameObject, "Buttons/ButtonLoadingIndicator").GetComponent<MyUGUIButton>();
            _btnDialog2Buttons = MyUtilities.FindObject(GameObject, "Buttons/ButtonDialog2Buttons").GetComponent<MyUGUIButton>();
            _btnReuasbleListView = MyUtilities.FindObject(GameObject, "Buttons/ButtonReusableListView").GetComponent<MyUGUIButton>();
            _btnGameScene = MyUtilities.FindObject(GameObject, "Buttons/ButtonGameScene").GetComponent<MyUGUIButton>();
            _btnLogger = MyUtilities.FindObject(GameObject, "Buttons/ButtonLogger").GetComponent<MyUGUIButton>();
            _btnCoroutine = MyUtilities.FindObject(GameObject, "Buttons/ButtonCoroutine").GetComponent<MyUGUIButton>();
            _btnPool = MyUtilities.FindObject(GameObject, "Buttons/ButtonPool").GetComponent<MyUGUIButton>();
            _btnLocalization = MyUtilities.FindObject(GameObject, "Buttons/ButtonLocalization").GetComponent<MyUGUIButton>();
            _btnAdMob = MyUtilities.FindObject(GameObject, "Buttons/ButtonAdMob").GetComponent<MyUGUIButton>();
        }

        public override void OnUGUIEnter()
        {
            this.LogInfo("OnUGUIEnter", "scene id = " + MyUGUIManager.Instance.CurrentScene.ID.ToString(), ELogColor.DARK_UI);

            base.OnUGUIEnter();

            _btnButton.OnEventPointerClick.AddListener(_OnClickButton);
            _btnRunningMessage.OnEventPointerDoubleClick.AddListener(_OnClickRunningMessage);
            _btnFlyingMessage.OnEventPointerClick.AddListener(_OnClickFlyingMessage);
            _btnToastMessage.OnEventPointerClick.AddListener(_OnClickToastMessage);
            _btnLoadingIndicator.OnEventPointerClick.AddListener(_OnClickLoadingIndicator);
            _btnDialog2Buttons.OnEventPointerClick.AddListener(_OnClickDialog2Buttons);
            _btnReuasbleListView.OnEventPointerClick.AddListener(_OnClickReuasbleListView);
            _btnGameScene.OnEventPointerClick.AddListener(_OnClickGameScene);
            _btnLogger.OnEventPointerClick.AddListener(_OnClickLogger);
            _btnCoroutine.OnEventPointerClick.AddListener(_OnClickCoroutine);
            _btnPool.OnEventPointerClick.AddListener(_OnClickPool);
            _btnLocalization.OnEventPointerClick.AddListener(_OnClickLocalization);
            _btnAdMob.OnEventPointerClick.AddListener(_OnClickAdMob);
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

            _btnButton.OnEventPointerClick.RemoveAllListeners();
            _btnRunningMessage.OnEventPointerDoubleClick.RemoveAllListeners();
            _btnFlyingMessage.OnEventPointerClick.RemoveAllListeners();
            _btnToastMessage.OnEventPointerClick.RemoveAllListeners();
            _btnLoadingIndicator.OnEventPointerClick.RemoveAllListeners();
            _btnDialog2Buttons.OnEventPointerClick.RemoveAllListeners();
            _btnReuasbleListView.OnEventPointerClick.RemoveAllListeners();
            _btnGameScene.OnEventPointerClick.RemoveAllListeners();
            _btnLogger.OnEventPointerClick.RemoveAllListeners();
            _btnCoroutine.OnEventPointerClick.RemoveAllListeners();
            _btnPool.OnEventPointerClick.RemoveAllListeners();
            _btnLocalization.OnEventPointerClick.RemoveAllListeners();
            _btnAdMob.OnEventPointerClick.RemoveAllListeners();
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
            this.LogInfo("OnUGUIBackKey", null, ELogColor.DARK_UI);
        }

        #endregion

        #region ----- Button Event -----

        private void _OnClickButton(PointerEventData arg0)
        {
            this.LogInfo("_OnClickButton", null, ELogColor.UI);

            if (_btnButton.IsNormal)
            {
                _btnButton.SetGrayMode(true, true);
                _btnButton.SetGray(true);
                _btnButton.SetText("Button\n(Grayscale)");
            }
            else if (_btnButton.IsGray)
            {
                _btnButton.SetDarkMode(true, true);
                _btnButton.SetDark(true);
                _btnButton.SetText("Button\n(Dark)");
            }
            else
            {
                _btnButton.Normalize();
                _btnButton.SetText("Button\n(Normal)");
            }
        }

        private void _OnClickRunningMessage(PointerEventData arg0)
        {
            this.LogInfo("_OnClickRunningMessage", null, ELogColor.UI);

            MyUGUIManager.Instance.SetRunningMessageMaxQueue(MyUGUIRunningMessage.EType.Default, 3);
            MyUGUIManager.Instance.ShowRunningMessage("This is Running Message 1 (will not show because out of queue)", ERunningMessageSpeed.Normal, MyUGUIRunningMessage.EType.Default);
            MyUGUIManager.Instance.ShowRunningMessage("This is Running Message 2 (will not show because out of queue)", ERunningMessageSpeed.Normal, MyUGUIRunningMessage.EType.Default);
            MyUGUIManager.Instance.ShowRunningMessage("This is Running Message 3", ERunningMessageSpeed.Normal, MyUGUIRunningMessage.EType.Default);
            MyUGUIManager.Instance.ShowRunningMessage("This is Running Message 4", ERunningMessageSpeed.Normal, MyUGUIRunningMessage.EType.Default);
            MyUGUIManager.Instance.ShowRunningMessage("This is Running Message 5", ERunningMessageSpeed.Normal, MyUGUIRunningMessage.EType.Default);
        }

        private void _OnClickFlyingMessage(PointerEventData arg0)
        {
            this.LogInfo("_OnClickFlyingMessage", null, ELogColor.UI);

            if (UnityEngine.Random.Range(0, 100) % 2 == 0)
            {
                MyUGUIManager.Instance.ShowFlyingMessage("This is Flying Message\nEType = ShortFlyFromBot", MyUGUIFlyingMessage.EType.ShortFlyFromBot);
            }
            else
            {
                MyUGUIManager.Instance.ShowFlyingMessage("This is Flying Message\nEType = ShortFlyFromMid", MyUGUIFlyingMessage.EType.ShortFlyFromMid);
            }
        }

        private void _OnClickToastMessage(PointerEventData arg0)
        {
            this.LogInfo("_OnClickToastMessage", null, ELogColor.UI);

            MyUGUIManager.Instance.ShowToastMessage("This is Toast Message", EToastMessageDuration.Medium);
        }

        private void _OnClickLoadingIndicator(PointerEventData arg0)
        {
            this.LogInfo("_OnClickLoadingIndicator", null, ELogColor.UI);

            int id1 = MyUGUIManager.Instance.ShowLoadingIndicator(1, () =>
            {
                this.LogInfo("_OnClickLoadingIndicator", "Loading Indicator 1 timeout", ELogColor.UI);
            });
            int id2 = MyUGUIManager.Instance.ShowLoadingIndicator(2, () =>
            {
                this.LogInfo("_OnClickLoadingIndicator", "Loading Indicator 2 timeout", ELogColor.UI);
            });
            int id3 = MyUGUIManager.Instance.ShowLoadingIndicator(3, () =>
            {
                this.LogInfo("_OnClickLoadingIndicator", "Loading Indicator 3 timeout", ELogColor.UI);
            });
        }

        private void _OnClickDialog2Buttons(PointerEventData arg0)
        {
            this.LogInfo("_OnClickDialog2Buttons", null, ELogColor.UI);

            MyUGUIPopup2Buttons popup = (MyUGUIPopup2Buttons)MyUGUIManager.Instance.ShowPopup(EPopupID.Dialog2ButtonsPopup);
            popup.SetData("TITLE", "Body", "Left", (data) =>
            {
                this.LogInfo("_OnClickDialog2Buttons", "Click Left Button", ELogColor.UI);
            }, "Right", (data) =>
            {
                this.LogInfo("_OnClickDialog2Buttons", "Click Right Button", ELogColor.UI);
            }, (data) =>
            {
                this.LogInfo("_OnClickDialog2Buttons", "Click Close Button", ELogColor.UI);
            }, false);
        }

        private void _OnClickReuasbleListView(PointerEventData arg0)
        {
            this.LogInfo("_OnClickReuasbleListView", null, ELogColor.UI);

            MyUGUIManager.Instance.ShowPopup(EPopupID.ReusableListViewPopup);
        }

        private void _OnClickGameScene(PointerEventData arg0)
        {
            this.LogInfo("_OnClickGameScene", null, ELogColor.UI);

            MyUGUIManager.Instance.ShowScene(ESceneID.GameScene);
        }

        private void _OnClickLogger(PointerEventData arg0)
        {
            this.LogInfo("_OnClickLogger", null, ELogColor.UI);

            this.LogInfo("_OnClickLogger", "log color DEFAULT");
            this.LogError("_OnClickLogger", "log color DARK_CORE", ELogColor.DARK_SDK);
            this.LogWarning("_OnClickLogger", "log color CORE", ELogColor.SDK);
            this.LogError("_OnClickLogger", "log color DARK_NETWORK", ELogColor.DARK_NETWORK);
            this.LogWarning("_OnClickLogger", "log color NETWORK", ELogColor.NETWORK);
            MyLogger.Error(typeof(MainMenuScene).Name, "_OnClickLogger", "log color DARK_UI", ELogColor.DARK_UI);
            MyLogger.Warning(typeof(MainMenuScene).Name, "_OnClickLogger", "log color UI", ELogColor.UI);
            MyLogger.Error(typeof(MainMenuScene).Name, "_OnClickLogger", "log color DARK_GAMEPLAY", ELogColor.DARK_GAMEPLAY);
            MyLogger.Warning(typeof(MainMenuScene).Name, "_OnClickLogger", "log color GAMEPLAY", ELogColor.GAMEPLAY);
        }

        private void _OnClickCoroutine(PointerEventData arg0)
        {
            this.LogInfo("_OnClickCoroutine", null, ELogColor.UI);

            MyCoroutiner.ExcuteAfterDelayFrame("DelayFrame", 2000, () =>
            {
                this.LogInfo("_OnClickCoroutine", "callback after 2000 frames delay");
            });
            MyCoroutiner.ExcuteAfterDelayTime("DelaySecond", 1.5f, () =>
            {
                this.LogInfo("_OnClickCoroutine", "callback after 1.5 second delay");
            });
            MyCoroutiner.ExcuteAfterEndOfFrame(() =>
            {
                this.LogInfo("_OnClickCoroutine", "callback after frame ends");
            });
        }

        private void _OnClickPool(PointerEventData arg0)
        {
            this.LogInfo("_OnClickPool", null, ELogColor.UI);

            MyUGUIManager.Instance.ShowPopup(EPopupID.PoolPopup);
        }

        private void _OnClickLocalization(PointerEventData arg0)
        {
            this.LogInfo("_OnClickLocalization", null, ELogColor.UI);

            MyUGUIManager.Instance.ShowPopup(EPopupID.LocalizationPopup);
        }

        private void _OnClickAdMob(PointerEventData arg0)
        {
            this.LogInfo("_OnClickAdMob", null, ELogColor.UI);

            MyUGUIManager.Instance.ShowPopup(EPopupID.AdMobPopup);
        }

        #endregion

        #region ----- Public Method -----



        #endregion

        #region ----- Private Method -----



        #endregion
    }
}