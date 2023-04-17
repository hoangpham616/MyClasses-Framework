/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUGUIPopup (version 2.24)
 */

using UnityEngine;
using System;

namespace MyClasses.UI
{
    public abstract class MyUGUIPopup : MyUGUIBase
    {
        #region ----- Variable -----

        private EPopupID _id;
        private Animator _animator;
        private bool _isFloat;
        private bool _isRepeatable;
        private bool _isRetainable;
        private object _attachedData;
        private Action<MyUGUIPopup> _onEnterCallback;
        private Action _onCloseCallback;

        #endregion

        #region ----- Property -----

        public EPopupID ID
        {
            get { return _id; }
        }

        public object AttachedData
        {
            get { return _attachedData; }
            set { _attachedData = value; }
        }

        public bool IsFloat
        {
            get { return _isFloat; }
        }

        public bool IsRepeatable
        {
            get { return _isRepeatable; }
        }

        public bool IsRetainable
        {
            get { return _isRetainable; }
        }

        public bool IsShowing
        {
            get { return State >= EBaseState.Enter; }
        }

        public Action<MyUGUIPopup> OnEnterCallback
        {
            set { _onEnterCallback = value; }
        }

        public Action OnCloseCallback
        {
            set { _onCloseCallback = value; }
        }

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="isRepeatable">show multiple popups at the same time</param>
        public MyUGUIPopup(EPopupID id, string prefabName, bool isFloat = false, bool isRepeatable = false)
            : base(prefabName, null)
        {
            _id = id;
            _isFloat = isFloat;
            _isRepeatable = isRepeatable;
            _isRetainable = !isRepeatable;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="isRepeatable">show multiple popups at the same time</param>
        public MyUGUIPopup(EPopupID id, string prefabName, string prefabName3D, bool isFloat = false, bool isRepeatable = false)
            : base(prefabName, prefabName3D)
        {
            _id = id;
            _isFloat = isFloat;
            _isRepeatable = isRepeatable;
            _isRetainable = !isRepeatable;
        }

        #endregion

        #region ----- MyUGUIBase Implementation -----

        /// <summary>
        /// OnUGUIInit.
        /// </summary>
        public override void OnUGUIInit()
        {
            base.OnUGUIInit();

            GameObject parent = _isFloat ? MyUGUIManager.Instance.CanvasOnTopFloatPopup : MyUGUIManager.Instance.CanvasOnTopPopup;

            if (_isRepeatable)
            {
                if (IsUseAssetBundle)
                {
                    if (Bundle == null)
                    {
                        Debug.LogError("[" + typeof(MyUGUIPopup).Name + "] OnUGUIInit(): Asset bundle null.");
                    }
                    else
                    {
                        GameObject = GameObject.Instantiate(Bundle.LoadAsset(PrefabName) as GameObject);
                        if (PrefabName3D.Length > 0)
                        {
                            GameObject3D = GameObject.Instantiate(Bundle.LoadAsset(PrefabName3D) as GameObject);
                        }
                    }
                }
                else
                {
                    GameObject = GameObject.Instantiate(Resources.Load(MyUGUIManager.POPUP_DIRECTORY + PrefabName), Vector3.zero, Quaternion.identity) as GameObject;
                    if (PrefabName3D.Length > 0)
                    {
                        GameObject3D = GameObject.Instantiate(Resources.Load(MyUGUIManager.POPUP_DIRECTORY + PrefabName3D), Vector3.zero, Quaternion.identity) as GameObject;
                    }
                }
                int random = UnityEngine.Random.Range(0, int.MaxValue);
                GameObject.name = PrefabName + "_Reaptable (" + random + ")";
                if (GameObject3D != null)
                {
                    GameObject3D.name = PrefabName3D + "_Reaptable (" + random + ")";
                }
            }
            else
            {
                GameObject = MyUtilities.FindObjectInFirstLayer(parent, PrefabName);
                if (GameObject == null)
                {
                    if (IsUseAssetBundle)
                    {
                        if (Bundle == null)
                        {
                            Debug.LogError("[" + typeof(MyUGUIPopup).Name + "] OnUGUIInit(): Asset bundle null.");
                        }
                        else
                        {
                            GameObject = GameObject.Instantiate(Bundle.LoadAsset(PrefabName) as GameObject);
                            if (PrefabName3D.Length > 0)
                            {
                                GameObject3D = GameObject.Instantiate(Bundle.LoadAsset(PrefabName3D) as GameObject);
                            }
                        }
                    }
                    else
                    {
                        GameObject = GameObject.Instantiate(Resources.Load(MyUGUIManager.POPUP_DIRECTORY + PrefabName), Vector3.zero, Quaternion.identity) as GameObject;
                        GameObject.name = PrefabName;

                        if (PrefabName3D.Length > 0)
                        {
                            GameObject3D = GameObject.Instantiate(Resources.Load(MyUGUIManager.POPUP_DIRECTORY + PrefabName3D), Vector3.zero, Quaternion.identity) as GameObject;
                            GameObject3D.name = PrefabName3D;
                        }
                    }
                }
            }

            GameObject.transform.SetParent(parent.transform, false);
            if (GameObject3D != null)
            {
                GameObject3D.SetActive(false);
            }
        }

        /// <summary>
        /// OnUGUIEnter.
        /// </summary>
        public override void OnUGUIEnter()
        {
            base.OnUGUIEnter();

            GameObject.transform.SetAsLastSibling();

            _animator = GameObject.GetComponent<Animator>();
            if (_animator != null)
            {
                _animator.Play("Show");
            }

            if (_onEnterCallback != null)
            {
                _onEnterCallback(this);
            }
        }

        /// <summary>
        /// OnUGUIVisible.
        /// </summary>
        public override bool OnUGUIVisible()
        {
            if (_animator != null && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                return false;
            }

            return base.OnUGUIVisible();
        }

        /// <summary>
        /// OnUGUIUpdate.
        /// </summary>
        public override void OnUGUIUpdate(float deltaTime)
        {
            base.OnUGUIUpdate(deltaTime);
        }

        /// <summary>
        /// OnUGUIExit.
        /// </summary>
        public override void OnUGUIExit()
        {
            base.OnUGUIExit();

            if (_animator != null)
            {
                _animator.Play("Hide");
            }
        }

        /// <summary>
        /// OnUGUIInvisible.
        /// </summary>
        public override bool OnUGUIInvisible()
        {
            if (_animator != null && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 && GameObject.activeSelf)
            {
                return false;
            }
            
            if (base.OnUGUIInvisible())
            {
                if (_onCloseCallback != null)
                {
                    _onCloseCallback();
                    _onCloseCallback = null;
                }
                return true;
            }

            return false;
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

        #region ----- Public Method -----

        /// <summary>
        /// Hide and destroy.
        /// </summary>
        public void HideAndDestroy()
        {
            _isRetainable = false;

            State = EBaseState.Exit;
        }

        /// <summary>
        /// Hide popup.
        /// </summary>
        public virtual void Hide()
        {
            State = EBaseState.Exit;
        }

        #endregion
    }
}