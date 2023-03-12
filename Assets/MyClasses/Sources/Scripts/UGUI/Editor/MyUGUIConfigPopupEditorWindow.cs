/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUGUIConfigPopupEditorWindow (version 2.7)
 */

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;

namespace MyClasses.UI.Tool
{
    public class MyUGUIConfigPopupEditorWindow : EditorWindow
    {
        #region ----- Variable -----

        private MyUGUIConfigPopups mPopups;
        private Vector2 mScrollPosition;

        private string[] mScriptPaths;
        private string[] mPrefabNames;

        #endregion

        #region ----- Implement MonoBehaviour -----

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
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
            _UpdateNewPopups();
            _CorrectValues();
        }

        /// <summary>
        /// OnFocus.
        /// </summary>
        void OnFocus()
        {
            _CorrectValues();
        }

        #endregion

        #region ----- GUI Implementation -----

        /// <summary>
        /// OnGUI.
        /// </summary>
        void OnGUI()
        {
            mScrollPosition = EditorGUILayout.BeginScrollView(mScrollPosition, new GUILayoutOption[0]);
            for (int i = 0, countI = mPopups.ListPopup.Count; i < countI; i++)
            {
                MyUGUIConfigPopup popup = mPopups.ListPopup[i];

                popup.IsFoldOut = EditorGUILayout.Foldout(popup.IsFoldOut, popup.ID.ToString());
                if (popup.IsFoldOut)
                {
                    EditorGUI.indentLevel++;
                    if (i < mPopups.NumDefault)
                    {
                        EditorGUI.BeginDisabledGroup(i < mPopups.NumDefault);
                        EditorGUILayout.TextField("Script", popup.ScriptPath + ".cs", GUILayout.Width(400));
                        EditorGUILayout.TextField("Prefab", popup.PrefabName + ".prefab", GUILayout.Width(400));
                        EditorGUI.EndDisabledGroup();
                    }
                    else
                    {
                        popup.ScriptPathIndex = EditorGUILayout.Popup("Script", popup.ScriptPathIndex, mScriptPaths);
                        popup.ScriptPath = mScriptPaths[popup.ScriptPathIndex];
                        if (popup.ScriptPathIndex > 0)
                        {
                            popup.ScriptName = popup.ScriptPath.Substring(popup.ScriptPath.LastIndexOf('/') + 1);
                            popup.ScriptName = popup.ScriptName.Replace(".cs", string.Empty);
                        }
                        else
                        {
                            popup.ScriptName = string.Empty;
                        }

                        popup.PrefabNameIndex = EditorGUILayout.Popup("Prefab", popup.PrefabNameIndex, mPrefabNames);
                        popup.PrefabName = mPrefabNames[popup.PrefabNameIndex];
                        popup.PrefabName = popup.PrefabName.Equals("<null>") ? string.Empty : popup.PrefabName.Substring(0, popup.PrefabName.Length - 7);
                    }
                    EditorGUI.indentLevel--;
                }

                if (i < countI - 1)
                {
                    EditorGUILayout.LabelField(string.Empty);
                }
            }
            EditorGUILayout.EndScrollView();

            EditorUtility.SetDirty(mPopups);

            EditorGUILayout.LabelField(string.Empty);
            if (GUILayout.Button("Reset", GUILayout.Width(100)))
            {
                _DeleteAssetFile();
                _LoadAssetFile();
                _UpdateNewPopups();

                Debug.Log("[MyClasses] Data was reset.");
            }
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Delete the asset file.
        /// </summary>
        private void _DeleteAssetFile()
        {
            string filePath = "Assets/Resources/" + MyUGUIManager.CONFIG_DIRECTORY + typeof(MyUGUIConfigPopups).Name + ".asset";
            if (File.Exists(filePath))
            {
                AssetDatabase.DeleteAsset(filePath);
            }

            mPopups = null;
        }

        /// <summary>
        /// Load the asset file.
        /// </summary>
        private void _LoadAssetFile()
        {
            if (mPopups != null)
            {
                return;
            }

            string frameworkDirectory = null;
            string[] frameworkDirectories = new string[] { "Assets/MyClasses", "Assets/Core/MyClasses", "Assets/Plugin/MyClasses", "Assets/Plugins/MyClasses", "Assets/Framework/MyClasses", "Assets/Frameworks/MyClasses" };
            for (int i = 0; i < frameworkDirectories.Length; i++)
            {
                frameworkDirectory = frameworkDirectories[i] + "/Sources/Scripts/UGUI/Core";
                if (Directory.Exists(frameworkDirectory))
                {
                    break;
                }
            }

            string filePath = "Assets/Resources/" + MyUGUIManager.CONFIG_DIRECTORY + typeof(MyUGUIConfigPopups).Name + ".asset";
            mPopups = AssetDatabase.LoadAssetAtPath(filePath, typeof(MyUGUIConfigPopups)) as MyUGUIConfigPopups;
            if (mPopups == null)
            {
                mPopups = ScriptableObject.CreateInstance<MyUGUIConfigPopups>();
                mPopups.ListPopup = new List<MyUGUIConfigPopup>();
                mPopups.ListPopup.Add(new MyUGUIConfigPopup()
                {
                    IsFoldOut = true,
                    ID = EPopupID.Dialog0ButtonPopup,
                    ScriptPath = frameworkDirectory + "/" + typeof(MyUGUIPopup0Button).Name + ".cs",
                    ScriptName = typeof(MyUGUIPopup0Button).Name,
                    PrefabName = EPopupID.Dialog0ButtonPopup.ToString()
                });
                mPopups.ListPopup.Add(new MyUGUIConfigPopup()
                {
                    IsFoldOut = true,
                    ID = EPopupID.Dialog1ButtonPopup,
                    ScriptPath = frameworkDirectory + "/" + typeof(MyUGUIPopup1Button).Name + ".cs",
                    ScriptName = typeof(MyUGUIPopup1Button).Name,
                    PrefabName = EPopupID.Dialog1ButtonPopup.ToString()
                });
                mPopups.ListPopup.Add(new MyUGUIConfigPopup()
                {
                    IsFoldOut = true,
                    ID = EPopupID.Dialog2ButtonsPopup,
                    ScriptPath = frameworkDirectory + "/" + typeof(MyUGUIPopup2Buttons).Name + ".cs",
                    ScriptName = typeof(MyUGUIPopup2Buttons).Name,
                    PrefabName = EPopupID.Dialog2ButtonsPopup.ToString()
                });
                mPopups.NumDefault = mPopups.ListPopup.Count;
                AssetDatabase.CreateAsset(mPopups, filePath);
                AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        /// Update new popups.
        /// </summary>
        private void _UpdateNewPopups()
        {
            if (mPopups == null)
            {
                return;
            }

            foreach (EPopupID item in Enum.GetValues(typeof(EPopupID)))
            {
                if (item == EPopupID.Dialog0ButtonPopup || item == EPopupID.Dialog1ButtonPopup || item == EPopupID.Dialog2ButtonsPopup)
                {
                    continue;
                }

                bool isNewPopup = true;
                for (int i = 0, countI = mPopups.ListPopup.Count; i < countI; i++)
                {
                    if (mPopups.ListPopup[i].ID == item)
                    {
                        isNewPopup = false;
                        break;
                    }
                }
                if (isNewPopup)
                {
                    mPopups.ListPopup.Add(new MyUGUIConfigPopup()
                    {
                        IsFoldOut = true,
                        ID = item,
                        ScriptPath = string.Empty,
                        PrefabName = string.Empty,
                    });
                }
            }
        }

        /// <summary>
        /// Correct values.
        /// </summary>
        private void _CorrectValues()
        {
            mScriptPaths = _GetPopupScriptPaths();
            mPrefabNames = _GetPopupPrefabNames();

            for (int i = 0, countI = mPopups.ListPopup.Count; i < countI; i++)
            {
                MyUGUIConfigPopup popup = mPopups.ListPopup[i];

                if (popup.ScriptPathIndex >= mScriptPaths.Length || !popup.ScriptPath.Equals(mScriptPaths[popup.ScriptPathIndex]))
                {
                    popup.ScriptPathIndex = 0;
                    for (int j = 0; j < mScriptPaths.Length; j++)
                    {
                        if (popup.ScriptPath.Equals(mScriptPaths[j]))
                        {
                            popup.ScriptPathIndex = j;
                            break;
                        }
                    }
                    if (popup.ScriptPathIndex > 0)
                    {
                        popup.ScriptPath = mScriptPaths[popup.ScriptPathIndex];
                        popup.ScriptName = popup.ScriptPath.Substring(popup.ScriptPath.LastIndexOf('/') + 1);
                        popup.ScriptName = popup.ScriptName.Replace(".cs", string.Empty);
                    }
                    else
                    {
                        popup.ScriptName = string.Empty;
                    }
                }

                if (popup.PrefabNameIndex >= mPrefabNames.Length || !popup.PrefabName.Equals(mPrefabNames[popup.PrefabNameIndex]))
                {
                    string prefabName = popup.PrefabName + ".prefab";
                    popup.PrefabNameIndex = 0;
                    for (int j = 0; j < mPrefabNames.Length; j++)
                    {
                        if (prefabName.Equals(mPrefabNames[j]))
                        {
                            popup.PrefabNameIndex = j;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Return list popup script path.
        /// </summary>
        private string[] _GetPopupScriptPaths()
        {
            List<string> listScriptPaths = new List<string>();

            string[] scriptPaths = Directory.GetFiles("Assets/", "*.cs", SearchOption.AllDirectories);
            if (scriptPaths != null && scriptPaths.Length > 0)
            {
                foreach (string scriptPath in scriptPaths)
                {
                    if (!(scriptPath.StartsWith("MyClasses", StringComparison.Ordinal)
                          || scriptPath.StartsWith("Assets/Core", StringComparison.Ordinal)
                          || scriptPath.StartsWith("Assets/Framework", StringComparison.Ordinal)
                          || scriptPath.StartsWith("Assets/Plugin", StringComparison.Ordinal)))
                    {
#if UNITY_EDITOR_WIN
                        listScriptPaths.Add(scriptPath.Replace('\\', '/'));
#else
                        listScriptPaths.Add(scriptPath);
#endif
                    }
                }
            }

            if (listScriptPaths.Count == 0)
            {
                listScriptPaths.Add("<null>");
            }

            return listScriptPaths.ToArray();
        }

        /// <summary>
        /// Return list popup prefab name.
        /// </summary>
        private string[] _GetPopupPrefabNames()
        {
            List<string> listPrefabNames = new List<string>();

            string[] prefabNames = Directory.GetFiles("Assets/Resources/" + MyUGUIManager.POPUP_DIRECTORY, "*.prefab");
            if (prefabNames != null && prefabNames.Length > 0)
            {
                foreach (string prefabName in prefabNames)
                {
                    listPrefabNames.Add(prefabName.Substring(prefabName.LastIndexOf('/') + 1));
                }
            }

            listPrefabNames.Remove(EPopupID.Dialog0ButtonPopup.ToString() + ".prefab");
            listPrefabNames.Remove(EPopupID.Dialog1ButtonPopup.ToString() + ".prefab");
            listPrefabNames.Remove(EPopupID.Dialog2ButtonsPopup.ToString() + ".prefab");

            if (listPrefabNames.Count == 0)
            {
                listPrefabNames.Add("<null>");
            }

            return listPrefabNames.ToArray();
        }

        #endregion
    }
}
