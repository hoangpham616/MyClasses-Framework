/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUGUISampleHUD (version 2.25)
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using MyClasses;
using MyClasses.UI;

namespace MyApp
{
    public class MyUGUISampleHUD : MyUGUIHUD
    {
        #region ----- Variable -----

        // private MyUGUIButton _btnBack;

        #endregion

        #region ----- Constructor -----

        public MyUGUISampleHUD(string prefabName)
            : base(prefabName)
        {
        }

        #endregion

        #region ----- MyUGUIHUD Implementation -----

        public override void OnUGUIInit()
        {
            this.LogInfo("OnUGUIInit", null, ELogColor.DARK_UI);

            base.OnUGUIInit();

            // _btnBack = MyUtilities.FindObject(GameObject, "Something/Something/ButtonBack").GetComponent<MyUGUIButton>();
        }

        public override void OnUGUIEnter()
        {
            this.LogInfo("OnUGUIEnter", null, ELogColor.DARK_UI);
            
            base.OnUGUIEnter();

            // _btnBack.OnEventPointerClick.AddListener(_OnClickBack);
        }

        public override void OnUGUIUpdate(float deltaTime)
        {
        }

        public override void OnUGUIExit()
        {
            this.LogInfo("OnUGUIExit", null, ELogColor.DARK_UI);
            
            base.OnUGUIExit();

            // _btnBack.OnEventPointerClick.RemoveAllListeners();
        }

        public override void OnUGUISceneSwitch(MyUGUIScene scene)
        {
            this.LogInfo("OnUGUISceneSwitch", null, ELogColor.DARK_UI);
            
            switch (scene.ID)
            {
                default:
                    {

                    }
                    break;
            }
        }

        public override void OnUGUIPopupShow(MyUGUIPopup popup)
        {
            this.LogInfo("OnUGUIPopupShow", null, ELogColor.DARK_UI);
            
            switch (popup.ID)
            {
                default:
                    {

                    }
                    break;
            }
        }

        #endregion

        #region ----- Button Event -----

        private void _OnClickBack(PointerEventData arg0)
        {
            this.LogInfo("_OnClickBack", null, ELogColor.UI);

            MyUGUIManager.Instance.Back();
        }

        #endregion

        #region ----- Public Method -----



        #endregion

        #region ----- Private Method -----



        #endregion
    }
}