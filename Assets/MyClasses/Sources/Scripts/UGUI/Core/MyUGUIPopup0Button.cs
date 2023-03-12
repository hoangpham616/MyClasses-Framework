﻿/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUGUIPopup0Button (version 2.13)
 */

#pragma warning disable 0114
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0649

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

#if USE_MY_UI_TMPRO
using TMPro;
#endif

namespace MyClasses.UI
{
    public class MyUGUIPopup0Button : MyUGUIPopup
    {
        #region ----- Variable -----

#if USE_MY_UI_TMPRO
        private TextMeshProUGUI mTitleTMPro;
        private TextMeshProUGUI mBodyTMPro;
#endif

        private Text mTitle;
        private Text mBody;
        private MyUGUIButton mButtonClose;
        private Action<object> mActionClose;

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyUGUIPopup0Button(EPopupID id, string prefabName, bool isFloat = false, bool isRepeatable = false)
            : base(id, prefabName, isFloat, isRepeatable)
        {
#if UNITY_EDITOR
            if (!_CheckPrefab())
            {
                _CreatePrefab();
            }
#endif
        }

        #endregion

        #region ----- MyUGUIPopup Implementation -----

        /// <summary>
        /// OnUGUIInit.
        /// </summary>
        public override void OnUGUIInit()
        {
            base.OnUGUIInit();

            GameObject container = MyUtilities.FindObjectInAllLayers(GameObject, "Container");

            GameObject title = MyUtilities.FindObjectInFirstLayer(container, "Title");
            if (title != null)
            {
                mTitle = title.GetComponent<Text>();
#if USE_MY_UI_TMPRO
                if (mTitle == null)
                {
                    mTitleTMPro = title.GetComponent<TextMeshProUGUI>();
                }
#endif
            }

            GameObject body = MyUtilities.FindObjectInFirstLayer(container, "Body");
            if (body != null)
            {
                mBody = body.GetComponent<Text>();
#if USE_MY_UI_TMPRO
                if (mBody == null)
                {
                    mBodyTMPro = body.GetComponent<TextMeshProUGUI>();
                }
#endif
            }

            GameObject close = MyUtilities.FindObjectInFirstLayer(container, "ButtonClose");
            if (close != null)
            {
                mButtonClose = close.GetComponent<MyUGUIButton>();
            }
        }

        /// <summary>
        /// OnEnter.
        /// </summary>
        public override void OnUGUIEnter()
        {
            base.OnUGUIEnter();

            if (mButtonClose != null)
            {
                mButtonClose.OnEventPointerClick.AddListener(_OnClickClose);
            }
        }

        /// <summary>
        /// OnUpdateUGUI.
        /// </summary>
        public override void OnUGUIUpdate(float deltaTime)
        {
            base.OnUGUIUpdate(deltaTime);
        }

        /// <summary>
        /// OnUGUIExit.
        /// </summary>
        public override void OnUGUIExit()
        {
            base.OnUGUIExit();

            if (mButtonClose != null)
            {
                mButtonClose.OnEventPointerClick.RemoveAllListeners();
            }

            mActionClose = null;
        }

        /// <summary>
        /// OnUGUIInvisible.
        /// </summary>
        public override bool OnUGUIInvisible()
        {
            return base.OnUGUIInvisible();
        }

        #endregion

        #region ----- Button Event -----

        /// <summary>
        /// Click on button close.
        /// </summary>
        private void _OnClickClose(PointerEventData arg0)
        {
            if (mActionClose != null)
            {
                mActionClose(AttachedData);
            }

            Hide();
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set data.
        /// </summary>
        public void SetData(string title, string body, Action<object> actionClose)
        {
            _SetData(title, body, true, actionClose);
        }

        /// <summary>
        /// Set data.
        /// </summary>
        public void SetData(string title, string body)
        {
            _SetData(title, body, false, null);
        }

        /// <summary>
        /// Set data.
        /// </summary>
        public void SetData(string body, Action<object> actionClose)
        {
            _SetData(string.Empty, body, true, actionClose);
        }

        /// <summary>
        /// Set data.
        /// </summary>
        public void SetData(string body, bool isShowCloseButton = false)
        {
            _SetData(string.Empty, body, isShowCloseButton, null);
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Set data.
        /// </summary>
        private void _SetData(string title, string body, bool isShowCloseButton, Action<object> actionClose)
        {
            if (mTitle != null)
            {
                mTitle.text = title;
            }
#if USE_MY_UI_TMPRO
            else if (mTitleTMPro != null)
            {
                mTitleTMPro.text = title;
            }
#endif

            if (mBody != null)
            {
                mBody.text = body;
            }
#if USE_MY_UI_TMPRO
            else if (mBodyTMPro != null)
            {
                mBodyTMPro.text = body;
            }
#endif

            if (mButtonClose != null)
            {
                mButtonClose.SetActive(isShowCloseButton);
            }
            mActionClose = actionClose;
        }

#if UNITY_EDITOR

        /// <summary>
        /// Check existence of prefab.
        /// </summary>
        private static bool _CheckPrefab(string subfixName = "")
        {
            string filePath = "Assets/Resources/" + MyUGUIManager.POPUP_DIRECTORY + "Dialog0ButtonPopup" + subfixName + ".prefab";
            return System.IO.File.Exists(filePath);
        }

        /// <summary>
        /// Create template prefab.
        /// </summary>
        private static void _CreatePrefab(string subfixName = "")
        {
            string prefabName = "Dialog0ButtonPopup" + subfixName;

            GameObject prefab = new GameObject(prefabName);

            Animator root_animator = prefab.AddComponent<Animator>();
            string[] paths = new string[] { "Assets/MyClasses", "Assets/Core/MyClasses", "Assets/Plugin/MyClasses", "Assets/Plugins/MyClasses", "Assets/Framework/MyClasses", "Assets/Frameworks/MyClasses" };
            for (int i = 0; i < paths.Length; i++)
            {
                if (System.IO.File.Exists(paths[i] + "/Sources/Animations/my_animator_dialog.controller"))
                {
                    root_animator.runtimeAnimatorController = (RuntimeAnimatorController)UnityEditor.AssetDatabase.LoadAssetAtPath(paths[i] + "/Sources/Animations/my_animator_dialog.controller", typeof(RuntimeAnimatorController));
                    Debug.LogError("[" + typeof(MyUGUIPopup0Button).Name + "] CreateTemplate(): please setup \"my_animator_dialog\" controller.");
                    Debug.LogError("[" + typeof(MyUGUIPopup0Button).Name + "] CreateTemplate(): mapping \"my_animation_dialog_show\" motion for \"Show\" state.");
                    Debug.LogError("[" + typeof(MyUGUIPopup0Button).Name + "] CreateTemplate(): mapping \"my_animation_dialog_hide\" motion for \"Hide\" state.");
                    break;
                }
            }

            RectTransform root_rect = prefab.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref root_rect, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);

            GameObject container = new GameObject("Container");
            container.transform.SetParent(prefab.transform, false);

            RectTransform container_rect = container.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref container_rect, MyUtilities.EAnchorPreset.MiddleCenter, MyUtilities.EAnchorPivot.MiddleCenter, 1000, 600, 0, 0);

            Image container_image = container.AddComponent<Image>();
            container_image.color = new Color(0.9f, 0.9f, 0.9f);
            container_image.raycastTarget = true;

            GameObject title = new GameObject("Title");
            title.transform.SetParent(container.transform, false);

            RectTransform title_rect = title.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref title_rect, MyUtilities.EAnchorPreset.TopCenter, MyUtilities.EAnchorPivot.TopCenter, 800, 100, 0, 0);

            Text title_text = title.AddComponent<Text>();
            title_text.text = "TITLE";
            title_text.color = Color.black;
            title_text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            title_text.fontSize = 50;
            title_text.fontStyle = FontStyle.Bold;
            title_text.alignment = TextAnchor.MiddleCenter;
            title_text.horizontalOverflow = HorizontalWrapMode.Wrap;
            title_text.verticalOverflow = VerticalWrapMode.Truncate;
            title_text.raycastTarget = false;

            GameObject body = new GameObject("Body");
            body.transform.SetParent(container.transform, false);

            RectTransform body_rect = body.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref body_rect, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, new Vector2(100, 120), new Vector2(-100, -120));

            Text body_text = body.AddComponent<Text>();
            body_text.text = "Body";
            body_text.color = Color.black;
            body_text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            body_text.fontSize = 40;
            body_text.alignment = TextAnchor.MiddleCenter;
            body_text.horizontalOverflow = HorizontalWrapMode.Wrap;
            body_text.verticalOverflow = VerticalWrapMode.Truncate;
            body_text.raycastTarget = false;

            GameObject buttonClose = new GameObject("ButtonClose");
            buttonClose.transform.SetParent(container.transform, false);

            RectTransform buttonClose_rect = buttonClose.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref buttonClose_rect, MyUtilities.EAnchorPreset.TopRight, MyUtilities.EAnchorPivot.TopRight, 80, 80, -10, -10);

            Image buttonClose_image = buttonClose.AddComponent<Image>();
            buttonClose_image.color = Color.red;
            buttonClose_image.raycastTarget = true;

            Button buttonClose_button = buttonClose.AddComponent<Button>();
            buttonClose_button.transition = Selectable.Transition.None;

            buttonClose.AddComponent<MyUGUIButton>();

            string folderPath = "Assets/Resources/" + MyUGUIManager.POPUP_DIRECTORY;
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }

            string filePath = folderPath + prefabName + ".prefab";
            UnityEditor.PrefabUtility.CreatePrefab(filePath, prefab, UnityEditor.ReplacePrefabOptions.ReplaceNameBased);

            GameObject.Destroy(prefab);
        }

#endif

        #endregion
    }
}