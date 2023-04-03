/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUGUIUnityScene (version 2.1)
 */

using System.Collections.Generic;

namespace MyClasses.UI
{
    public class MyUGUIUnityScene
    {
        #region ----- Variable -----

        private EUnitySceneID _id;
        private EUnitySceneState _state;
        private MyUGUIHUD _hud;
        private List<MyUGUIScene> _listScene;
        private string _name;
        
        #endregion

        #region ----- Property -----

        public EUnitySceneID ID
        {
            get { return _id; }
        }

        public EUnitySceneState State
        {
            get { return _state; }
            set { _state = value; }
        }

        public MyUGUIHUD HUD
        {
            get { return _hud; }
        }

        public List<MyUGUIScene> ListScene
        {
            get { return _listScene; }
        }

        public string Name
        {
            get { return _name; }
        }

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyUGUIUnityScene(EUnitySceneID sceneID, string unitySceneName)
        {
            _id = sceneID;
            _state = EUnitySceneState.Idle;
            _name = unitySceneName;
            _hud = null;
            _listScene = new List<MyUGUIScene>();
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set HUD.
        /// </summary>
        public void SetHUD(MyUGUIHUD hud)
        {
            _hud = hud;
        }

        /// <summary>
        /// Add a scene.
        /// </summary>
        public void AddScene(MyUGUIScene scene)
        {
            if (scene != null)
            {
                _listScene.Add(scene);
            }
        }

        #endregion
    }
}