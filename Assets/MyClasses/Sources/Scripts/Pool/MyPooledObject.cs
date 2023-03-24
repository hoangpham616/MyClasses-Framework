/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyPooledObject (version 2.1)
 */

using UnityEngine;

namespace MyClasses
{
    public class MyPooledObject : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        private string _pool;

        #endregion

        #region ----- Property -----

        public string Pool
        {
            get { return _pool; }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set pool.
        /// </summary>
        public void SetPool(string pool)
        {
            _pool = pool;
        }

        #endregion
    }
}