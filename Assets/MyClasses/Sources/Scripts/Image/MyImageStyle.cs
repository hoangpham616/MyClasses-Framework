/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyImageStyle (version 1.1)
 */

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;

namespace MyClasses
{
    [RequireComponent(typeof(Image))]
    public class MyImageStyle : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        private MyImageStyleManager.EStyle _style = MyImageStyleManager.EStyle.UNDEFINED;
        [SerializeField]
        private MyImageStyleManager.MyImageStyleInfo _info;

        private RectTransform _rectTransform;
        private Image _image;

        #endregion

        #region ----- Property -----

        public MyImageStyleManager.EStyle Style
        {
            get { return _style; }
            set
            {
                _style = value;
                Refresh();
            }
        }

        #endregion

        #region ----- Implement MonoBehaviour -----

        /// <summary>
        /// Start.
        /// </summary>
        void Start()
        {
            Refresh();
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Refresh.
        /// </summary>
        public void Refresh()
        {
            MyImageStyleManager.MyImageStyleInfo info = MyImageStyleManager.Instance.GetInfo(_style);
            if (info == null)
            {
                return;
            }

            if (_image == null)
            {
                _image = gameObject.GetComponent<Image>();
            }

            if (info.Image != null)
            {
                _image.sprite = info.Image;
            }

            if (info.Material != null)
            {
                _image.material = info.Material;
            }
            
            _image.color = info.Color;

            if (info.Size.x > 0 && info.Size.y > 0)
            {
                if (_rectTransform == null)
                {
                    _rectTransform = gameObject.GetComponent<RectTransform>();
                }
                if (_rectTransform != null)
                {
                    _rectTransform.sizeDelta = info.Size;
                }
            }

            switch (info.Type)
            {
                case MyImageStyleManager.EImageType.SIMPLE:
                    {
                        _image.type = Image.Type.Simple;
                        _image.preserveAspect = false;
                    }
                    break;
                case MyImageStyleManager.EImageType.SIMPLE_PRESERVE_ASPECT:
                    {
                        _image.type = Image.Type.Simple;
                        _image.preserveAspect = true;
                    }
                    break;
                case MyImageStyleManager.EImageType.SLICED:
                    {
                        _image.type = Image.Type.Sliced;
                        _image.fillCenter = false;
                    }
                    break;
                case MyImageStyleManager.EImageType.SLICED_FILL_CENTER:
                    {
                        _image.type = Image.Type.Sliced;
                        _image.fillCenter = true;
                    }
                    break;
            }
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyImageStyle)), CanEditMultipleObjects]
    public class MyImageEditor : Editor
    {
        private MyImageStyle _script;
        private SerializedProperty _style;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            _script = (MyImageStyle)target;
            _style = serializedObject.FindProperty("_style");
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(_script), typeof(MyImageStyle), false);

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            _style.enumValueIndex = (int)(MyImageStyleManager.EStyle)EditorGUILayout.EnumPopup("Style", (MyImageStyleManager.EStyle)_style.enumValueIndex);

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                _script.Refresh();
            }

            MyImageStyleManager.MyImageStyleInfo info = MyImageStyleManager.Instance.GetInfo((MyImageStyleManager.EStyle)_style.enumValueIndex);
            EditorGUILayout.LabelField("Your Note", info != null ? info.Note : string.Empty);
        }
    }

#endif
}