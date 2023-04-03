/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUGUIConfigPopupEditorWindow (version 2.9)
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

        private MyUGUIConfigPopups _popups;
        private Vector2 _scrollPosition;

        private string[] _scriptPaths;
        private string[] _prefabNames;

        #endregion

        #region ----- EditorWindow MonoBehaviour -----

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            titleContent = new GUIContent("[MyClasses] Popup Configuration");
            minSize = new Vector2(1024, 512);

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

        /// <summary>
        /// OnGUI.
        /// </summary>
        void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, new GUILayoutOption[0]);
            for (int i = 0, countI = _popups.ListPopup.Count; i < countI; i++)
            {
                MyUGUIConfigPopup popup = _popups.ListPopup[i];

                popup.IsFoldOut = EditorGUILayout.Foldout(popup.IsFoldOut, popup.ID.ToString());
                if (popup.IsFoldOut)
                {
                    EditorGUI.indentLevel++;
                    if (i < _popups.NumDefault)
                    {
                        EditorGUI.BeginDisabledGroup(i < _popups.NumDefault);
                        EditorGUILayout.TextField("Script", popup.ScriptPath + ".cs", GUILayout.Width(400));
                        EditorGUILayout.TextField("Prefab Canvas", popup.PrefabName + ".prefab", GUILayout.Width(400));
                        EditorGUILayout.TextField("Prefab 3D", popup.PrefabName3D + ".prefab", GUILayout.Width(400));
                        EditorGUI.EndDisabledGroup();
                    }
                    else
                    {
                        popup.ScriptPathIndex = EditorGUILayout.Popup("Script", popup.ScriptPathIndex, _scriptPaths);
                        popup.ScriptPath = _scriptPaths[popup.ScriptPathIndex];
                        if (popup.ScriptPathIndex > 0)
                        {
                            popup.ScriptName = popup.ScriptPath.Substring(popup.ScriptPath.LastIndexOf('/') + 1);
                            popup.ScriptName = popup.ScriptName.Replace(".cs", string.Empty);
                        }
                        else
                        {
                            popup.ScriptName = string.Empty;
                        }

                        if (popup.PrefabNameIndex == -1)
                        {
                            popup.PrefabNameIndex = _prefabNames.Length - 1;
                        }
                        popup.PrefabNameIndex = EditorGUILayout.Popup("Prefab Canvas", popup.PrefabNameIndex, _prefabNames);
                        popup.PrefabName = _prefabNames[popup.PrefabNameIndex];
                        popup.PrefabName = popup.PrefabName.Equals("<null>") ? string.Empty : popup.PrefabName.Substring(0, popup.PrefabName.Length - 7);

                        if (popup.PrefabNameIndex3D == -1)
                        {
                            popup.PrefabNameIndex3D = _prefabNames.Length - 1;
                        }
                        popup.PrefabNameIndex3D = EditorGUILayout.Popup("Prefab 3D", popup.PrefabNameIndex3D, _prefabNames);
                        popup.PrefabName3D = _prefabNames[popup.PrefabNameIndex3D];
                        popup.PrefabName3D = popup.PrefabName3D.Equals("<null>") ? string.Empty : popup.PrefabName3D.Substring(0, popup.PrefabName3D.Length - 7);
                    }
                    EditorGUI.indentLevel--;
                }

                if (i < countI - 1)
                {
                    EditorGUILayout.LabelField(string.Empty);
                }
            }
            EditorGUILayout.EndScrollView();

            EditorUtility.SetDirty(_popups);

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

            _popups = null;
        }

        /// <summary>
        /// Load the asset file.
        /// </summary>
        private void _LoadAssetFile()
        {
            if (_popups != null)
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
            _popups = AssetDatabase.LoadAssetAtPath(filePath, typeof(MyUGUIConfigPopups)) as MyUGUIConfigPopups;
            if (_popups == null)
            {
                _popups = ScriptableObject.CreateInstance<MyUGUIConfigPopups>();
                _popups.ListPopup = new List<MyUGUIConfigPopup>();
                _popups.ListPopup.Add(new MyUGUIConfigPopup()
                {
                    IsFoldOut = true,
                    ID = EPopupID.Dialog0ButtonPopup,
                    ScriptPath = frameworkDirectory + "/" + typeof(MyUGUIPopup0Button).Name + ".cs",
                    ScriptName = typeof(MyUGUIPopup0Button).Name,
                    PrefabName = EPopupID.Dialog0ButtonPopup.ToString()
                });
                _popups.ListPopup.Add(new MyUGUIConfigPopup()
                {
                    IsFoldOut = true,
                    ID = EPopupID.Dialog1ButtonPopup,
                    ScriptPath = frameworkDirectory + "/" + typeof(MyUGUIPopup1Button).Name + ".cs",
                    ScriptName = typeof(MyUGUIPopup1Button).Name,
                    PrefabName = EPopupID.Dialog1ButtonPopup.ToString()
                });
                _popups.ListPopup.Add(new MyUGUIConfigPopup()
                {
                    IsFoldOut = true,
                    ID = EPopupID.Dialog2ButtonsPopup,
                    ScriptPath = frameworkDirectory + "/" + typeof(MyUGUIPopup2Buttons).Name + ".cs",
                    ScriptName = typeof(MyUGUIPopup2Buttons).Name,
                    PrefabName = EPopupID.Dialog2ButtonsPopup.ToString()
                });
                _popups.NumDefault = _popups.ListPopup.Count;
                AssetDatabase.CreateAsset(_popups, filePath);
                AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        /// Update new popups.
        /// </summary>
        private void _UpdateNewPopups()
        {
            if (_popups == null)
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
                for (int i = 0, countI = _popups.ListPopup.Count; i < countI; i++)
                {
                    if (_popups.ListPopup[i].ID == item)
                    {
                        isNewPopup = false;
                        break;
                    }
                }
                if (isNewPopup)
                {
                    _popups.ListPopup.Add(new MyUGUIConfigPopup()
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
            _scriptPaths = _GetPopupScriptPaths();
            _prefabNames = _GetPopupPrefabNames();

            for (int i = 0, countI = _popups.ListPopup.Count; i < countI; i++)
            {
                MyUGUIConfigPopup popup = _popups.ListPopup[i];

                if (popup.ScriptPathIndex >= _scriptPaths.Length || !popup.ScriptPath.Equals(_scriptPaths[popup.ScriptPathIndex]))
                {
                    popup.ScriptPathIndex = 0;
                    for (int j = 0; j < _scriptPaths.Length; j++)
                    {
                        if (popup.ScriptPath.Equals(_scriptPaths[j]))
                        {
                            popup.ScriptPathIndex = j;
                            break;
                        }
                    }
                    if (popup.ScriptPathIndex > 0)
                    {
                        popup.ScriptPath = _scriptPaths[popup.ScriptPathIndex];
                        popup.ScriptName = popup.ScriptPath.Substring(popup.ScriptPath.LastIndexOf('/') + 1);
                        popup.ScriptName = popup.ScriptName.Replace(".cs", string.Empty);
                    }
                    else
                    {
                        popup.ScriptName = string.Empty;
                    }
                }

                if (popup.PrefabNameIndex >= _prefabNames.Length || (popup.PrefabNameIndex3D >= 0 && !popup.PrefabName.Equals(_prefabNames[popup.PrefabNameIndex])))
                {
                    string prefabName = popup.PrefabName + ".prefab";
                    popup.PrefabNameIndex = -1;
                    for (int j = 0; j < _prefabNames.Length; j++)
                    {
                        if (prefabName.Equals(_prefabNames[j]))
                        {
                            popup.PrefabNameIndex = j;
                            break;
                        }
                    }
                    if (popup.PrefabNameIndex == -1)
                    {
                        popup.PrefabNameIndex = _prefabNames.Length - 1;
                    }
                    if (popup.PrefabNameIndex >= 0)
                    {
                        popup.PrefabName = _prefabNames[popup.PrefabNameIndex];
                        popup.PrefabName = popup.PrefabName.Equals("<null>") ? string.Empty : popup.PrefabName.Substring(0, popup.PrefabName.Length - 7);
                    }
                }

                if (popup.PrefabNameIndex3D >= _prefabNames.Length || (popup.PrefabNameIndex3D >= 0 && !popup.PrefabName3D.Equals(_prefabNames[popup.PrefabNameIndex3D])))
                {
                    string prefabName = popup.PrefabName3D + ".prefab";
                    popup.PrefabNameIndex3D = -1;
                    for (int j = 0; j < _prefabNames.Length; j++)
                    {
                        if (prefabName.Equals(_prefabNames[j]))
                        {
                            popup.PrefabNameIndex3D = j;
                            break;
                        }
                    }
                    if (popup.PrefabNameIndex3D == -1)
                    {
                        popup.PrefabNameIndex3D = _prefabNames.Length - 1;
                    }
                    if (popup.PrefabNameIndex3D >= 0)
                    {
                        popup.PrefabName3D = _prefabNames[popup.PrefabNameIndex3D];
                        popup.PrefabName3D = popup.PrefabName3D.Equals("<null>") ? string.Empty : popup.PrefabName3D.Substring(0, popup.PrefabName3D.Length - 7);
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

            listPrefabNames.Add("<null>");

            return listPrefabNames.ToArray();
        }

        #endregion
    }
}
