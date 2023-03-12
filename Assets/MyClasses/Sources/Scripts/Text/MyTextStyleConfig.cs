﻿/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyTextStyleConfig (version 1.0)
 */

using UnityEngine;
using System;
using MyClasses;

namespace MyClasses
{
    [Serializable]
    public class MyTextStyleConfig : ScriptableObject
    {
        public MyTextStyleManager.MyTextStyleInfo[] Infos;
    }
}