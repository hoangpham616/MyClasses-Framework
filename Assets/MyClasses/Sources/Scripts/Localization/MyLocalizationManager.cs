/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyLocalizationManager (version 3.6)
 */

#pragma warning disable 0162
#pragma warning disable 0414

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MyClasses
{
    public class MyLocalizationManager : MonoBehaviour
    {
        #region ----- Variable -----

#if USE_MY_LOCALIZATION_ARABIC
        public readonly string[] ARABIC_SYMBOLS = new string[] { ".", "؟", "(" };
#endif

        public static string CONFIG_DIRECTORY = "Configs/";

        [SerializeField]
        private MyLocalizationConfig _config;
        [SerializeField]
        private bool _isAutoSaveOnChange = true;

        private ELanguage _languageType = ELanguage.None;
        private string[] _languageKeys;
        private int _languageIndex;
        private Dictionary<string, string[]> _dictionary = new Dictionary<string, string[]>();
        private List<MyLocalization> _listLocalization = new List<MyLocalization>();

        #endregion

        #region ----- Property -----

        public MyLocalizationConfig Config
        {
            get { return _config; }
        }

        public ELanguage Language
        {
            get
            {
                if (_languageType == ELanguage.None)
                {
                    _LoadLanguage();
                    _LoadLocalization();
                }
                return _languageType;
            }
            set
            {
                if (_languageType == ELanguage.None)
                {
                    _LoadLanguage();
                    _LoadLocalization();
                }
                _languageType = value;

                PlayerPrefs.SetInt("MyLocalizationManager_Language", (int)_languageType);
                PlayerPrefs.Save();
            }
        }

        #endregion

        #region ----- Singleton -----

        private static object _singletonLock = new object();
        private static MyLocalizationManager _instance;

        public static MyLocalizationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_singletonLock)
                    {
                        _instance = (MyLocalizationManager)FindObjectOfType(typeof(MyLocalizationManager));
                        if (_instance == null)
                        {
                            GameObject obj = new GameObject(typeof(MyLocalizationManager).Name);
                            _instance = obj.AddComponent<MyLocalizationManager>();
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

        #region ----- Public Method -----

#if UNITY_EDITOR

        /// <summary>
        /// Create a template.
        /// </summary>
        public static void CreateTemplate()
        {
            GameObject obj = new GameObject(typeof(MyLocalizationManager).Name);
            MyLocalizationManager script = obj.AddComponent<MyLocalizationManager>();
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
            if (!Directory.Exists("Assets/Resources/" + MyLocalizationManager.CONFIG_DIRECTORY))
            {
                Directory.CreateDirectory("Assets/Resources/" + MyLocalizationManager.CONFIG_DIRECTORY);
            }
#endif

            string filePath = MyLocalizationManager.CONFIG_DIRECTORY + typeof(MyLocalizationConfig).Name + ".asset";
            _config = Resources.Load(filePath, typeof(MyLocalizationConfig)) as MyLocalizationConfig;
#if UNITY_EDITOR
            if (_config == null)
            {
                _config = ScriptableObject.CreateInstance<MyLocalizationConfig>();
                AssetDatabase.CreateAsset(_config, "Assets/Resources/" + filePath);
                AssetDatabase.SaveAssets();
            }
#endif
        }

        /// <summary>
        /// Load language.
        /// </summary>
        public void LoadLanguage(ELanguage language, bool isForce = false)
        {
            Language = language;

            if (isForce)
            {
                _languageKeys = null;
            }

#if USE_MY_LOCALIZATION_KHMER
            MyFontKhmerConverter.Initialize();
#endif

            for (int i = 0; i < _languageKeys.Length; i++)
            {
                if (_languageKeys[i].Equals(Language.ToString()))
                {
                    _languageIndex = i;
                    break;
                }
            }
        }

        /// <summary>
        /// Load text by key.
        /// </summary>
        public string LoadKey(string key)
        {
            if (_languageType == ELanguage.None)
            {
                LoadLanguage(Language);
            }

            if (_dictionary.ContainsKey(key))
            {
#if USE_MY_LOCALIZATION_ARABIC
                // please import "Arabic Support" package
                if (_languageType == ELanguage.Arabic)
                {
                    string value = _dictionary[key][_languageIndex];
                    string arabic = ArabicSupport.ArabicFixer.Fix(value, false, false);
                    for (int i = 0; i < 10; ++i)
                    {
                        string format = "{" + i + "}";
                        if (value.Contains(format))
                        {
                            arabic = arabic.Replace("}{" + i, format);
                            for (int j = 0; j < ARABIC_SYMBOLS.Length; ++j)
                            {
                                arabic = arabic.Replace(format + ARABIC_SYMBOLS[j], ARABIC_SYMBOLS[j] + format);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    return arabic;
                }
#endif
#if USE_MY_LOCALIZATION_KHMER
                if (_languageType == ELanguage.Unknown)
                {
                    return MyFontKhmerConverter.Convert(_dictionary[key][_languageIndex]);
                }
#endif
                return _dictionary[key][_languageIndex];
            }

            Debug.LogWarning("[" + typeof(MyLocalizationManager).Name + "] LoadKey(): Key \"" + key + "\" missing or null");

            return key;
        }

        /// <summary>
        /// Register an object which uses localization.
        /// </summary>
        public void Register(MyLocalization localization)
        {
            _listLocalization.Add(localization);
        }

        /// <summary>
        /// Unregister an object which uses localization.
        /// </summary>
        public void Unregister(MyLocalization localization)
        {
            _listLocalization.Remove(localization);
        }

        /// <summary>
        /// Localize all objects which active in hierarchy.
        /// </summary>
        public void Refresh()
        {
            for (int i = _listLocalization.Count - 1; i >= 0; i--)
            {
                if (_listLocalization[i] == null)
                {
                    _listLocalization.RemoveAt(i);
                }
                else if (_listLocalization[i].gameObject.activeInHierarchy)
                {
                    _listLocalization[i].Localize();
                }
            }
        }

        /// <summary>
        /// Localize all keys in string.
        /// </summary>
        /// <param name="prefixLoc">prefix string to define strings which need localize</param>
        public string LocalizeKeys(string input, string prefixLoc = "[loc]")
        {
            StringBuilder stringBuilder = new StringBuilder();

            string tmp, tmp2;

            string[] values = input.Split(' ');
            for (int i = 0; i < values.Length; i++)
            {
                tmp = values[i];
                int indexLoc = tmp.IndexOf(prefixLoc);

                if (indexLoc >= 0)
                {
                    tmp = tmp.Substring(0, indexLoc) + tmp.Substring(indexLoc + 5);

                    int indexEnd = tmp.IndexOf("</color>");
                    if (indexEnd > 0)
                    {
                        tmp2 = tmp.Substring(indexLoc, indexEnd - indexLoc);
                    }
                    else if (tmp.EndsWith(",") || tmp.EndsWith("."))
                    {
                        tmp2 = tmp.Substring(indexLoc);
                        tmp2 = tmp2.Substring(indexLoc, tmp2.Length - 1);
                    }
                    else
                    {
                        tmp2 = tmp.Substring(indexLoc);
                    }

                    tmp = tmp.Replace(tmp2, LoadKey(tmp2));
                }
                stringBuilder.Append(tmp + " ");
            }

            return stringBuilder.ToString();
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Load localization from csv file.
        /// </summary>
        private void _LoadLocalization()
        {
            if (Config.Location == ELocation.PERSISTENT)
            {
                if (!File.Exists(Application.persistentDataPath + Config.PersistentPath))
                {
                    Debug.LogError("[" + typeof(MyLocalizationManager).Name + "] Reload(): Could not find file \"" + (Application.persistentDataPath + Config.PersistentPath) + "\".");

                    TextAsset textAsset = Resources.Load(Config.ResourcesPath) as TextAsset;
                    if (textAsset == null)
                    {
                        Debug.LogError("[" + typeof(MyLocalizationManager).Name + "] Reload(): Could not find file \"" + Config.ResourcesPath + "\" too.");
                    }
                    else
                    {
                        _dictionary = MyCSV.DeserializeByRowAndRowName(textAsset.text);
                        _languageKeys = _dictionary.First().Value;
                    }
                }
                else
                {
                    string text = File.ReadAllText(Application.persistentDataPath + Config.PersistentPath);
                    _dictionary = MyCSV.DeserializeByRowAndRowName(text);
                    _languageKeys = _dictionary.First().Value;
                }
            }
            else
            {
                TextAsset textAsset = Resources.Load(Config.ResourcesPath) as TextAsset;
                if (textAsset == null)
                {
                    Debug.LogError("[" + typeof(MyLocalizationManager).Name + "] Reload(): Could not find file \"" + Config.ResourcesPath + "\".");
                }
                else
                {
                    _dictionary = MyCSV.DeserializeByRowAndRowName(textAsset.text);
                    _languageKeys = _dictionary.First().Value;
                }
            }

            if (_languageKeys == null)
            {
                _languageKeys = new string[1];
                _languageKeys[0] = Config.DefaultLanguage.ToString();
            }
        }

        /// <summary>
        /// Load language type by config.
        /// </summary>
        private void _LoadLanguage()
        {
            switch (Config.Mode)
            {
                case EMode.CACHE_ONLY:
                    {
                        _LoadLanguageFromCache();
                    }
                    break;

                case EMode.DEVICE_LANGUAGE_ONLY:
                    {
                        _LoadLanguageBasedOnDevice();
                    }
                    break;

                case EMode.DEVICE_LANGUAGE_AND_CACHE:
                    {
                        _LoadLanguageBasedOnDeviceAndCache();
                    }
                    break;
            }
        }

        /// <summary>
        /// Load language type from cache.
        /// </summary>
        private void _LoadLanguageFromCache()
        {
            int languageValue = PlayerPrefs.GetInt("MyLocalizationManager_Language", (int)ELanguage.None);
            if (languageValue != (int)ELanguage.None && Enum.IsDefined(typeof(ELanguage), languageValue))
            {
                _languageType = (ELanguage)languageValue;
            }
            else
            {
                _languageType = Config.DefaultLanguage;
            }
        }

        /// <summary>
        /// Load language type based on device language.
        /// </summary>
        private void _LoadLanguageBasedOnDevice()
        {
            if (Application.systemLanguage < SystemLanguage.Unknown && Enum.IsDefined(typeof(ELanguage), (int)Application.systemLanguage))
            {
                _languageType = (ELanguage)Application.systemLanguage;
            }
            else
            {
                _languageType = Config.DefaultLanguage;
            }
        }

        /// <summary>
        /// Load language type based on device language and cache.
        /// </summary>
        private void _LoadLanguageBasedOnDeviceAndCache()
        {
            int languageValue = PlayerPrefs.GetInt("MyLocalizationManager_Language", (int)ELanguage.None);
            if (languageValue != (int)ELanguage.None && Enum.IsDefined(typeof(ELanguage), languageValue))
            {
                _languageType = (ELanguage)languageValue;
            }
            else
            {
                _LoadLanguageBasedOnDevice();
            }
        }

        #endregion

        #region ----- Enumeration -----

        public enum ELocation
        {
            RESOURCES,
            PERSISTENT
        }

        public enum EMode
        {
            CACHE_ONLY,
            DEVICE_LANGUAGE_ONLY,
            DEVICE_LANGUAGE_AND_CACHE
        }

        public enum ELanguage
        {
            None = -1,
            Afrikaans = 0,
            Arabic = 1,
            Basque = 2,
            Belarusian = 3,
            Bulgarian = 4,
            Catalan = 5,
            Chinese = 6,
            Czech = 7,
            Danish = 8,
            Dutch = 9,
            English = 10,
            Estonian = 11,
            Faroese = 12,
            Finnish = 13,
            French = 14,
            German = 15,
            Greek = 16,
            Hebrew = 17,
            Hugarian = 18,
            Hungarian = 18,
            Icelandic = 19,
            Indonesian = 20,
            Italian = 21,
            Japanese = 22,
            Korean = 23,
            Lithuanian = 25,
            Norwegian = 26,
            Polish = 27,
            Portuguese = 28,
            Romanian = 29,
            Russian = 30,
            SerboCroatian = 31,
            Slovak = 32,
            Slovenian = 33,
            Spanish = 34,
            Swedish = 35,
            Thai = 36,
            Turkish = 37,
            Ukrainian = 38,
            Vietnamese = 39,
            ChineseSimplified = 40,
            ChineseTraditional = 41,
            Unknown = 42,
            Hindi = 43,
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyLocalizationManager))]
    public class MyLocalizationManagerEditor : Editor
    {
        private MyLocalizationManager _script;
        private SerializedProperty _config;
        private SerializedProperty _isAutoSaveOnChange;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            _script = (MyLocalizationManager)target;
            _config = serializedObject.FindProperty("_config");
            _isAutoSaveOnChange = serializedObject.FindProperty("_isAutoSaveOnChange");

            if (_script.Config == null)
            {
                _script.LoadConfig();
            }
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(_script), typeof(MyLocalizationManager), false);

            serializedObject.Update();

            EditorGUILayout.ObjectField(_config, new GUIContent("Config File"));

            EditorGUI.BeginChangeCheck();

            _isAutoSaveOnChange.boolValue = EditorGUILayout.Toggle("Auto Save On Change", _isAutoSaveOnChange.boolValue);

            EditorGUILayout.LabelField(string.Empty);
            if (GUILayout.Button("Save"))
            {
                _script.SaveConfig();
            }

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Location", EditorStyles.boldLabel);
            _script.Config.Location = (MyLocalizationManager.ELocation)EditorGUILayout.EnumPopup("   Location", _script.Config.Location);
            if (_script.Config.Location == MyLocalizationManager.ELocation.RESOURCES)
            {
                _script.Config.ResourcesPath = EditorGUILayout.TextField("   Path", _script.Config.ResourcesPath);
            }
            else
            {
                _script.Config.PersistentPath = EditorGUILayout.TextField("   Path", _script.Config.PersistentPath);
            }

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Other", EditorStyles.boldLabel);
            _script.Config.Mode = (MyLocalizationManager.EMode)EditorGUILayout.EnumPopup("   Mode", _script.Config.Mode);
            _script.Config.DefaultLanguage = (MyLocalizationManager.ELanguage)EditorGUILayout.EnumPopup("   Default Language", _script.Config.DefaultLanguage);

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                if (_isAutoSaveOnChange.boolValue)
                {
                    _script.SaveConfig();
                }
            }
        }
    }

#endif
}