/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyAssetBundleManager (version 1.2)
 */

#pragma warning disable 0114
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0649

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyClasses
{
    public static class MyAssetBundleManager
    {
        #region ----- Internal Class -----

        public class MyAssetBundle
        {
            public AssetBundle Bundle;
            public int Version;
            public ECacheMode CacheMode;
        }

        #endregion

        #region ----- Variable -----

#if DEBUG_MY_ASSET_BUNDLE
        private static string COLOR = "red";
#endif

        private static Dictionary<string, MyAssetBundle> mDictBundle = new Dictionary<string, MyAssetBundle>();

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Return a cached asset bundle.
        /// </summary>
        public static AssetBundle Get(string url, int version)
        {
#if DEBUG_MY_ASSET_BUNDLE
            Debug.Log(string.Format("<color={0}>[{1}] Get()</color>: url=\"{2}\" - version=\"{3}\"", COLOR, typeof(MyAssetBundleManager).Name, url, version));
#endif

            if (mDictBundle.ContainsKey(url))
            {
                MyAssetBundle bundle = mDictBundle[url];
                if (bundle.Version >= version && bundle.Bundle != null)
                {
#if DEBUG_MY_ASSET_BUNDLE
                    Debug.Log(string.Format("<color={0}>[{1}] Get()</color>: successed", COLOR, typeof(MyAssetBundleManager).Name));
#endif

                    return bundle.Bundle;
                }
            }

            return null;
        }

        /// <summary>
        /// Load a asset bundle.
        /// </summary>
        public static void Load(string url, int version, Action<AssetBundle> onLoadComplete, ECacheMode cacheMode = ECacheMode.Cache)
        {
#if DEBUG_MY_ASSET_BUNDLE
            Debug.Log(string.Format("<color={0}>[{1}] Load()</color>: url=\"{2}\" - version=\"{3}\" - cacheMode=\"{4}\"", COLOR, typeof(MyAssetBundleManager).Name, url, version, cacheMode));
#endif

            if (mDictBundle.ContainsKey(url))
            {
                MyAssetBundle bundle = mDictBundle[url];
                if (bundle.Version >= version && bundle.Bundle != null)
                {
#if DEBUG_MY_ASSET_BUNDLE
                    Debug.Log(string.Format("<color={0}>[{1}] Load()</color>: loaded from cache", COLOR, typeof(MyAssetBundleManager).Name));
#endif

                    bundle.CacheMode = cacheMode;
                    if (onLoadComplete != null)
                    {
                        onLoadComplete(bundle.Bundle);
                    }
                    return;
                }
            }

            MyPrivateCoroutiner.Start(_DoLoadAssetBundle(url, version, onLoadComplete, cacheMode));
        }

        /// <summary>
        /// Unload a asset bundle.
        /// </summary>
        public static void Unload(string url)
        {
#if DEBUG_MY_ASSET_BUNDLE
            Debug.Log(string.Format("<color={0}>[{1}] Unload()</color>: url=\"{2}\"", COLOR, typeof(MyAssetBundleManager).Name, url));
#endif

            if (mDictBundle.ContainsKey(url))
            {
                MyAssetBundle bundle = mDictBundle[url];
                if (bundle.Bundle != null)
                {
#if DEBUG_MY_ASSET_BUNDLE
                    Debug.Log(string.Format("<color={0}>[{1}] Unload()</color>: succesed", COLOR, typeof(MyAssetBundleManager).Name));
#endif

                    bundle.Bundle.Unload(false);
                }
                mDictBundle.Remove(url);
            }
        }

        /// <summary>
        /// Unload all asset bundles.
        /// </summary>
        public static void UnloadAll(bool isIncludeUnremovableCache = false)
        {
#if DEBUG_MY_ASSET_BUNDLE
            Debug.Log(string.Format("<color={0}>[{1}] UnloadAll()</color>: isIncludeUnremovableCache=\"{2}\"", COLOR, typeof(MyAssetBundleManager).Name, isIncludeUnremovableCache));
#endif

            if (isIncludeUnremovableCache)
            {
                mDictBundle.Clear();
                return;
            }

            foreach (var item in mDictBundle)
            {
                MyAssetBundle bundle = mDictBundle[item.Key];
                if (bundle.CacheMode != ECacheMode.UnremovableCache)
                {
                    mDictBundle.Remove(item.Key);
                }
            }
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Load a asset bundle asynchronously.
        /// </summary>
        private static IEnumerator _DoLoadAssetBundle(string url, int version, Action<AssetBundle> onLoadComplete, ECacheMode cacheMode = ECacheMode.Cache)
        {
            using (WWW www = WWW.LoadFromCacheOrDownload(url, version))
            {
                yield return www;

                if (!string.IsNullOrEmpty(www.error))
                {
                    Debug.LogError("[" + typeof(MyAssetBundleManager).Name + "] _DoLoadAssetBundle(): Could not load asset bundle url=\"" + url + "\" - version=\"" + version + "\"");
                }
                else if (cacheMode != ECacheMode.None)
                {
                    mDictBundle[url] = new MyAssetBundle()
                    {
                        Bundle = www.assetBundle,
                        Version = version,
                        CacheMode = cacheMode
                    };
                }

                if (onLoadComplete != null)
                {
                    onLoadComplete(www.assetBundle);
                }
                else if (cacheMode == ECacheMode.None)
                {
                    www.assetBundle.Unload(false);
                }
            }
        }

        #endregion

        #region ----- Enumeration -----

        public enum ECacheMode
        {
            None,
            Cache,
            UnremovableCache
        }

        #endregion
    }
}
