﻿/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUGUIToastMessage (version 2.14)
 */

#pragma warning disable 0114
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0649

using UnityEngine;
using UnityEngine.UI;

namespace MyClasses.UI
{
    public class MyUGUIToastMessage
    {
        #region ----- Define -----

        public const string PREFAB_NAME = "ToastMessage";

        #endregion

        #region ----- Variable -----

        private Text _text;

        private GameObject _gameObject;
        private Animator _animator;
        private MyTimer _timer;

        private int _numFrameNeedResize;

        #endregion

        #region ----- Property -----

        public GameObject GameObject
        {
            get { return _gameObject; }
            set { _gameObject = value; }
        }

        public Transform Transform
        {
            get { return _gameObject != null ? _gameObject.transform : null; }
        }

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyUGUIToastMessage()
        {
#if UNITY_EDITOR
            if (!_CheckPrefab())
            {
                _CreatePrefab();
            }
#endif
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Show.
        /// </summary>
        public void Show(string content, float duration)
        {
            if (_gameObject != null)
            {
                if (_text == null)
                {
                    _text = MyUtilities.FindObjectInAllLayers(_gameObject, "Text").GetComponent<Text>();
                    if (_text == null)
                    {
                        Debug.LogError("[" + typeof(MyUGUIToastMessage).Name + "] Show(): Could not find Text component.");
                        return;
                    }
                }

                if (_text.transform.parent != null)
                {
                    LayoutElement layoutElement = _text.transform.parent.GetComponent<LayoutElement>();
                    if (layoutElement != null)
                    {
                        layoutElement.preferredWidth = -1;
                    }
                }

                _timer = new MyTimer(duration);

                _gameObject.SetActive(true);

                _text.text = content;

                if (_animator == null)
                {
                    _animator = _gameObject.GetComponent<Animator>();
                }
                if (_animator != null)
                {
                    _animator.Play("Show");
                }

                _numFrameNeedResize = 3;
            }
        }

        /// <summary>
        /// Hide.
        /// </summary>
        public void Hide()
        {
            if (_gameObject != null)
            {
                _timer = null;

                if (_animator != null)
                {
                    _animator.Play("Hide");
                    return;
                }

                _gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Record the time has passed.
        /// </summary>
        public void LateUpdate(float dt)
        {
            if (_timer != null)
            {
                if (_numFrameNeedResize > 0)
                {
                    if (_gameObject != null && _text.transform.parent != null)
                    {
                        CanvasScaler canvasScaler = MyUGUIManager.Instance.CanvasOnTop.GetComponent<CanvasScaler>();
                        RectTransform rectTransform = _text.transform.parent.GetComponent<RectTransform>();
                        if (canvasScaler != null && rectTransform != null)
                        {
                            if (rectTransform.sizeDelta.x > 0)
                            {
                                float limitWidth = canvasScaler.referenceResolution.x * 0.95f;
                                if (rectTransform.sizeDelta.x > limitWidth)
                                {
                                    LayoutElement layoutElement = _text.transform.parent.GetComponent<LayoutElement>();
                                    if (layoutElement != null)
                                    {
                                        layoutElement.preferredWidth = limitWidth;
                                    }
                                }
                            }
                        }
                    }
                    _numFrameNeedResize--;
                }

                _timer.Update(dt);
                if (_timer.IsJustDone())
                {
                    Hide();
                }
            }
        }

        /// <summary>
        /// Create a template game object.
        /// </summary>
        public static GameObject CreateTemplate()
        {
            GameObject obj = new GameObject(PREFAB_NAME);

            RectTransform root_rect = obj.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref root_rect, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);

#if UNITY_EDITOR
            Animator root_animator = obj.AddComponent<Animator>();
            string[] paths = new string[] { "Assets/MyClasses", "Assets/Core/MyClasses", "Assets/Plugin/MyClasses", "Assets/Plugins/MyClasses", "Assets/Framework/MyClasses", "Assets/Frameworks/MyClasses" };
            for (int i = 0; i < paths.Length; i++)
            {
                if (System.IO.File.Exists(paths[i] + "/Sources/Animations/MyAnimatorToastMessage.controller"))
                {
                    root_animator.runtimeAnimatorController = (RuntimeAnimatorController)UnityEditor.AssetDatabase.LoadAssetAtPath(paths[i] + "/Sources/Animations/MyAnimatorToastMessage.controller", typeof(RuntimeAnimatorController));
                    Debug.LogError("[" + typeof(MyUGUIToastMessage).Name + "] CreateTemplate(): please setup \"MyAnimatorToastMessage\" controller.");
                    Debug.LogError("[" + typeof(MyUGUIToastMessage).Name + "] CreateTemplate(): mapping \"MyAnimationToastMessageShow\" motion for \"Show\" state.");
                    Debug.LogError("[" + typeof(MyUGUIToastMessage).Name + "] CreateTemplate(): mapping \"MyAnimationToastMessageIdle\" motion for \"Idle\" state.");
                    Debug.LogError("[" + typeof(MyUGUIToastMessage).Name + "] CreateTemplate(): mapping \"MyAnimationToastMessageHide\" motion for \"Hide\" state.");
                    break;
                }
            }
#endif

            GameObject container = new GameObject("Container");
            container.transform.SetParent(obj.transform, false);

            RectTransform container_rect = container.AddComponent<RectTransform>();
            MyUtilities.Anchor(ref container_rect, MyUtilities.EAnchorPreset.BottomCenter, MyUtilities.EAnchorPivot.BottomCenter, 500, 80, 0, 40);

            Image container_image = container.AddComponent<Image>();
            container_image.color = new Color(0, 0, 0, 0.5f);
            container_image.raycastTarget = false;

            LayoutElement container_layout_element = container.AddComponent<LayoutElement>();
            container_layout_element.minWidth = 80;
            container_layout_element.minHeight = 80;

            HorizontalLayoutGroup container_horizontal_layout_group = container.AddComponent<HorizontalLayoutGroup>();
            container_horizontal_layout_group.padding = new RectOffset(20, 20, 20, 20);
            container_horizontal_layout_group.childAlignment = TextAnchor.MiddleCenter;
            container_horizontal_layout_group.childControlWidth = true;
            container_horizontal_layout_group.childControlHeight = true;
            container_horizontal_layout_group.childForceExpandWidth = true;
            container_horizontal_layout_group.childForceExpandHeight = true;

            ContentSizeFitter container_content_size_filter = container.AddComponent<ContentSizeFitter>();
            container_content_size_filter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            container_content_size_filter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            CanvasGroup container_canvas_group = container.AddComponent<CanvasGroup>();
            container_canvas_group.alpha = 1;
            container_canvas_group.interactable = false;
            container_canvas_group.blocksRaycasts = false;

            GameObject text = new GameObject("Text");
            text.transform.SetParent(container.transform, false);

            Text text_text = text.AddComponent<Text>();
            text_text.text = "This is toast message";
            text_text.color = Color.white;
            text_text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text_text.fontSize = 50;
            text_text.alignment = TextAnchor.MiddleCenter;
            text_text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text_text.verticalOverflow = VerticalWrapMode.Overflow;
            text_text.raycastTarget = false;

            return obj;
        }

        #endregion

        #region ----- Private Method -----

#if UNITY_EDITOR

        /// <summary>
        /// Check existence of prefab.
        /// </summary>
        private static bool _CheckPrefab()
        {
            string filePath = "Assets/Resources/" + MyUGUIManager.SPECIALITY_DIRECTORY + PREFAB_NAME + ".prefab";
            return System.IO.File.Exists(filePath);
        }

        /// <summary>
        /// Create template prefab.
        /// </summary>
        private static void _CreatePrefab()
        {
            Debug.Log("[" + typeof(MyUGUIToastMessage).Name + "] CreatePrefab(): a template prefab was created.");

            GameObject prefab = CreateTemplate();

            string folderPath = "Assets/Resources/" + MyUGUIManager.SPECIALITY_DIRECTORY;
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }

            string filePath = "Assets/Resources/" + MyUGUIManager.SPECIALITY_DIRECTORY + PREFAB_NAME;
            UnityEditor.PrefabUtility.CreatePrefab(filePath + ".prefab", prefab, UnityEditor.ReplacePrefabOptions.ReplaceNameBased);
        }

#endif

        #endregion
    }
}