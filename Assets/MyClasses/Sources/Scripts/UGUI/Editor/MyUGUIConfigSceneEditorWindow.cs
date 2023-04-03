/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUGUIConfigSceneEditorWindow (version 2.9)
 */

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;

namespace MyClasses.UI.Tool
{
    public class MyUGUIConfigSceneEditorWindow : EditorWindow
    {
        #region ----- Variable -----

        private MyUGUIConfigUnityScenes _unityScenes;
        private Vector2 _scrollPosition;

        private string[] _scriptPaths;
        private string[] _unitySceneNames;
        private string[] _scenePrefabNames;
        private string[] _hudPrefabNames;

        #endregion

        #region ----- EditorWindow MonoBehaviour -----

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            titleContent = new GUIContent("[MyClasses] Scene Configuration");
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
            _AddNewUnityScenes();
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
            for (int i = 0, countI = _unityScenes.ListUnityScene.Count; i < countI; i++)
            {
                MyUGUIConfigUnityScene unityScene = _unityScenes.ListUnityScene[i];

                EditorGUILayout.BeginHorizontal();
                unityScene.IsFoldOut = EditorGUILayout.Foldout(unityScene.IsFoldOut, unityScene.ID.ToString());
                if (unityScene.IsFoldOut)
                {
                    if (GUILayout.Button("+", GUILayout.Width(30)))
                    {
                        unityScene.ListScene.Add(new MyUGUIConfigScene()
                        {
                            IsFoldOut = true,
                            IsInitWhenLoadUnityScene = false,
                            IsHideHUD = false,
                            FadeInDuration = 0.2f,
                            FadeOutDuration = 0.2f,
                        });
                    }
                    EditorGUI.BeginDisabledGroup(unityScene.ListScene.Count == 0);
                    if (GUILayout.Button("-", GUILayout.Width(30)))
                    {
                        unityScene.ListScene.RemoveAt(unityScene.ListScene.Count - 1);
                    }
                    EditorGUI.EndDisabledGroup();
                }
                EditorGUILayout.EndHorizontal();

                if (unityScene.IsFoldOut)
                {
                    EditorGUI.indentLevel++;

                    unityScene.SceneNameIndex = EditorGUILayout.Popup("Unity Scene", unityScene.SceneNameIndex, _unitySceneNames);
                    unityScene.SceneName = _unitySceneNames[unityScene.SceneNameIndex];
                    unityScene.SceneName = unityScene.SceneName.Equals("<null>") ? string.Empty : unityScene.SceneName.Substring(0, unityScene.SceneName.Length - 6);

                    unityScene.HUDScriptPathIndex = EditorGUILayout.Popup("HUD Script (Nullable)", unityScene.HUDScriptPathIndex, _scriptPaths);
                    unityScene.HUDScriptPath = _scriptPaths[unityScene.HUDScriptPathIndex];
                    if (unityScene.HUDScriptPathIndex > 0)
                    {
                        unityScene.HUDScriptName = unityScene.HUDScriptPath.Substring(unityScene.HUDScriptPath.LastIndexOf('/') + 1);
                        unityScene.HUDScriptName = unityScene.HUDScriptName.Replace(".cs", string.Empty);
                    }
                    else
                    {
                        unityScene.HUDScriptName = string.Empty;
                    }

                    unityScene.HUDPrefabNameIndex = EditorGUILayout.Popup("HUD Prefab Canvas (Nullable)", unityScene.HUDPrefabNameIndex, _hudPrefabNames);
                    unityScene.HUDPrefabName = _hudPrefabNames[unityScene.HUDPrefabNameIndex];
                    unityScene.HUDPrefabName = unityScene.HUDPrefabName.Equals("<null>") ? string.Empty : unityScene.HUDPrefabName.Substring(0, unityScene.HUDPrefabName.Length - 7);

                    unityScene.HUDPrefabNameIndex3D = EditorGUILayout.Popup("HUD Prefab 3D (Nullable)", unityScene.HUDPrefabNameIndex3D, _hudPrefabNames);
                    unityScene.HUDPrefabName3D = _hudPrefabNames[unityScene.HUDPrefabNameIndex3D];
                    unityScene.HUDPrefabName3D = unityScene.HUDPrefabName3D.Equals("<null>") ? string.Empty : unityScene.HUDPrefabName3D.Substring(0, unityScene.HUDPrefabName3D.Length - 7);

                    for (int j = 0, countJ = unityScene.ListScene.Count; j < countJ; j++)
                    {
                        MyUGUIConfigScene scene = unityScene.ListScene[j];

                        scene.IsFoldOut = EditorGUILayout.Foldout(scene.IsFoldOut, scene.ID.ToString());
                        if (scene.IsFoldOut)
                        {
                            EditorGUI.indentLevel++;

                            scene.ID = (ESceneID)EditorGUILayout.EnumPopup("ID", scene.ID);

                            scene.ScriptPathIndex = EditorGUILayout.Popup("Script", scene.ScriptPathIndex, _scriptPaths);
                            scene.ScriptPath = _scriptPaths[scene.ScriptPathIndex];
                            if (scene.ScriptPathIndex > 0)
                            {
                                scene.ScriptName = scene.ScriptPath.Substring(scene.ScriptPath.LastIndexOf('/') + 1);
                                scene.ScriptName = scene.ScriptName.Replace(".cs", string.Empty);
                            }
                            else
                            {
                                scene.ScriptName = string.Empty;
                            }
                            
                            scene.PrefabNameIndex = EditorGUILayout.Popup("Prefab Canvas", scene.PrefabNameIndex, _scenePrefabNames);
                            scene.PrefabName = _scenePrefabNames[scene.PrefabNameIndex];
                            scene.PrefabName = scene.PrefabName.Equals("<null>") ? string.Empty : scene.PrefabName.Substring(0, scene.PrefabName.Length - 7);
                            
                            scene.PrefabNameIndex3D = EditorGUILayout.Popup("Prefab 3D", scene.PrefabNameIndex3D, _scenePrefabNames);
                            scene.PrefabName3D = _scenePrefabNames[scene.PrefabNameIndex3D];
                            scene.PrefabName3D = scene.PrefabName3D.Equals("<null>") ? string.Empty : scene.PrefabName3D.Substring(0, scene.PrefabName3D.Length - 7);

                            scene.IsInitWhenLoadUnityScene = EditorGUILayout.Toggle("Is Init When Load Unity Scene", scene.IsInitWhenLoadUnityScene);
                            scene.IsHideHUD = EditorGUILayout.Toggle("Is Hide HUD", scene.IsHideHUD);
                            scene.FadeInDuration = EditorGUILayout.FloatField("Fade-In Duration", scene.FadeInDuration);
                            scene.FadeOutDuration = EditorGUILayout.FloatField("Fade-Out Duration", scene.FadeOutDuration);

                            EditorGUI.indentLevel--;
                        }
                    }

                    EditorGUI.indentLevel--;
                }

                if (i < countI - 1)
                {
                    EditorGUILayout.LabelField(string.Empty);
                }
            }
            EditorGUILayout.EndScrollView();

            EditorUtility.SetDirty(_unityScenes);

            EditorGUILayout.LabelField(string.Empty);
            if (GUILayout.Button("Reset", GUILayout.Width(100)))
            {
                _DeleteAssetFile();
                _LoadAssetFile();
                _AddNewUnityScenes();

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
            string filePath = "Assets/Resources/" + MyUGUIManager.CONFIG_DIRECTORY + typeof(MyUGUIConfigUnityScenes).Name + ".asset";
            if (File.Exists(filePath))
            {
                AssetDatabase.DeleteAsset(filePath);
            }

            _unityScenes = null;
        }

        /// <summary>
        /// Load the asset file.
        /// </summary>
        private void _LoadAssetFile()
        {
            if (_unityScenes != null)
            {
                return;
            }

            string filePath = "Assets/Resources/" + MyUGUIManager.CONFIG_DIRECTORY + typeof(MyUGUIConfigUnityScenes).Name + ".asset";
            _unityScenes = AssetDatabase.LoadAssetAtPath(filePath, typeof(MyUGUIConfigUnityScenes)) as MyUGUIConfigUnityScenes;
            if (_unityScenes == null)
            {
                _unityScenes = ScriptableObject.CreateInstance<MyUGUIConfigUnityScenes>();
                AssetDatabase.CreateAsset(_unityScenes, filePath);
                AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        /// Update new popups.
        /// </summary>
        private void _AddNewUnityScenes()
        {
            if (_unityScenes == null)
            {
                return;
            }

            foreach (EUnitySceneID id in Enum.GetValues(typeof(EUnitySceneID)))
            {
                bool isNewUnityScene = true;
                for (int i = 0, countI = _unityScenes.ListUnityScene.Count; i < countI; i++)
                {
                    if (_unityScenes.ListUnityScene[i].ID == id)
                    {
                        isNewUnityScene = false;
                        break;
                    }
                }
                if (isNewUnityScene)
                {
                    _unityScenes.ListUnityScene.Add(new MyUGUIConfigUnityScene()
                    {
                        IsFoldOut = true,
                        ID = id,
                        ListScene = new List<MyUGUIConfigScene>()
                    });
                }
            }
        }

        /// <summary>
        /// Correct values.
        /// </summary>
        private void _CorrectValues()
        {
            _scriptPaths = _GetScriptPaths();
            _unitySceneNames = _GetUnitySceneNames();
            _scenePrefabNames = _GetPrefabNames(MyUGUIManager.SCENE_DIRECTORY);
            _hudPrefabNames = _GetPrefabNames(MyUGUIManager.HUD_DIRECTORY);

            for (int i = 0, countI = _unityScenes.ListUnityScene.Count; i < countI; i++)
            {
                MyUGUIConfigUnityScene unityScene = _unityScenes.ListUnityScene[i];

                if (!string.IsNullOrEmpty(unityScene.SceneName) && (unityScene.SceneNameIndex >= _unitySceneNames.Length || !unityScene.SceneName.Equals(_unitySceneNames[unityScene.SceneNameIndex])))
                {
                    string unitySceneName = unityScene.SceneName + ".unity";
                    unityScene.SceneName = string.Empty;
                    unityScene.SceneNameIndex = 0;
                    for (int j = 0; j < _unitySceneNames.Length; j++)
                    {
                        if (unitySceneName.Equals(_unitySceneNames[j]))
                        {
                            unityScene.SceneNameIndex = j;
                            break;
                        }
                    }
                    if (unityScene.SceneNameIndex > 0)
                    {
                        unityScene.SceneName = _unitySceneNames[unityScene.SceneNameIndex];
                        unityScene.SceneName = unityScene.SceneName.Equals("<null>") ? string.Empty : unityScene.SceneName.Substring(0, unityScene.SceneName.Length - 6);
                    }
                }

                if (!string.IsNullOrEmpty(unityScene.HUDScriptPath) && (unityScene.HUDScriptPathIndex >= _scriptPaths.Length || !unityScene.HUDScriptPath.Equals(_scriptPaths[unityScene.HUDScriptPathIndex])))
                {
                    unityScene.HUDScriptPathIndex = 0;
                    for (int j = 0; j < _scriptPaths.Length; j++)
                    {
                        if (unityScene.HUDScriptPath.Equals(_scriptPaths[j]))
                        {
                            unityScene.HUDScriptPathIndex = j;
                            break;
                        }
                    }
                    if (unityScene.HUDScriptPathIndex > 0)
                    {
                        unityScene.HUDScriptPath = _scriptPaths[unityScene.HUDScriptPathIndex];
                        unityScene.HUDScriptName = unityScene.HUDScriptPath.Substring(unityScene.HUDScriptPath.LastIndexOf('/') + 1);
                        unityScene.HUDScriptName = unityScene.HUDScriptName.Replace(".cs", string.Empty);
                    }
                    else
                    {
                        unityScene.HUDScriptPath = string.Empty;
                    }
                }

                if (!string.IsNullOrEmpty(unityScene.HUDPrefabName) && (unityScene.HUDPrefabNameIndex >= _hudPrefabNames.Length || !unityScene.HUDPrefabName.Equals(_hudPrefabNames[unityScene.HUDPrefabNameIndex])))
                {
                    string hudPrefabName = unityScene.HUDPrefabName + ".prefab";
                    unityScene.HUDPrefabName = string.Empty;
                    unityScene.HUDPrefabNameIndex = 0;
                    for (int j = 0; j < _hudPrefabNames.Length; j++)
                    {
                        if (hudPrefabName.Equals(_hudPrefabNames[j]))
                        {
                            unityScene.HUDPrefabNameIndex = j;
                            break;
                        }
                    }
                    if (unityScene.HUDPrefabNameIndex > 0)
                    {
                        unityScene.HUDPrefabName = _hudPrefabNames[unityScene.HUDPrefabNameIndex];
                        unityScene.HUDPrefabName = unityScene.HUDPrefabName.Equals("<null>") ? string.Empty : unityScene.HUDPrefabName.Substring(0, unityScene.HUDPrefabName.Length - 7);
                    }
                }

                if (!string.IsNullOrEmpty(unityScene.HUDPrefabName3D) && (unityScene.HUDPrefabNameIndex3D >= _hudPrefabNames.Length || !unityScene.HUDPrefabName3D.Equals(_hudPrefabNames[unityScene.HUDPrefabNameIndex3D])))
                {
                    string hudPrefabName = unityScene.HUDPrefabName + ".prefab";
                    unityScene.HUDPrefabName3D = string.Empty;
                    unityScene.HUDPrefabNameIndex3D = 0;
                    for (int j = 0; j < _hudPrefabNames.Length; j++)
                    {
                        if (hudPrefabName.Equals(_hudPrefabNames[j]))
                        {
                            unityScene.HUDPrefabNameIndex3D = j;
                            break;
                        }
                    }
                    if (unityScene.HUDPrefabNameIndex3D > 0)
                    {
                        unityScene.HUDPrefabName3D = _hudPrefabNames[unityScene.HUDPrefabNameIndex3D];
                        unityScene.HUDPrefabName3D = unityScene.HUDPrefabName3D.Equals("<null>") ? string.Empty : unityScene.HUDPrefabName3D.Substring(0, unityScene.HUDPrefabName3D.Length - 7);
                    }
                }

                for (int j = 0, countJ = unityScene.ListScene.Count; j < countJ; j++)
                {
                    MyUGUIConfigScene scene = unityScene.ListScene[j];

                    if (scene.ScriptPathIndex >= _scriptPaths.Length || !scene.ScriptPath.Equals(_scriptPaths[scene.ScriptPathIndex]))
                    {
                        scene.ScriptPathIndex = 0;
                        for (int k = 0; k < _scriptPaths.Length; k++)
                        {
                            if (scene.ScriptPath.Equals(_scriptPaths[k]))
                            {
                                scene.ScriptPathIndex = k;
                                break;
                            }
                        }
                        if (scene.ScriptPathIndex > 0)
                        {
                            scene.ScriptPath = _scriptPaths[scene.ScriptPathIndex];
                            scene.ScriptName = scene.ScriptPath.Substring(scene.ScriptPath.LastIndexOf('/') + 1);
                            scene.ScriptName = scene.ScriptName.Replace(".cs", string.Empty);
                        }
                        else
                        {
                            scene.ScriptPath = string.Empty;
                            scene.ScriptName = string.Empty;
                        }
                    }

                    if (scene.PrefabNameIndex >= _scenePrefabNames.Length || !scene.PrefabName.Equals(_scenePrefabNames[scene.PrefabNameIndex]))
                    {
                        string prefabName = scene.PrefabName + ".prefab";
                        scene.PrefabName = string.Empty;
                        scene.PrefabNameIndex = 0;
                        for (int k = 0; k < _scenePrefabNames.Length; k++)
                        {
                            if (prefabName.Equals(_scenePrefabNames[k]))
                            {
                                scene.PrefabNameIndex = k;
                                break;
                            }
                        }
                        if (scene.PrefabNameIndex > 0)
                        {
                            scene.PrefabName = _scenePrefabNames[scene.PrefabNameIndex];
                            scene.PrefabName = scene.PrefabName.Equals("<null>") ? string.Empty : scene.PrefabName.Substring(0, scene.PrefabName.Length - 7);
                        }
                    }

                    if (scene.PrefabNameIndex3D >= _scenePrefabNames.Length || !scene.PrefabName3D.Equals(_scenePrefabNames[scene.PrefabNameIndex3D]))
                    {
                        string prefabName = scene.PrefabName3D + ".prefab";
                        scene.PrefabName3D = string.Empty;
                        scene.PrefabNameIndex3D = 0;
                        for (int k = 0; k < _scenePrefabNames.Length; k++)
                        {
                            if (prefabName.Equals(_scenePrefabNames[k]))
                            {
                                scene.PrefabNameIndex3D = k;
                                break;
                            }
                        }
                        if (scene.PrefabNameIndex3D > 0)
                        {
                            scene.PrefabName3D = _scenePrefabNames[scene.PrefabNameIndex3D];
                            scene.PrefabName3D = scene.PrefabName3D.Equals("<null>") ? string.Empty : scene.PrefabName3D.Substring(0, scene.PrefabName3D.Length - 7);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Return list unity scene name.
        /// </summary>
        private string[] _GetUnitySceneNames()
        {
            List<string> listUnitySceneNames = new List<string>() { "<null>" };

            string[] unitySceneNames = Directory.GetFiles("Assets/", "*.unity", SearchOption.AllDirectories);
            if (unitySceneNames != null && unitySceneNames.Length > 0)
            {
                foreach (string unitySceneName in unitySceneNames)
                {
                    if (!(unitySceneName.StartsWith("MyClasses", StringComparison.Ordinal)
                          || unitySceneName.StartsWith("Assets/Core", StringComparison.Ordinal)
                          || unitySceneName.StartsWith("Assets/Framework", StringComparison.Ordinal)
                          || unitySceneName.StartsWith("Assets/Plugin", StringComparison.Ordinal)))
                    {
#if UNITY_EDITOR_WIN
                        listUnitySceneNames.Add(unitySceneName.Substring(unitySceneName.LastIndexOf('\\') + 1));
#else
                        listUnitySceneNames.Add(unitySceneName.Substring(unitySceneName.LastIndexOf('/') + 1));
#endif
                    }
                }
            }

            return listUnitySceneNames.ToArray();
        }

        /// <summary>
        /// Return list script path.
        /// </summary>
        private string[] _GetScriptPaths()
        {
            List<string> listScriptPaths = new List<string>() { "<null>" };

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

            return listScriptPaths.ToArray();
        }

        /// <summary>
        /// Return list prefab name.
        /// </summary>
        private string[] _GetPrefabNames(string directory)
        {
            List<string> listPrefabNames = new List<string> { "<null>" };

            string[] prefabNames = Directory.GetFiles("Assets/Resources/" + directory, "*.prefab");
            if (prefabNames != null && prefabNames.Length > 0)
            {
                foreach (string prefabName in prefabNames)
                {
                    listPrefabNames.Add(prefabName.Substring(prefabName.LastIndexOf('/') + 1));
                }
            }

            return listPrefabNames.ToArray();
        }

#endregion
    }
}