/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUGUIReusableListItem (version 2.2)
 */

#pragma warning disable 0114
#pragma warning disable 0414
#pragma warning disable 0649

using UnityEngine;

namespace MyClasses.UI
{
    public abstract class MyUGUIReusableListItem : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        private MyUGUIReusableListView mListView;
        [SerializeField]
        private int mIndex;

        #endregion

        #region ----- Property -----

        public MyUGUIReusableListView ListView
        { 
            get { return mListView; } 
        }

        public int Index 
        { 
            get { return mIndex; }
        }

        public object Model
        { 
            get
            { 
                if (mListView != null && mListView.Models != null && mIndex < mListView.Models.Length)
                {
                    return mListView.Models[mIndex];
                }
                return null;
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set list view.
        /// </summary>
        public void SetListView(MyUGUIReusableListView listView)
        {
            mListView = listView;
        }

        /// <summary>
        /// Set index.
        /// </summary>
        public void SetIndex(int index)
        {
            mIndex = index;
        }

        #endregion

        #region ----- Event -----

        /// <summary>
        /// OnReload.
        /// </summary>
        public abstract void OnReload();

        #endregion
    }
}
