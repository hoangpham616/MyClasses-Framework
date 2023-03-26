/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyTextStyleManager (version 1.1)
 */

#pragma warning disable 0162
#pragma warning disable 0414

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System;
using System.IO;
using TMPro;

namespace MyClasses
{
    public class MyTextStyleManager : MonoBehaviour
    {
        #region ----- Variable -----

        public static string CONFIG_DIRECTORY = "Configs/";

        [SerializeField]
        private MyTextStyleConfig _config;
        [SerializeField]
        private bool _isAutoSaveOnChange = true;

        #endregion

        #region ----- Property -----

        public MyTextStyleConfig Config
        {
            get { return _config; }
        }

        public MyTextStyleInfo[] Infos
        {
            get { return _config.Infos; }
        }

        #endregion

        #region ----- Singleton -----

        private static object _singletonLock = new object();
        private static MyTextStyleManager _instance;

        public static MyTextStyleManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_singletonLock)
                    {
                        _instance = (MyTextStyleManager)FindObjectOfType(typeof(MyTextStyleManager));
                        if (_instance == null)
                        {
                            GameObject obj = new GameObject(typeof(MyTextStyleManager).Name);
                            _instance = obj.AddComponent<MyTextStyleManager>();
                            if (Application.isPlaying)
                            {
                                DontDestroyOnLoad(obj);
                            }
                        }
                        else if (Application.isPlaying)
                        {
                            DontDestroyOnLoad(_instance);
                        }
                        _instance.LoadConfig();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region ----- MonoBehaviour Implementation -----

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            LoadConfig();
        }

        #endregion

        #region ----- Public Method -----

#if UNITY_EDITOR

        /// <summary>
        /// Create a template.
        /// </summary>
        public static void CreateTemplate()
        {
            GameObject obj = new GameObject(typeof(MyTextStyleManager).Name);
            MyTextStyleManager script = obj.AddComponent<MyTextStyleManager>();
            script.LoadConfig();

            EditorGUIUtility.PingObject(obj);
            Selection.activeGameObject = obj.gameObject;
        }

        /// <summary>
        /// Save config to asset file.
        /// </summary>
        public void SaveConfig()
        {
            EditorUtility.SetDirty(_config);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

#endif

        /// <summary>
        /// Load config from asset file.
        /// </summary>
        public void LoadConfig()
        {
            if (_config != null)
            {
                return;
            }

#if UNITY_EDITOR
            if (!Directory.Exists("Assets/Resources/" + MyTextStyleManager.CONFIG_DIRECTORY))
            {
                Directory.CreateDirectory("Assets/Resources/" + MyTextStyleManager.CONFIG_DIRECTORY);
            }
#endif

            string filePath = MyTextStyleManager.CONFIG_DIRECTORY + typeof(MyTextStyleConfig).Name + ".asset";
            _config = Resources.Load(filePath, typeof(MyTextStyleConfig)) as MyTextStyleConfig;
#if UNITY_EDITOR
            if (_config == null)
            {
                _config = ScriptableObject.CreateInstance<MyTextStyleConfig>();
                _config.Infos = new MyTextStyleInfo[(int)EStyle.Length];
                for (int i = 0; i < _config.Infos.Length; ++i)
                {
                    _config.Infos[i] = new MyTextStyleInfo();
                    _config.Infos[i].Type = (EStyle)i;
                }
                AssetDatabase.CreateAsset(_config, filePath);
                AssetDatabase.SaveAssets();
            }
#endif
        }

        /// <summary>
        /// Return info by type.
        /// </summary>
        public MyTextStyleInfo GetInfo(EStyle type)
        {
            if (type <= EStyle.UNDEFINED || EStyle.Length <= type)
            {
                return null;
            }
            return _config.Infos[(int)type];
        }

        #endregion

        #region ----- Internal Class -----

        [Serializable]
        public class MyTextStyleInfo
        {
            public EStyle Type = EStyle.UNDEFINED;
            public Font Font = null;
            public TMP_FontAsset TMPFontAsset = null;
            public int FontSizeMin = 0;
            public int FontSizeMax = 0;
            public Color Color = Color.white;
            public string Note = "";
        }

        #endregion

        #region ----- Enumeration -----

        public enum EStyle
        {
            UNDEFINED = 0,

            SCENE_TITLE_1,
            SCENE_TITLE_2,
            SCENE_TITLE_3,
            SCENE_TITLE_4,
            SCENE_TITLE_5,

            SCENE_BUTTON_1,
            SCENE_BUTTON_2,
            SCENE_BUTTON_3,
            SCENE_BUTTON_4,
            SCENE_BUTTON_5,

            POPUP_TITLE_1,
            POPUP_TITLE_2,
            POPUP_TITLE_3,
            POPUP_TITLE_4,
            POPUP_TITLE_5,

            POPUP_BUTTON_1,
            POPUP_BUTTON_2,
            POPUP_BUTTON_3,
            POPUP_BUTTON_4,
            POPUP_BUTTON_5,

            SMALL_BUTTON_1,
            SMALL_BUTTON_2,
            SMALL_BUTTON_3,
            SMALL_BUTTON_4,
            SMALL_BUTTON_5,

            MEDIUM_BUTTON_1,
            MEDIUM_BUTTON_2,
            MEDIUM_BUTTON_3,
            MEDIUM_BUTTON_4,
            MEDIUM_BUTTON_5,

            LARGE_BUTTON_1,
            LARGE_BUTTON_2,
            LARGE_BUTTON_3,
            LARGE_BUTTON_4,
            LARGE_BUTTON_5,

            CUSTOM_BUTTON_1,
            CUSTOM_BUTTON_2,
            CUSTOM_BUTTON_3,
            CUSTOM_BUTTON_4,
            CUSTOM_BUTTON_5,
            CUSTOM_BUTTON_6,
            CUSTOM_BUTTON_7,
            CUSTOM_BUTTON_8,
            CUSTOM_BUTTON_9,
            CUSTOM_BUTTON_10,

            MONEY_TEXT_1,
            MONEY_TEXT_2,
            MONEY_TEXT_3,
            MONEY_TEXT_4,
            MONEY_TEXT_5,

            TINY_TEXT_1,
            TINY_TEXT_2,
            TINY_TEXT_3,
            TINY_TEXT_4,

            SMALL_TEXT_1,
            SMALL_TEXT_2,
            SMALL_TEXT_3,
            SMALL_TEXT_4,

            MEDIUM_TEXT_1,
            MEDIUM_TEXT_2,
            MEDIUM_TEXT_3,
            MEDIUM_TEXT_4,

            LARGE_TEXT_1,
            LARGE_TEXT_2,
            LARGE_TEXT_3,
            LARGE_TEXT_4,

            HUGE_TEXT_1,
            HUGE_TEXT_2,
            HUGE_TEXT_3,
            HUGE_TEXT_4,

            GIGANTIC_TEXT_1,
            GIGANTIC_TEXT_2,
            GIGANTIC_TEXT_3,
            GIGANTIC_TEXT_4,

            CUSTOM_1,
            CUSTOM_2,
            CUSTOM_3,
            CUSTOM_4,
            CUSTOM_5,
            CUSTOM_6,
            CUSTOM_7,
            CUSTOM_8,
            CUSTOM_9,
            CUSTOM_10,
            CUSTOM_11,
            CUSTOM_12,
            CUSTOM_13,
            CUSTOM_14,
            CUSTOM_15,
            CUSTOM_16,
            CUSTOM_17,
            CUSTOM_18,
            CUSTOM_19,
            CUSTOM_20,

            Length,
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyTextStyleManager))]
    public class MyTextStyleManagerEditor : Editor
    {
        private MyTextStyleManager _script;
        private SerializedProperty _config;
        private SerializedProperty _isAutoSaveOnChange;
        private bool _isSceneTitlesVisible = false;
        private bool _isSceneButtonsVisible = false;
        private bool _isPopupTitlesVisible = false;
        private bool _isPopupButtonsVisible = false;
        private bool _isSmallButtonsVisible = false;
        private bool _isMediumButtonsVisible = false;
        private bool _isLargeButtonsVisible = false;
        private bool _isCustomButtonsVisible = false;
        private bool _isMoneyTextsVisible = false;
        private bool _isTinyTextsVisible = false;
        private bool _isSmallTextsVisible = false;
        private bool _isMediumTextsVisible = false;
        private bool _isLargeTextsVisible = false;
        private bool _isHugeTextsVisible = false;
        private bool _isGiganticTextsVisible = false;
        private bool _isCustomsVisible = false;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            _script = (MyTextStyleManager)target;
            _config = serializedObject.FindProperty("_config");
            _isAutoSaveOnChange = serializedObject.FindProperty("_isAutoSaveOnChange");

            _script.LoadConfig();
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(_script), typeof(MyTextStyleManager), false);

            serializedObject.Update();

            EditorGUILayout.ObjectField(_config, new GUIContent("Config File"));

            EditorGUI.BeginChangeCheck();

            _isAutoSaveOnChange.boolValue = EditorGUILayout.Toggle("Auto Save On Change", _isAutoSaveOnChange.boolValue);

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Show All"))
            {
                _isSceneTitlesVisible = true;
                _isSceneButtonsVisible = true;
                _isPopupTitlesVisible = true;
                _isPopupButtonsVisible = true;
                _isSmallButtonsVisible = true;
                _isMediumButtonsVisible = true;
                _isLargeButtonsVisible = true;
                _isCustomButtonsVisible = true;
                _isMoneyTextsVisible = true;
                _isTinyTextsVisible = true;
                _isSmallTextsVisible = true;
                _isMediumTextsVisible = true;
                _isLargeTextsVisible = true;
                _isHugeTextsVisible = true;
                _isGiganticTextsVisible = true;
                _isCustomsVisible = true;
            }
            if (GUILayout.Button("Hide All"))
            {
                _isSceneTitlesVisible = false;
                _isSceneButtonsVisible = false;
                _isPopupTitlesVisible = false;
                _isPopupButtonsVisible = false;
                _isSmallButtonsVisible = false;
                _isMediumButtonsVisible = false;
                _isLargeButtonsVisible = false;
                _isCustomButtonsVisible = false;
                _isMoneyTextsVisible = false;
                _isTinyTextsVisible = false;
                _isSmallTextsVisible = false;
                _isMediumTextsVisible = false;
                _isLargeTextsVisible = false;
                _isHugeTextsVisible = false;
                _isGiganticTextsVisible = false;
                _isCustomsVisible = false;
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Save"))
            {
                _script.SaveConfig();
            }

            bool isVisible = false;
            for (int i = 1; i < _script.Infos.Length; ++i)
            {
                switch ((MyTextStyleManager.EStyle)i)
                {
                    case MyTextStyleManager.EStyle.SCENE_TITLE_1:
                        {
                            EditorGUILayout.LabelField(string.Empty);
                            _isSceneTitlesVisible = EditorGUILayout.Foldout(_isSceneTitlesVisible, "Scene Titles");
                            if (_isSceneTitlesVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = _isSceneTitlesVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.SCENE_BUTTON_1:
                        {
                            if (_isSceneTitlesVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            _isSceneButtonsVisible = EditorGUILayout.Foldout(_isSceneButtonsVisible, "Scene Buttons");
                            if (_isSceneButtonsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = _isSceneButtonsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.POPUP_TITLE_1:
                        {
                            if (_isSceneButtonsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            _isPopupTitlesVisible = EditorGUILayout.Foldout(_isPopupTitlesVisible, "Popup Titles");
                            if (_isPopupTitlesVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = _isPopupTitlesVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.POPUP_BUTTON_1:
                        {
                            if (_isPopupTitlesVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            _isPopupButtonsVisible = EditorGUILayout.Foldout(_isPopupButtonsVisible, "Popup Buttons");
                            if (_isPopupButtonsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = _isPopupButtonsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.SMALL_BUTTON_1:
                        {
                            if (_isPopupButtonsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            _isSmallButtonsVisible = EditorGUILayout.Foldout(_isSmallButtonsVisible, "Small Buttons");
                            if (_isSmallButtonsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = _isSmallButtonsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.MEDIUM_BUTTON_1:
                        {
                            if (_isSmallButtonsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            _isMediumButtonsVisible = EditorGUILayout.Foldout(_isMediumButtonsVisible, "Medium Buttons");
                            if (_isMediumButtonsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = _isMediumButtonsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.LARGE_BUTTON_1:
                        {
                            if (_isMediumButtonsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            _isLargeButtonsVisible = EditorGUILayout.Foldout(_isLargeButtonsVisible, "Large Buttons");
                            if (_isLargeButtonsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = _isLargeButtonsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.CUSTOM_BUTTON_1:
                        {
                            if (_isLargeButtonsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            _isCustomButtonsVisible = EditorGUILayout.Foldout(_isCustomButtonsVisible, "Custom Buttons");
                            if (_isCustomButtonsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = _isCustomButtonsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.MONEY_TEXT_1:
                        {
                            if (_isPopupButtonsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            _isMoneyTextsVisible = EditorGUILayout.Foldout(_isMoneyTextsVisible, "Money Texts");
                            if (_isMoneyTextsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = _isMoneyTextsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.TINY_TEXT_1:
                        {
                            if (_isMoneyTextsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            _isTinyTextsVisible = EditorGUILayout.Foldout(_isTinyTextsVisible, "Tiny Texts");
                            if (_isTinyTextsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = _isTinyTextsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.SMALL_TEXT_1:
                        {
                            if (_isTinyTextsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            _isSmallTextsVisible = EditorGUILayout.Foldout(_isSmallTextsVisible, "Small Texts");
                            if (_isSmallTextsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = _isSmallTextsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.MEDIUM_TEXT_1:
                        {
                            if (_isSmallTextsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            _isMediumTextsVisible = EditorGUILayout.Foldout(_isMediumTextsVisible, "Medium Texts");
                            if (_isMediumTextsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = _isMediumTextsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.LARGE_TEXT_1:
                        {
                            if (_isMediumTextsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            _isLargeTextsVisible = EditorGUILayout.Foldout(_isLargeTextsVisible, "Large Texts");
                            if (_isLargeTextsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = _isLargeTextsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.HUGE_TEXT_1:
                        {
                            if (_isLargeTextsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            _isHugeTextsVisible = EditorGUILayout.Foldout(_isHugeTextsVisible, "Huge Texts");
                            if (_isHugeTextsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = _isHugeTextsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.GIGANTIC_TEXT_1:
                        {
                            if (_isHugeTextsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            _isGiganticTextsVisible = EditorGUILayout.Foldout(_isGiganticTextsVisible, "Gigantic Texts");
                            if (_isGiganticTextsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = _isGiganticTextsVisible;
                        }
                        break;
                    case MyTextStyleManager.EStyle.CUSTOM_1:
                        {
                            if (_isGiganticTextsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            _isCustomsVisible = EditorGUILayout.Foldout(_isCustomsVisible, "Customs");
                            if (_isCustomsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = _isCustomsVisible;
                        }
                        break;
                }
                if (isVisible)
                {
                    EditorGUILayout.LabelField(_script.Infos[i].Type.ToString(), EditorStyles.foldoutHeader);
                    _script.Infos[i].Font = (Font)EditorGUILayout.ObjectField("Font For Text", _script.Infos[i].Font, typeof(Font), true);
                    _script.Infos[i].TMPFontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField("Font For TMPro", _script.Infos[i].TMPFontAsset, typeof(TMP_FontAsset), true);
                    _script.Infos[i].FontSizeMin = EditorGUILayout.IntField("Min Font Size", _script.Infos[i].FontSizeMin);
                    _script.Infos[i].FontSizeMax = EditorGUILayout.IntField("Max Font Size", _script.Infos[i].FontSizeMax);
                    _script.Infos[i].Color = EditorGUILayout.ColorField("Color", _script.Infos[i].Color);
                    _script.Infos[i].Note = EditorGUILayout.TextField("Your Note", _script.Infos[i].Note);
                    EditorGUILayout.LabelField(string.Empty);
                }
                if (i == _script.Infos.Length - 1)
                {
                    if (_isCustomsVisible)
                    {
                        EditorGUI.indentLevel--;
                    }
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                if (_isAutoSaveOnChange.boolValue)
                {
                    _script.SaveConfig();
                }
            }

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Show All"))
            {
                _isSceneTitlesVisible = true;
                _isSceneButtonsVisible = true;
                _isPopupTitlesVisible = true;
                _isPopupButtonsVisible = true;
                _isSmallButtonsVisible = true;
                _isMediumButtonsVisible = true;
                _isLargeButtonsVisible = true;
                _isCustomButtonsVisible = true;
                _isMoneyTextsVisible = true;
                _isTinyTextsVisible = true;
                _isSmallTextsVisible = true;
                _isMediumTextsVisible = true;
                _isLargeTextsVisible = true;
                _isHugeTextsVisible = true;
                _isGiganticTextsVisible = true;
                _isCustomsVisible = true;
            }
            if (GUILayout.Button("Hide All"))
            {
                _isSceneTitlesVisible = false;
                _isSceneButtonsVisible = false;
                _isPopupTitlesVisible = false;
                _isPopupButtonsVisible = false;
                _isSmallButtonsVisible = false;
                _isMediumButtonsVisible = false;
                _isLargeButtonsVisible = false;
                _isCustomButtonsVisible = false;
                _isMoneyTextsVisible = false;
                _isTinyTextsVisible = false;
                _isSmallTextsVisible = false;
                _isMediumTextsVisible = false;
                _isLargeTextsVisible = false;
                _isHugeTextsVisible = false;
                _isGiganticTextsVisible = false;
                _isCustomsVisible = false;
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Save"))
            {
                _script.SaveConfig();
            }
        }
    }

#endif
}