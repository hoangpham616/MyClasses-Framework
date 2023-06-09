﻿/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyImageDownloader (version 1.4)
 */

#pragma warning disable 0114
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0649

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyClasses
{
    public class MyImageDownloader : MonoBehaviour
    {
        #region ----- Variable -----

        private Dictionary<string, Sprite> _dictionarySprite = new Dictionary<string, Sprite>();
        private Dictionary<string, Texture2D> _dictionaryTexture2D = new Dictionary<string, Texture2D>();

        #endregion

        #region ----- Singleton -----

        private static object _singletonLock = new object();
        private static MyImageDownloader _instance;

        public static MyImageDownloader Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_singletonLock)
                    {
                        _instance = (MyImageDownloader)FindObjectOfType(typeof(MyImageDownloader));
                        if (_instance == null)
                        {
                            GameObject obj = new GameObject(typeof(MyImageDownloader).Name);
                            _instance = obj.AddComponent<MyImageDownloader>();
                            DontDestroyOnLoad(obj);
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Clear all cached images.
        /// </summary>
        public void Clear()
        {
            _dictionarySprite.Clear();
            _dictionaryTexture2D.Clear();
        }

        /// <summary>
        /// Load a sprite from a url.
        /// </summary>
        public void LoadSprite(string url, float delayDownload = 0, Action<string, Sprite> onLoadSuccess = null, Action<string> onLoadError = null)
        {
            if (!string.IsNullOrEmpty(url))
            {
                if (_dictionarySprite.ContainsKey(url))
                {
                    if (onLoadSuccess != null)
                    {
                        onLoadSuccess(url, _dictionarySprite[url]);
                    }
                }
                else
                {
                    StartCoroutine(_DoLoadSprite(url, delayDownload, onLoadSuccess, onLoadError));
                }
            }
            else
            {
                if (onLoadError != null)
                {
                    onLoadError(url);
                }
            }
        }

        /// <summary>
        /// Load a texture 2D from a url.
        /// </summary>
        public void LoadTexture2D(string url, float delayDownload = 0, Action<string, Texture2D> onLoadSuccess = null, Action<string> onLoadError = null)
        {
            if (!string.IsNullOrEmpty(url))
            {
                if (_dictionaryTexture2D.ContainsKey(url))
                {
                    if (onLoadSuccess != null)
                    {
                        onLoadSuccess(url, _dictionaryTexture2D[url]);
                    }
                }
                else
                {
                    StartCoroutine(_DoLoadTexture2D(url, delayDownload, onLoadSuccess, onLoadError));
                }
            }
            else
            {
                if (onLoadError != null)
                {
                    onLoadError(url);
                }
            }
        }

        /// <summary>
        /// Load an image from a url.
        /// </summary>
        public void LoadImage(Image image, string url, float delayDownload = 0, Action<string> onLoadSuccess = null, Action<string> onLoadError = null)
        {
            if (image != null && !string.IsNullOrEmpty(url))
            {
                if (_dictionarySprite.ContainsKey(url))
                {
                    image.sprite = _dictionarySprite[url];
                    image.enabled = true;
                    if (onLoadSuccess != null)
                    {
                        onLoadSuccess(url);
                    }
                }
                else
                {
                    StartCoroutine(_DoLoadImage(image, url, delayDownload, onLoadSuccess, onLoadError));
                }
            }
            else
            {
                if (onLoadError != null)
                {
                    onLoadError(url);
                }
            }
        }

        /// <summary>
        /// Load a raw image from a url.
        /// </summary>
        public void LoadRawImage(RawImage rawImage, string url, float delayDownload = 0, Action<string> onLoadSuccess = null, Action<string> onLoadError = null)
        {
            if (rawImage != null && !string.IsNullOrEmpty(url))
            {
                if (_dictionaryTexture2D.ContainsKey(url))
                {
                    Texture2D texture = _dictionaryTexture2D[url];
                    _dictionaryTexture2D[url] = texture;
                    rawImage.texture = texture;
                    rawImage.enabled = true;
                    if (onLoadSuccess != null)
                    {
                        onLoadSuccess(url);
                    }
                }
                else
                {
                    StartCoroutine(_DoLoadRawImage(rawImage, url, delayDownload, onLoadSuccess, onLoadError));
                }
            }
            else
            {
                if (onLoadError != null)
                {
                    onLoadError(url);
                }
            }
        }

        #endregion

        #region ----- Private Method -----

        private IEnumerator _DoLoadSprite(string url, float delayDownload = 0, Action<string, Sprite> onLoadSuccess = null, Action<string> onLoadError = null)
        {
            if (delayDownload > 0)
            {
                yield return new WaitForSeconds(delayDownload);
            }

            WWW www = new WWW(url);
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                Sprite sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
                _dictionarySprite[url] = sprite;
                if (onLoadSuccess != null)
                {
                    onLoadSuccess(url, sprite);
                }
            }
            else
            {
                if (onLoadError != null)
                {
                    onLoadError(url);
                }
            }
        }

        private IEnumerator _DoLoadTexture2D(string url, float delayDownload = 0, Action<string, Texture2D> onLoadSuccess = null, Action<string> onLoadError = null)
        {
            if (delayDownload > 0)
            {
                yield return new WaitForSeconds(delayDownload);
            }

            WWW www = new WWW(url);
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                Texture2D texture = new Texture2D(4, 4, TextureFormat.DXT1, false);
                www.LoadImageIntoTexture(texture);
                _dictionaryTexture2D[url] = texture;
                if (onLoadSuccess != null)
                {
                    onLoadSuccess(url, texture);
                }
            }
            else
            {
                if (onLoadError != null)
                {
                    onLoadError(url);
                }
            }
        }

        private IEnumerator _DoLoadImage(Image image, string url, float delayDownload = 0, Action<string> onLoadSuccess = null, Action<string> onLoadError = null)
        {
            if (image.sprite == null)
            {
                image.enabled = false;
            }

            if (delayDownload > 0)
            {
                yield return new WaitForSeconds(delayDownload);
            }

            WWW www = new WWW(url);
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                Sprite sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
                _dictionarySprite[url] = sprite;
                if (image != null)
                {
                    image.sprite = sprite;
                    image.enabled = true;
                }
                if (onLoadSuccess != null)
                {
                    onLoadSuccess(url);
                }
            }
            else
            {
                if (onLoadError != null)
                {
                    onLoadError(url);
                }
            }
        }

        private IEnumerator _DoLoadRawImage(RawImage rawImage, string url, float delayDownload = 0, Action<string> onLoadSuccess = null, Action<string> onLoadError = null)
        {
            if (rawImage.texture == null)
            {
                rawImage.enabled = false;
            }

            if (delayDownload > 0)
            {
                yield return new WaitForSeconds(delayDownload);
            }

            WWW www = new WWW(url);
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                Texture2D texture = new Texture2D(4, 4, TextureFormat.DXT1, false);
                www.LoadImageIntoTexture(texture);
                _dictionaryTexture2D[url] = texture;
                if (rawImage != null)
                {
                    rawImage.texture = texture;
                    rawImage.enabled = true;
                }
                if (onLoadSuccess != null)
                {
                    onLoadSuccess(url);
                }
            }
            else
            {
                if (onLoadError != null)
                {
                    onLoadError(url);
                }
            }
        }

        #endregion
    }
}