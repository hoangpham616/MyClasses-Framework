using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using MyClasses;
using MyClasses.UI;

namespace MyApp
{
    public class ReusableListViewPopup : MyUGUIPopup
    {
        #region ----- Variable -----

        private MyUGUIButton _btnClose;
        private MyUGUIButton _btnTop;
        private MyUGUIButton _btnMid;
        private MyUGUIButton _btnBot;
        private MyUGUIReusableListView _listView;
        private MyUGUIScrollView _scrollView;

        #endregion

        #region ----- Constructor -----

        public ReusableListViewPopup(EPopupID id, string prefabName, bool isFloat = false, bool isRepeatable = false)
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
            _btnTop = MyUtilities.FindObject(GameObject, "Container/ButtonTop").GetComponent<MyUGUIButton>();
            _btnMid = MyUtilities.FindObject(GameObject, "Container/ButtonMid").GetComponent<MyUGUIButton>();
            _btnBot = MyUtilities.FindObject(GameObject, "Container/ButtonBot").GetComponent<MyUGUIButton>();
            _listView = MyUtilities.FindObject(GameObject, "Container/ListView").GetComponent<MyUGUIReusableListView>();
            _scrollView = _listView.GetComponent<MyUGUIScrollView>();
        }

        public override void OnUGUIEnter()
        {
            this.LogInfo("OnUGUIEnter", null, ELogColor.DARK_UI);

            base.OnUGUIEnter();

            _btnClose.OnEventPointerClick.AddListener(_OnClickClose);
            _btnTop.OnEventPointerClick.AddListener(_OnClickTop);
            _btnMid.OnEventPointerClick.AddListener(_OnClickMid);
            _btnBot.OnEventPointerClick.AddListener(_OnClickBot);

            int quantity = 1000;
            ReusableListViewItemModel[] models = new ReusableListViewItemModel[quantity];
            for (int i = 0; i < quantity; ++i)
            {
                models[i] = new ReusableListViewItemModel();
                models[i].Letter = ((char)UnityEngine.Random.Range(65, 90)).ToString();
            }
            _listView.Initialize();
            _listView.SetModels(models);
            _listView.Reload(quantity);
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
            _btnTop.OnEventPointerClick.RemoveAllListeners();
            _btnMid.OnEventPointerClick.RemoveAllListeners();
            _btnBot.OnEventPointerClick.RemoveAllListeners();
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
            this.LogInfo("OnUGUIBackKey", null, ELogColor.DARK_UI);

            MyUGUIManager.Instance.Back();
        }

        #endregion

        #region ----- Button Event -----

        private void _OnClickClose(PointerEventData arg0)
        {
            this.LogInfo("_OnClickClose", null, ELogColor.UI);

            Hide();
        }

        private void _OnClickTop(PointerEventData arg0)
        {
            this.LogInfo("_OnClickTop", null, ELogColor.UI);

            float moveTime = 1f - _scrollView.VerticalNormalizedPosition;
            moveTime = Mathf.Max(moveTime, 0.2f);
            _scrollView.MoveToStart(1f - _scrollView.VerticalNormalizedPosition, () =>
            {
                this.LogInfo("_OnClickTop", "list has move in " + moveTime + " seconds");
            });
        }

        private void _OnClickMid(PointerEventData arg0)
        {
            this.LogInfo("_OnClickMid", null, ELogColor.UI);

            float moveTime = Mathf.Abs(0.5f - _scrollView.VerticalNormalizedPosition);
            moveTime = Mathf.Max(moveTime, 0.2f);
            _scrollView.MoveToMiddle(moveTime, () =>
            {
                this.LogInfo("_OnClickMid", "list has move in " + moveTime + " seconds");
            });
        }

        private void _OnClickBot(PointerEventData arg0)
        {
            this.LogInfo("_OnClickBot", null, ELogColor.UI);

            float moveTime = _scrollView.VerticalNormalizedPosition;
            moveTime = Mathf.Max(moveTime, 0.2f);
            _scrollView.MoveToEnd(moveTime, () =>
            {
                this.LogInfo("_OnClickBot", "list has move in " + moveTime + " seconds");
            });
        }

        #endregion

        #region ----- Public Method -----



        #endregion

        #region ----- Private Method -----



        #endregion
    }
}