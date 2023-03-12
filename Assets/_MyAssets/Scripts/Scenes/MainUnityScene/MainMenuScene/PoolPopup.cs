using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using MyClasses;
using MyClasses.UI;

namespace MyApp
{
    public class PoolPopup : MyUGUIPopup
    {
        #region ----- Variable -----

        private MyUGUIButton _btnClose;
        private MyUGUIButton _btnUse;
        private MyUGUIButton _btnReturn;
        private Transform _trfItemParent;

        private List<Text> _txtItems = new List<Text>();

        #endregion

        #region ----- Constructor -----

        public PoolPopup(EPopupID id, string prefabName, bool isFloat = false, bool isRepeatable = false)
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
            _btnUse = MyUtilities.FindObject(GameObject, "Container/ButtonUse").GetComponent<MyUGUIButton>();
            _btnReturn = MyUtilities.FindObject(GameObject, "Container/ButtonReturn").GetComponent<MyUGUIButton>();
            _trfItemParent = MyUtilities.FindObject(GameObject, "Container/Items").transform;
        }

        public override void OnUGUIEnter()
        {
            this.LogInfo("OnUGUIEnter", "popup id = " + MyUGUIManager.Instance.CurrentPopup.ID.ToString(), ELogColor.DARK_UI);

            base.OnUGUIEnter();

            _btnClose.OnEventPointerClick.AddListener(_OnClickClose);
            _btnUse.OnEventPointerClick.AddListener(_OnClickUse);
            _btnReturn.OnEventPointerClick.AddListener(_OnClickReturn);
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
            _btnUse.OnEventPointerClick.RemoveAllListeners();
            _btnReturn.OnEventPointerClick.RemoveAllListeners();

            for (int i = _txtItems.Count - 1; i >= 0; --i)
            {
                MyPoolManager.Instance.Return(_txtItems[i].gameObject);
            }
            _txtItems.Clear();
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

        private void _OnClickUse(PointerEventData arg0)
        {
            this.LogInfo("_OnClickUse", null, ELogColor.UI);

            GameObject prefab = MyResourceManager.LoadPrefab("Prefabs/TextPoolObject");

            Text text = MyPoolManager.Instance.Use(prefab).GetComponent<Text>();
            text.transform.SetParent(_trfItemParent, false);
            text.transform.localPosition = new Vector3(UnityEngine.Random.Range(-300, 300), UnityEngine.Random.Range(-300, 300), 0);
            _txtItems.Add(text);
        }

        private void _OnClickReturn(PointerEventData arg0)
        {
            this.LogInfo("_OnClickReturn", null, ELogColor.UI);

            if (_txtItems.Count > 0)
            {
                MyPoolManager.Instance.Return(_txtItems[0].gameObject);
                _txtItems.RemoveAt(0);
            }
        }

        #endregion

        #region ----- Public Method -----



        #endregion

        #region ----- Private Method -----



        #endregion
    }
}