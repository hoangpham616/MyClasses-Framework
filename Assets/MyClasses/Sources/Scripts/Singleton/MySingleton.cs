/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MySingleton (version 1.1)
 */

namespace MyClasses
{
    public abstract class MySingleton<T> where T : MySingleton<T>, new()
    {
        #region ----- Variable -----

        private static T _instance = null;

        #endregion

        #region ----- Property -----

        /// <summary>
        /// Return the only one instance of inherited class.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Destroy the instance.
        /// </summary>
        public virtual void DestroyInstance()
        {
            _instance = null;
        }

        #endregion
    }
}