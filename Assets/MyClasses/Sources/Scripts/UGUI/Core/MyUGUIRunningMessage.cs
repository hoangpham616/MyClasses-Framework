/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUGUIRunningMessage (version 2.24)
 */

#pragma warning disable 0114
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0649

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MyClasses.UI
{
    public class MyUGUIRunningMessage
    {
        #region ----- Define -----

        public const string PREFAB_NAME = "RunningMessage";

        #endregion

        #region ----- Variable -----

        private Text _text;

        private Animator _animator;
        private GameObject _gameObject;
        private GameObject _container;
        private RectTransform _mask;
        private Vector3 _currentPosition;
        private EState _state;
        private EType _type;
        private Color _colorText;
        private float _speed;
        private float _minSpeed;
        private float _maxSpeed;
        private float _endX;

        private List<string> _listContent = new List<string>();
        private int _maxContentQueue = 3;

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

        public bool IsShow
        {
            get { return _gameObject != null && _gameObject.activeSelf; }
        }

        public EType Type
        {
            get { return _type; }
        }

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyUGUIRunningMessage(EType type)
        {
            _type = type;
#if UNITY_EDITOR
            if (!_CheckPrefab(_type))
            {
                _CreatePrefab(_type);
            }
#endif
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set max content queue.
        /// </summary>
        public void SetMaxQueue(int maxQueue)
        {
            if (maxQueue < 0)
            {
                maxQueue = 999;
            }
            _maxContentQueue = maxQueue;
        }

        /// <summary>
        /// Show.
        /// </summary>
        public void Show(string content, float minSpeed, float maxSpeed)
        {
            if (_gameObject != null)
            {
                _listContent.Add(content);
                if (_listContent.Count > _maxContentQueue)
                {
                    _listContent.RemoveAt(0);
                }

                _minSpeed = minSpeed;
                _maxSpeed = maxSpeed;

                if (!_gameObject.activeSelf)
                {
                    _Init();

                    _state = EState.PreShow;
                }
            }
        }

        /// <summary>
        /// Hide.
        /// </summary>
        public void Hide()
        {
            if (_gameObject != null)
            {
                _Init();

                _listContent.Clear();

                _state = EState.Hide;
            }
        }

        /// <summary>
        /// Hide immedialy.
        /// </summary>
        public void HideImmedialy()
        {
            if (_gameObject != null)
            {
                _Init();

                _listContent.Clear();
                _animator = null;

                _state = EState.Hide;

                Update(Time.deltaTime);
            }
        }

        /// <summary>
        /// Update state machine.
        /// </summary>
        public void Update(float dt)
        {
            if (_text == null)
            {
                return;
            }

            switch (_state)
            {
                case EState.PreShow:
                    {
                        if (_listContent.Count < 0)
                        {
                            return;
                        }

                        Color colorAlpha0 = _text.color;
                        colorAlpha0.a = 0;
                        _text.color = colorAlpha0;
                        _text.text = _listContent[0];
                        _listContent.RemoveAt(0);

                        _gameObject.SetActive(true);
                        _animator = _gameObject.GetComponent<Animator>();
                        if (_animator != null)
                        {
                            _animator.Play("Show");
                        }

                        _state = EState.Show;
                    }
                    break;
                case EState.Show:
                    {
                        _speed = _minSpeed * (_text.rectTransform.rect.width / _mask.rect.width);
                        _speed = Mathf.Clamp(_speed, _minSpeed, _maxSpeed);

                        float halfWidth = _text.rectTransform.rect.width / 2;
                        float beginX = (_mask.rect.width / 2) + halfWidth;
                        _endX = _mask.rect.x - halfWidth;

                        _currentPosition = Vector3.zero;
                        _currentPosition.x = beginX + (_speed / 2);
                        _text.color = _colorText;
                        _text.rectTransform.localPosition = _currentPosition;

                        _state = EState.Update;
                    }
                    break;
                case EState.Update:
                    {
                        _currentPosition.x -= _speed * dt;
                        _text.rectTransform.localPosition = _currentPosition;

                        if (_currentPosition.x < _endX)
                        {
                            if (_listContent.Count > 0)
                            {
                                _text.text = _listContent[0];
                                _listContent.RemoveAt(0);

                                _state = EState.Show;
                            }
                            else
                            {
                                _animator = _gameObject.GetComponent<Animator>();

                                _state = EState.Hide;
                            }
                        }
                    }
                    break;
                case EState.Hide:
                    {
                        _currentPosition.x = (_mask.rect.width / 2) + _text.rectTransform.rect.width;
                        _text.rectTransform.localPosition = _currentPosition;

                        if (_animator != null)
                        {
                            _animator.Play("Hide");
                            _state = EState.Hiding;
                        }
                        else
                        {
                            _gameObject.SetActive(false);
                            _state = EState.Idle;
                        }

                    }
                    break;
                case EState.Hiding:
                    {
                        if (_animator == null || _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                        {
                            _gameObject.SetActive(false);
                            _state = EState.Idle;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Return name of game object by type.
        /// </summary>
        public static string GetGameObjectName(EType type)
        {
            return PREFAB_NAME + (type == EType.Default ? string.Empty : "_" + type.ToString());
        }

        /// <summary>
        /// Create a template game object.
        /// </summary>
        public static GameObject CreateTemplate(EType type)
        {
            GameObject obj = new GameObject(GetGameObjectName(type));

            obj.transform.SetParent(MyUGUIManager.Instance.CanvasOnTop.transform, false);
            obj.layer = LayerMask.NameToLayer("UI");
            obj.AddComponent<CanvasRenderer>();
            obj.AddComponent<Canvas>();

            RectTransform root_rect = obj.GetComponent<RectTransform>();
            MyUtilities.Anchor(ref root_rect, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, Vector2.zero, Vector2.zero);

            GameObject container = new GameObject("Container");
            container.transform.SetParent(obj.transform, false);
            container.layer = LayerMask.NameToLayer("UI");

            Image container_image = container.AddComponent<Image>();
            container_image.color = new Color(0, 0, 0, 0.5f);
            container_image.raycastTarget = false;

            RectTransform container_rect = container.GetComponent<RectTransform>();
            MyUtilities.Anchor(ref container_rect, MyUtilities.EAnchorPreset.HorizontalStretchTop, MyUtilities.EAnchorPivot.TopCenter, new Vector2(200, -160), new Vector2(-200, -80));

            GameObject mask = new GameObject("Mask");
            mask.transform.SetParent(container.transform, false);
            mask.layer = LayerMask.NameToLayer("UI");
            mask.AddComponent<CanvasRenderer>();
            mask.AddComponent<RectMask2D>();

            RectTransform mask_rect = mask.GetComponent<RectTransform>();
            MyUtilities.Anchor(ref mask_rect, MyUtilities.EAnchorPreset.DualStretch, MyUtilities.EAnchorPivot.MiddleCenter, new Vector2(100, 0), new Vector2(-100, 0));

            GameObject text = new GameObject("Text");
            text.transform.SetParent(mask.transform, false);
            text.layer = LayerMask.NameToLayer("UI");

            Text text_text = text.AddComponent<Text>();
            text_text.text = "This is running message";
            text_text.color = Color.white;
            text_text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text_text.fontSize = 50;
            text_text.alignment = TextAnchor.MiddleLeft;
            text_text.verticalOverflow = VerticalWrapMode.Overflow;
            text_text.raycastTarget = false;

            RectTransform text_rect = text.GetComponent<RectTransform>();
            MyUtilities.Anchor(ref text_rect, MyUtilities.EAnchorPreset.MiddleCenter, MyUtilities.EAnchorPivot.MiddleCenter, 0, 0);

            ContentSizeFitter text_csf = text.AddComponent<ContentSizeFitter>();
            text_csf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

            return obj;
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Init.
        /// </summary>
        private void _Init()
        {
            if (_gameObject != null)
            {
                if (_container == null || _mask == null || _text == null)
                {
                    _gameObject.SetActive(false);

                    _container = MyUtilities.FindObjectInFirstLayer(_gameObject, "Container");
                    _mask = MyUtilities.FindObjectInFirstLayer(_container.gameObject, "Mask").GetComponent<RectTransform>();
                    _text = MyUtilities.FindObjectInFirstLayer(_mask.gameObject, "Text").GetComponent<Text>();
                    _colorText = _text.color;

                    _state = EState.Idle;
                }
            }
        }

#if UNITY_EDITOR

        /// <summary>
        /// Check existence of prefab.
        /// </summary>
        private static bool _CheckPrefab(EType type)
        {
            string filePath = "Assets/Resources/" + MyUGUIManager.SPECIALITY_DIRECTORY + GetGameObjectName(type) + ".prefab";
            return System.IO.File.Exists(filePath);
        }

        /// <summary>
        /// Create template prefab.
        /// </summary>
        private static void _CreatePrefab(EType type)
        {
            Debug.Log("[" + typeof(MyUGUIToastMessage).Name + "] CreatePrefab(): a template prefab was created.");

            GameObject prefab = CreateTemplate(type);

            string folderPath = "Assets/Resources/" + MyUGUIManager.SPECIALITY_DIRECTORY;
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }

            string filePath = "Assets/Resources/" + MyUGUIManager.SPECIALITY_DIRECTORY + GetGameObjectName(type);
            UnityEditor.PrefabUtility.CreatePrefab(filePath + ".prefab", prefab, UnityEditor.ReplacePrefabOptions.ReplaceNameBased);
        }

#endif

        #endregion

        #region ----- Enumeration -----

        private enum EState
        {
            Idle,
            PreShow,
            Show,
            Update,
            Hide,
            Hiding,
        }

        public enum EType
        {
            Default,
            Custom,
            Custom2,
            Custom3
        }

        #endregion
    }
}