using UnityEngine.EventSystems;
using MyClasses;
using MyClasses.UI;

namespace MyApp
{
    public class LocalizationPopup : MyUGUIPopup
    {
        #region ----- Variable -----

        private MyUGUIButton _btnClose;
        private MyUGUIButton _btnEnglish;
        private MyUGUIButton _btnVietnamese;

        #endregion

        #region ----- Constructor -----

        public LocalizationPopup(EPopupID id, string prefabName, bool isFloat = false, bool isRepeatable = false)
            : base(id, prefabName, isFloat, isRepeatable)
        {
        }

        #endregion

        #region ----- MyUGUIPopup Implementation -----

        public override void OnUGUIInit()
        {
            this.LogInfo("OnUGUIInit", null, ELogColor.DARK_UI);

            base.OnUGUIInit();

            _btnClose = MyUtilities.FindObject(GameObject, "Container/ButtonClose").GetComponent<MyUGUIButton>();
            _btnEnglish = MyUtilities.FindObject(GameObject, "Container/ButtonEnglish").GetComponent<MyUGUIButton>();
            _btnVietnamese = MyUtilities.FindObject(GameObject, "Container/ButtonVietnamese").GetComponent<MyUGUIButton>();
        }

        public override void OnUGUIEnter()
        {
            this.LogInfo("OnUGUIEnter", "popup id = " + MyUGUIManager.Instance.CurrentPopup.ID.ToString(), ELogColor.DARK_UI);

            base.OnUGUIEnter();

            _btnClose.OnEventPointerClick.AddListener(_OnClickClose);
            _btnEnglish.OnEventPointerClick.AddListener(_OnClickEnglish);
            _btnVietnamese.OnEventPointerClick.AddListener(_OnClickVietnamese);
        }

        public override bool OnUGUIVisible()
        {
            if (base.OnUGUIVisible())
            {
                this.LogInfo("OnUGUIVisible", null, ELogColor.DARK_UI);
                return true;
            }
            return false;
        }

        public override void OnUGUIUpdate(float deltaTime)
        {
        }

        public override void OnUGUIExit()
        {
            this.LogInfo("OnUGUIExit", null, ELogColor.DARK_UI);

            base.OnUGUIExit();

            _btnClose.OnEventPointerClick.RemoveAllListeners();
            _btnEnglish.OnEventPointerClick.RemoveAllListeners();
            _btnVietnamese.OnEventPointerClick.RemoveAllListeners();
        }

        public override bool OnUGUIInvisible()
        {
            if (base.OnUGUIInvisible())
            {
                this.LogInfo("OnUGUIInvisible", null, ELogColor.DARK_UI);
                return true;
            }
            return false;
        }

        public override void OnUGUIBackKey()
        {
            MyUGUIManager.Instance.Back();
        }

        #endregion

        #region ----- Button Event -----

        private void _OnClickClose(PointerEventData arg0)
        {
            Hide();
        }

        private void _OnClickEnglish(PointerEventData arg0)
        {
            this.LogInfo("_OnClickEnglish", null, ELogColor.UI);

            MyLocalizationManager.Instance.LoadLanguage(MyLocalizationManager.ELanguage.English);
            MyLocalizationManager.Instance.Refresh();
        }

        private void _OnClickVietnamese(PointerEventData arg0)
        {
            this.LogInfo("_OnClickVietnamese", null, ELogColor.UI);

            MyLocalizationManager.Instance.LoadLanguage(MyLocalizationManager.ELanguage.Vietnamese);
            MyLocalizationManager.Instance.Refresh();
        }

        #endregion

        #region ----- Public Method -----



        #endregion

        #region ----- Private Method -----



        #endregion
    }
}