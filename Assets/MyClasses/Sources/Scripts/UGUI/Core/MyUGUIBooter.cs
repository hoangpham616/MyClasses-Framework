/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUGUIConfig (version 2.12)
 */

#pragma warning disable 0414
#pragma warning disable 0649

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

namespace MyClasses.UI
{
    public class MyUGUIBooter : MonoBehaviour
    {
        #region ----- Internal Class -----

        [Serializable]
        public class UnityEventAction : UnityEvent<Action> { }
        [Serializable]
        public class UnityEventVoid : UnityEvent { }

        #endregion

        #region ----- Variable -----

        [SerializeField]
        private EBootMode _bootMode = EBootMode.Instant;
        [SerializeField]
        private EShowMode _showMode = EShowMode.Default;
        [SerializeField]
        private EUnitySceneID _defaultUnitySceneID;
        [SerializeField]
        private ESceneID _defaultSceneID;
        [SerializeField]
        private float _delayTimeOnEditor = 0;
        [SerializeField]
        private float _delayTimeOnDevice = 0;
        [SerializeField]
        private UnityEventAction _onPreShowSync;
        [SerializeField]
        private UnityEventVoid _onPreShow;
        [SerializeField]
        private UnityEventVoid _onCustomShow;
        [SerializeField]
        private UnityEventVoid _onPostShow;

        private static bool _isBooted = false;

        #endregion

        #region ----- MonoBehaviour Implementation -----

        /// <summary>
        /// Start.
        /// </summary>
        void Start()
        {
            if (_isBooted)
            {
                return;
            }

            switch (_bootMode)
            {
                case EBootMode.Instant:
                    {
                        if (_onPreShow != null)
                        {
                            _onPreShow.Invoke();
                        }

                        _ShowDefaultScene();
                    }
                    break;

                case EBootMode.FixedTimeDelay:
                    {
                        if (_onPreShow != null)
                        {
                            _onPreShow.Invoke();
                        }

#if UNITY_EDITOR
                        if (_delayTimeOnEditor > 0)
                        {
                            StartCoroutine(_ShowDefaultSceneWithDelay(_delayTimeOnEditor));
                        }
#else
                        if (_delayTimeOnDevice > 0)
                        {
                            StartCoroutine(_ShowDefaultSceneWithDelay(_delayTimeOnDevice));
                        }
#endif
                        else
                        {
                            _ShowDefaultScene();
                        }
                    }
                    break;

                case EBootMode.WaitForInitializing:
                    {
                        if (_onPreShowSync != null)
                        {
                            _onPreShowSync.Invoke(_ShowDefaultScene);
                        }
                        else
                        {
                            _ShowDefaultScene();
                        }
                    }
                    break;
            }
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Show default unity scene.
        /// </summary>
        private IEnumerator _ShowDefaultSceneWithDelay(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            _ShowDefaultScene();
        }

        /// <summary>
        /// Show default scene.
        /// </summary>
        private void _ShowDefaultScene()
        {
            if (_showMode == EShowMode.Custom && _onCustomShow != null)
            {
                _onCustomShow.Invoke();
            }
            else
            {
                MyUGUIManager.Instance.ShowUnityScene(_defaultUnitySceneID, _defaultSceneID);
            }

            _isBooted = true;

            if (_onPostShow != null)
            {
                _onPostShow.Invoke();
            }
        }

        #endregion

        #region ----- Enumeration -----

        public enum EShowMode
        {
            Default = 0,
            Custom = 1,
        }

        public enum EBootMode
        {
            Instant = 0,
            FixedTimeDelay = 1,
            WaitForInitializing = 2,
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyUGUIBooter))]
    public class MyUGUIConfigEditor : Editor
    {
        private MyUGUIBooter _script;
        private SerializedProperty _showMode;
        private SerializedProperty _bootMode;
        private SerializedProperty _defaultUnitySceneID;
        private SerializedProperty _defaultSceneID;
        private SerializedProperty _delayTimeOnEditor;
        private SerializedProperty _delayTimeOnDevice;
        private SerializedProperty _onPreShowSync;
        private SerializedProperty _onPreShow;
        private SerializedProperty _onCustomShow;
        private SerializedProperty _onPostShow;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            _script = (MyUGUIBooter)target;
            _showMode = serializedObject.FindProperty("_showMode");
            _bootMode = serializedObject.FindProperty("_bootMode");
            _defaultUnitySceneID = serializedObject.FindProperty("_defaultUnitySceneID");
            _defaultSceneID = serializedObject.FindProperty("_defaultSceneID");
            _delayTimeOnEditor = serializedObject.FindProperty("_delayTimeOnEditor");
            _delayTimeOnDevice = serializedObject.FindProperty("_delayTimeOnDevice");
            _onPreShowSync = serializedObject.FindProperty("_onPreShowSync");
            _onPreShow = serializedObject.FindProperty("_onPreShow");
            _onCustomShow = serializedObject.FindProperty("_onCustomShow");
            _onPostShow = serializedObject.FindProperty("_onPostShow");
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(_script), typeof(MyUGUIBooter), false);

            serializedObject.Update();

            _bootMode.enumValueIndex = (int)(MyUGUIBooter.EBootMode)EditorGUILayout.EnumPopup("Boot Mode", (MyUGUIBooter.EBootMode)_bootMode.enumValueIndex);
            _showMode.enumValueIndex = (int)(MyUGUIBooter.EShowMode)EditorGUILayout.EnumPopup("Show Mode", (MyUGUIBooter.EShowMode)_showMode.enumValueIndex);
            if ((MyUGUIBooter.EShowMode)_showMode.enumValueIndex == MyUGUIBooter.EShowMode.Default)
            {
                _defaultUnitySceneID.enumValueIndex = (int)(EUnitySceneID)EditorGUILayout.EnumPopup("Default Unity Scene", (EUnitySceneID)_defaultUnitySceneID.enumValueIndex);
                _defaultSceneID.enumValueIndex = (int)(ESceneID)EditorGUILayout.EnumPopup("Default Scene", (ESceneID)_defaultSceneID.enumValueIndex);
            }
            switch ((MyUGUIBooter.EBootMode)_bootMode.enumValueIndex)
            {
                case MyUGUIBooter.EBootMode.Instant:
                    {
                        EditorGUI.BeginChangeCheck();
                        EditorGUILayout.PropertyField(_onPreShow, new GUIContent("On Pre Show"));
                        if (EditorGUI.EndChangeCheck())
                        {
                            serializedObject.ApplyModifiedProperties();
                        }
                    }
                    break;

                case MyUGUIBooter.EBootMode.FixedTimeDelay:
                    {
                        _delayTimeOnEditor.floatValue = EditorGUILayout.FloatField("Delay Second (On Editor)", _delayTimeOnEditor.floatValue);
                        _delayTimeOnDevice.floatValue = EditorGUILayout.FloatField("Delay Second (On Device)", _delayTimeOnDevice.floatValue);
                        EditorGUI.BeginChangeCheck();
                        EditorGUILayout.PropertyField(_onPreShow, new GUIContent("On Pre Show"));
                        if (EditorGUI.EndChangeCheck())
                        {
                            serializedObject.ApplyModifiedProperties();
                        }
                    }
                    break;

                case MyUGUIBooter.EBootMode.WaitForInitializing:
                    {
                        EditorGUI.BeginChangeCheck();
                        EditorGUILayout.PropertyField(_onPreShowSync, new GUIContent("On Pre Show"));
                        if (EditorGUI.EndChangeCheck())
                        {
                            serializedObject.ApplyModifiedProperties();
                        }
                    }
                    break;
            }
            EditorGUI.BeginChangeCheck();
            if ((MyUGUIBooter.EShowMode)_showMode.enumValueIndex == MyUGUIBooter.EShowMode.Custom)
            {
                EditorGUILayout.PropertyField(_onCustomShow, new GUIContent("On Custom Show", "You should call MyUGUIManager.Instance.ShowUnityScene() here"));
            }
            EditorGUILayout.PropertyField(_onPostShow, new GUIContent("On Post Show"));
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}