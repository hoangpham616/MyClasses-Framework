/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyFPSCounter (version 1.0)
 */

#pragma warning disable 0414

using UnityEngine;
using System.Collections;

namespace MyClasses
{
    public class MyFPSCounter : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        private bool _isShowOnEditor = true;
        [SerializeField]
        private bool _isShowOnDevice = false;
        [SerializeField]
        private float _updateInterval = 0.1f;

        private float _fps;

        #endregion

        #region ----- MonoBehaviour Implementation -----
    
        /// <summary>
        /// Start.
        /// </summary>
        IEnumerator Start()
        {
            while (true)
            {
                _fps = 1f / Time.unscaledDeltaTime;
                yield return new WaitForSeconds(_updateInterval);
            }
        }

        #endregion

        #region ----- GUI Implementation -----

        /// <summary>
        /// OnGUI.
        /// </summary>
        void OnGUI()
        {
#if UNITY_EDITOR
            if (_isShowOnEditor)
            {
                GUI.Label(new Rect(5, 40, 100, 25), "FPS: " + Mathf.Round(_fps));
            }
#else
            if (_isShowOnDevice)
            {
                GUI.Label(new Rect(5, 40, 100, 25), "FPS: " + Mathf.Round(_fps));
            }
#endif
        }

        #endregion
    }
}