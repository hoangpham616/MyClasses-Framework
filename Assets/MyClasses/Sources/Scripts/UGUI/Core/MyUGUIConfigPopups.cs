﻿/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUGUIConfigPopups (version 2.0)
 */

using UnityEngine;
using System;
using System.Collections.Generic;

namespace MyClasses.UI
{
    [Serializable]
    public class MyUGUIConfigPopups : ScriptableObject
    {
        public int NumDefault;
        public List<MyUGUIConfigPopup> ListPopup = new List<MyUGUIConfigPopup>();
    }

    [Serializable]
    public class MyUGUIConfigPopup
    {
        public bool IsFoldOut;
        public EPopupID ID;
        public int ScriptPathIndex;
        public string ScriptPath;
        public string ScriptName;
        public int PrefabNameIndex;
        public string PrefabName;
        public string AssetBundleURL;
        public int AssetBundleVersion;
    }
}