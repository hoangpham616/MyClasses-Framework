/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUGUIConfigUnityScenes (version 2.1)
 */

using UnityEngine;
using System;
using System.Collections.Generic;

namespace MyClasses.UI
{
    [Serializable]
    public class MyUGUIConfigUnityScenes : ScriptableObject
    {
        public List<MyUGUIConfigUnityScene> ListUnityScene = new List<MyUGUIConfigUnityScene>();
    }

    [Serializable]
    public class MyUGUIConfigUnityScene
    {
        public bool IsFoldOut;
        public EUnitySceneID ID;
        public string SceneName;
        public int SceneNameIndex;
        public int HUDScriptPathIndex;
        public int HUDPrefabNameIndex;
        public int HUDPrefabNameIndex3D;
        public string HUDScriptPath;
        public string HUDScriptName;
        public string HUDPrefabName;
        public string HUDPrefabName3D;
        public List<MyUGUIConfigScene> ListScene;
    }

    [Serializable]
    public class MyUGUIConfigScene
    {
        public bool IsFoldOut;
        public ESceneID ID;
        public int ScriptPathIndex;
        public int PrefabNameIndex;
        public int PrefabNameIndex3D;
        public string ScriptPath;
        public string ScriptName;
        public string PrefabName;
        public string PrefabName3D;
        public string AssetBundleURL;
        public int AssetBundleVersion;
        public bool IsInitWhenLoadUnityScene;
        public bool IsHideHUD;
        public float FadeInDuration;
        public float FadeOutDuration;
    }
}