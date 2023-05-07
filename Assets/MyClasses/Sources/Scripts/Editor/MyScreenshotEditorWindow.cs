/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyScreenshotEditorWindow (version 1.0)
 */

using UnityEditor;
using UnityEngine;
using System;
using System.IO;

namespace MyClasses.Tool
{
    public class MyScreenshotEditorWindow : EditorWindow
    {
        #region ----- Variable -----

        private const string KEY_DIRECTORY = "MyScreenshotEditorWindow_Directory";
        private const string KEY_SUPER_SIZE = "MyScreenshotEditorWindow_SuperSize";

        #endregion

        #region ----- Variable -----

        private string _lastPath;
        private string _directory;
        private int _superSize;
        private Vector2 _resolution;
        private Vector2 _size;

        #endregion

        #region ----- EditorWindow Implementation -----

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            titleContent = new GUIContent("[MyClasses] Screenshot");
            minSize = new Vector2(512, 768);

            _directory = PlayerPrefs.GetString(KEY_DIRECTORY, "Assets/Screenshot/");
            _superSize = PlayerPrefs.GetInt(KEY_SUPER_SIZE, 1);
        }

        /// <summary>
        /// OnDisable.
        /// </summary>
        void OnDisable()
        {
            PlayerPrefs.SetString(KEY_DIRECTORY, _directory);
            PlayerPrefs.SetInt(KEY_SUPER_SIZE, _superSize);
        }

        /// <summary>
        /// OnGUI.
        /// </summary>
        void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            _directory = EditorGUILayout.TextField("Directory", _directory, GUILayout.Width(400));
            if (!_directory.EndsWith("/"))
            {
                _directory += "/";
            }
            if (GUILayout.Button("Browse", GUILayout.Width(400)))
            {
                _directory = EditorUtility.OpenFolderPanel("Save screenshots folder", _directory, "") + "/";
            }
            _superSize = EditorGUILayout.IntField("Super Size", _superSize, GUILayout.Width(400));
            _resolution = UnityEditor.Handles.GetMainGameViewSize();
            _size = _resolution * _superSize;
            EditorGUILayout.LabelField("Resolution=" + _resolution.x + "x" + _resolution.y);
            EditorGUILayout.LabelField("Size=" + _size.x + "x" + _size.y);
            if (GUILayout.Button("Take a Screenshot (Space bar)", GUILayout.Width(400)) 
                || (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Space))
            {
                if (!Directory.Exists(_directory))
                {
                    Directory.CreateDirectory(_directory);
                }

                PlayerPrefs.SetString(KEY_DIRECTORY, _directory);
                PlayerPrefs.SetFloat(KEY_SUPER_SIZE, _superSize);

                string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH'-'mm'-'ss");
                _lastPath = _directory + currentTime + ".png";
                RenderTexture renderTexture = RenderTexture.GetTemporary((int)_size.x, (int)_size.y, 32, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
                RenderTexture.active = renderTexture;
                ScreenCapture.CaptureScreenshot(_lastPath, _superSize);
            }
            if (GUILayout.Button("Refresh Editor", GUILayout.Width(400)))
            {
                AssetDatabase.Refresh();
            }

            if (_lastPath != null)
            {
                EditorGUILayout.LabelField(string.Empty);
                EditorGUILayout.LabelField("___________________________________________________________________________________", GUILayout.MaxWidth(400));
                EditorGUILayout.LabelField(string.Empty);
                GUIStyle styleCenter = new GUIStyle(GUI.skin.label);
                styleCenter.alignment = TextAnchor.MiddleCenter;
                EditorGUILayout.LabelField("A screenshot saved at\n" + _lastPath, GUILayout.MaxWidth(400), GUILayout.Height(32));
                if (GUILayout.Button("Reveal in Explorer", GUILayout.Width(400)))
                {
                    PlayerPrefs.GetString("MyScreenshotEditorWindow_Directory", _directory);
                    if (_lastPath != null && File.Exists(_lastPath) && _lastPath.StartsWith(_directory))
                    {
                        EditorUtility.RevealInFinder(_lastPath);
                    }
                    else
                    {
                        if (!Directory.Exists(_directory))
                        {
                            Directory.CreateDirectory(_directory);
                        }
                        EditorUtility.RevealInFinder(_directory);
                    }
                }
            }

            EditorGUILayout.EndVertical();
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Take a screenshot.
        /// </summary>
        private void _CaptureScreenshot()
        {
            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }

            PlayerPrefs.SetString(KEY_DIRECTORY, _directory);
            PlayerPrefs.SetFloat(KEY_SUPER_SIZE, _superSize);

            string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH'-'mm'-'ss");
            _lastPath = _directory + currentTime + ".png";
            RenderTexture renderTexture = RenderTexture.GetTemporary((int)_size.x, (int)_size.y, 32, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
            RenderTexture.active = renderTexture;
            ScreenCapture.CaptureScreenshot(_lastPath, _superSize);
            AssetDatabase.Refresh();
        }

        #endregion
    }
}