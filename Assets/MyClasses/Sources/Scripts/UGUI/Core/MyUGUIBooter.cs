﻿/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUGUIConfig (version 2.10)
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
        private EUnitySceneID mDefaultUnitySceneID;
        [SerializeField]
        private ESceneID mDefaultSceneID;
        [SerializeField]
        private EBootMode mBootMode = 0;
        [SerializeField]
        private float mDelayTimeOnEditor = 0;
        [SerializeField]
        private float mDelayTimeOnDevice = 0;
        [SerializeField]
        private UnityEventAction mOnPreLoadSync;
        [SerializeField]
        private UnityEventVoid mOnPreLoad;
        [SerializeField]
        private UnityEventVoid mOnPostLoad;

        private static bool mIsBooted = false;

        #endregion

        #region ----- Implement MonoBehaviour -----

        /// <summary>
        /// Start.
        /// </summary>
        void Start()
        {
            if (mIsBooted)
            {
                return;
            }

            switch (mBootMode)
            {
                case EBootMode.Instant:
                    {
                        if (mOnPreLoad != null)
                        {
                            mOnPreLoad.Invoke();
                        }

                        _ShowDefaultScene();
                    }
                    break;
                case EBootMode.FixedTimeDelay:
                    {
                        if (mOnPreLoad != null)
                        {
                            mOnPreLoad.Invoke();
                        }

#if UNITY_EDITOR
                        if (mDelayTimeOnEditor > 0)
                        {
                            StartCoroutine(_ShowDefaultSceneWithDelay(mDelayTimeOnEditor));
                        }
#else
                        if (mDelayTimeOnDevice > 0)
                        {
                            StartCoroutine(_ShowDefaultSceneWithDelay(mDelayTimeOnDevice));
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
                        if (mOnPreLoadSync != null)
                        {
                            mOnPreLoadSync.Invoke(_ShowDefaultScene);
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
            MyUGUIManager.Instance.ShowUnityScene(mDefaultUnitySceneID, mDefaultSceneID);
            mIsBooted = true;

            if (mOnPostLoad != null)
            {
                mOnPostLoad.Invoke();
            }
        }

        #endregion

        #region ----- Enumeration -----

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
        private MyUGUIBooter mScript;
        private SerializedProperty mDefaultUnitySceneID;
        private SerializedProperty mDefaultSceneID;
        private SerializedProperty mBootMode;
        private SerializedProperty mDelayTimeOnEditor;
        private SerializedProperty mDelayTimeOnDevice;
        private SerializedProperty mOnPreLoadSync;
        private SerializedProperty mOnPreLoad;
        private SerializedProperty mOnPostLoad;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyUGUIBooter)target;
            mDefaultUnitySceneID = serializedObject.FindProperty("mDefaultUnitySceneID");
            mDefaultSceneID = serializedObject.FindProperty("mDefaultSceneID");
            mBootMode = serializedObject.FindProperty("mBootMode");
            mDelayTimeOnEditor = serializedObject.FindProperty("mDelayTimeOnEditor");
            mDelayTimeOnDevice = serializedObject.FindProperty("mDelayTimeOnDevice");
            mOnPreLoadSync = serializedObject.FindProperty("mOnPreLoadSync");
            mOnPreLoad = serializedObject.FindProperty("mOnPreLoad");
            mOnPostLoad = serializedObject.FindProperty("mOnPostLoad");
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyUGUIBooter), false);

            serializedObject.Update();

            mDefaultUnitySceneID.enumValueIndex = (int)(EUnitySceneID)EditorGUILayout.EnumPopup("Default Unity Scene", (EUnitySceneID)mDefaultUnitySceneID.enumValueIndex);
            mDefaultSceneID.enumValueIndex = (int)(ESceneID)EditorGUILayout.EnumPopup("Default Scene", (ESceneID)mDefaultSceneID.enumValueIndex);
            mBootMode.enumValueIndex = (int)(MyUGUIBooter.EBootMode)EditorGUILayout.EnumPopup("Boot Mode", (MyUGUIBooter.EBootMode)mBootMode.enumValueIndex);
            switch ((MyUGUIBooter.EBootMode)mBootMode.enumValueIndex)
            {
                case MyUGUIBooter.EBootMode.Instant:
                    {
                        EditorGUI.BeginChangeCheck();
                        EditorGUILayout.PropertyField(mOnPreLoad, new GUIContent("On Pre Load"));
                        if (EditorGUI.EndChangeCheck())
                        {
                            serializedObject.ApplyModifiedProperties();
                        }
                    }
                    break;
                case MyUGUIBooter.EBootMode.FixedTimeDelay:
                    {
                        mDelayTimeOnEditor.floatValue = EditorGUILayout.FloatField("Delay Second (On Editor)", mDelayTimeOnEditor.floatValue);
                        mDelayTimeOnDevice.floatValue = EditorGUILayout.FloatField("Delay Second (On Device)", mDelayTimeOnDevice.floatValue);
                        EditorGUI.BeginChangeCheck();
                        EditorGUILayout.PropertyField(mOnPreLoad, new GUIContent("On Pre Load"));
                        if (EditorGUI.EndChangeCheck())
                        {
                            serializedObject.ApplyModifiedProperties();
                        }
                    }
                    break;
                case MyUGUIBooter.EBootMode.WaitForInitializing:
                    {
                        EditorGUI.BeginChangeCheck();
                        EditorGUILayout.PropertyField(mOnPreLoadSync, new GUIContent("On Pre Load"));
                        if (EditorGUI.EndChangeCheck())
                        {
                            serializedObject.ApplyModifiedProperties();
                        }
                    }
                    break;
            }
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(mOnPostLoad, new GUIContent("On Post Load"));
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}