/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyTextStyle (version 1.1)
 */

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;

#if USE_MY_UI_TMPRO
using TMPro;
#endif

namespace MyClasses
{
#if !USE_MY_UI_TMPRO
    [RequireComponent(typeof(Text))]
#endif
    public class MyTextStyle : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        private MyTextStyleManager.EStyle _style = MyTextStyleManager.EStyle.UNDEFINED;

#if USE_MY_UI_TMPRO
        private TextMeshProUGUI _textTMPro;
#endif

        private Text _text;

        #endregion

        #region ----- Property -----

        public MyTextStyleManager.EStyle Style
        {
            get { return _style; }
            set
            {
                _style = value;
                Refresh();
            }
        }

        #endregion

        #region ----- MonoBehaviour Implementation -----

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
            MyTextStyleManager.MyTextStyleInfo info = MyTextStyleManager.Instance.GetInfo(_style);
            if (info == null)
            {
                return;
            }

            if (_text == null)
            {
                _text = gameObject.GetComponent<Text>();
#if USE_MY_UI_TMPRO
                if (_text == null)
                {
                    _textTMPro = gameObject.GetComponent<TextMeshProUGUI>();
                }
#endif
            }

            if (_text != null)
            {
                if (info.Font != null)
                {
                    _text.font = info.Font;
                }
                _text.fontSize = info.FontSizeMin;
                _text.color = info.Color;
            }
#if USE_MY_UI_TMPRO
            else if (_textTMPro != null)
            {
                if (info.TMPFontAsset != null)
                {
                    _textTMPro.font = info.TMPFontAsset;
                }
                _textTMPro.fontSize = info.FontSizeMin;
                _textTMPro.fontSizeMin = info.FontSizeMin;
                _textTMPro.fontSizeMax = info.FontSizeMax;
                _textTMPro.enableAutoSizing = info.FontSizeMin < info.FontSizeMax;
                _textTMPro.color = info.Color;
            }
#endif
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyTextStyle)), CanEditMultipleObjects]
    public class MyTextEditor : Editor
    {
        private MyTextStyle _script;
        private SerializedProperty _style;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            _script = (MyTextStyle)target;
            _style = serializedObject.FindProperty("_style");
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(_script), typeof(MyTextStyle), false);

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            _style.enumValueIndex = (int)(MyTextStyleManager.EStyle)EditorGUILayout.EnumPopup("Style", (MyTextStyleManager.EStyle)_style.enumValueIndex);

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                _script.Refresh();
            }

            MyTextStyleManager.MyTextStyleInfo info = MyTextStyleManager.Instance.GetInfo((MyTextStyleManager.EStyle)_style.enumValueIndex);
            EditorGUILayout.LabelField("Your Note", info != null ? info.Note : string.Empty);
        }
    }

#endif
}