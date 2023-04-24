/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUGUIConfigIDEditor (version 2.10)
 */

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace MyClasses.UI.Tool
{
    public class MyUGUIConfigIDEditorWindow : EditorWindow
    {
        #region ----- Variable -----

        private MyUGUIConfigGroups _groups;
        private Vector2 _scrollPosition;

        #endregion

        #region ----- EditorWindow MonoBehaviour -----

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            titleContent = new GUIContent("[MyClasses] ID Configuration");
            minSize = new Vector2(512, 512);

            if (!Directory.Exists("Assets/Resources/" + MyUGUIManager.CONFIG_DIRECTORY))
            {
                Directory.CreateDirectory("Assets/Resources/" + MyUGUIManager.CONFIG_DIRECTORY);
            }

            if (!Directory.Exists("Assets/Resources/" + MyUGUIManager.HUD_DIRECTORY))
            {
                Directory.CreateDirectory("Assets/Resources/" + MyUGUIManager.HUD_DIRECTORY);
            }

            if (!Directory.Exists("Assets/Resources/" + MyUGUIManager.SCENE_DIRECTORY))
            {
                Directory.CreateDirectory("Assets/Resources/" + MyUGUIManager.SCENE_DIRECTORY);
            }

            if (!Directory.Exists("Assets/Resources/" + MyUGUIManager.POPUP_DIRECTORY))
            {
                Directory.CreateDirectory("Assets/Resources/" + MyUGUIManager.POPUP_DIRECTORY);
            }

            _LoadAssetFile();
        }

        /// <summary>
        /// OnGUI.
        /// </summary>
        void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Directory");
            _groups.Directory = EditorGUILayout.TextField(_groups.Directory, GUILayout.Width(400));
            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.EndVertical();

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, new GUILayoutOption[0]);
            for (int i = 0, countI = _groups.ListGroup.Count; i < countI; i++)
            {
                MyUGUIConfigGroup group = _groups.ListGroup[i];

                EditorGUILayout.BeginHorizontal();
                group.IsFoldOut = EditorGUILayout.Foldout(group.IsFoldOut, group.Name + " (" + group.ListID.Count + ")");
                if (group.IsFoldOut)
                {
                    if (GUILayout.Button("+", GUILayout.Width(30)))
                    {
                        group.ListID.Add(string.Empty);
                    }
                    EditorGUI.BeginDisabledGroup(group.ListID.Count <= group.NumDefault);
                    if (GUILayout.Button("-", GUILayout.Width(30)))
                    {
                        if (group.ListID.Count > group.NumDefault)
                        {
                            group.ListID.RemoveAt(group.ListID.Count - 1);
                        }
                    }
                    EditorGUI.EndDisabledGroup();
                }
                EditorGUILayout.EndHorizontal();

                if (group.IsFoldOut)
                {
                    EditorGUI.indentLevel++;
                    EditorGUI.BeginDisabledGroup(true);
                    for (int j = 0; j < group.NumDefault; j++)
                    {
                        EditorGUILayout.TextField(group.ListID[j], GUILayout.Width(400));
                    }
                    EditorGUI.EndDisabledGroup();
                    for (int j = group.NumDefault, countJ = group.ListID.Count; j < countJ; j++)
                    {
                        group.ListID[j] = EditorGUILayout.TextField(group.ListID[j], GUILayout.Width(400));
                    }
                    EditorGUI.indentLevel--;
                }

                if (i < countI - 1)
                {
                    EditorGUILayout.LabelField(string.Empty);
                }
            }
            EditorGUILayout.EndScrollView();

            EditorUtility.SetDirty(_groups);

            EditorGUILayout.LabelField(string.Empty);
            if (GUILayout.Button("Apply", GUILayout.Width(100)))
            {
                _GenerateScript();
            }
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Load the asset file.
        /// </summary>
        private void _LoadAssetFile()
        {
            if (_groups != null)
            {
                return;
            }

            string filePath = "Assets/Resources/" + MyUGUIManager.CONFIG_DIRECTORY + typeof(MyUGUIConfigGroups).Name + ".asset";
            _groups = AssetDatabase.LoadAssetAtPath(filePath, typeof(MyUGUIConfigGroups)) as MyUGUIConfigGroups;
            if (_groups == null)
            {
                _groups = ScriptableObject.CreateInstance<MyUGUIConfigGroups>();
                _groups.ListGroup = new List<MyUGUIConfigGroup>();
                _groups.ListGroup.Add(new MyUGUIConfigGroup()
                {
                    IsFoldOut = true,
                    Name = "EUnitySceneID",
                    NumDefault = 0,
                    ListID = new List<string>() { "StartupUnityScene", "MainUnityScene", "GameUnityScene" }
                });
                _groups.ListGroup.Add(new MyUGUIConfigGroup()
                {
                    IsFoldOut = true,
                    Name = "ESceneID",
                    NumDefault = 0,
                    ListID = new List<string>()
                });
                _groups.ListGroup.Add(new MyUGUIConfigGroup()
                {
                    IsFoldOut = true,
                    Name = "EPopupID",
                    NumDefault = 3,
                    ListID = new List<string>() { "Dialog0ButtonPopup", "Dialog1ButtonPopup", "Dialog2ButtonsPopup" }
                });
                AssetDatabase.CreateAsset(_groups, filePath);
                AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        /// Generate the script.
        /// </summary>
        private void _GenerateScript()
        {
            if (_groups == null)
            {
                return;
            }

            string scriptName = typeof(MyUGUIConfigIDEditorWindow).Name;
            string unityScenes = string.Empty;
            for (int i = 0, countI = _groups.ListGroup[0].ListID.Count; i < countI; i++)
            {
                unityScenes += "\n\t\t" + _groups.ListGroup[0].ListID[i] + ",";
            }
            string scenes = string.Empty;
            for (int i = 0, countI = _groups.ListGroup[1].ListID.Count; i < countI; i++)
            {
                scenes += "\n\t\t" + _groups.ListGroup[1].ListID[i] + ",";
            }
            string popups = string.Empty;
            for (int i = 0, countI = _groups.ListGroup[2].ListID.Count; i < countI; i++)
            {
                popups += "\n\t\t" + _groups.ListGroup[2].ListID[i] + ",";
            }
            string content = "/*\n * Copyright (c) 2016 Phạm Minh Hoàng\n * Email:\t\thoangpham61691@gmail.com\n * Framework:\tMyClasses\n * Description:\tThis script is generated by " + scriptName + "\n */\n\nnamespace MyClasses.UI\n{\n\tpublic enum EUnitySceneID\n\t{" + unityScenes + "\n\t}\n\n\tpublic enum ESceneID\n\t{" + scenes + "\n\t}\n\n\tpublic enum EPopupID\n\t{" + popups + "\n\t}\n}";

            if (_groups.Directory.Length == 0)
            {
                _groups.Directory = "Assets/";
            }
            else if (_groups.Directory[_groups.Directory.Length - 1] != '/')
            {
                _groups.Directory += "/";
            }
            if (!Directory.Exists(_groups.Directory))
            {
                Directory.CreateDirectory(_groups.Directory);
            }
            if (Directory.Exists(_groups.Directory))
            {
                File.WriteAllText(_groups.Directory + "MyUGUIConfigID.cs", content);
                AssetDatabase.Refresh();
                Debug.Log("[MyClasses] MyUGUIConfigID was created.");
                return;
            }

            Debug.LogError("[MyClasses] Could not find MyClasses location.");
        }

        #endregion
    }
}