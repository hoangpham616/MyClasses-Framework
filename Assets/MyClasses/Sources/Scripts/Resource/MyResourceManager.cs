/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyResourceManager (version 1.11)
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyClasses
{
    public static class MyResourceManager
    {
        #region ----- Variable -----

        private static Dictionary<string, GameObject> _dictionaryPrefab = new Dictionary<string, GameObject>();
        private static Dictionary<string, Material> _dictionaryMaterial = new Dictionary<string, Material>();
        private static Dictionary<string, Texture> _dictionaryTexture = new Dictionary<string, Texture>();
        private static Dictionary<string, Sprite> _dictionarySprite = new Dictionary<string, Sprite>();
        private static Dictionary<string, Dictionary<string, Sprite>> _dictionaryAtlas = new Dictionary<string, Dictionary<string, Sprite>>();

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Return AlphaMask material.
        /// </summary>
        public static Material GetMaterialAlphaMask()
        {
#if UNITY_EDITOR
            Material material = LoadMaterial("Materials/MyAlphaMask");
            if (material == null)
            {
                Debug.Log("[" + typeof(MyResourceManager).Name + "] GetMaterialMask(): A material was created at \"Assets/Resources/Materials/MyAlphaMask.mat\".");
                if (!System.IO.Directory.Exists("Assets/Resources/Materials"))
                {
                    System.IO.Directory.CreateDirectory("Assets/Resources/Materials");
                }
                material = new Material(Shader.Find("MyClasses/Unlit/Alpha Mask"));
                UnityEditor.AssetDatabase.CreateAsset(material, "Assets/Resources/Materials/MyAlphaMask.mat");
            }
#endif
            return LoadMaterial("Materials/MyAlphaMask", true);
        }

        /// <summary>
        /// Return Blended Color material.
        /// </summary>
        public static Material GetMaterialBlendedColor()
        {
#if UNITY_EDITOR
            Material material = LoadMaterial("Materials/MyBlendedColor");
            if (material == null)
            {
                Debug.Log("[" + typeof(MyResourceManager).Name + "] GetMaterialBlendedColor(): A material was created at \"Assets/Resources/Materials/MyBlendedColor.mat\".");
                if (!System.IO.Directory.Exists("Assets/Resources/Materials"))
                {
                    System.IO.Directory.CreateDirectory("Assets/Resources/Materials");
                }
                material = new Material(Shader.Find("MyClasses/Unlit/Blended Color"));
                UnityEditor.AssetDatabase.CreateAsset(material, "Assets/Resources/Materials/MyBlendedColor.mat");
            }
#endif
            return LoadMaterial("Materials/MyBlendedColor", true);
        }

        /// <summary>
        /// Return Blur material.
        /// </summary>
        public static Material GetMaterialBlur()
        {
#if UNITY_EDITOR
            Material material = LoadMaterial("Materials/MyBlur");
            if (material == null)
            {
                Debug.Log("[" + typeof(MyResourceManager).Name + "] GetMaterialBlur(): A material was created at \"Assets/Resources/Materials/MyBlur.mat\".");
                if (!System.IO.Directory.Exists("Assets/Resources/Materials"))
                {
                    System.IO.Directory.CreateDirectory("Assets/Resources/Materials");
                }
                material = new Material(Shader.Find("MyClasses/Unlit/Blur"));
                UnityEditor.AssetDatabase.CreateAsset(material, "Assets/Resources/Materials/MyBlur.mat");
            }
#endif
            return LoadMaterial("Materials/MyBlur", true);
        }

        /// <summary>
        /// Return Darkening material.
        /// </summary>
        public static Material GetMaterialDarkening()
        {
#if UNITY_EDITOR
            Material material = LoadMaterial("Materials/MyDarkening");
            if (material == null)
            {
                Debug.Log("[" + typeof(MyResourceManager).Name + "] GetMaterialDarkening(): A material was created at \"Assets/Resources/Materials/MyDarkening.mat\".");
                if (!System.IO.Directory.Exists("Assets/Resources/Materials"))
                {
                    System.IO.Directory.CreateDirectory("Assets/Resources/Materials");
                }
                material = new Material(Shader.Find("MyClasses/Unlit/Darkening"));
                UnityEditor.AssetDatabase.CreateAsset(material, "Assets/Resources/Materials/MyDarkening.mat");
            }
#endif
            return LoadMaterial("Materials/MyDarkening", true);
        }

        /// <summary>
        /// Return Grayscale material.
        /// </summary>
        public static Material GetMaterialGrayscale()
        {
#if UNITY_EDITOR
            Material material = LoadMaterial("Materials/MyGrayscale");
            if (material == null)
            {
                Debug.Log("[" + typeof(MyResourceManager).Name + "] GetMaterialGrayscale(): A material was created at \"Assets/Resources/Materials/MyGrayscale.mat\".");
                if (!System.IO.Directory.Exists("Assets/Resources/Materials"))
                {
                    System.IO.Directory.CreateDirectory("Assets/Resources/Materials");
                }
                material = new Material(Shader.Find("MyClasses/Unlit/Grayscale"));
                UnityEditor.AssetDatabase.CreateAsset(material, "Assets/Resources/Materials/MyGrayscale.mat");
            }
#endif
            return LoadMaterial("Materials/MyGrayscale", true);
        }

        /// <summary>
        /// Load a prefab.
        /// </summary>
        public static GameObject LoadPrefab(string path, bool isCache = true)
        {
            if (_dictionaryPrefab.ContainsKey(path))
            {
                return _dictionaryPrefab[path];
            }

            GameObject asset = Resources.Load(path, typeof(GameObject)) as GameObject;
            if (asset == null)
            {
                Debug.LogError("[" + typeof(MyResourceManager).Name + "] LoadPrefab(): Could not find file \"" + path + "\".");
            }
            else if (isCache)
            {
                _dictionaryPrefab[path] = asset;
            }
            return asset;
        }

        /// <summary>
        /// Load some prefabs.
        /// </summary>
        public static List<GameObject> LoadPrefabs(List<string> paths, bool isCache = true)
        {
            List<GameObject> assets = new List<GameObject>();

            foreach (var path in paths)
            {
                assets.Add(LoadPrefab(path, isCache));
            }

            return assets;
        }

        /// <summary>
        /// Load a prefab asynchronously.
        /// </summary>
        public static void LoadAsyncPrefab(string path, Action<float> onLoading, Action<GameObject> onLoadComplete, bool isCache = true)
        {
            if (_dictionaryPrefab.ContainsKey(path))
            {
                if (onLoadComplete != null)
                {
                    onLoadComplete(_dictionaryPrefab[path]);
                }
                return;
            }

            MyPrivateCoroutiner.Start(_DoLoadAsyncPrefab(path, onLoading, onLoadComplete, isCache));
        }

        /// <summary>
        /// Load some prefabs asynchronously.
        /// </summary>
        public static void LoadAsyncPrefabs(List<string> paths, Action<List<GameObject>> onLoadComplete, bool isCache = true)
        {
            MyPrivateCoroutiner.Start(_DoLoadAsyncPrefabs(paths, onLoadComplete, isCache));
        }

        /// <summary>
        /// Load a material.
        /// </summary>
        public static Material LoadMaterial(string path, bool isCache = true)
        {
            if (_dictionaryMaterial.ContainsKey(path))
            {
                return _dictionaryMaterial[path];
            }

            Material asset = Resources.Load(path, typeof(Material)) as Material;
            if (asset == null)
            {
                Debug.LogError("[" + typeof(MyResourceManager).Name + "] LoadMaterial(): Could not find file \"" + path + "\".");
            }
            else if (isCache)
            {
                _dictionaryMaterial[path] = asset;
            }
            return asset;
        }

        /// <summary>
        /// Load some materials.
        /// </summary>
        public static List<Material> LoadMaterials(List<string> paths, bool isCache = true)
        {
            List<Material> assets = new List<Material>();

            foreach (var path in paths)
            {
                assets.Add(LoadMaterial(path, isCache));
            }

            return assets;
        }

        /// <summary>
        /// Load a material asynchronously.
        /// </summary>
        public static void LoadAsyncMaterial(string path, Action<Material> onLoadComplete, bool isCache = true)
        {
            if (_dictionaryMaterial.ContainsKey(path))
            {
                if (onLoadComplete != null)
                {
                    onLoadComplete(_dictionaryMaterial[path]);
                }
                return;
            }

            MyPrivateCoroutiner.Start(_DoLoadAsyncMaterial(path, onLoadComplete, isCache));
        }

        /// <summary>
        /// Load some materials asynchronously.
        /// </summary>
        public static void LoadAsyncMaterials(List<string> paths, Action<List<Material>> onLoadComplete, bool isCache = true)
        {
            MyPrivateCoroutiner.Start(_DoLoadAsyncMaterials(paths, onLoadComplete, isCache));
        }

        /// <summary>
        /// Load a texture.
        /// </summary>
        public static Texture LoadTexture(string path, bool isCache = true)
        {
            if (_dictionaryTexture.ContainsKey(path))
            {
                return _dictionaryTexture[path];
            }

            Texture asset = Resources.Load(path, typeof(Texture)) as Texture;
            if (asset == null)
            {
                Debug.LogError("[" + typeof(MyResourceManager).Name + "] LoadTexture(): Could not find file \"" + path + "\".");
            }
            else if (isCache)
            {
                _dictionaryTexture[path] = asset;
            }
            return asset;
        }

        /// <summary>
        /// Load some textures.
        /// </summary>
        public static List<Texture> LoadTextures(List<string> paths, bool isCache = true)
        {
            List<Texture> assets = new List<Texture>();

            foreach (var path in paths)
            {
                assets.Add(LoadTexture(path, isCache));
            }

            return assets;
        }

        /// <summary>
        /// Load a texture asynchronously.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        public static void LoadAsyncTexture(string path, Action<Texture> onLoadComplete, bool isCache = true)
        {
            if (_dictionaryTexture.ContainsKey(path))
            {
                if (onLoadComplete != null)
                {
                    onLoadComplete(_dictionaryTexture[path]);
                }
                return;
            }

            MyPrivateCoroutiner.Start(_DoLoadAsyncTexture(path, onLoadComplete, isCache));
        }

        /// <summary>
        /// Load some textures asynchronously.
        /// </summary>
        /// <param name="isCache">texture will be saved to re-use</param>
        public static void LoadAsyncTextures(List<string> paths, Action<List<Texture>> onLoadComplete, bool isCache = true)
        {
            MyPrivateCoroutiner.Start(_DoLoadAsyncTextures(paths, onLoadComplete, isCache));
        }

        /// <summary>
        /// Load a sprite.
        /// </summary>
        public static Sprite LoadSprite(string path, bool isCache = true)
        {
            if (_dictionarySprite.ContainsKey(path))
            {
                return _dictionarySprite[path];
            }

            Sprite asset = Resources.Load(path, typeof(Sprite)) as Sprite;
            if (asset == null)
            {
                Debug.LogError("[" + typeof(MyResourceManager).Name + "] LoadSprite(): Could not find file \"" + path + "\".");
            }
            else if (isCache)
            {
                _dictionarySprite[path] = asset;
            }
            return asset;
        }

        /// <summary>
        /// Load a sprite from a atlas
        /// </summary>
        public static Sprite LoadSpriteFromAtlas(string atlasPath, string spriteName, bool isCache = true)
        {
            if (_dictionaryAtlas.ContainsKey(atlasPath))
            {
                Dictionary<string, Sprite> dictSprite = _dictionaryAtlas[atlasPath];
                if (!dictSprite.ContainsKey(spriteName))
                {
                    Debug.LogError("[" + typeof(MyResourceManager).Name + "] LoadSpriteFromAtlas(): Could not find sprite \"" + spriteName + "\".");
                    return null;
                }
                else
                {
                    return dictSprite[spriteName];
                }
            }

            if (isCache)
            {
                LoadAtlas(atlasPath);

                Dictionary<string, Sprite> dictSprite = _dictionaryAtlas[atlasPath];
                if (!dictSprite.ContainsKey(spriteName))
                {
                    Debug.LogError("[" + typeof(MyResourceManager).Name + "] LoadSpriteFromAtlas(): Could not find sprite \"" + spriteName + "\".");
                    return null;
                }
                else
                {
                    return dictSprite[spriteName];
                }
            }
            else
            {
                Sprite[] sprites = Resources.LoadAll(atlasPath, typeof(Sprite)) as Sprite[];
                if (sprites == null)
                {
                    Debug.LogError("[" + typeof(MyResourceManager).Name + "] LoadSpriteFromAtlas(): Could not find atlas \"" + atlasPath + "\".");
                    return null;
                }
                else
                {
                    for (int i = 0; i < sprites.Length; i++)
                    {
                        if (sprites[i].name.Equals(spriteName))
                        {
                            return sprites[i];
                        }
                    }
                    Debug.LogError("[" + typeof(MyResourceManager).Name + "] LoadSpriteFromAtlas(): Could not find sprite \"" + spriteName + "\".");
                }
            }

            return null;
        }

        /// <summary>
        /// Load some sprites.
        /// </summary>
        public static List<Sprite> LoadSprites(List<string> paths, bool isCache = true)
        {
            List<Sprite> assets = new List<Sprite>();

            foreach (var path in paths)
            {
                assets.Add(LoadSprite(path, isCache));
            }

            return assets;
        }

        /// <summary>
        /// Load a sprite asynchronously.
        /// </summary>
        public static void LoadAsyncSprite(string path, Action<Sprite> onLoadComplete, bool isCache = true)
        {
            if (_dictionarySprite.ContainsKey(path))
            {
                if (onLoadComplete != null)
                {
                    onLoadComplete(_dictionarySprite[path]);
                }
                return;
            }

            MyPrivateCoroutiner.Start(_DoLoadAsyncSprite(path, onLoadComplete, isCache));
        }

        /// <summary>
        /// Load some sprites asynchronously.
        /// </summary>
        public static void LoadAsyncSprites(List<string> paths, Action<List<Sprite>> onLoadComplete, bool isCache = true)
        {
            MyPrivateCoroutiner.Start(_DoLoadAsyncSprites(paths, onLoadComplete, isCache));
        }

        /// <summary>
        /// Load a atlas.
        /// </summary>
        public static void LoadAtlas(string path)
        {
            if (!_dictionaryAtlas.ContainsKey(path))
            {
                Sprite[] assets = Resources.LoadAll<Sprite>(path);
                if (assets == null)
                {
                    Debug.LogError("[" + typeof(MyResourceManager).Name + "] LoadAtlas(): Could not find file \"" + path + "\".");
                }
                else
                {
                    Dictionary<string, Sprite> dictSprite = new Dictionary<string, Sprite>();
                    for (int i = 0; i < assets.Length; i++)
                    {
                        Sprite sprite = assets[i];
                        dictSprite[sprite.name] = sprite;
                    }
                    _dictionaryAtlas[path] = dictSprite;
                }
            }
        }

        /// <summary>
        /// Unload all cached resources.
        /// </summary>
        public static void UnloadAll()
        {
            _dictionaryPrefab.Clear();
            _dictionaryMaterial.Clear();
            _dictionaryTexture.Clear();
            _dictionarySprite.Clear();
            _dictionaryAtlas.Clear();
        }

        /// <summary>
        /// Unload all cached prefabs.
        /// </summary>
        public static void UnloadPrefabs()
        {
            _dictionaryPrefab.Clear();
        }

        /// <summary>
        /// Unload all cached materials.
        /// </summary>
        public static void UnloadMaterials()
        {
            _dictionaryMaterial.Clear();
        }

        /// <summary>
        /// Unload all cached textures.
        /// </summary>
        public static void UnloadTextures()
        {
            _dictionaryTexture.Clear();
        }

        /// <summary>
        /// Unload all cached atlases.
        /// </summary>
        public static void UnloadAllAtlases()
        {
            _dictionaryAtlas.Clear();
        }

        /// <summary>
        /// Unload a cached atlas.
        /// </summary>
        public static void UnloadAtlas(string path)
        {
            _dictionaryAtlas.Remove(path);
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Load a prefab asynchronously.
        /// </summary>
        private static IEnumerator _DoLoadAsyncPrefab(string path, Action<float> onLoading, Action<GameObject> onLoadComplete, bool isCache)
        {
            ResourceRequest requester = Resources.LoadAsync<GameObject>(path);
            while (!requester.isDone)
            {
                if (onLoading != null)
                {
                    onLoading(requester.progress);
                }
                yield return null;
            }

            if (requester.asset == null)
            {
                Debug.LogError("[" + typeof(MyResourceManager).Name + "] _DoLoadAsyncPrefab(): Could not find file \"" + path + "\".");
                if (onLoadComplete != null)
                {
                    onLoadComplete(null);
                }
                yield break;
            }

            GameObject asset = requester.asset as GameObject;
            if (isCache)
            {
                _dictionaryPrefab[path] = asset;
            }
            if (onLoadComplete != null)
            {
                onLoadComplete(asset);
            }
        }

        /// <summary>
        /// Load some prefabs asynchronously.
        /// </summary>
        private static IEnumerator _DoLoadAsyncPrefabs(List<string> paths, Action<List<GameObject>> onLoadComplete, bool isCache)
        {
            List<GameObject> assets = new List<GameObject>();

            foreach (var path in paths)
            {
                if (_dictionaryPrefab.ContainsKey(path))
                {
                    assets.Add(_dictionaryPrefab[path]);
                    continue;
                }

                ResourceRequest requester = Resources.LoadAsync<GameObject>(path);
                yield return requester;

                if (requester.asset == null)
                {
                    Debug.LogError("[" + typeof(MyResourceManager).Name + "] _DoLoadAsyncPrefabs(): Could not find file \"" + path + "\".");
                    continue;
                }

                GameObject asset = requester.asset as GameObject;
                if (isCache)
                {
                    _dictionaryPrefab[path] = asset;
                }
                assets.Add(asset);
            }

            if (onLoadComplete != null)
            {
                onLoadComplete(assets);
            }
        }

        /// <summary>
        /// Load a material asynchronously.
        /// </summary>
        private static IEnumerator _DoLoadAsyncMaterial(string path, Action<Material> onLoadComplete, bool isCache)
        {
            ResourceRequest requester = Resources.LoadAsync<Material>(path);
            yield return requester;

            if (requester.asset == null)
            {
                Debug.LogError("[" + typeof(MyResourceManager).Name + "] _DoLoadAsyncMaterial(): Could not find file \"" + path + "\".");
                if (onLoadComplete != null)
                {
                    onLoadComplete(null);
                }
                yield break;
            }

            Material asset = requester.asset as Material;
            if (isCache)
            {
                _dictionaryMaterial[path] = asset;
            }
            if (onLoadComplete != null)
            {
                onLoadComplete(asset);
            }
        }

        /// <summary>
        /// Load some materials asynchronously.
        /// </summary>
        private static IEnumerator _DoLoadAsyncMaterials(List<string> paths, Action<List<Material>> onLoadComplete, bool isCache)
        {
            List<Material> assets = new List<Material>();

            foreach (var path in paths)
            {
                if (_dictionaryMaterial.ContainsKey(path))
                {
                    assets.Add(_dictionaryMaterial[path]);
                    continue;
                }

                ResourceRequest requester = Resources.LoadAsync<Material>(path);
                yield return requester;

                if (requester.asset == null)
                {
                    Debug.LogError("[" + typeof(MyResourceManager).Name + "] _DoLoadAsyncMaterials(): Could not find file \"" + path + "\".");
                    continue;
                }

                Material asset = requester.asset as Material;
                if (isCache)
                {
                    _dictionaryMaterial[path] = asset;
                }
                assets.Add(asset);
            }

            if (onLoadComplete != null)
            {
                onLoadComplete(assets);
            }
        }

        /// <summary>
        /// Load a texture asynchronously.
        /// </summary>
        private static IEnumerator _DoLoadAsyncTexture(string path, Action<Texture> onLoadComplete, bool isCache)
        {
            ResourceRequest requester = Resources.LoadAsync<Texture>(path);
            yield return requester;

            if (requester.asset == null)
            {
                Debug.LogError("[" + typeof(MyResourceManager).Name + "] _DoLoadAsyncTexture(): Could not find file \"" + path + "\".");
                if (onLoadComplete != null)
                {
                    onLoadComplete(null);
                }
                yield break;
            }

            Texture asset = requester.asset as Texture;
            if (isCache)
            {
                _dictionaryTexture[path] = asset;
            }
            if (onLoadComplete != null)
            {
                onLoadComplete(asset);
            }
        }

        /// <summary>
        /// Load some textures asynchronously.
        /// </summary>
        private static IEnumerator _DoLoadAsyncTextures(List<string> paths, Action<List<Texture>> onLoadComplete, bool isCache)
        {
            List<Texture> assets = new List<Texture>();

            foreach (var path in paths)
            {
                if (_dictionaryTexture.ContainsKey(path))
                {
                    assets.Add(_dictionaryTexture[path]);
                    continue;
                }

                ResourceRequest requester = Resources.LoadAsync<Texture>(path);
                yield return requester;

                if (requester.asset == null)
                {
                    Debug.LogError("[" + typeof(MyResourceManager).Name + "] _DoLoadAsyncTextures(): Could not find file \"" + path + "\".");
                    continue;
                }

                Texture asset = requester.asset as Texture;
                if (isCache)
                {
                    _dictionaryTexture[path] = asset;
                }
                assets.Add(asset);
            }

            if (onLoadComplete != null)
            {
                onLoadComplete(assets);
            }
        }

        /// <summary>
        /// Load a sprite asynchronously.
        /// </summary>
        private static IEnumerator _DoLoadAsyncSprite(string path, Action<Sprite> onLoadComplete, bool isCache)
        {
            ResourceRequest requester = Resources.LoadAsync<Sprite>(path);
            yield return requester;

            if (requester.asset == null)
            {
                Debug.LogError("[" + typeof(MyResourceManager).Name + "] _DoLoadAsyncSprite(): Could not find file \"" + path + "\".");
                if (onLoadComplete != null)
                {
                    onLoadComplete(null);
                }
                yield break;
            }

            Sprite asset = requester.asset as Sprite;
            if (isCache)
            {
                _dictionarySprite[path] = asset;
            }
            if (onLoadComplete != null)
            {
                onLoadComplete(asset);
            }
        }

        /// <summary>
        /// Load some sprites asynchronously.
        /// </summary>
        private static IEnumerator _DoLoadAsyncSprites(List<string> paths, Action<List<Sprite>> onLoadComplete, bool isCache)
        {
            List<Sprite> assets = new List<Sprite>();

            foreach (var path in paths)
            {
                if (_dictionarySprite.ContainsKey(path))
                {
                    assets.Add(_dictionarySprite[path]);
                    continue;
                }

                ResourceRequest requester = Resources.LoadAsync<Sprite>(path);
                yield return requester;

                if (requester.asset == null)
                {
                    Debug.LogError("[" + typeof(MyResourceManager).Name + "] _DoLoadAsyncSprites(): Could not find file \"" + path + "\".");
                    continue;
                }

                Sprite asset = requester.asset as Sprite;
                if (isCache)
                {
                    _dictionarySprite[path] = asset;
                }
                assets.Add(asset);
            }

            if (onLoadComplete != null)
            {
                onLoadComplete(assets);
            }
        }

        #endregion
    }
}