/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyLocalizationConfig (version 3.2)
 */

using UnityEngine;
using System;

namespace MyClasses
{
    [Serializable]
    public class MyLocalizationConfig : ScriptableObject
    {
        public MyLocalizationManager.ELocation Location = MyLocalizationManager.ELocation.RESOURCES;
        public string PersistentPath = "/localization.csv";
        public string ResourcesPath = "Configs/localization";
        public MyLocalizationManager.EMode Mode = MyLocalizationManager.EMode.DEVICE_LANGUAGE_AND_CACHE;
        public MyLocalizationManager.ELanguage DefaultLanguage = MyLocalizationManager.ELanguage.English;
    }
}