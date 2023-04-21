/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUGUIManager (version 2.39)
 */

#pragma warning disable 0162
#pragma warning disable 0429

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace MyClasses.UI
{
    public class MyUGUIManager : MonoBehaviour
    {
        #region ----- Internal Class -----

        private class AssetBundleConfig
        {
            public string URL;
            public int Version;
        }

        #endregion

        #region ----- Define -----

        public static readonly string CONFIG_DIRECTORY = "Configs/UGUI/";
        public static readonly string HUD_DIRECTORY = "Prefabs/UGUI/HUDs/";
        public static readonly string SCENE_DIRECTORY = "Prefabs/UGUI/Scenes/";
        public static readonly string POPUP_DIRECTORY = "Prefabs/UGUI/Popups/";
        public static readonly string SPECIALITY_DIRECTORY = "Prefabs/UGUI/Specialities/";

        #endregion

        #region ----- Variable -----

        private Camera _cameraUI;

        private Canvas _canvas;
        private Canvas _canvasOnTop;
        private CanvasScaler _canvasScaler;
        private GameObject _canvasOnTopHUD;
        private GameObject _canvasOnTopPopup;
        private GameObject _canvasOnTopFloatPopup;
        private GameObject _canvasOnTopLoadingIndicator;
        private GameObject _canvasSceneFading;

        private Dictionary<EPopupID, MyUGUIConfigPopup> _dictPopupConfig = new Dictionary<EPopupID, MyUGUIConfigPopup>();
        private AssetBundleConfig _coreAssetBundleConfig;

        private List<MyUGUIUnityScene> _listUnityScene = new List<MyUGUIUnityScene>();
        private List<MyUGUIScene> _listScene = new List<MyUGUIScene>();
        private List<MyUGUIPopup> _listPopup = new List<MyUGUIPopup>();
        private List<MyUGUIPopup> _listFloatPopup = new List<MyUGUIPopup>();
        private List<MyUGUIFlyingMessage> _listFlyingMessage = new List<MyUGUIFlyingMessage>();
        private List<MyUGUIRunningMessage> _listRunningMessage = new List<MyUGUIRunningMessage>();

        private MyUGUIUnityScene _currentUnityScene;
        private MyUGUIUnityScene _nextUnityScene;

        private MyUGUIScene _previousScene;
        private MyUGUIScene _nextScene;
        private MyUGUIScene _currentScene;
        private MyUGUISceneFading _currentSceneFading;

        private MyUGUIPopupOverlay _currentPopupOverlay;
        private MyUGUIPopup _currentPopup;
        private MyUGUIPopup _currentFloatPopup;

        private MyUGUILoadingIndicator _currentLoadingIndicator;
        private MyUGUIFlyingMessage _currentFlyingMessage;
        private MyUGUIRunningMessage _currentRunningMessage;
        private MyUGUIToastMessage _currentToastMessage;

        private Dictionary<MyUGUIRunningMessage.EType, int> _dictRunningMessageMaxQueue = new Dictionary<MyUGUIRunningMessage.EType, int>();

        private AsyncOperation _unitySceneUnloadUnusedAsset;
        private AsyncOperation _unitySceneLoad;

        private Action _onScenePreEnterCallback;
        private Action _onScenePostEnterCallback;
        private Action _onScenePostVisibleCallback;

        private bool _isClosePopupByClickingOutside;
        private bool _isHideRunningMessageWhenSwitchingScene;
        private bool _isHideToastWhenSwitchingScene;

        private int _previousInitSceneIndex;

        #endregion

        #region ----- Property -----

        public Camera Camera
        {
            get { return _cameraUI; }
        }

        public Canvas Canvas
        {
            get { return _canvas; }
        }

        public Canvas CanvasOnTop
        {
            get { return _canvasOnTop; }
        }

        public Vector2 DesignResolution
        {
            get
            {
                if (_canvasScaler == null)
                {
                    _canvasScaler = _canvas.GetComponent<CanvasScaler>();
                }
                return _canvasScaler.referenceResolution;
            }
        }

        public GameObject CanvasOnTopHUD
        {
            get { return _canvasOnTopHUD; }
        }

        public GameObject CanvasOnTopPopup
        {
            get { return _canvasOnTopPopup; }
        }

        public GameObject CanvasOnTopFloatPopup
        {
            get { return _canvasOnTopFloatPopup; }
        }

        public MyUGUIUnityScene CurrentUnityScene
        {
            get { return _currentUnityScene; }
        }

        public MyUGUIHUD CurrentHUD
        {
            get { return _currentUnityScene.HUD; }
        }

        public MyUGUIScene CurrentScene
        {
            get { return _currentScene; }
        }

        public MyUGUIScene PreviousScene
        {
            get { return _previousScene; }
        }

        public MyUGUISceneFading CurrentSceneFading
        {
            get { return _currentSceneFading; }
        }

        public MyUGUIPopup CurrentPopup
        {
            get { return _currentPopup; }
        }

        public MyUGUIPopup CurrentFloatPopup
        {
            get { return _currentFloatPopup; }
        }

        public MyUGUILoadingIndicator CurrentLoadingIndicator
        {
            get { return _currentLoadingIndicator; }
        }

        public bool IsShowingLoadingIndicator
        {
            get { return _currentLoadingIndicator != null && _currentLoadingIndicator.IsActive; }
        }

        public bool IsClosePopupByClickingOutside
        {
            get { return _isClosePopupByClickingOutside; }
            set { _isClosePopupByClickingOutside = value; }
        }

        public bool IsTouchingOnUI
        {
            get
            {
                if (EventSystem.current == null)
                {
                    return false;
                }
                return EventSystem.current.IsPointerOverGameObject();
            }
        }

        #endregion

        #region ----- Singleton -----

        private static object mSingletonLock = new object();
        private static MyUGUIManager mInstance;

        public static MyUGUIManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (mSingletonLock)
                    {
                        mInstance = (MyUGUIManager)FindObjectOfType(typeof(MyUGUIManager));
                        if (mInstance == null)
                        {
                            GameObject obj = new GameObject(typeof(MyUGUIManager).Name);
                            mInstance = obj.AddComponent<MyUGUIManager>();
                            DontDestroyOnLoad(obj);
                        }
                    }
                }
                return mInstance;
            }
        }

        #endregion

        #region ----- MonoBehaviour Implementation -----

        /// <summary>
        /// Awake.
        /// </summary>
        void Awake()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            _InitConfig();
        }

        /// <summary>
        /// Update.
        /// </summary>
        void Update()
        {
            _UpdateUnityScene();
            _UpdateScene(Time.deltaTime);
            _UpdateHUD(Time.deltaTime);

            for (int i = _listPopup.Count - 1; i >= 0; --i)
            {
                MyUGUIPopup popup = _listPopup[i];
                if (popup == null)
                {
                    _listPopup.RemoveAt(i);
                }
                else
                {
                    _UpdatePopup(ref popup, Time.deltaTime);
                    _listPopup[i] = popup;
                }
            }
            for (int i = _listFloatPopup.Count - 1; i >= 0; --i)
            {
                MyUGUIPopup popup = _listFloatPopup[i];
                if (popup == null)
                {
                    _listFloatPopup.RemoveAt(i);
                }
                else
                {
                    _UpdateFloatPopup(ref popup, Time.deltaTime);
                    _listFloatPopup[i] = popup;
                }
            }

            if (_currentRunningMessage != null)
            {
                _currentRunningMessage.Update(Time.deltaTime);
            }
        }

        /// <summary>
        /// LateUpdate.
        /// </summary>
        void LateUpdate()
        {
            if (_currentToastMessage != null)
            {
                _currentToastMessage.LateUpdate(Time.deltaTime);
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set asset bundle for core UI (scene fading, popup overlay, toast, running text, loading indicator).
        /// </summary>
        public void SetAssetBundleForCore(string url, int version)
        {
            _coreAssetBundleConfig = new AssetBundleConfig()
            {
                URL = url,
                Version = version
            };
        }

        /// <summary>
        /// Set asset bundle for HUDs.
        /// </summary>
        public void SetAssetBundleForHUDs(string url, int version)
        {
            foreach (MyUGUIUnityScene unityScene in _listUnityScene)
            {
                if (unityScene.HUD != null)
                {
                    unityScene.HUD.SetAssetBundle(url, version);
                }
            }
        }

        /// <summary>
        /// Set asset bundle for scene.
        /// </summary>
        public void SetAssetBundleForScene(ESceneID sceneID, string url, int version)
        {
            foreach (MyUGUIUnityScene unityScene in _listUnityScene)
            {
                foreach (MyUGUIScene scene in unityScene.ListScene)
                {
                    if (scene.ID == sceneID)
                    {
                        scene.SetAssetBundle(url, version);
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Set asset bundle for popup.
        /// </summary>
        public void SetAssetBundleForPopup(EPopupID popupID, string url, int version)
        {
            foreach (var item in _dictPopupConfig)
            {
                MyUGUIConfigPopup configPopup = _dictPopupConfig[item.Key];
                if (configPopup.ID == popupID)
                {
                    configPopup.AssetBundleURL = url;
                    configPopup.AssetBundleVersion = version;
                    return;
                }
            }
        }

        /// <summary>
        /// Back to previous scene.
        /// </summary>
        public bool Back()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>Back()</color>");
#endif

            if (_currentLoadingIndicator != null && _currentLoadingIndicator.IsActive)
            {
                return false;
            }

            if (_currentPopup != null && _currentPopup.IsActive)
            {
                HideCurrentPopup();
                return true;
            }

            int countScene = _listScene.Count;
            if (countScene >= 2)
            {
                MyUGUIScene scene = _listScene[countScene - 2];
                if (scene.UnitySceneID == _currentUnityScene.ID)
                {
                    ShowScene(scene.ID);
                }
                else
                {
                    ShowUnityScene(scene.UnitySceneID, scene.ID);
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Show a scene.
        /// </summary>
        /// <param name="unitySceneID">Empty: without scene</param>
        public void ShowUnityScene(EUnitySceneID unitySceneID, ESceneID sceneID)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowUnityScene()</color>: " + unitySceneID + " - " + sceneID);
#endif

            int countUnityScene = _listUnityScene.Count;
            for (int i = 0; i < countUnityScene; i++)
            {
                if (_listUnityScene[i].ID == unitySceneID)
                {
                    if (_currentUnityScene == null)
                    {
                        _currentUnityScene = _listUnityScene[i];
                        _currentUnityScene.State = EUnitySceneState.Unload;
                    }

                    _nextUnityScene = _listUnityScene[i];

                    int countScene = _nextUnityScene.ListScene.Count;
                    for (int j = 0; j < countScene; j++)
                    {
                        if (_nextUnityScene.ListScene[j].ID == sceneID)
                        {
                            if (_currentScene != null && _currentScene.IsLoaded)
                            {
                                _currentScene.OnUGUIExit();
                                _currentScene.OnUGUIInvisible();
                            }
                            _currentScene = null;
                            _nextScene = _nextUnityScene.ListScene[j];
                            _nextScene.UnitySceneID = _nextUnityScene.ID;
                            _AddSceneIntoSceneStack(_nextScene);
                            break;
                        }
                    }

                    return;
                }
            }
        }

        /// <summary>
        /// Return a scene which showed before in current Unity scene.
        /// </summary>
        public MyUGUIScene GetScene(ESceneID sceneID)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>GetScene()</color>: " + sceneID);
#endif

            int countScene = _currentUnityScene.ListScene.Count;
            for (int i = 0; i < countScene; i++)
            {
                if (_currentUnityScene.ListScene[i].ID == sceneID)
                {
                    return _currentUnityScene.ListScene[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Show a scene.
        /// </summary>
        public void ShowScene(ESceneID sceneID, bool isHideRunningMessageWhenSwitchingScene = false, bool isHideToastWhenSwitchingScene = true, Action onPreEnterCallback = null, Action onPostEnterCallback = null, Action onPostVisibleCallback = null)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowScene()</color>: " + sceneID);
#endif

            int countScene = _currentUnityScene.ListScene.Count;
            for (int i = 0; i < countScene; i++)
            {
                if (_currentUnityScene.ListScene[i].ID == sceneID)
                {
                    _isHideRunningMessageWhenSwitchingScene = isHideRunningMessageWhenSwitchingScene;
                    _isHideToastWhenSwitchingScene = isHideToastWhenSwitchingScene;

                    _onScenePreEnterCallback = onPreEnterCallback;
                    _onScenePostEnterCallback = onPostEnterCallback;
                    _onScenePostVisibleCallback = onPostVisibleCallback;

                    if (_currentScene == null)
                    {
                        _currentScene = _currentUnityScene.ListScene[i];
                        _currentScene.State = EBaseState.LoadAssetBundle;
                    }

                    _nextScene = _currentUnityScene.ListScene[i];
                    _nextScene.UnitySceneID = _currentUnityScene.ID;
                    _AddSceneIntoSceneStack(_nextScene);

                    return;
                }
            }
        }

        /// <summary>
        /// Update popup overlay.
        /// </summary>
        public void UpdatePopupOverlay()
        {
            if (_currentPopupOverlay != null)
            {
                MyPrivateCoroutiner.Start("_DoUpdatePopupOverlay", _DoUpdatePopupOverlay());
            }
        }

        /// <summary>
        /// Show a popup.
        /// </summary>
        public MyUGUIPopup ShowPopup(EPopupID popupID)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowPopup()</color>: " + popupID);
#endif

            bool isRepeatable = popupID == EPopupID.Dialog0ButtonPopup || popupID == EPopupID.Dialog1ButtonPopup || popupID == EPopupID.Dialog2ButtonsPopup;

            return _ShowPopup(popupID, isRepeatable, null, null, null);
        }

        /// <summary>
        /// Show a popup.
        /// </summary>
        public MyUGUIPopup ShowPopup(EPopupID popupID, Action<MyUGUIPopup> onCloseCallback)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowPopup()</color>: " + popupID);
#endif

            bool isRepeatable = popupID == EPopupID.Dialog0ButtonPopup || popupID == EPopupID.Dialog1ButtonPopup || popupID == EPopupID.Dialog2ButtonsPopup;

            return _ShowPopup(popupID, isRepeatable, null, null, onCloseCallback);
        }

        /// <summary>
        /// Show a popup.
        /// </summary>
        public MyUGUIPopup ShowPopup(EPopupID popupID, Action<MyUGUIPopup> onEnterCallback, Action<MyUGUIPopup> onCloseCallback)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowPopup()</color>: " + popupID);
#endif

            bool isRepeatable = popupID == EPopupID.Dialog0ButtonPopup || popupID == EPopupID.Dialog1ButtonPopup || popupID == EPopupID.Dialog2ButtonsPopup;

            return _ShowPopup(popupID, isRepeatable, null, onEnterCallback, onCloseCallback);
        }

        /// <summary>
        /// Show a popup.
        /// </summary>
        public MyUGUIPopup ShowPopup(EPopupID popupID, object attachedData, Action<MyUGUIPopup> onCloseCallback)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowPopup()</color>: " + popupID);
#endif

            bool isRepeatable = popupID == EPopupID.Dialog0ButtonPopup || popupID == EPopupID.Dialog1ButtonPopup || popupID == EPopupID.Dialog2ButtonsPopup;

            return _ShowPopup(popupID, isRepeatable, attachedData, null, onCloseCallback);
        }

        /// <summary>
        /// Show a popup.
        /// </summary>
        public MyUGUIPopup ShowPopup(EPopupID popupID, object attachedData, Action<MyUGUIPopup> onEnterCallback, Action<MyUGUIPopup> onCloseCallback)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowPopup()</color>: " + popupID);
#endif

            bool isRepeatable = popupID == EPopupID.Dialog0ButtonPopup || popupID == EPopupID.Dialog1ButtonPopup || popupID == EPopupID.Dialog2ButtonsPopup;

            return _ShowPopup(popupID, isRepeatable, attachedData, onEnterCallback, onCloseCallback);
        }

        /// <summary>
        /// Show a repeatable popup.
        /// </summary>
        public MyUGUIPopup ShowRepeatablePopup(EPopupID popupID)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowRepeatablePopup()</color>: " + popupID);
#endif

            return _ShowPopup(popupID, true, null, null, null);
        }

        /// <summary>
        /// Show a repeatable popup.
        /// </summary>
        public MyUGUIPopup ShowRepeatablePopup(EPopupID popupID, Action<MyUGUIPopup> onCloseCallback)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowRepeatablePopup()</color>: " + popupID);
#endif

            return _ShowPopup(popupID, true, null, null, onCloseCallback);
        }

        /// <summary>
        /// Show a repeatable popup.
        /// </summary>
        public MyUGUIPopup ShowRepeatablePopup(EPopupID popupID, Action<MyUGUIPopup> onEnterCallback, Action<MyUGUIPopup> onCloseCallback)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowRepeatablePopup()</color>: " + popupID);
#endif

            return _ShowPopup(popupID, true, null, onEnterCallback, onCloseCallback);
        }

        /// <summary>
        /// Show a repeatable popup.
        /// </summary>
        public MyUGUIPopup ShowRepeatablePopup(EPopupID popupID, object attachedData)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowRepeatablePopup()</color>: " + popupID);
#endif

            return _ShowPopup(popupID, true, attachedData, null, null);
        }

        /// <summary>
        /// Show a repeatable popup.
        /// </summary>
        public MyUGUIPopup ShowRepeatablePopup(EPopupID popupID, object attachedData, Action<MyUGUIPopup> onCloseCallback)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowRepeatablePopup()</color>: " + popupID);
#endif

            return _ShowPopup(popupID, true, attachedData, null, onCloseCallback);
        }

        /// <summary>
        /// Show a repeatable popup.
        /// </summary>
        public MyUGUIPopup ShowRepeatablePopup(EPopupID popupID, object attachedData, Action<MyUGUIPopup> onEnterCallback, Action<MyUGUIPopup> onCloseCallback)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowRepeatablePopup()</color>: " + popupID);
#endif

            return _ShowPopup(popupID, true, attachedData, onEnterCallback, onCloseCallback);
        }

        /// <summary>
        /// Show a float popup.
        /// </summary>
        public MyUGUIPopup ShowFloatPopup(EPopupID popupID, object attachedData = null)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowFloatPopup()</color>: " + popupID);
#endif

            return _ShowFloatPopup(popupID, false, attachedData);
        }

        /// <summary>
        /// Show a repeatable float popup.
        /// </summary>
        public MyUGUIPopup ShowRepeatableFloatPopup(EPopupID popupID, object attachedData = null)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowRepeatableFloatPopup()</color>: " + popupID);
#endif

            return _ShowFloatPopup(popupID, true, attachedData);
        }

        /// <summary>
        /// Hide current popup.
        /// </summary>
        public void HideCurrentPopup()
        {
            int countPopup = _listPopup.Count;
            for (int i = countPopup - 1; i >= 0; i--)
            {
                if (_listPopup[i] != null)
                {
                    _listPopup[i].Hide();
                    return;
                }
            }
        }

        /// <summary>
        /// Hide current float popup.
        /// </summary>
        public void HideCurrentFloatPopup()
        {
            int countPopup = _listFloatPopup.Count;
            for (int i = countPopup - 1; i >= 0; i--)
            {
                if (_listFloatPopup[i] != null)
                {
                    _listFloatPopup[i].Hide();
                    return;
                }
            }
        }

        /// <summary>
        /// Hide all popups.
        /// </summary>
        public void HideAllPopups(bool isHidePopup = true, bool isHideFloatPopup = true)
        {
            if (isHidePopup)
            {
                foreach (MyUGUIPopup popup in _listPopup)
                {
                    if (popup != null && popup.IsActive)
                    {
                        popup.Hide();
                    }
                }
            }

            if (isHideFloatPopup)
            {
                foreach (MyUGUIPopup popup in _listFloatPopup)
                {
                    if (popup != null)
                    {
                        popup.Hide();
                    }
                }
            }
        }

        /// <summary>
        /// Hide current tips loading indicator, show simple loading indicator and return loading id.
        /// </summary>
        public int ShowLoadingIndicator()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowLoadingIndicator()</color>");
#endif

            _InitLoadingIndicator();

            return _currentLoadingIndicator.ShowSimple();
        }

        /// <summary>
        /// Hide current tips loading indicator, show simple loading indicator and return loading id.
        /// </summary>
        public int ShowLoadingIndicator(float timeOut, Action timeOutCallback = null)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowLoadingIndicator()</color>: timeOut=" + timeOut);
#endif

            _InitLoadingIndicator();

            return _currentLoadingIndicator.ShowSimple(timeOut, timeOutCallback);
        }

        /// <summary>
        /// Hide current simple loading indicator and show tips loading indicator.
        /// </summary>
        public void ShowLoadingIndicatorWithTips(string tips, string description, bool isThreeDots, Action cancelCallback = null)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowLoadingIndicatorWithTips()</color>: tips=" + tips + " | description=" + description + " | isThreeDots=" + isThreeDots);
#endif

            _InitLoadingIndicator();

            _currentLoadingIndicator.ShowTips(tips, description, isThreeDots, -1, null, cancelCallback);
        }

        /// <summary>
        /// Hide current simple loading indicator and show tips loading indicator.
        /// </summary>
        public void ShowLoadingIndicatorWithTips(string tips, string description, bool isThreeDots, float timeOut, Action timeOutCallback = null, Action cancelCallback = null)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowLoadingIndicatorWithTips()</color>: tips=" + tips + " | description=" + description + " | isThreeDots=" + isThreeDots + " | timeOut=" + timeOut);
#endif

            _InitLoadingIndicator();

            _currentLoadingIndicator.ShowTips(tips, description, isThreeDots, timeOut, timeOutCallback, cancelCallback);
        }

        /// <summary>
        /// Hide loading indicator.
        /// </summary>
        /// <param name="minLiveTime">minimum seconds have to show before hiding</param>
        public void HideLoadingIndicator(float minLiveTime = 0.15f)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>HideLoadingIndicator()</color>: minLiveTime=" + minLiveTime);
#endif

            if (_currentLoadingIndicator != null)
            {
                _currentLoadingIndicator.Hide(minLiveTime);
            }
        }

        /// <summary>
        /// Hide loading indicator by loading id.
        /// </summary>
        /// <param name="minLiveTime">minimum seconds have to show before hiding</param>
        public void HideLoadingIndicator(int loadingID, float minLiveTime = 0.15f)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>HideLoadingIndicator()</color>: loadingID=" + loadingID + " minLiveTime=" + minLiveTime);
#endif

            if (_currentLoadingIndicator != null)
            {
                _currentLoadingIndicator.HideSimple(loadingID, minLiveTime);
            }
        }

        /// <summary>
        /// Show flying message.
        /// </summary>
        public void ShowFlyingMessage(string content, MyUGUIFlyingMessage.EType type = MyUGUIFlyingMessage.EType.ShortFlyFromBot)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowFlyingMessage()</color>");
#endif

            _InitFlyingMessage();

            _currentFlyingMessage.Show(content, type);
        }

        /// <summary>
        /// Set queue limit for running message.
        /// </summary>
        public void SetRunningMessageMaxQueue(MyUGUIRunningMessage.EType type = MyUGUIRunningMessage.EType.Default, int maxQueue = -1)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>SetRunningMessageMaxQueue()</color>");
#endif

            _dictRunningMessageMaxQueue[type] = maxQueue;
        }

        /// <summary>
        /// Show running message.
        /// </summary>
        public void ShowRunningMessage(string content, ERunningMessageSpeed speed = ERunningMessageSpeed.Normal, MyUGUIRunningMessage.EType type = MyUGUIRunningMessage.EType.Default)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowRunningMessage()</color>");
#endif

            _InitRunningMessage(type);

            if (_dictRunningMessageMaxQueue.ContainsKey(type))
            {
                _currentRunningMessage.SetMaxQueue(_dictRunningMessageMaxQueue[type]);
            }
            _currentRunningMessage.Show(content, (int)speed, (int)speed * 1.2f);
        }

        /// <summary>
        /// Hide running message.
        /// </summary>
        public void HideRunningMessage()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>HideRunningMessage()</color>");
#endif

            if (_currentRunningMessage != null)
            {
                _currentRunningMessage.Hide();
            }
        }

        /// <summary>
        /// Show toast message.
        /// </summary>
        public void ShowToastMessage(string content, EToastMessageDuration duration = EToastMessageDuration.Medium)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowToastMessage()</color>: duration=" + duration.ToString());
#endif

            _InitToastMessage();

            _currentToastMessage.Show(content, (int)duration);
        }

        /// <summary>
        /// Show toast message.
        /// </summary>
        public void ShowToastMessage(string content, float duration)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>ShowToastMessage()</color>: duration=" + duration);
#endif

            _InitToastMessage();

            _currentToastMessage.Show(content, duration);
        }

        /// <summary>
        /// Hide toast.
        /// </summary>
        public void HideToast()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#0000FFFF>HideToast()</color>");
#endif

            if (_currentToastMessage != null)
            {
                _currentToastMessage.Hide();
            }
        }

        /// <summary>
        /// Convert screen point to world point.
        /// </summary>
        public Vector3 ScreenToWorldPoint(Vector3 screenPoint)
        {
            screenPoint.z = _canvas.planeDistance;
            return _cameraUI.ScreenToWorldPoint(screenPoint);
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Init scenes and popups config.
        /// </summary>
        private void _InitConfig()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#FF0000FF>_InitConfig()</color>");
#endif

#if UNITY_EDITOR
            if (!System.IO.File.Exists("Assets/Resources/" + CONFIG_DIRECTORY + typeof(MyUGUIConfigUnityScenes).Name + ".asset"))
            {
                Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _InitConfig(): Could not find scene config file. Please setup it on Menu Bar first.");
                return;
            }

            if (!System.IO.File.Exists("Assets/Resources/" + CONFIG_DIRECTORY + typeof(MyUGUIConfigPopups).Name + ".asset"))
            {
                Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _InitConfig(): Could not find popup config file. Please setup it on Menu Bar first.");
                return;
            }
#endif

            _listUnityScene.Clear();
            MyUGUIConfigUnityScenes unityScenesConfig = Resources.Load<MyUGUIConfigUnityScenes>(CONFIG_DIRECTORY + typeof(MyUGUIConfigUnityScenes).Name);
            if (unityScenesConfig != null)
            {
                foreach (MyUGUIConfigUnityScene unitySceneConfig in unityScenesConfig.ListUnityScene)
                {
                    MyUGUIUnityScene unityScene = new MyUGUIUnityScene(unitySceneConfig.ID, unitySceneConfig.SceneName);
                    if (!string.IsNullOrEmpty(unitySceneConfig.HUDScriptName))
                    {
                        if (string.IsNullOrEmpty(unitySceneConfig.HUDPrefabName))
                        {
                            Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _InitConfig(): HUD prefab of " + unitySceneConfig.HUDScriptPath + " is empty. Please setup it on Menu Bar.");
                            return;
                        }
                        MyUGUIHUD hud = (MyUGUIHUD)Activator.CreateInstance(MyUtilities.FindTypesByName(unitySceneConfig.HUDScriptName)[0], unitySceneConfig.HUDPrefabName);
                        hud.SetPrefabName3D(unitySceneConfig.HUDPrefabName3D);
                        unityScene.SetHUD(hud);
                    }
                    foreach (MyUGUIConfigScene sceneConfig in unitySceneConfig.ListScene)
                    {
                        MyUGUIScene scene = (MyUGUIScene)Activator.CreateInstance(MyUtilities.FindTypesByName(sceneConfig.ScriptName)[0], sceneConfig.ID, sceneConfig.PrefabName, sceneConfig.IsInitWhenLoadUnityScene, sceneConfig.IsHideHUD, sceneConfig.FadeInDuration, sceneConfig.FadeOutDuration);
                        scene.SetPrefabName3D(sceneConfig.PrefabName3D);
                        unityScene.AddScene((MyUGUIScene)scene);
                    }
                    _listUnityScene.Add(unityScene);
                }
            }
            else
            {
                Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _InitConfig(): Could not scene config file.");
            }

            _dictPopupConfig.Clear();
            MyUGUIConfigPopups popupsConfig = Resources.Load<MyUGUIConfigPopups>(CONFIG_DIRECTORY + typeof(MyUGUIConfigPopups).Name);
            if (popupsConfig != null)
            {
                foreach (MyUGUIConfigPopup popupConfig in popupsConfig.ListPopup)
                {
                    _dictPopupConfig[popupConfig.ID] = popupConfig;
                }
            }
            else
            {
                Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _InitConfig(): Could not load popup config file.");
            }
        }

        /// <summary>
        /// Init canvases.
        /// </summary>
        private void _InitCanvas()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#FF7777FF>_InitCanvas()</color>");
#endif

            GameObject goCanvas = GameObject.Find("Canvas");
            if (goCanvas == null)
            {
                Debug.LogError("[" + typeof(MyUGUIManager).Name + "] OnUGUIInit(): Could not find \"Canvas\" in scene \"" + SceneManager.GetActiveScene().name + "\". Please create \"Canvas\" first.");
            }
            if (goCanvas != null)
            {
                _canvas = goCanvas.GetComponent<Canvas>();
                _canvas.sortingOrder = -1000;
            }

            GameObject goCanvasOnTop = GameObject.Find("CanvasOnTop");
            if (goCanvasOnTop == null)
            {
                Debug.LogError("[" + typeof(MyUGUIManager).Name + "] OnUGUIInit(): Could not find \"CanvasOnTop\" in scene \"" + SceneManager.GetActiveScene().name + "\". Please create \"CanvasOnTop\" first.");
            }
            if (goCanvasOnTop != null)
            {
                _canvasOnTop = goCanvasOnTop.GetComponent<Canvas>();
                _canvasOnTop.sortingOrder = 1000;

                if (_currentUnityScene.HUD != null)
                {
                    _canvasOnTopHUD = MyUtilities.FindObjectInFirstLayer(_canvasOnTop.gameObject, "HUD");
                    if (_canvasOnTopHUD == null)
                    {
                        _canvasOnTopHUD = new GameObject("HUD");

                        RectTransform rect = _canvasOnTopHUD.AddComponent<RectTransform>();
                        MyUtilities.Anchor(ref rect, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);

                        _canvasOnTopHUD.AddComponent<CanvasRenderer>();

                        _canvasOnTopHUD.transform.SetParent(_canvasOnTop.transform, false);
                    }
                    if (_canvasOnTopHUD != null)
                    {
                        _canvasOnTopHUD.transform.SetAsFirstSibling();
                    }
                }

                _canvasOnTopPopup = MyUtilities.FindObjectInFirstLayer(_canvasOnTop.gameObject, "Popups");
                if (_canvasOnTopPopup == null)
                {
                    _canvasOnTopPopup = new GameObject("Popups");

                    RectTransform rect = _canvasOnTopPopup.AddComponent<RectTransform>();
                    MyUtilities.Anchor(ref rect, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);

                    _canvasOnTopPopup.AddComponent<CanvasRenderer>();

                    _canvasOnTopPopup.transform.SetParent(_canvasOnTop.transform, false);
                }
                if (_canvasOnTopPopup != null)
                {
                    _canvasOnTopPopup.transform.SetAsLastSibling();
                }

                _canvasOnTopFloatPopup = MyUtilities.FindObjectInFirstLayer(_canvasOnTop.gameObject, "FloatPopups");
                if (_canvasOnTopFloatPopup == null)
                {
                    _canvasOnTopFloatPopup = new GameObject("FloatPopups");

                    RectTransform rect = _canvasOnTopFloatPopup.AddComponent<RectTransform>();
                    MyUtilities.Anchor(ref rect, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);

                    _canvasOnTopFloatPopup.AddComponent<CanvasRenderer>();

                    _canvasOnTopFloatPopup.transform.SetParent(_canvasOnTop.transform, false);
                }
                if (_canvasOnTopFloatPopup != null)
                {
                    _canvasOnTopFloatPopup.transform.SetAsLastSibling();
                }

                _canvasOnTopLoadingIndicator = MyUtilities.FindObjectInFirstLayer(_canvasOnTop.gameObject, "LoadingIndicator");
                if (_canvasOnTopLoadingIndicator == null)
                {
                    _canvasOnTopLoadingIndicator = new GameObject("LoadingIndicator");

                    RectTransform rect = _canvasOnTopLoadingIndicator.AddComponent<RectTransform>();
                    MyUtilities.Anchor(ref rect, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);

                    _canvasOnTopLoadingIndicator.AddComponent<CanvasRenderer>();

                    _canvasOnTopLoadingIndicator.transform.SetParent(_canvasOnTop.transform, false);
                }
                if (_canvasOnTopLoadingIndicator != null)
                {
                    _canvasOnTopLoadingIndicator.transform.SetAsLastSibling();
                }
            }

            if (_canvasSceneFading == null)
            {
                _canvasSceneFading = GameObject.Find("CanvasSceneFading");
                if (_canvasSceneFading == null)
                {
                    Debug.LogError("[" + typeof(MyUGUIManager).Name + "] OnUGUIInit(): Could not find \"CanvasSceneFading\" in scene \"" + SceneManager.GetActiveScene().name + "\". A template was created instead.");

                    _canvasSceneFading = new GameObject("CanvasSceneFading");
                    _canvasSceneFading.AddComponent<Canvas>();
                    _canvasSceneFading.AddComponent<CanvasScaler>();
                    _canvasSceneFading.AddComponent<GraphicRaycaster>();
                }
                Canvas canvas = _canvasSceneFading.GetComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 10000;
                DontDestroyOnLoad(_canvasSceneFading);
            }
        }

        /// <summary>
        /// Init camera.
        /// </summary>
        private void _InitCamera()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#FF7777FF>_InitCamera()</color>");
#endif

            if (_cameraUI == null)
            {
                GameObject uiCamera = MyUtilities.FindObjectInRoot("UICamera");
                if (uiCamera == null)
                {
                    uiCamera = new GameObject("UICamera");
                    uiCamera.AddComponent<Camera>();
                    uiCamera.transform.localPosition = new Vector3(0, 1, -10);

                    _cameraUI = uiCamera.GetComponent<Camera>();
                    _cameraUI.clearFlags = CameraClearFlags.Nothing;
                    _cameraUI.cullingMask |= LayerMask.GetMask("UI");

                    if (_canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                    {
                        _cameraUI.gameObject.SetActive(false);
                    }
                }
                else
                {
                    _cameraUI = uiCamera.GetComponent<Camera>();
                }

                if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
                {
                    _canvas.worldCamera = _cameraUI;

                    CanvasScaler canvasScaler = _canvas.GetComponent<CanvasScaler>();
                    _cameraUI.transform.position = new Vector3(canvasScaler.referenceResolution.x / 2, canvasScaler.referenceResolution.y / 2, -_canvas.planeDistance);
                }
            }
        }

        /// <summary>
        /// Init scene fading.
        /// </summary>
        private void _InitSceneFading()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#FF7777FF>_InitSceneFading()</color>");
#endif

            if (_currentSceneFading == null || _currentSceneFading.GameObject == null)
            {
                _currentSceneFading = new MyUGUISceneFading();
                _currentSceneFading.GameObject = MyUtilities.FindObjectInFirstLayer(_canvasOnTop.gameObject, MyUGUISceneFading.PREFAB_NAME);

                if (_currentSceneFading.GameObject == null)
                {
                    if (_coreAssetBundleConfig != null && !string.IsNullOrEmpty(_coreAssetBundleConfig.URL))
                    {
                        AssetBundle bundle = MyAssetBundleManager.Get(_coreAssetBundleConfig.URL, _coreAssetBundleConfig.Version);
                        if (bundle == null)
                        {
                            Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _InitSceneFading(): Could not get asset bundle which contains Scene Fading. A template was created instead.");
                            _currentSceneFading.GameObject = MyUGUISceneFading.CreateTemplate();
                        }
                        else
                        {
                            _currentSceneFading.GameObject = Instantiate(bundle.LoadAsset(MyUGUISceneFading.PREFAB_NAME) as GameObject);
                        }
                    }
                    else
                    {
                        _currentSceneFading.GameObject = Instantiate(Resources.Load(SPECIALITY_DIRECTORY + MyUGUISceneFading.PREFAB_NAME) as GameObject);
                    }
                }

                _currentSceneFading.GameObject.name = MyUGUISceneFading.PREFAB_NAME;
                _currentSceneFading.Transform.SetParent(_canvasSceneFading.transform, false);
                _currentSceneFading.TurnOnFading();
            }
        }

        /// <summary>
        /// Init popup overlay.
        /// </summary>
        private void _InitPopupOverlay()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#FF7777FF>_InitPopupOverlay()</color>");
#endif

            if (_currentPopupOverlay == null || _currentPopupOverlay.GameObject == null)
            {
                _currentPopupOverlay = new MyUGUIPopupOverlay();
                _currentPopupOverlay.GameObject = MyUtilities.FindObjectInFirstLayer(_canvasOnTopPopup, MyUGUIPopupOverlay.PREFAB_NAME);

                if (_currentPopupOverlay.GameObject == null)
                {
                    if (_coreAssetBundleConfig != null && !string.IsNullOrEmpty(_coreAssetBundleConfig.URL))
                    {
                        AssetBundle bundle = MyAssetBundleManager.Get(_coreAssetBundleConfig.URL, _coreAssetBundleConfig.Version);
                        if (bundle == null)
                        {
                            Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _InitPopupOverlay(): Could not get asset bundle which contains Popup Overlay. A template was created instead.");
                            _currentPopupOverlay.GameObject = MyUGUIPopupOverlay.CreateTemplate();
                        }
                        else
                        {
                            _currentPopupOverlay.GameObject = Instantiate(bundle.LoadAsset(MyUGUIPopupOverlay.PREFAB_NAME) as GameObject);
                        }
                    }
                    else
                    {
                        _currentPopupOverlay.GameObject = Instantiate(Resources.Load(SPECIALITY_DIRECTORY + MyUGUIPopupOverlay.PREFAB_NAME) as GameObject);
                    }
                }

                _currentPopupOverlay.GameObject.name = MyUGUIPopupOverlay.PREFAB_NAME;
                _currentPopupOverlay.GameObject.SetActive(false);
                _currentPopupOverlay.Transform.SetParent(_canvasOnTopPopup.transform, false);
            }
        }

        /// <summary>
        /// Init loading indicator.
        /// </summary>
        private void _InitLoadingIndicator()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#FF7777FF>_InitLoadingIndicator()</color>");
#endif

            if (_currentLoadingIndicator == null || _currentLoadingIndicator.GameObject == null)
            {
                _currentLoadingIndicator = new MyUGUILoadingIndicator();
                GameObject go = MyUtilities.FindObjectInFirstLayer(_canvasOnTopLoadingIndicator, MyUGUILoadingIndicator.PREFAB_NAME);

                if (_currentLoadingIndicator.GameObject == null)
                {
                    if (_coreAssetBundleConfig != null && !string.IsNullOrEmpty(_coreAssetBundleConfig.URL))
                    {
                        AssetBundle bundle = MyAssetBundleManager.Get(_coreAssetBundleConfig.URL, _coreAssetBundleConfig.Version);
                        if (bundle == null)
                        {
                            Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _InitLoadingIndicator(): Could not get asset bundle which contains Loading Indicator. A template was created instead.");
                            go = MyUGUILoadingIndicator.CreateTemplate();
                        }
                        else
                        {
                            go = Instantiate(bundle.LoadAsset(MyUGUILoadingIndicator.PREFAB_NAME) as GameObject);
                        }
                    }
                    else
                    {
                        go = Instantiate(Resources.Load(SPECIALITY_DIRECTORY + MyUGUILoadingIndicator.PREFAB_NAME) as GameObject);
                    }
                }

                _currentLoadingIndicator.Initialize(go);
                _currentLoadingIndicator.GameObject.name = MyUGUILoadingIndicator.PREFAB_NAME;
                _currentLoadingIndicator.GameObject.SetActive(false);
                _currentLoadingIndicator.Transform.SetParent(_canvasOnTopLoadingIndicator.transform, false);
            }
        }

        /// <summary>
        /// Init flying message.
        /// </summary>
        private void _InitFlyingMessage()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#FF7777FF>_InitFlyingMessage()</color>");
#endif

            _currentFlyingMessage = null;

            for (int i = 0, count = _listFlyingMessage.Count; i < count; ++i)
            {
                if (!_listFlyingMessage[i].IsPlaying)
                {
                    if (_currentFlyingMessage == null)
                    {
                        _currentFlyingMessage = _listFlyingMessage[i];
                        _currentFlyingMessage.GameObject.SetActive(true);
                    }
                    else
                    {
                        _listFlyingMessage[i].GameObject.SetActive(false);
                    }
                }
            }

            if (_currentFlyingMessage == null)
            {
                _currentFlyingMessage = new MyUGUIFlyingMessage();

                if (_coreAssetBundleConfig != null && !string.IsNullOrEmpty(_coreAssetBundleConfig.URL))
                {
                    AssetBundle bundle = MyAssetBundleManager.Get(_coreAssetBundleConfig.URL, _coreAssetBundleConfig.Version);
                    if (bundle == null)
                    {
                        Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _InitFlyingMessage(): Could not get asset bundle which contains Flying Message. A template was created instead.");
                        _currentFlyingMessage.GameObject = MyUGUIFlyingMessage.CreateTemplate();
                    }
                    else
                    {
                        _currentFlyingMessage.GameObject = Instantiate(bundle.LoadAsset(MyUGUIFlyingMessage.PREFAB_NAME) as GameObject);
                    }
                }
                else
                {
                    _currentFlyingMessage.GameObject = Instantiate(Resources.Load(SPECIALITY_DIRECTORY + MyUGUIFlyingMessage.PREFAB_NAME) as GameObject);
                }

                _currentFlyingMessage.GameObject.name = MyUGUIFlyingMessage.PREFAB_NAME;
                _currentFlyingMessage.Transform.SetParent(_canvasOnTop.transform, false);

                _listFlyingMessage.Add(_currentFlyingMessage);
            }

            _currentFlyingMessage.Transform.SetAsLastSibling();
        }

        /// <summary>
        /// Init running message.
        /// </summary>
        private void _InitRunningMessage(MyUGUIRunningMessage.EType type = MyUGUIRunningMessage.EType.Default)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#FF7777FF>_InitRunningMessage()</color>: type=" + type);
#endif

            if (_currentRunningMessage != null && _currentRunningMessage.GameObject != null && _currentRunningMessage.Type != type)
            {
                _currentRunningMessage.HideImmedialy();
                _currentRunningMessage = null;
                for (int i = 0, count = _listRunningMessage.Count; i < count; i++)
                {
                    if (_listRunningMessage[i].Type == type)
                    {
                        _currentRunningMessage = _listRunningMessage[i];
                        break;
                    }
                }
            }

            if (_currentRunningMessage == null || _currentRunningMessage.GameObject == null)
            {
                _currentRunningMessage = new MyUGUIRunningMessage(type);
                _currentRunningMessage.GameObject = MyUtilities.FindObjectInFirstLayer(_canvasOnTop.gameObject, MyUGUIRunningMessage.GetGameObjectName(type));

                if (_currentRunningMessage.GameObject == null)
                {
                    if (_coreAssetBundleConfig != null && !string.IsNullOrEmpty(_coreAssetBundleConfig.URL))
                    {
                        AssetBundle bundle = MyAssetBundleManager.Get(_coreAssetBundleConfig.URL, _coreAssetBundleConfig.Version);
                        if (bundle == null)
                        {
                            Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _InitRunningMessage(): Could not get asset bundle which contains Running Message. A template was created instead.");
                            _currentRunningMessage.GameObject = MyUGUIRunningMessage.CreateTemplate(type);
                        }
                        else
                        {
                            _currentRunningMessage.GameObject = Instantiate(bundle.LoadAsset(MyUGUIRunningMessage.GetGameObjectName(type)) as GameObject);
                        }
                    }
                    else
                    {
                        _currentRunningMessage.GameObject = Instantiate(Resources.Load(SPECIALITY_DIRECTORY + MyUGUIRunningMessage.GetGameObjectName(type)) as GameObject);
                    }
                }

                _currentRunningMessage.GameObject.name = MyUGUIRunningMessage.GetGameObjectName(type);
                _currentRunningMessage.GameObject.SetActive(false);
                _currentRunningMessage.Transform.SetParent(_canvasOnTop.transform, false);

                _listRunningMessage.Add(_currentRunningMessage);
            }

            _currentRunningMessage.Transform.SetAsLastSibling();
        }

        /// <summary>
        /// Init toast message.
        /// </summary>
        private void _InitToastMessage()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#FF7777FF>_InitToastMessage()</color>");
#endif

            if (_currentToastMessage == null || _currentToastMessage.GameObject == null)
            {
                _currentToastMessage = new MyUGUIToastMessage();
                _currentToastMessage.GameObject = MyUtilities.FindObjectInFirstLayer(_canvasOnTop.gameObject, MyUGUIToastMessage.PREFAB_NAME);

                if (_currentToastMessage.GameObject == null)
                {
                    if (_coreAssetBundleConfig != null && !string.IsNullOrEmpty(_coreAssetBundleConfig.URL))
                    {
                        AssetBundle bundle = MyAssetBundleManager.Get(_coreAssetBundleConfig.URL, _coreAssetBundleConfig.Version);
                        if (bundle == null)
                        {
                            Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _InitToastMessage(): Could not get asset bundle which contains Toast Message. A template was created instead.");
                            _currentToastMessage.GameObject = MyUGUIToastMessage.CreateTemplate();
                        }
                        else
                        {
                            _currentToastMessage.GameObject = Instantiate(bundle.LoadAsset(MyUGUIToastMessage.PREFAB_NAME) as GameObject);
                        }
                    }
                    else
                    {
                        _currentToastMessage.GameObject = Instantiate(Resources.Load(SPECIALITY_DIRECTORY + MyUGUIToastMessage.PREFAB_NAME) as GameObject);
                    }
                }

                _currentToastMessage.GameObject.name = MyUGUIToastMessage.PREFAB_NAME;
                _currentToastMessage.GameObject.SetActive(false);
                _currentToastMessage.Transform.SetParent(_canvasOnTop.transform, false);
            }

            _currentToastMessage.Transform.SetAsLastSibling();
        }

        /// <summary>
        /// Show a popup.
        /// </summary>
        private MyUGUIPopup _ShowPopup(EPopupID popupID, bool isRepeatable, object attachedData, Action<MyUGUIPopup> onEnterCallback, Action<MyUGUIPopup> onCloseCallback)
        {
            MyUGUIPopup popup = null;
            bool isReuse = false;

            for (int i = _listPopup.Count - 1; i >= 0; i--)
            {
                MyUGUIPopup tmpPopup = _listPopup[i];
                if (tmpPopup != null)
                {
                    if (!isRepeatable && popupID == tmpPopup.ID)
                    {
                        popup = tmpPopup;
                        isReuse = true;
                    }
                }
            }

            if (popup == null && _dictPopupConfig.ContainsKey(popupID))
            {
                MyUGUIConfigPopup popupConfig = _dictPopupConfig[popupID];
                popup = (MyUGUIPopup)Activator.CreateInstance(MyUtilities.FindTypesByName(popupConfig.ScriptName)[0], popupConfig.ID, popupConfig.PrefabName, false, isRepeatable);
                popup.SetPrefabName3D(popupConfig.PrefabName3D);
                if (popup != null)
                {
                    popup.SetAssetBundle(popupConfig.AssetBundleURL, popupConfig.AssetBundleVersion);
                }
            }

            if (popup == null)
            {
                Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _ShowPopup(): Could not create popup " + popupID + ". Please re-check popup config.");
            }
            else
            {
                _currentPopup = null;

                popup.AttachedData = attachedData;
                popup.OnEnterCallback = onEnterCallback;
                popup.OnCloseCallback = onCloseCallback;
                popup.State = popup.State == EBaseState.Idle && popup.IsLoaded ? EBaseState.Enter : EBaseState.LoadAssetBundle;

                _UpdatePopup(ref popup, 0f);

                if (!isReuse)
                {
                    _listPopup.Add(popup);
                }
            }

            return popup;
        }

        /// <summary>
        /// Show a float popup.
        /// </summary>
        private MyUGUIPopup _ShowFloatPopup(EPopupID popupID, bool isRepeatable, object attachedData)
        {
            MyUGUIPopup popup = null;

            if (_dictPopupConfig.ContainsKey(popupID))
            {
                MyUGUIConfigPopup popupConfig = _dictPopupConfig[popupID];
                popup = (MyUGUIPopup)Activator.CreateInstance(MyUtilities.FindTypesByName(popupConfig.ScriptName)[0], popupConfig.ID, popupConfig.PrefabName, true, isRepeatable);
                popup.SetPrefabName3D(popupConfig.PrefabName3D);
                if (popup != null)
                {
                    popup.SetAssetBundle(popupConfig.AssetBundleURL, popupConfig.AssetBundleVersion);
                }
            }

            if (popup == null)
            {
                Debug.LogError("[" + typeof(MyUGUIManager).Name + "] _ShowFloatPopup(): Could not create float popup " + popupID + ". Please re-check popup config.");
            }
            else
            {
                if (!popup.IsRepeatable)
                {
                    _currentFloatPopup = null;
                }

                popup.AttachedData = attachedData;
                popup.State = EBaseState.LoadAssetBundle;

                _UpdateFloatPopup(ref popup, 0f);

                _listFloatPopup.Add(popup);
            }

            return popup;
        }

        /// <summary>
        /// Update unity scene.
        /// </summary>
        private void _UpdateUnityScene()
        {
            if (_currentUnityScene == null)
            {
                return;
            }

#if DEBUG_MY_UI
            if (mCurrentUnityScene.State != EUnitySceneState.Update && mCurrentUnityScene.State != EUnitySceneState.Idle)
            {
                Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#FFFF00FF>_UpdateUnityScene()</color>: " + mCurrentUnityScene.Name + " - " + mCurrentUnityScene.State);
            }
#endif

            switch (_currentUnityScene.State)
            {
                case EUnitySceneState.Unload:
                    {
                        if (_currentSceneFading == null || !_currentSceneFading.IsFading)
                        {
                            _currentPopupOverlay = null;
                            _currentRunningMessage = null;
                            _currentToastMessage = null;

                            if (_currentUnityScene.HUD != null && _currentUnityScene.HUD.IsLoaded)
                            {
                                _currentUnityScene.HUD.State = EBaseState.Exit;
                            }

                            MyUGUIBase scene;
                            int countScene = _currentUnityScene.ListScene.Count;
                            for (int i = 0; i < countScene; i++)
                            {
                                scene = _currentUnityScene.ListScene[i];
                                if (scene != null)
                                {
                                    scene.OnUGUIDestroy();
                                }
                            }

                            _listPopup.Clear();
                            _listRunningMessage.Clear();
                            _listFlyingMessage.Clear();

                            _unitySceneUnloadUnusedAsset = Resources.UnloadUnusedAssets();

                            _currentUnityScene.State = EUnitySceneState.Unloading;
                        }
                    }
                    break;
                case EUnitySceneState.Unloading:
                    {
                        if (_unitySceneUnloadUnusedAsset == null || _unitySceneUnloadUnusedAsset.isDone)
                        {
                            MyUtilities.StartThread(_CollectGC, null);

                            _currentUnityScene = _nextUnityScene;
                            _currentUnityScene.State = EUnitySceneState.Load;
                            _UpdateUnityScene();
                        }
                    }
                    break;
                case EUnitySceneState.Load:
                    {
                        if (_coreAssetBundleConfig != null && !string.IsNullOrEmpty(_coreAssetBundleConfig.URL))
                        {
                            MyAssetBundleManager.Load(_coreAssetBundleConfig.URL, _coreAssetBundleConfig.Version, null, MyAssetBundleManager.ECacheMode.UnremovableCache);
                        }

                        if (_currentUnityScene.Name != SceneManager.GetActiveScene().name)
                        {
                            _unitySceneLoad = SceneManager.LoadSceneAsync(_currentUnityScene.Name);
                        }

                        _currentUnityScene.State = EUnitySceneState.Loading;
                        _UpdateUnityScene();
                    }
                    break;
                case EUnitySceneState.Loading:
                    {
                        if (_unitySceneLoad == null || _unitySceneLoad.isDone)
                        {
                            _InitCanvas();
                            _InitCamera();
                            _InitSceneFading();
                            _InitPopupOverlay();

                            _previousInitSceneIndex = 0;
                            _currentUnityScene.State = EUnitySceneState.PosLoad;
                        }
                    }
                    break;
                case EUnitySceneState.PosLoad:
                    {
                        int INIT_PER_FRAME = 2;

                        int countScene = _currentUnityScene.ListScene.Count;
                        int countInit = 0;

                        while (countInit < INIT_PER_FRAME && _previousInitSceneIndex < countScene)
                        {
                            if (_previousInitSceneIndex < countScene && _currentUnityScene.ListScene[_previousInitSceneIndex].IsInitWhenLoadUnityScene)
                            {
#if DEBUG_MY_UI
                                Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#FFFF00FF>_UpdateUnityScene()</color>: Init " + mCurrentUnityScene.ListScene[mPreviousInitSceneIndex].ID);
#endif

                                _currentUnityScene.ListScene[_previousInitSceneIndex].OnUGUIInit();
                                countInit++;
                            }

                            _previousInitSceneIndex++;
                        }

                        if (_previousInitSceneIndex >= countScene)
                        {
                            if (_nextScene != null)
                            {
                                _currentScene = _nextScene;
                                _currentScene.State = EBaseState.LoadAssetBundle;
                            }

                            _currentUnityScene.State = EUnitySceneState.Update;
                        }
                    }
                    break;
                case EUnitySceneState.Update:
                    {
                        if (_nextUnityScene != _currentUnityScene)
                        {
                            _currentUnityScene.State = EUnitySceneState.Unload;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Update scene.
        /// </summary>
        private void _UpdateScene(float deltaTime)
        {
            if (_currentScene == null)
            {
                return;
            }

#if DEBUG_MY_UI
            if (mCurrentScene.State != EBaseState.Update && mCurrentScene.State != EBaseState.Idle)
            {
                Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#00FF00FF>_UpdateScene()</color>: " + mCurrentScene.ID + " - " + mCurrentScene.State);
            }
#endif

            switch (_currentScene.State)
            {
                case EBaseState.LoadAssetBundle:
                    {
                        if (_currentScene.OnUGUILoadAssetBundle())
                        {
                            _currentScene.State = EBaseState.Init;
                            _UpdateScene(deltaTime);
                        }

                        if (_currentUnityScene.HUD != null)
                        {
                            if ((_currentUnityScene.HUD.GameObject == null) || (!_currentUnityScene.HUD.IsLoaded && !_currentScene.IsHideHUD))
                            {
                                _currentUnityScene.HUD.State = EBaseState.LoadAssetBundle;
                            }
                        }
                    }
                    break;
                case EBaseState.Init:
                    {
                        if (!_currentScene.IsLoaded)
                        {
                            _currentScene.OnUGUIInit();
                            _currentScene.State = EBaseState.Enter;
                        }
                        else
                        {
                            _currentScene.State = EBaseState.Enter;
                            _UpdateScene(deltaTime);
                        }
                    }
                    break;
                case EBaseState.Enter:
                    {
                        HideAllPopups(true, false);

                        if (_previousScene != null)
                        {
                            if (_previousScene.GameObject != null)
                            {
                                _previousScene.GameObject.SetActive(false);
                            }
                            if (_previousScene.GameObject3D != null)
                            {
                                _previousScene.GameObject3D.SetActive(false);
                            }
                        }

                        if (_onScenePreEnterCallback != null)
                        {
                            _onScenePreEnterCallback();
                            _onScenePreEnterCallback = null;
                        }

                        _currentScene.OnUGUIEnter();

                        if (_onScenePostEnterCallback != null)
                        {
                            _onScenePostEnterCallback();
                            _onScenePostEnterCallback = null;
                        }

                        if (_currentUnityScene.HUD != null && _currentUnityScene.HUD.IsLoaded)
                        {
                            if (_currentUnityScene.HUD.State != EBaseState.Invisible)
                            {
                                _currentUnityScene.HUD.OnUGUISceneSwitch(_currentScene);
                            }
                            else if (!_currentScene.IsHideHUD)
                            {
                                _currentUnityScene.HUD.State = EBaseState.Enter;
                            }
                        }

                        _currentScene.State = EBaseState.Visible;
                    }
                    break;
                case EBaseState.Visible:
                    {
                        if (_currentScene.OnUGUIVisible())
                        {
                            if (_onScenePostVisibleCallback != null)
                            {
                                _onScenePostVisibleCallback();
                                _onScenePostVisibleCallback = null;
                            }

                            _currentScene.State = EBaseState.Update;
                            _UpdateScene(deltaTime);
                        }
                    }
                    break;
                case EBaseState.Update:
                    {
                        if (_nextScene != _currentScene)
                        {
                            _currentScene.State = EBaseState.Exit;
                            _UpdateScene(deltaTime);
                        }
                        else
                        {
                            _currentScene.OnUGUIUpdate(deltaTime);

#if !UNITY_IOS
                            if (_currentPopup == null && Input.GetKeyDown(KeyCode.Escape))
                            {
#if DEBUG_MY_UI
                                Debug.Log("[" + mCurrentScene + "] <color=#00FF00FF>OnUGUIBackKey()</color>");
#endif
                                _currentScene.OnUGUIBackKey();
                            }
#endif
                        }
                    }
                    break;
                case EBaseState.Exit:
                    {
                        _currentScene.OnUGUIExit();
                        _currentScene.State = EBaseState.Invisible;

                        if (_currentUnityScene.HUD != null && _currentUnityScene.HUD.IsLoaded)
                        {
                            if (_nextScene != null && _nextScene.IsHideHUD)
                            {
                                _currentUnityScene.HUD.State = EBaseState.Exit;
                            }
                        }
                    }
                    break;
                case EBaseState.Invisible:
                    {
                        if (_currentScene.OnUGUIInvisible())
                        {
                            if (_nextScene == null && _currentScene.GameObject != null)
                            {
                                _currentScene.GameObject.SetActive(false);
                                if (_currentScene.GameObject3D != null)
                                {
                                    _currentScene.GameObject3D.SetActive(false);
                                }
                            }

                            if (_isHideRunningMessageWhenSwitchingScene)
                            {
                                HideRunningMessage();
                                _isHideRunningMessageWhenSwitchingScene = false;
                            }

                            if (_isHideToastWhenSwitchingScene)
                            {
                                HideToast();
                                _isHideToastWhenSwitchingScene = false;
                            }

                            _previousScene = _currentScene;
                            _currentScene = _nextScene;
                            _currentScene.State = EBaseState.LoadAssetBundle;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Update HUD.
        /// </summary>
        private void _UpdateHUD(float deltaTime)
        {
            if (_currentUnityScene == null || _currentUnityScene.HUD == null)
            {
                return;
            }

            if (_currentScene == null || _currentScene.IsHideHUD)
            {
                return;
            }

#if DEBUG_MY_UI
            if (mCurrentUnityScene.HUD.State != EBaseState.Update && mCurrentUnityScene.HUD.State != EBaseState.Idle)
            {
                Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#00FF00FF>_UpdateHUD()</color>: " + " - " + mCurrentUnityScene.HUD.State);
            }
#endif

            switch (_currentUnityScene.HUD.State)
            {
                case EBaseState.LoadAssetBundle:
                    {
                        if (_currentUnityScene.HUD.OnUGUILoadAssetBundle())
                        {
                            _currentUnityScene.HUD.State = EBaseState.Init;
                            _UpdateHUD(deltaTime);
                        }
                    }
                    break;
                case EBaseState.Init:
                    {
                        if (_currentUnityScene.HUD.GameObject == null || !_currentUnityScene.HUD.IsLoaded)
                        {
                            _currentUnityScene.HUD.OnUGUIInit();
                            _currentUnityScene.HUD.State = EBaseState.Enter;
                        }
                        else
                        {
                            _currentUnityScene.HUD.State = EBaseState.Enter;
                            _UpdateHUD(deltaTime);
                        }
                    }
                    break;
                case EBaseState.Enter:
                    {
                        _currentUnityScene.HUD.OnUGUIEnter();
                        _currentUnityScene.HUD.OnUGUIVisible();
                        _currentUnityScene.HUD.State = EBaseState.Update;
                    }
                    break;
                case EBaseState.Update:
                    {
                        if (_currentScene != null && !_currentScene.IsHideHUD)
                        {
                            _currentUnityScene.HUD.OnUGUIUpdate(deltaTime);
                        }
                    }
                    break;
                case EBaseState.Exit:
                    {
                        _currentUnityScene.HUD.OnUGUIExit();
                        _currentUnityScene.HUD.State = EBaseState.Invisible;
                        _currentUnityScene.HUD.OnUGUIInvisible();
                    }
                    break;
            }
        }

        /// <summary>
        /// Update popups.
        /// </summary>
        private void _UpdatePopup(ref MyUGUIPopup popup, float deltaTime)
        {
            if (popup == null)
            {
                return;
            }

#if DEBUG_MY_UI
            if (popup.State != EBaseState.Update && popup.State != EBaseState.Idle)
            {
                Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#00FF00FF>_UpdatePopup()</color>: " + popup.ID + " - " + popup.State);
            }
#endif

            switch (popup.State)
            {
                case EBaseState.LoadAssetBundle:
                    {
                        if (popup.OnUGUILoadAssetBundle())
                        {
                            popup.State = EBaseState.Init;
                            _UpdatePopup(ref popup, deltaTime);
                        }
                    }
                    break;
                case EBaseState.Init:
                    {
                        if (!popup.IsLoaded)
                        {
                            popup.OnUGUIInit();
                        }
                        popup.State = EBaseState.Enter;
                    }
                    break;
                case EBaseState.Enter:
                    {
                        _currentPopup = popup;

                        popup.OnUGUIEnter();
                        popup.State = EBaseState.Visible;

                        UpdatePopupOverlay();

                        if (_currentUnityScene.HUD != null && _currentUnityScene.HUD.IsLoaded)
                        {
                            if (_currentUnityScene.HUD.State != EBaseState.Invisible)
                            {
                                _currentUnityScene.HUD.OnUGUIPopupShow(_currentPopup);
                            }
                        }
                    }
                    break;
                case EBaseState.Visible:
                    {
                        if (popup.OnUGUIVisible())
                        {
                            popup.State = EBaseState.Update;
                            _UpdatePopup(ref popup, deltaTime);
                        }
                    }
                    break;
                case EBaseState.Update:
                    {
                        popup.OnUGUIUpdate(deltaTime);

#if !UNITY_IOS
                        if (popup == _currentPopup && Input.GetKeyDown(KeyCode.Escape))
                        {
#if DEBUG_MY_UI
                            Debug.Log("[" + mCurrentPopup + "] <color=#00FF00FF>OnUGUIBackKey()</color>");
#endif
                            _currentPopup.OnUGUIBackKey();
                        }
#endif
                    }
                    break;
                case EBaseState.Exit:
                    {
                        popup.OnUGUIExit();
                        popup.State = EBaseState.Invisible;
                    }
                    break;
                case EBaseState.Invisible:
                    {
                        if (popup.OnUGUIInvisible())
                        {
                            if (_currentPopup == popup)
                            {
                                _currentPopup = null;
                            }

                            if (popup.IsRetainable)
                            {
                                popup.State = EBaseState.Idle;

                                if (popup.GameObject != null)
                                {
                                    popup.GameObject.SetActive(false);
                                }
                                if (popup.GameObject3D != null)
                                {
                                    popup.GameObject3D.SetActive(false);
                                }
                            }
                            else
                            {
                                if (popup.GameObject != null)
                                {
                                    Destroy(popup.GameObject);
                                }
                                if (popup.GameObject3D != null)
                                {
                                    Destroy(popup.GameObject3D);
                                }

                                popup = null;
                            }
                        }

                        UpdatePopupOverlay();
                    }
                    break;
            }
        }

        /// <summary>
        /// Update float popups.
        /// </summary>
        private void _UpdateFloatPopup(ref MyUGUIPopup popup, float deltaTime)
        {
            if (popup == null)
            {
                return;
            }

#if DEBUG_MY_UI
            if (popup.State != EBaseState.Update && popup.State != EBaseState.Idle)
            {
                Debug.Log("[" + typeof(MyUGUIManager).Name + "] <color=#00FF00FF>_UpdateFloatPopup()</color>: " + popup.ID + " - " + popup.State);
            }
#endif

            switch (popup.State)
            {
                case EBaseState.Init:
                    {
                        if (!popup.IsLoaded)
                        {
                            popup.OnUGUIInit();
                        }
                        popup.State = EBaseState.Enter;
                    }
                    break;
                case EBaseState.Enter:
                    {
                        _currentFloatPopup = popup;

                        popup.OnUGUIEnter();
                        popup.State = EBaseState.Visible;

                        UpdatePopupOverlay();
                    }
                    break;
                case EBaseState.Visible:
                    {
                        if (popup.OnUGUIVisible())
                        {
                            popup.State = EBaseState.Update;
                            _UpdatePopup(ref popup, deltaTime);
                        }
                    }
                    break;
                case EBaseState.Update:
                    {
                        popup.OnUGUIUpdate(deltaTime);
                    }
                    break;
                case EBaseState.Exit:
                    {
                        popup.OnUGUIExit();
                        popup.State = EBaseState.Invisible;
                    }
                    break;
                case EBaseState.Invisible:
                    {
                        if (popup.OnUGUIInvisible())
                        {
                            if (_currentFloatPopup == popup)
                            {
                                _currentFloatPopup = null;
                            }

                            if (popup.IsRetainable)
                            {
                                popup.State = EBaseState.Idle;

                                if (popup.GameObject != null)
                                {
                                    popup.GameObject.SetActive(false);
                                }
                                if (popup.GameObject3D != null)
                                {
                                    popup.GameObject3D.SetActive(false);
                                }
                            }
                            else
                            {
                                if (popup.GameObject != null)
                                {
                                    Destroy(popup.GameObject);
                                }
                                if (popup.GameObject3D != null)
                                {
                                    Destroy(popup.GameObject3D);
                                }

                                popup = null;
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Updates the popup overlay.
        /// </summary>
        /// <returns>The popup overlay.</returns>
        private IEnumerator _DoUpdatePopupOverlay()
        {
            if (IsShowingLoadingIndicator)
            {
                _currentPopupOverlay.Transform.SetParent(_canvasOnTopLoadingIndicator.transform, false);
                _currentPopupOverlay.Transform.SetAsFirstSibling();
                _currentPopupOverlay.Show();
            }
            else
            {
                _currentPopupOverlay.Transform.SetParent(_canvasOnTopPopup.transform, false);
                for (int i = _canvasOnTopPopup.transform.childCount - 1; i >= 0; i--)
                {
                    GameObject popup = _canvasOnTopPopup.transform.GetChild(i).gameObject;
                    if (popup.activeSelf && !popup.name.Equals(_currentPopupOverlay.GameObject.name))
                    {
                        _currentPopupOverlay.Transform.SetSiblingIndex(i - 1);
                        _currentPopupOverlay.Show();
                        break;
                    }
                }
            }

            yield return null;

            if (IsShowingLoadingIndicator)
            {
                _currentPopupOverlay.Transform.SetParent(_canvasOnTopLoadingIndicator.transform, false);
                _currentPopupOverlay.Transform.SetAsFirstSibling();
                _currentPopupOverlay.Show();
            }
            else if (_currentPopupOverlay != null)
            {
                _currentPopupOverlay.Transform.SetParent(_canvasOnTopPopup.transform, false);

                bool isHasActivedPopup = false;
                foreach (MyUGUIPopup popup in _listPopup)
                {
                    if (popup != null && popup.State != EBaseState.Init && popup.State != EBaseState.Idle)
                    {
                        isHasActivedPopup = true;
                        break;
                    }
                }
                if (isHasActivedPopup)
                {
                    for (int i = _canvasOnTopPopup.transform.childCount - 1; i >= 0; i--)
                    {
                        GameObject popup = _canvasOnTopPopup.transform.GetChild(i).gameObject;
                        if (popup.activeSelf && !popup.name.Equals(_currentPopupOverlay.GameObject.name))
                        {
                            if (i == 0)
                            {
                                _currentPopupOverlay.Transform.SetAsFirstSibling();
                            }
                            else
                            {
                                _currentPopupOverlay.Transform.SetSiblingIndex(i - 1);
                            }
                            _currentPopupOverlay.Show();
                            break;
                        }
                    }
                }
                else
                {
                    _currentPopupOverlay.Transform.SetAsFirstSibling();
                    _currentPopupOverlay.Hide();
                }
            }
        }

        /// <summary>
        /// Call GC.
        /// </summary>
        private void _CollectGC()
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] _CollectGC()");
#endif

            GC.Collect();
        }

        /// <summary>
        /// Add a new scene to scene stack.
        /// </summary>
        private void _AddSceneIntoSceneStack(MyUGUIScene scene)
        {
#if DEBUG_MY_UI
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] _AddSceneIntoSceneStack(): UnitySceneID=" + scene.UnitySceneID + " - SceneID=" + scene.ID);
#endif

#if DEBUG_MY_UI
            string stringScene = " ";
            for (int j = mListScene.Count - 1; j >= 0; j--)
            {
                stringScene = " " + mListScene[j].ID + stringScene;
            }
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] _AddSceneIntoSceneStack(): before={" + stringScene + "}");
#endif

            int countScene = _listScene.Count;
            for (int i = 0; i < countScene; i++)
            {
                if (_listScene[i].ID == scene.ID && _listScene[i].UnitySceneID == scene.UnitySceneID)
                {
                    for (int j = countScene - 1; j >= i; j--)
                    {
                        _listScene.RemoveAt(j);
                    }
                    break;
                }
            }
            _listScene.Add(scene);

#if DEBUG_MY_UI
            stringScene = " ";
            for (int j = mListScene.Count - 1; j >= 0; j--)
            {
                stringScene = " " + mListScene[j].ID + stringScene;
            }
            Debug.Log("[" + typeof(MyUGUIManager).Name + "] _AddSceneIntoSceneStack(): after={" + stringScene + "}");
#endif
        }

        #endregion
    }

    #region ----- Enumeration -----

    public enum EUnitySceneState
    {
        Idle,
        Unload,
        Unloading,
        Load,
        Loading,
        PosLoad,
        Update,
    }

    public enum EBaseState
    {
        Idle,
        LoadAssetBundle,
        Init,
        Enter,
        Visible,
        Update,
        Exit,
        Invisible
    }

    public enum ERunningMessageSpeed
    {
        VerySlow = 50,
        Slow = 100,
        Normal = 150,
        Fast = 200,
        VeryFast = 300
    }

    public enum EToastMessageDuration
    {
        Short = 2,
        Medium = 3,
        Long = 5
    }

    #endregion
}