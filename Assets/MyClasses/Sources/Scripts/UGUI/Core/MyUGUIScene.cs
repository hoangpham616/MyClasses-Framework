/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUGUIScene (version 2.9)
 */

using UnityEngine;

namespace MyClasses.UI
{
    public abstract class MyUGUIScene : MyUGUIBase
    {
        #region ----- Variable -----

        private ESceneID _id;
        private EUnitySceneID _unitySceneID;
        private bool _isInitWhenLoadUnityScene;
        private bool _isHideHUD;
        private float _fadeInDuration;
        private float _fadeOutDuration;

        #endregion

        #region ----- Property -----

        public ESceneID ID
        {
            get { return _id; }
        }

        public EUnitySceneID UnitySceneID
        {
            get { return _unitySceneID; }
            set { _unitySceneID = value; }
        }

        public bool IsInitWhenLoadUnityScene
        {
            get { return _isInitWhenLoadUnityScene; }
        }

        public bool IsHideHUD
        {
            get { return _isHideHUD; }
        }

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyUGUIScene(ESceneID id, string prefabName, bool isInitWhenLoadUnityScene, bool isHideHUD = false, float fadeInDuration = 0.5f, float fadeOutDuration = 0.5f)
            : base(prefabName, null)
        {
            _id = id;
            _isInitWhenLoadUnityScene = isInitWhenLoadUnityScene;
            _isHideHUD = isHideHUD;
            _fadeInDuration = fadeInDuration;
            _fadeOutDuration = fadeOutDuration;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyUGUIScene(ESceneID id, string prefabName, string prefabName3D, bool isInitWhenLoadUnityScene, bool isHideHUD = false, float fadeInDuration = 0.5f, float fadeOutDuration = 0.5f)
            : base(prefabName, prefabName3D)
        {
            _id = id;
            _isInitWhenLoadUnityScene = isInitWhenLoadUnityScene;
            _isHideHUD = isHideHUD;
            _fadeInDuration = fadeInDuration;
            _fadeOutDuration = fadeOutDuration;
        }

        #endregion

        #region ----- MyUGUIBase Implementation -----

        /// <summary>
        /// OnUGUIInit.
        /// </summary>
        public override void OnUGUIInit()
        {
            base.OnUGUIInit();

            GameObject = MyUtilities.FindObjectInFirstLayer(MyUGUIManager.Instance.Canvas.gameObject, PrefabName);
            if (GameObject == null)
            {
                if (IsUseAssetBundle)
                {
                    if (Bundle == null)
                    {
                        Debug.LogError("[" + typeof(MyUGUIScene).Name + "] OnUGUIInit(): Asset bundle null.");
                    }
                    GameObject = GameObject.Instantiate(Bundle.LoadAsset(PrefabName), Vector3.zero, Quaternion.identity) as GameObject;
                }
                else
                {
                    string path = MyUGUIManager.SCENE_DIRECTORY + PrefabName;
                    GameObject template = Resources.Load<GameObject>(path);
                    if (template == null)
                    {
                        Debug.LogError("[" + typeof(MyUGUIScene).Name + "] OnUGUIInit(): Could not find file \"" + path + "\".");
                    }
                    GameObject = GameObject.Instantiate(template, Vector3.zero, Quaternion.identity) as GameObject;
                }
                GameObject.name = PrefabName;
                GameObject.transform.SetParent(MyUGUIManager.Instance.Canvas.transform, false);
            }
            GameObject.SetActive(false);

            if (PrefabName3D.Length > 0)
            {
                GameObject3D = MyUtilities.FindObjectInRoot(PrefabName3D);
                if (GameObject3D == null)
                {
                    string path = MyUGUIManager.SCENE_DIRECTORY + PrefabName3D;
                    GameObject template = Resources.Load<GameObject>(path);
                    if (template == null)
                    {
                        Debug.LogError("[" + typeof(MyUGUIScene).Name + "] OnUGUIInit(): Could not find file \"" + path + "\".");
                    }
                    GameObject3D = GameObject.Instantiate(template, Vector3.zero, Quaternion.identity) as GameObject;
                }
                if (GameObject3D != null)
                {
                    GameObject3D.SetActive(false);
                }
            }
        }

        /// <summary>
        /// OnUGUIEnter.
        /// </summary>
        public override void OnUGUIEnter()
        {
            base.OnUGUIEnter();

            MyUGUIManager.Instance.CurrentSceneFading.FadeIn(_fadeInDuration);
        }

        /// <summary>
        /// OnUGUIVisible.
        /// </summary>
        public override bool OnUGUIVisible()
        {
            return !MyUGUIManager.Instance.CurrentSceneFading.IsFading;
        }

        /// <summary>
        /// OnUGUIUpdate.
        /// </summary>
        public override void OnUGUIUpdate(float deltaTime)
        {
        }

        /// <summary>
        /// OnUGUIExit.
        /// </summary>
        public override void OnUGUIExit()
        {
            base.OnUGUIExit();

            MyUGUIManager.Instance.CurrentSceneFading.FadeOut(_fadeOutDuration);
        }

        /// <summary>
        /// OnUGUIInvisible.
        /// </summary>
        public override bool OnUGUIInvisible()
        {
            return !MyUGUIManager.Instance.CurrentSceneFading.IsFading;
        }

        /// <summary>
        /// OnUGUIDestroy.
        /// </summary>
        public override void OnUGUIDestroy()
        {
            base.OnUGUIDestroy();
        }

        /// <summary>
        /// OnUGUIBackKey.
        /// </summary>
        public virtual void OnUGUIBackKey()
        {
            MyUGUIManager.Instance.Back();
        }

        #endregion
    }
}