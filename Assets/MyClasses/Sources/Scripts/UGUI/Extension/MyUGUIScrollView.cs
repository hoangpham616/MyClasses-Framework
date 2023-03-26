/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUGUIScrollView (version 2.2)
 */

#pragma warning disable 0114
#pragma warning disable 0414
#pragma warning disable 0649

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace MyClasses.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class MyUGUIScrollView : MonoBehaviour
    {
        #region ----- Variable -----

        private ScrollRect mScrollRect;

        #endregion

        #region ----- Property -----

        public ScrollRect ScrollRect
        {
            get { return mScrollRect; }
        }

        public float HorizontalNormalizedPosition
        {
            get { return mScrollRect.horizontalNormalizedPosition; }
        }

        public float VerticalNormalizedPosition
        {
            get { return mScrollRect.verticalNormalizedPosition; }
        }

        #endregion

        #region ----- MonoBehaviour Implementation -----

        void Awake()
        {
            mScrollRect = gameObject.GetComponent<ScrollRect>();
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Move x-axis.
        /// </summary>
        /// <param name="targetPosition">0: left, 1: right</param>
        public void MoveX(float targetPosition, float second, Action callback = null)
        {
            StartCoroutine(_DoMoveX(targetPosition, second, callback));
        }

        /// <summary>
        /// Move y-axis.
        /// </summary>
        /// <param name="targetPosition">0: top, 1: bottom</param>
        public void MoveY(float targetPosition, float second, Action callback = null)
        {
            StartCoroutine(_DoMoveY(1 - targetPosition, second, callback));
        }

        /// <summary>
        /// Move to start of scrollview.
        /// </summary>
        public void MoveToStart(float second = 0, Action callback = null)
        {
            if (mScrollRect.horizontal)
            {
                MoveX(0, second, callback);
            }
            if (mScrollRect.vertical)
            {
                MoveY(0, second, callback);
            }
        }

        /// <summary>
        /// Move to middle of scrollview.
        /// </summary>
        public void MoveToMiddle(float second = 0, Action callback = null)
        {
            if (mScrollRect.horizontal)
            {
                MoveX(0.5f, second, callback);
            }
            if (mScrollRect.vertical)
            {
                MoveY(0.5f, second, callback);
            }
        }

        /// <summary>
        /// Move to end of scrollview.
        /// </summary>
        public void MoveToEnd(float second = 0, Action callback = null)
        {
            if (mScrollRect.horizontal)
            {
                MoveX(1, second, callback);
            }
            if (mScrollRect.vertical)
            {
                MoveY(1, second, callback);
            }
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Move x-axis.
        /// </summary>
        private IEnumerator _DoMoveX(float targetPosition, float second, Action callback = null)
        {
            float startPosition = mScrollRect.horizontalNormalizedPosition;
            float velocity = 1 / second;
            float position = 0;

            while (position < 1)
            {
                position += velocity * Time.deltaTime;
                mScrollRect.horizontalNormalizedPosition = Mathf.Lerp(startPosition, targetPosition, position);
                
                yield return 0;
            }

            if (callback != null)
            {
                callback();
            }
        }

        /// <summary>
        /// Move y-axis.
        /// </summary>
        private IEnumerator _DoMoveY(float targetPosition, float second, Action callback = null)
        {
            float startPosition = mScrollRect.verticalNormalizedPosition;
            float velocity = 1 / second;
            float position = 0;

            while (position < 1)
            {
                position += velocity * Time.deltaTime;
                mScrollRect.verticalNormalizedPosition = Mathf.Lerp(startPosition, targetPosition, position);
                
                yield return 0;
            }

            if (callback != null)
            {
                callback();
            }
        }

        #endregion
    }
}
