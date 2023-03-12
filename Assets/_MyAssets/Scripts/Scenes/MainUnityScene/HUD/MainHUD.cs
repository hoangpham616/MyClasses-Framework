using UnityEngine.UI;
using MyClasses;
using MyClasses.UI;

namespace MyApp.UI
{
    public class MainHUD : MyUGUIHUD
    {
        #region ----- Variable -----

        private Text _txtSceneName;

        #endregion

        #region ----- Constructor -----

        public MainHUD(string prefabName)
            : base(prefabName)
        {
        }

        #endregion

        #region ----- MyUGUIHUD Implementation -----

        public override void OnUGUIInit()
        {
            this.LogInfo("OnUGUIInit", null, ELogColor.DARK_UI);

            base.OnUGUIInit();

            _txtSceneName = MyUtilities.FindObject(GameObject, "SceneName").GetComponent<Text>();
        }

        public override void OnUGUIEnter()
        {
            this.LogInfo("OnUGUIEnter", null, ELogColor.DARK_UI);

            base.OnUGUIEnter();
        }

        public override void OnUGUIUpdate(float deltaTime)
        {
        }

        public override void OnUGUIExit()
        {
            this.LogInfo("OnUGUIExit", null, ELogColor.DARK_UI);

            base.OnUGUIExit();
        }

        public override void OnUGUISceneSwitch(MyUGUIScene scene)
        {
            this.LogInfo("OnUGUISceneSwitch", "switch to " + scene.ID.ToString(), ELogColor.UI);

            switch (scene.ID)
            {
                case ESceneID.MainMenuScene:
                    {
                        _txtSceneName.text = "HUD\nMAIN MENU SCENE";
                    }
                    break;

                case ESceneID.GameScene:
                    {
                        _txtSceneName.text = "HUD\nGAME SCENE";
                    }
                    break;
                    
                default:
                    {
                        _txtSceneName.text = "HUD\nnot implemented yet";
                    }
                    break;
            }
        }

        public override void OnUGUIPopupShow(MyUGUIPopup popup)
        {
            this.LogInfo("OnUGUIPopupShow", popup.ID.ToString() + " shows", ELogColor.UI);
        }

        #endregion

        #region ----- Button Event -----



        #endregion

        #region ----- Public Method -----



        #endregion

        #region ----- Private Method -----



        #endregion
    }
}