/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUGUIHUD (version 2.6)
 */

using UnityEngine;

namespace MyClasses.UI
{
    public abstract class MyUGUIHUD : MyUGUIBase
    {
        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyUGUIHUD(string prefabName)
            : base(prefabName, null)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyUGUIHUD(string prefabName, string prefabName3D)
            : base(prefabName, prefabName3D)
        {
        }

        #endregion

        #region ----- MyUGUIBase Implementation -----

        /// <summary>
        /// OnUGUIInit.
        /// </summary>
        public override void OnUGUIInit()
        {
            base.OnUGUIInit();

            GameObject = MyUtilities.FindObjectInFirstLayer(MyUGUIManager.Instance.CanvasOnTopHUD, PrefabName);
            if (GameObject == null)
            {
                GameObject = GameObject.Instantiate(Resources.Load(MyUGUIManager.HUD_DIRECTORY + PrefabName), Vector3.zero, Quaternion.identity) as GameObject;
                GameObject.name = PrefabName;
                GameObject.transform.SetParent(MyUGUIManager.Instance.CanvasOnTopHUD.transform, false);
            }
            GameObject.SetActive(false);
            
            if (PrefabName3D.Length > 0)
            {
                GameObject3D = MyUtilities.FindObjectInRoot(PrefabName3D);
                if (GameObject3D == null)
                {
                    GameObject3D = GameObject.Instantiate(Resources.Load(MyUGUIManager.HUD_DIRECTORY + PrefabName3D), Vector3.zero, Quaternion.identity) as GameObject;
                    GameObject3D.name = PrefabName3D;
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
        }

        /// <summary>
        /// OnUGUIVisible.
        /// </summary>
        public override bool OnUGUIVisible()
        {
            return true;
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
        }

        /// <summary>
        /// OnUGUIInvisible.
        /// </summary>
        public override bool OnUGUIInvisible()
        {
            if (GameObject != null)
            {
                GameObject.SetActive(false);
            }
            return true;
        }

        /// <summary>
        /// OnUGUIDestroy.
        /// </summary>
        public override void OnUGUIDestroy()
        {
            base.OnUGUIDestroy();

            IsLoaded = false;
        }

        /// <summary>
        /// OnUGUISceneSwitch.
        /// </summary>
        public virtual void OnUGUISceneSwitch(MyUGUIScene scene)
        {
        }

        /// <summary>
        /// OnUGUIPopupShow.
        /// </summary>
        public virtual void OnUGUIPopupShow(MyUGUIPopup popup)
        {
        }

        #endregion
    }
}