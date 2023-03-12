/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUGUITouchZone (version 2.1)
 */

#pragma warning disable 0114
#pragma warning disable 0414
#pragma warning disable 0649

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine.UI;

namespace MyClasses.UI
{
    public class MyUGUITouchZone : Graphic
    {
        #region ----- Graphic Implementation -----

        /// <summary>
        /// OnPopulateMesh.
        /// </summary>
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyUGUITouchZone))]
    public class MyUGUITouchZoneEditor : Editor
    {
        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
        }
    }

#endif
}
