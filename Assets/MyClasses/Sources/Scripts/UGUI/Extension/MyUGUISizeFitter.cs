/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUGUISizeFitter (version 2.0)
 */

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

namespace MyClasses.UI
{
    public class MyUGUISizeFitter : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        private RectTransform _rectTransformTarget;
        [SerializeField]
        private EFrequency _frequency = EFrequency.Always;
        [SerializeField]
        private EMode _mode = EMode.Width;
        [SerializeField]
        private Vector2 _extraSize = new Vector2(50, 0);
        [SerializeField]
        private Vector2 _minSize = new Vector2(100, 100);
        [SerializeField]
        private Vector2 _maxSize = new Vector2(500, 100);
        [SerializeField]
        private Vector2 _originalSize = Vector2.zero;
        [SerializeField]
        private Vector2 _targetLastSize = Vector2.zero;

        private RectTransform _rectTransform;

        #endregion

        #region ----- Property -----

        public EFrequency Frequency
        {
            get { return _frequency; }
            set { _frequency = value; }
        }

        public EMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        public RectTransform Target
        {
            get { return _rectTransformTarget; }
            set { _rectTransformTarget = value; }
        }

        public Vector2 ExtraSize
        {
            get { return _extraSize; }
            set { _extraSize = value; }
        }

        public Vector2 MinSize
        {
            get { return _minSize; }
            set { _minSize = value; }
        }

        public Vector2 MaxSize
        {
            get { return _maxSize; }
            set { _maxSize = value; }
        }

        #endregion

        #region ----- MonoBehaviour Implementation -----

        /// <summary>
        /// Awake.
        /// </summary>
        void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalSize = _rectTransform.sizeDelta;
        }

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            if (_mode != EMode.None)
            {
                StartCoroutine(_DoResize());
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Resize.
        /// </summary>
        public void Resize()
        {
            if (_mode == EMode.None)
            {
                _rectTransform.sizeDelta = _originalSize;
            }
            else if (_mode == EMode.Original)
            {
                _rectTransform.sizeDelta = _originalSize;
            }
            else
            {
                if (_rectTransformTarget == null)
                {
                    Debug.LogError("[" + typeof(MyUGUISizeFitter).Name + "] Resize(): Could not find the target.");
                }
                else
                {
#if UNITY_EDITOR
                    if (_rectTransformTarget.GetComponent<UnityEngine.UI.ContentSizeFitter>() == null)
                    {
                        Debug.LogWarning("[" + typeof(MyUGUISizeFitter).Name + "] Resize(): Target does not have ContentSizeFitter component. Please make sure you have set it up correctly.");
                    }
#endif

                    Vector2 targetSize = _rectTransformTarget.sizeDelta;
                    switch (_mode)
                    {
                        case EMode.Width:
                            {
                                targetSize.x = Mathf.Clamp(targetSize.x + _extraSize.x, _minSize.x, _maxSize.x);
                                targetSize.y = _originalSize.y;
                                _rectTransform.sizeDelta = targetSize;
                            }
                            break;

                        case EMode.Height:
                            {
                                targetSize.x = _originalSize.x;
                                targetSize.y = Mathf.Clamp(targetSize.y + _extraSize.y, _minSize.y, _maxSize.y);
                                _rectTransform.sizeDelta = targetSize;
                            }
                            break;

                        case EMode.Both:
                            {
                                targetSize.x = Mathf.Clamp(targetSize.x + _extraSize.x, _minSize.x, _maxSize.x);
                                targetSize.y = Mathf.Clamp(targetSize.y + _extraSize.y, _minSize.y, _maxSize.y);
                                _rectTransform.sizeDelta = targetSize;
                            }
                            break;
                    }
                    _targetLastSize = targetSize;
                }
            }
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Handle resizing.
        /// </summary>
        private IEnumerator _DoResize()
        {
            yield return new WaitForEndOfFrame();

            Resize();

            while (_frequency == EFrequency.Always && gameObject.activeInHierarchy)
            {
                Vector2 targetSize = _rectTransformTarget.sizeDelta;
                if (_targetLastSize.x != targetSize.x || _targetLastSize.y != targetSize.y)
                {
                    Resize();
                }
                yield return null;
            }
        }

        #endregion

        #region ----- Enumeration -----

        public enum EFrequency
        {
            EverytimeActive,
            Always
        }

        public enum EMode
        {
            None,
            Original,
            Width,
            Height,
            Both
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyUGUISizeFitter))]
    public class MyUGUISizeFitterEditor : Editor
    {
        private MyUGUISizeFitter _script;

        private SerializedProperty _frequency;
        private SerializedProperty _mode;
        private SerializedProperty _rectTransformTarget;
        private SerializedProperty _extraSize;
        private SerializedProperty _minSize;
        private SerializedProperty _maxSize;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            _script = (MyUGUISizeFitter)target;

            _frequency = serializedObject.FindProperty("_frequency");
            _mode = serializedObject.FindProperty("_mode");
            _rectTransformTarget = serializedObject.FindProperty("_rectTransformTarget");
            _extraSize = serializedObject.FindProperty("_extraSize");
            _minSize = serializedObject.FindProperty("_minSize");
            _maxSize = serializedObject.FindProperty("_maxSize");
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(_script), typeof(MyUGUISizeFitter), false);

            serializedObject.Update();

            _script.Target = EditorGUILayout.ObjectField("Target", _script.Target, typeof(RectTransform), true) as RectTransform;
            _script.Frequency = (MyUGUISizeFitter.EFrequency)EditorGUILayout.EnumPopup("Frequency", _script.Frequency);
            _script.Mode = (MyUGUISizeFitter.EMode)EditorGUILayout.EnumPopup("Mode", _script.Mode);
            switch (_script.Mode)
            {
                case MyUGUISizeFitter.EMode.Original:
                    {
                    }
                    break;

                case MyUGUISizeFitter.EMode.Width:
                    {
                        Vector2 extraSize = _extraSize.vector2Value;
                        extraSize.x = EditorGUILayout.FloatField("   Extra Width", extraSize.x);
                        _extraSize.vector2Value = extraSize;

                        Vector2 minSize = _minSize.vector2Value;
                        minSize.x = EditorGUILayout.FloatField("   Min Width", minSize.x);
                        _minSize.vector2Value = minSize;

                        Vector2 maxSize = _maxSize.vector2Value;
                        maxSize.x = EditorGUILayout.FloatField("   Max Width", maxSize.x);
                        _maxSize.vector2Value = maxSize;
                    }
                    break;

                case MyUGUISizeFitter.EMode.Height:
                    {
                        Vector2 extraSize = _extraSize.vector2Value;
                        extraSize.y = EditorGUILayout.FloatField("   Extra Height", extraSize.y);
                        _extraSize.vector2Value = extraSize;

                        Vector2 minSize = _minSize.vector2Value;
                        minSize.y = EditorGUILayout.FloatField("   Min Height", minSize.y);
                        _minSize.vector2Value = minSize;

                        Vector2 maxSize = _maxSize.vector2Value;
                        maxSize.y = EditorGUILayout.FloatField("   Max Height", maxSize.y);
                        _maxSize.vector2Value = maxSize;
                    }
                    break;

                case MyUGUISizeFitter.EMode.Both:
                    {
                        _extraSize.vector2Value = EditorGUILayout.Vector2Field("   Extra Size", _extraSize.vector2Value);
                        _minSize.vector2Value = EditorGUILayout.Vector2Field("   Min Size", _minSize.vector2Value);
                        _maxSize.vector2Value = EditorGUILayout.Vector2Field("   Max Size", _maxSize.vector2Value);
                    }
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}