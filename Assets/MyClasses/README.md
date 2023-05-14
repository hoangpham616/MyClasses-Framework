********** Compile Flag **********

Ads: DEBUG_MY_UNITY_ADS		USE_MY_UNITY_ADS

AssetBundles: DEBUG_MY_ASSET_BUNDLE

IAP: DEBUG_MY_IAP   USE_MY_IAP

Logger: DISABLE_MY_LOGGER_ALL   DISABLE_MY_LOGGER_INFO    DISABLE_MY_LOGGER_WARNING   DISABLE_MY_LOGGER_ERROR

Sound: DEBUG_MY_SOUND

UGUI: DEBUG_MY_UI   USE_MY_UI_TMPRO

**********************************





********** How to setup **********

1/ On Menu Bar, choose "MyClasses/UGUI/Config ID" to config UI ID

2/ On Menu Bar, choose "MyClasses/UGUI/Config Scenes" to config Unity Scenes, Scenes, HUDs

3/ On Menu Bar, choose "MyClasses/UGUI/Config Popups" to config Popups

4/ On Menu Bar, choose "MyClasses/UGUI/Create MyUGUIBooter" to create MyUGUIBooter

5/ On Hierarchy, choose "MyUGUIBooter" to config Boot Mode

6/ Click "Play" to start

***********************************





********** How to use **********

+ Create prefabs and store them at Resources\Prefabs\UGUI\

+ Genegrate scripts at Menu Bar -> MyClasses -> UGUI -> Create

+ Call methods from MyUGUIManager class:

  - MyUGUIManager.Instance.ShowUnityScene(EUnitySceneID.Main, ESceneID.Lobby)

  - MyUGUIManager.Instance.ShowScene(ESceneID.MainMenu)

  - MyUGUIManager.Instance.ShowPopup(EPopupID.Setting)

  - MyUGUIManager.Instance.ShowFloatPopup(EPopupID.BattleInvite)

  - MyUGUIManager.Instance.ShowLoadingIndicator(10, onTimeOutCallback)

  - MyUGUIManager.Instance.ShowFlyingMessage("This is Flying Message", MyUGUIFlyingMessage.EType.ShortFlyFromBot)

  - MyUGUIManager.Instance.ShowRunningMessage("This is Running Message", ERunningMessageSpeed.Normal)

  - MyUGUIManager.Instance.ShowToastMessage("This is Toast Message", EToastMessageDuration.Medium)

  - MyUGUIManager.Instance.Back()

 + Methods that support AssetBundles:

  - MyUGUIManager.Instance.SetAssetBundleForCore("url", 1)

  - MyUGUIManager.Instance.SetAssetBundleForHUDs("url", 1)

  - MyUGUIManager.Instance.SetAssetBundleForScene(ESceneID.Login, "url", 1)

  - MyUGUIManager.Instance.SetAssetBundleForPopup(EPopupID.Event, "url", 1)

********************************





********** How to custom UI **********

+ HUDs: "Resources\Prefabs\UGUI\HUDs\"

+ Scenes: "Resources\Prefabs\UGUI\Scenes\"

+ Popups: "Resources\Prefabs\UGUI\Popups\"

+ Popup Overlay, Loading Indicator, Toast Message, Flying Message, Running Message...: "Resources\Prefabs\UGUI\Specialities\"

**************************************