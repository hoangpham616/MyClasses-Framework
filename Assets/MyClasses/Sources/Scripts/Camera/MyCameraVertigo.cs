/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyCameraVertigo (version 1.1)
 * Requirement: MyShaderImageVertigo.shader
 */

using UnityEditor;
using UnityEngine;

namespace MyClasses
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class MyCameraVertigo : MonoBehaviour
    {
        #region ----- Define -----

        private const string _WAVE = "_Wave";
        private const string _INTENSITY = "_Intensity";

        #endregion

        #region ----- Variable -----

        [SerializeField, Range(0, 1)]
        private float mWave = 0.01f;
        [SerializeField, Range(0, 1)]
        private float _intensity = 0.01f;

        private Material _material;
        private Shader _shader;

        #endregion

        #region ----- MonoBehaviour Implementation -----

        /// <summary>
        /// Start.
        /// </summary>
        private void Start()
        {
            _Initialize();
        }

        /// <summary>
        /// OnRenderImage.
        /// </summary>
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
#if UNITY_EDITOR
            if (_material == null)
            {
                _Initialize();
            }
#endif

            _material.SetFloat(_WAVE, mWave);
            _material.SetFloat(_INTENSITY, _intensity);
            Graphics.Blit(source, destination, _material);
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Initialize.
        /// </summary>
        private void _Initialize()
        {
#if UNITY_EDITOR
            if (_shader == null)
            {
                string[] paths = new string[] { "Assets/MyClasses", "Assets/Core/MyClasses", "Assets/Plugin/MyClasses", "Assets/Plugins/MyClasses", "Assets/Framework/MyClasses", "Assets/Frameworks/MyClasses" };
                for (int i = 0; i < paths.Length; i++)
                {
                    _shader = AssetDatabase.LoadAssetAtPath<Shader>(paths[i] + "/Sources/Shaders/ImageEffect/MyShaderImageVertigo.shader");
                    if (_shader != null)
                    {
                        break;
                    }
                }
            }
#endif

            _material = new Material(_shader);
            _material.hideFlags = HideFlags.DontSave;
        }

        #endregion
    }
}