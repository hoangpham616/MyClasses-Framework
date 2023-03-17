/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyCameraScanLineGlitch (version 1.0)
 */

using UnityEditor;
using UnityEngine;

namespace MyClasses
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class MyCameraScanLineGlitch : MonoBehaviour
    {
        #region ----- Define -----

        private readonly string INTENSITY = "_Intensity";

        #endregion

        #region ----- Variable -----

        [SerializeField, Range(0, 1)]
        private float mIntensity = 0.3f;

        private Material mMaterial;
        private Shader mShader;

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
            if (mMaterial == null)
            {
                _Initialize();
            }
#endif

            mMaterial.SetFloat(INTENSITY, mIntensity);
            Graphics.Blit(source, destination, mMaterial);
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Initialize.
        /// </summary>
        private void _Initialize()
        {
#if UNITY_EDITOR
            if (mShader == null)
            {
                string[] paths = new string[] { "Assets/MyClasses", "Assets/Core/MyClasses", "Assets/Plugin/MyClasses", "Assets/Plugins/MyClasses", "Assets/Framework/MyClasses", "Assets/Frameworks/MyClasses" };
                for (int i = 0; i < paths.Length; i++)
                {
                    mShader = AssetDatabase.LoadAssetAtPath<Shader>(paths[i] + "/Sources/Shaders/ImageEffect/MyShaderImageScanLineGlitch.shader");
                    if (mShader != null)
                    {
                        break;
                    }
                }
            }
#endif

            mMaterial = new Material(mShader);
            mMaterial.hideFlags = HideFlags.DontSave;
        }

        #endregion
    }
}