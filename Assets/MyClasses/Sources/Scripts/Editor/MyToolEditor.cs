/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyToolEditor (version 1.6)
 */

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace MyClasses.Tool
{
    public class MyToolEditor
    {
        #region ----- Managers -----

        /// <summary>
        /// Create a game object with MyLocalizationManager attached.
        /// </summary>
        [MenuItem("MyClasses/Managers/MyLocalizationManager/Create", false, 1)]
        public static void CreateMyLocalizationManager()
        {
            MyLocalizationManager script = GameObject.FindObjectOfType<MyLocalizationManager>();
            if (script != null)
            {
                EditorGUIUtility.PingObject(script);
                Selection.activeGameObject = script.gameObject;

                Debug.Log("[MyClasses] " + typeof(MyLocalizationManager).Name + " is existed.");
            }
            else
            {
                MyLocalizationManager.CreateTemplate();

                Debug.Log("[MyClasses] " + typeof(MyLocalizationManager).Name + " was created.");
            }
        }

        /// <summary>
        /// Localize all MyLocalization in current scene.
        /// </summary>
        [MenuItem("MyClasses/Managers/MyLocalizationManager/Localize All", false, 2)]
        public static void LocalizeAllMyLocalization()
        {
            MyLocalizationManager script = GameObject.FindObjectOfType<MyLocalizationManager>();
            if (script != null)
            {
                script.LoadLanguage(script.Language, true);
            }

            MyLocalization[] scripts = GameObject.FindObjectsOfType<MyLocalization>();
            foreach (var item in scripts)
            {
                item.Initialize();
                item.Localize();
            }

            Debug.Log("[MyClasses] All " + typeof(MyLocalization).Name + "s in scene were lozalized.");
        }

        /// <summary>
        /// Create a game object with MyTextStyleManager attached.
        /// </summary>
        [MenuItem("MyClasses/Managers/MyTextStyleManager/Create", false, 1)]
        public static void CreateMyTextStyleManager()
        {
            MyTextStyleManager script = GameObject.FindObjectOfType<MyTextStyleManager>();
            if (script != null)
            {
                EditorGUIUtility.PingObject(script);
                Selection.activeGameObject = script.gameObject;

                Debug.Log("[MyClasses] " + typeof(MyTextStyleManager).Name + " is existed.");
            }
            else
            {
                MyTextStyleManager.CreateTemplate();

                Debug.Log("[MyClasses] " + typeof(MyTextStyleManager).Name + " was created.");
            }
        }

        /// <summary>
        /// Refresh all MyTextStyle in current scene.
        /// </summary>
        [MenuItem("MyClasses/Managers/MyTextStyleManager/Refresh All", false, 2)]
        public static void LocalizeAllMyTextStyles()
        {
            MyTextStyle[] scripts = GameObject.FindObjectsOfType<MyTextStyle>();
            foreach (var item in scripts)
            {
                item.Refresh();
            }

            Debug.Log("[MyClasses] All " + typeof(MyTextStyle).Name + "s in scene were refreshed.");
        }

        /// <summary>
        /// Create a game object with MyImageStyleManager attached.
        /// </summary>
        [MenuItem("MyClasses/Managers/MyImageStyleManager/Create", false, 1)]
        public static void CreateMyImageStyleManager()
        {
            MyImageStyleManager script = GameObject.FindObjectOfType<MyImageStyleManager>();
            if (script != null)
            {
                EditorGUIUtility.PingObject(script);
                Selection.activeGameObject = script.gameObject;

                Debug.Log("[MyClasses] " + typeof(MyImageStyleManager).Name + " is existed.");
            }
            else
            {
                MyImageStyleManager.CreateTemplate();

                Debug.Log("[MyClasses] " + typeof(MyImageStyleManager).Name + " was created.");
            }
        }

        /// <summary>
        /// Refresh all MyImageStyle in current scene.
        /// </summary>
        [MenuItem("MyClasses/Managers/MyImageStyleManager/Refresh All", false, 2)]
        public static void RefreshAllMyImageStyles()
        {
            MyImageStyle[] scripts = GameObject.FindObjectsOfType<MyImageStyle>();
            foreach (var item in scripts)
            {
                item.Refresh();
            }

            Debug.Log("[MyClasses] All " + typeof(MyImageStyle).Name + "s in scene were refreshed.");
        }

        #endregion

        #region ----- Panels -----

        /// <summary>
        /// Open Screnshot Panel.
        /// </summary>
        [MenuItem("MyClasses/Panels/Screenshot", false, 1)]
        public static void OpenScreenshotPanel()
        {
            EditorWindow.GetWindow(typeof(MyScreenshotEditorWindow));
        }

        /// <summary>
        /// Open UV Viewer Panel.
        /// </summary>
        [MenuItem("MyClasses/Panels/UV Viewer", false, 2)]
        public static void OpenUVViewerPanel()
        {
            EditorWindow.GetWindow(typeof(MyUVViewerEditorWindow));
        }

        #endregion

        #region ----- Utilities -----

        /// <summary>
        /// Delete all PlayerPrefs.
        /// </summary>
        [MenuItem("MyClasses/Utilities/Clear PlayerPrefs", false, 1)]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();

            Debug.Log("[MyClasses] PlayerPrefs was cleared.");
        }

        /// <summary>
        /// Clear cached AssetBundles.
        /// </summary>
        [MenuItem("MyClasses/Utilities/Clear AssetBundles", false, 1)]
        public static void ClearAssetBundles()
        {
            Caching.ClearCache();

            Debug.Log("[MyClasses] AssetBundles was cleared.");
        }

        /// <summary>
        /// Check Packing Tag and Asset Bundle.
        /// </summary>
        [MenuItem("MyClasses/Utilities/Check Sprite (Packing Tag and Asset Bundle)", false, 12)]
        public static void CheckSpriteTagsAndBundles()
        {
            Dictionary<string, string> spriteDict = new Dictionary<string, string>();
            string[] spriteNames = AssetDatabase.FindAssets("t:sprite");
            foreach (string spriteName in spriteNames)
            {
                string spritePath = AssetDatabase.GUIDToAssetPath(spriteName);
                TextureImporter textureImporter = TextureImporter.GetAtPath(spritePath) as TextureImporter;
                if (!spriteDict.ContainsKey(textureImporter.spritePackingTag))
                {
                    spriteDict.Add(textureImporter.spritePackingTag, textureImporter.assetBundleName);
                }
                else if (spriteDict[textureImporter.spritePackingTag] != textureImporter.assetBundleName)
                {
                    Debug.LogError("[MyClasses] Sprite \"" + textureImporter.assetPath + "\" (PackingTag=\"" + textureImporter.spritePackingTag + "\") should be packed in Asset Bundle \"" + spriteDict[textureImporter.spritePackingTag] + "\".");
                }
            }

            Debug.Log("[MyClasses] Sprite checking completed.");
        }

        /// <summary>
        /// Create some 32x32 noise texture.
        /// </summary>
        [MenuItem("MyClasses/Utilities/Create Noise Texture (32x32)", false, 23)]
        public static void CreateNoiseTexture32()
        {
            int quantity = 5;
            int size = 32;
            for (int i = 0; i < quantity; ++i)
            {
                _CreateNoiseTexture(size);
            }
            AssetDatabase.Refresh();

            Debug.Log(string.Format("[MyClasses] {0} Noise Textures ({1}x{2}) was created.", quantity, size, size));
        }

        /// <summary>
        /// Create some 64x64 noise texture.
        /// </summary>
        [MenuItem("MyClasses/Utilities/Create Noise Texture (64x64)", false, 23)]
        public static void CreateNoiseTexture64()
        {
            int quantity = 5;
            int size = 64;
            for (int i = 0; i < quantity; ++i)
            {
                _CreateNoiseTexture(size);
            }
            AssetDatabase.Refresh();

            Debug.Log(string.Format("[MyClasses] {0} Noise Textures ({1}x{2}) was created.", quantity, size, size));
        }

        /// <summary>
        /// Create some 128x128 noise texture.
        /// </summary>
        [MenuItem("MyClasses/Utilities/Create Noise Texture (128x128)", false, 23)]
        public static void CreateNoiseTexture128()
        {
            int quantity = 5;
            int size = 128;
            for (int i = 0; i < quantity; ++i)
            {
                _CreateNoiseTexture(size);
            }
            AssetDatabase.Refresh();

            Debug.Log(string.Format("[MyClasses] {0} Noise Textures ({1}x{2}) was created.", quantity, size, size));
        }
        
        /// <summary>
        /// Create a noise texture.
        /// </summary>
        private static void _CreateNoiseTexture(int size)
        {
            float randomRate = Random.Range(10, 90);
            Texture2D noiseTexture = new Texture2D(size, size, TextureFormat.ARGB32, false);
            Color color = Color.white;
            for (var c = 0; c < noiseTexture.width; c++)
            {
                for (var r = 0; r < noiseTexture.height; r++)
                {
                    if (Random.Range(0, 100) < randomRate)
                    {
                        float v = Random.Range(0f, 1f);
                        color.r = v;
                        color.g = v;
                        color.b = v;
                    }
                    noiseTexture.SetPixel(r, c, color);
                }
            }
            noiseTexture.Apply();

            for (int i = 1; i <= 1000; ++i)
            {
                string name = "Assets/tex_noise_" + size + " (" + i + ").png";
                if (!System.IO.File.Exists(name))
                {
                    System.IO.File.WriteAllBytes(name, noiseTexture.EncodeToPNG());
                    break;
                }
            }
        }

        /// <summary>
        /// Create some 256x256 perlin noise textures.
        /// </summary>
        [MenuItem("MyClasses/Utilities/Create Perlin Noise Texture (256x256)", false, 23)]
        public static void CreatePerlinNoiseTexture256()
        {
            int quantity = 5;
            int size = 256;
            for (int i = 0; i < quantity; ++i)
            {
                _CreatePerlinNoiseTexture(size, Random.Range(5, 30));
            }
            AssetDatabase.Refresh();

            Debug.Log(string.Format("[MyClasses] {0} Perlin Noise Textures ({1}x{2}) was created.", quantity, size, size));
        }

        /// <summary>
        /// Create some 512x512 perlin noise textures.
        /// </summary>
        [MenuItem("MyClasses/Utilities/Create Perlin Noise Texture (512x512)", false, 23)]
        public static void CreatePerlinNoiseTexture512()
        {
            int quantity = 5;
            int size = 512;
            for (int i = 0; i < quantity; ++i)
            {
                _CreatePerlinNoiseTexture(size, Random.Range(5, 30));
            }
            AssetDatabase.Refresh();

            Debug.Log(string.Format("[MyClasses] {0} Perlin Noise Textures ({1}x{2}) was created.", quantity, size, size));
        }

        /// <summary>
        /// Create some 1024x1024 perlin noise textures.
        /// </summary>
        [MenuItem("MyClasses/Utilities/Create Perlin Noise Texture (1024x1024)", false, 23)]
        public static void CreatePerlinNoiseTexture1024()
        {
            int quantity = 5;
            int size = 1024;
            for (int i = 0; i < quantity; ++i)
            {
                _CreatePerlinNoiseTexture(size, Random.Range(5, 30));
            }
            AssetDatabase.Refresh();

            Debug.Log(string.Format("[MyClasses] {0} Perlin Noise Textures ({1}x{2}) was created.", quantity, size, size));
        }
        
        /// <summary>
        /// Create a perlin noise texture.
        /// </summary>
        private static void _CreatePerlinNoiseTexture(int size, float scale)
        {
            float offsetX = Random.Range(0, size);
            float offsetY = Random.Range(0, size);
            
            Texture2D noiseTexture = new Texture2D(size, size, TextureFormat.ARGB32, false);
            for (int c = 0; c < noiseTexture.width; ++c)
            {
                for (int r = 0; r < noiseTexture.height; ++r)
                {
                    float x = ((c / (float)noiseTexture.width) + offsetX) * scale;
                    float y = ((r / (float)noiseTexture.height) + offsetY) * scale;
                    float sample = Mathf.PerlinNoise(x, y);
                    Color color = new Color(sample, sample, sample);
                    noiseTexture.SetPixel(r, c, color);
                }
            }
            noiseTexture.Apply();

            for (int i = 1; i <= 1000; ++i)
            {
                string name = "Assets/tex_perlin_noise_" + size + " (" + i + ").png";
                if (!System.IO.File.Exists(name))
                {
                    System.IO.File.WriteAllBytes(name, noiseTexture.EncodeToPNG());
                    break;
                }
            }
        }

        #endregion
    }
}