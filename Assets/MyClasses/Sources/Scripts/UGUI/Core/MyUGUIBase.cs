/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUGUIBase (version 2.9)
 */

using UnityEngine;

namespace MyClasses.UI
{
    public abstract class MyUGUIBase
    {
        #region ----- Variable -----

        private EBaseState _state;
        private GameObject _gameObject;
        private GameObject _gameObject3D;
        private AssetBundle _assetBundle;
        private string _assetBundleURL;
        private string _prefabName;
        private string _prefabName3D;
        private int _assetBundleVersion;
        private bool _isAssetBundleLoading;
        private bool _isAssetBundleLoaded;
        private bool _isLoaded;

        #endregion

        #region ----- Property -----

        public EBaseState State
        {
            get { return _state; }
            set { _state = value; }
        }

        public bool IsUseAssetBundle
        {
            get { return !string.IsNullOrEmpty(_assetBundleURL); }
        }

        public string PrefabName
        {
            get { return _prefabName; }
        }

        public string PrefabName3D
        {
            get { return _prefabName3D; }
        }

        public GameObject GameObject
        {
            get { return _gameObject; }
            protected set { _gameObject = value; }
        }

        public GameObject GameObject3D
        {
            get { return _gameObject3D; }
            protected set { _gameObject3D = value; }
        }

        public Transform Transform
        {
            get { return _gameObject != null ? _gameObject.transform : null; }
        }

        public Transform Transform3D
        {
            get { return _gameObject3D != null ? _gameObject3D.transform : null; }
        }

        public AssetBundle Bundle
        {
            get { return _assetBundle; }
        }

        public bool IsActive
        {
            get { return _gameObject != null && _gameObject.activeSelf && State != EBaseState.Idle; }
        }

        public bool IsLoaded
        {
            get { return _isLoaded; }
            protected set { _isLoaded = value; }
        }

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyUGUIBase(string prefabName, string prefabName3D)
        {
            _state = EBaseState.Idle;
            _prefabName = prefabName;
            _prefabName3D = prefabName3D;
            _isAssetBundleLoaded = false;
        }

        #endregion

        #region ----- UGUI Event -----

        /// <summary>
        /// OnUGUILoadAssetBundle.
        /// </summary>
        public bool OnUGUILoadAssetBundle()
        {
            if (!IsUseAssetBundle || _isAssetBundleLoaded)
            {
                return true;
            }

            if (!_isAssetBundleLoading)
            {
                MyAssetBundleManager.Load(_assetBundleURL, _assetBundleVersion, _OnLoadAssetBundleComplete, MyAssetBundleManager.ECacheMode.UnremovableCache);
                _isAssetBundleLoading = true;
            }

            return _isAssetBundleLoaded;
        }

        /// <summary>
        /// OnUGUIInit.
        /// </summary>
        public virtual void OnUGUIInit()
        {
            _isLoaded = true;
        }

        /// <summary>
        /// OnUGUIEnter.
        /// </summary>
        public virtual void OnUGUIEnter()
        {
            if (_gameObject != null)
            {
                _gameObject.SetActive(true);
            }
            if (_gameObject3D != null)
            {
                _gameObject3D.SetActive(true);
            }
        }

        /// <summary>
        /// OnUGUIVisible.
        /// </summary>
        public virtual bool OnUGUIVisible()
        {
            return true;
        }

        /// <summary>
        /// OnUGUIUpdate.
        /// </summary>
        public virtual void OnUGUIUpdate(float deltaTime)
        {
        }

        /// <summary>
        /// OnUGUIExit.
        /// </summary>
        public virtual void OnUGUIExit()
        {
        }

        /// <summary>
        /// OnUGUIInvisible.
        /// </summary>
        public virtual bool OnUGUIInvisible()
        {
            return true;
        }

        /// <summary>
        /// OnDestroy.
        /// </summary>
        public virtual void OnUGUIDestroy()
        {
            _isAssetBundleLoading = false;
            _isAssetBundleLoaded = false;
            _isLoaded = false;
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Setup asset bundle.
        /// </summary>
        public void SetAssetBundle(string url, int versionCode)
        {
            _assetBundleURL = url;
            _assetBundleVersion = versionCode;
        }

        /// <summary>
        /// Setup prefab 3D name.
        /// </summary>
        public void SetPrefabName3D(string prefabName3D)
        {
            _prefabName3D = prefabName3D;
        }

        #endregion

        #region ----- Asset Bundle Event -----

        /// <summary>
        /// Loading asset bundle complete.
        /// </summary>
        private void _OnLoadAssetBundleComplete(AssetBundle assetBundle)
        {
            _assetBundle = assetBundle;
            _isAssetBundleLoaded = true;
        }

        #endregion
    }
}