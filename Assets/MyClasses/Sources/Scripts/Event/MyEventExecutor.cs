/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyEventExecutor (version 1.1)
 */

using System.Collections.Generic;

namespace MyClasses
{
    public class MyEventExecutor
    {
        #region ----- Variable -----

        public delegate void EventFunction<T>(T listener);

        private List<object> _listObject = new List<object>();

        #endregion

        #region ----- Singleton -----

        private static MyEventExecutor _instance;

        public static MyEventExecutor Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MyEventExecutor();
                }
                return _instance;
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Register a new object.
        /// </summary>
        /// <param name="isHighPriority">add new object at the first index</param>
        public void Register(object obj, bool isHighPriority = false)
        {
            if (!_listObject.Contains(obj))
            {
                if (isHighPriority)
                {
                    _listObject.Insert(0, obj);
                }
                else
                {
                    _listObject.Add(obj);
                }
            }
        }

        /// <summary>
        /// Unregister a object.
        /// </summary>
        public void Unregister(object obj)
        {
            _listObject.Remove(obj);
        }

        /// <summary>
        /// Unregister all objects.
        /// </summary>
        public void UnregisterAll()
        {
            _listObject.Clear();
        }

        /// <summary>
        /// Execute the specified functions.
        /// </summary>
        public void Execute<T>(EventFunction<T> functor) where T : class
        {
            foreach (var obj in _listObject)
            {
                var listener = obj as T;
                if (listener != null)
                {
                    functor.Invoke(listener);
                }
            }
        }

        #endregion
    }
}