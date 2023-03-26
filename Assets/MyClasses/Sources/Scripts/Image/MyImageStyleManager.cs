/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyImageStyleManager (version 1.1)
 */

#pragma warning disable 0162
#pragma warning disable 0414

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System;
using System.IO;

namespace MyClasses
{
    public class MyImageStyleManager : MonoBehaviour
    {
        #region ----- Define -----

        public static string CONFIG_DIRECTORY = "Configs/";

        #endregion

        #region ----- Variable -----

        [SerializeField]
        private MyImageStyleConfig _config;
        [SerializeField]
        private bool _isAutoSaveOnChange = true;

        #endregion

        #region ----- Property -----
        
        public MyImageStyleConfig Config
        {
            get { return _config; }
        }

        public MyImageStyleInfo[] Infos
        {
            get { return _config.Infos; }
        }

        #endregion

        #region ----- Singleton -----

        private static object _singletonLock = new object();
        private static MyImageStyleManager _instance;

        public static MyImageStyleManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_singletonLock)
                    {
                        _instance = (MyImageStyleManager)FindObjectOfType(typeof(MyImageStyleManager));
                        if (_instance == null)
                        {
                            GameObject obj = new GameObject(typeof(MyImageStyleManager).Name);
                            _instance = obj.AddComponent<MyImageStyleManager>();
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
            GameObject obj = new GameObject(typeof(MyImageStyleManager).Name);
            MyImageStyleManager script = obj.AddComponent<MyImageStyleManager>();
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
            if (!Directory.Exists("Assets/Resources/" + MyImageStyleManager.CONFIG_DIRECTORY))
            {
                Directory.CreateDirectory("Assets/Resources/" + MyImageStyleManager.CONFIG_DIRECTORY);
            }
#endif

            string filePath = MyImageStyleManager.CONFIG_DIRECTORY + typeof(MyImageStyleConfig).Name + ".asset";
            _config = Resources.Load(filePath, typeof(MyImageStyleConfig)) as MyImageStyleConfig;
#if UNITY_EDITOR
            if (_config == null)
            {
                _config = ScriptableObject.CreateInstance<MyImageStyleConfig>();
                _config.Infos = new MyImageStyleInfo[(int)EStyle.Length];
                for (int i = 0; i < _config.Infos.Length; ++i)
                {
                    _config.Infos[i] = new MyImageStyleInfo();
                    _config.Infos[i].Style = (EStyle)i;
                    switch (_config.Infos[i].Style)
                    {
                        case EStyle.COLOR_AQUA:
                            _config.Infos[i].Color = new Color(0, 255 / 255f, 255 / 255f);
                            break;

                        case EStyle.COLOR_BLACK:
                            _config.Infos[i].Color = new Color(0, 0, 0);
                            break;

                        case EStyle.COLOR_BLUE:
                        case EStyle.CUSTOM_COLOR_BLUE:
                            _config.Infos[i].Color = new Color(0, 0, 255 / 255f);
                            break;

                        case EStyle.COLOR_BROWN:
                        case EStyle.CUSTOM_COLOR_BROWN:
                            _config.Infos[i].Color = new Color(165 / 255f, 42 / 255f, 42 / 255f);
                            break;

                        case EStyle.COLOR_FUCHSIA:
                            _config.Infos[i].Color = new Color(255 / 255f, 0, 255 / 255f);
                            break;

                        case EStyle.COLOR_GRAY:
                        case EStyle.CUSTOM_COLOR_GRAY:
                            _config.Infos[i].Color = new Color(128 / 255f, 128 / 255f, 128 / 255f);
                            break;

                        case EStyle.COLOR_GOLD:
                        case EStyle.CUSTOM_COLOR_GOLD:
                            _config.Infos[i].Color = new Color(255 / 255f, 215 / 255f, 0);
                            break;

                        case EStyle.COLOR_GREEN:
                        case EStyle.CUSTOM_COLOR_GREEN:
                            _config.Infos[i].Color = new Color(0, 128 / 255f, 0);
                            break;

                        case EStyle.COLOR_MAROON:
                            _config.Infos[i].Color = new Color(128 / 255f, 0, 0);
                            break;

                        case EStyle.COLOR_NAVY:
                            _config.Infos[i].Color = new Color(0, 0, 128 / 255f);
                            break;

                        case EStyle.COLOR_OLIVE:
                            _config.Infos[i].Color = new Color(128 / 255f, 128 / 255f, 0);
                            break;

                        case EStyle.COLOR_ORANGE:
                        case EStyle.CUSTOM_COLOR_ORANGE:
                            _config.Infos[i].Color = new Color(255 / 255f, 165 / 255f, 0);
                            break;

                        case EStyle.COLOR_PURPLE:
                        case EStyle.CUSTOM_COLOR_PURPLE:
                            _config.Infos[i].Color = new Color(128 / 255f, 0, 128 / 255f);
                            break;

                        case EStyle.COLOR_RED:
                        case EStyle.CUSTOM_COLOR_RED:
                            _config.Infos[i].Color = new Color(255 / 255f, 0, 0);
                            break;

                        case EStyle.COLOR_SILVER:
                        case EStyle.CUSTOM_COLOR_SILVER:
                            _config.Infos[i].Color = new Color(192 / 255f, 192 / 255f, 192 / 255f);
                            break;

                        case EStyle.COLOR_TEAL:
                            _config.Infos[i].Color = new Color(0, 128 / 255f, 128 / 255f);
                            break;

                        case EStyle.COLOR_YELLOW:
                        case EStyle.CUSTOM_COLOR_YELLOW:
                            _config.Infos[i].Color = new Color(255 / 255f, 255 / 255f, 0);
                            break;

                        case EStyle.COLOR_WHITE:
                            _config.Infos[i].Color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
                            break;
                    }
                }
                AssetDatabase.CreateAsset(_config, filePath);
                AssetDatabase.SaveAssets();
            }
#endif
        }

        /// <summary>
        /// Return info by type.
        /// </summary>
        public MyImageStyleInfo GetInfo(EStyle type)
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
        public class MyImageStyleInfo
        {
            public EStyle Style = EStyle.UNDEFINED;
            public EImageType Type = EImageType.NO_OVERRIDE;
            public Sprite Image = null;
            public Material Material = null;
            public Color Color = Color.white;
            public Vector2 Size = Vector2.zero;
            public string Note = "";
        }

        #endregion

        #region ----- Enumeration -----

        public enum EStyle
        {
            UNDEFINED = 0,

            COLOR_AQUA,
            COLOR_BLACK,
            COLOR_BLUE,
            COLOR_BROWN,
            COLOR_FUCHSIA,
            COLOR_GOLD,
            COLOR_GRAY,
            COLOR_GREEN,
            COLOR_MAROON,
            COLOR_NAVY,
            COLOR_OLIVE,
            COLOR_ORANGE,
            COLOR_PURPLE,
            COLOR_RED,
            COLOR_SILVER,
            COLOR_TEAL,
            COLOR_YELLOW,
            COLOR_WHITE,

            CUSTOM_COLOR_RED,
            CUSTOM_COLOR_GREEN,
            CUSTOM_COLOR_BLUE,
            CUSTOM_COLOR_GOLD,
            CUSTOM_COLOR_SILVER,
            CUSTOM_COLOR_BROWN,
            CUSTOM_COLOR_YELLOW,
            CUSTOM_COLOR_ORANGE,
            CUSTOM_COLOR_PURPLE,
            CUSTOM_COLOR_GRAY,

            SCENE_BACKGROUND_1,
            SCENE_BACKGROUND_2,
            SCENE_BACKGROUND_3,
            SCENE_BACKGROUND_4,
            SCENE_BACKGROUND_5,

            SCENE_HEADER_1,
            SCENE_HEADER_2,
            SCENE_HEADER_3,
            SCENE_HEADER_4,
            SCENE_HEADER_5,

            SCENE_BUTTON_1,
            SCENE_BUTTON_2,
            SCENE_BUTTON_3,
            SCENE_BUTTON_4,
            SCENE_BUTTON_5,

            POPUP_BACKGROUND_1,
            POPUP_BACKGROUND_2,
            POPUP_BACKGROUND_3,
            POPUP_BACKGROUND_4,
            POPUP_BACKGROUND_5,

            POPUP_HEADER_1,
            POPUP_HEADER_2,
            POPUP_HEADER_3,
            POPUP_HEADER_4,
            POPUP_HEADER_5,

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

            Length
        }

        public enum EImageType
        {
            NO_OVERRIDE = 0,
            SIMPLE,
            SIMPLE_PRESERVE_ASPECT,
            SLICED,
            SLICED_FILL_CENTER
        }

        #endregion
        [SerializeField]
        private Color x = new Color(88, 188, 8);
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyImageStyleManager))]
    public class MyImageStyleManagerEditor : Editor
    {
        private MyImageStyleManager _script;
        private SerializedProperty _config;
        private SerializedProperty _isAutoSaveOnChange;
        private bool _isColorsVisible = false;
        private bool _isCustomColorsVisible = false;
        private bool _isSceneBackgroundsVisible = false;
        private bool _isSceneHeadersVisible = false;
        private bool _isSceneButtonsVisible = false;
        private bool _isPopupBackgroundsVisible = false;
        private bool _isPopupHeadersVisible = false;
        private bool _isPopupButtonsVisible = false;
        private bool _isSmallButtonsVisible = false;
        private bool _isMediumButtonsVisible = false;
        private bool _isLargeButtonsVisible = false;
        private bool _isCustomButtonsVisible = false;
        private bool _isCustomsVisible = false;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            _script = (MyImageStyleManager)target;
            _config = serializedObject.FindProperty("_config");
            _isAutoSaveOnChange = serializedObject.FindProperty("_isAutoSaveOnChange");

            _script.LoadConfig();
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(_script), typeof(MyImageStyleManager), false);

            serializedObject.Update();

            EditorGUILayout.ObjectField(_config, new GUIContent("Config File"));

            EditorGUI.BeginChangeCheck();

            _isAutoSaveOnChange.boolValue = EditorGUILayout.Toggle("Auto Save On Change", _isAutoSaveOnChange.boolValue);

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Show All"))
            {
                _isColorsVisible = true;
                _isCustomColorsVisible = true;
                _isSceneBackgroundsVisible = true;
                _isSceneHeadersVisible = true;
                _isSceneButtonsVisible = true;
                _isPopupBackgroundsVisible = true;
                _isPopupHeadersVisible = true;
                _isPopupButtonsVisible = true;
                _isSmallButtonsVisible = true;
                _isMediumButtonsVisible = true;
                _isLargeButtonsVisible = true;
                _isCustomButtonsVisible = true;
                _isCustomsVisible = true;
            }
            if (GUILayout.Button("Hide All"))
            {
                _isColorsVisible = false;
                _isCustomColorsVisible = false;
                _isSceneBackgroundsVisible = false;
                _isSceneHeadersVisible = false;
                _isSceneButtonsVisible = false;
                _isPopupBackgroundsVisible = false;
                _isPopupHeadersVisible = false;
                _isPopupButtonsVisible = false;
                _isSmallButtonsVisible = false;
                _isMediumButtonsVisible = false;
                _isLargeButtonsVisible = false;
                _isCustomButtonsVisible = false;
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
                switch ((MyImageStyleManager.EStyle)i)
                {
                    case MyImageStyleManager.EStyle.COLOR_AQUA:
                        {
                            EditorGUILayout.LabelField(string.Empty);
                            _isColorsVisible = EditorGUILayout.Foldout(_isColorsVisible, "Standard Colors");
                            if (_isColorsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = _isColorsVisible;
                        }
                        break;
                    case MyImageStyleManager.EStyle.CUSTOM_COLOR_RED:
                        {
                            if (_isColorsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            _isCustomColorsVisible = EditorGUILayout.Foldout(_isCustomColorsVisible, "Custom Colors");
                            if (_isCustomColorsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = _isCustomColorsVisible;
                        }
                        break;
                    case MyImageStyleManager.EStyle.SCENE_BACKGROUND_1:
                        {
                            if (_isCustomColorsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            _isSceneBackgroundsVisible = EditorGUILayout.Foldout(_isSceneBackgroundsVisible, "Scene Backgrounds");
                            if (_isSceneBackgroundsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = _isSceneBackgroundsVisible;
                        }
                        break;
                    case MyImageStyleManager.EStyle.SCENE_HEADER_1:
                        {
                            if (_isSceneBackgroundsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            _isSceneHeadersVisible = EditorGUILayout.Foldout(_isSceneHeadersVisible, "Scene Headers");
                            if (_isSceneHeadersVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = _isSceneHeadersVisible;
                        }
                        break;
                    case MyImageStyleManager.EStyle.SCENE_BUTTON_1:
                        {
                            if (_isSceneHeadersVisible)
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
                    case MyImageStyleManager.EStyle.POPUP_BACKGROUND_1:
                        {
                            if (_isSceneButtonsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            _isPopupBackgroundsVisible = EditorGUILayout.Foldout(_isPopupBackgroundsVisible, "Popup Backgrounds");
                            if (_isPopupBackgroundsVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = _isPopupBackgroundsVisible;
                        }
                        break;
                    case MyImageStyleManager.EStyle.POPUP_HEADER_1:
                        {
                            if (_isPopupBackgroundsVisible)
                            {
                                EditorGUI.indentLevel--;
                            }

                            EditorGUILayout.LabelField(string.Empty);
                            _isPopupHeadersVisible = EditorGUILayout.Foldout(_isPopupHeadersVisible, "Popup Headers");
                            if (_isPopupHeadersVisible)
                            {
                                EditorGUI.indentLevel++;
                            }
                            isVisible = _isPopupHeadersVisible;
                        }
                        break;
                    case MyImageStyleManager.EStyle.POPUP_BUTTON_1:
                        {
                            if (_isPopupHeadersVisible)
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
                    case MyImageStyleManager.EStyle.SMALL_BUTTON_1:
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
                    case MyImageStyleManager.EStyle.MEDIUM_BUTTON_1:
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
                    case MyImageStyleManager.EStyle.LARGE_BUTTON_1:
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
                    case MyImageStyleManager.EStyle.CUSTOM_BUTTON_1:
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
                    case MyImageStyleManager.EStyle.CUSTOM_1:
                        {
                            if (_isCustomButtonsVisible)
                            {
                                EditorGUI.indentLevel--;
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
                    EditorGUILayout.LabelField(_script.Infos[i].Style.ToString(), EditorStyles.foldoutHeader);
                    _script.Infos[i].Type = (MyImageStyleManager.EImageType)EditorGUILayout.EnumPopup("Type", _script.Infos[i].Type);
                    _script.Infos[i].Image = (Sprite)EditorGUILayout.ObjectField("Image", _script.Infos[i].Image, typeof(Sprite), false);
                    _script.Infos[i].Material = (Material)EditorGUILayout.ObjectField("Material", _script.Infos[i].Material, typeof(Material), false);
                    _script.Infos[i].Color = EditorGUILayout.ColorField("Color", _script.Infos[i].Color);
                    _script.Infos[i].Size = EditorGUILayout.Vector2Field("Size", _script.Infos[i].Size);
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
                _isColorsVisible = true;
                _isCustomColorsVisible = true;
                _isSceneBackgroundsVisible = true;
                _isSceneHeadersVisible = true;
                _isSceneButtonsVisible = true;
                _isPopupBackgroundsVisible = true;
                _isPopupHeadersVisible = true;
                _isPopupButtonsVisible = true;
                _isSmallButtonsVisible = true;
                _isMediumButtonsVisible = true;
                _isLargeButtonsVisible = true;
                _isCustomButtonsVisible = true;
                _isCustomsVisible = true;
            }
            if (GUILayout.Button("Hide All"))
            {
                _isColorsVisible = false;
                _isCustomColorsVisible = false;
                _isSceneBackgroundsVisible = false;
                _isSceneHeadersVisible = false;
                _isSceneButtonsVisible = false;
                _isPopupBackgroundsVisible = false;
                _isPopupHeadersVisible = false;
                _isPopupButtonsVisible = false;
                _isSmallButtonsVisible = false;
                _isMediumButtonsVisible = false;
                _isLargeButtonsVisible = false;
                _isCustomButtonsVisible = false;
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