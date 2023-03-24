/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyEventEmitter (version 1.1)
 */

using System;
using System.Collections.Generic;

namespace MyClasses
{
    public static class MyEventEmitter
    {
        #region ----- Variable -----

        private static Dictionary<int, List<Action<object>>> _eventData = new Dictionary<int, List<Action<object>>>();

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Register an event.
        /// </summary>
        public static void Register(int eventID, Action<object> callback)
        {
            if (!_eventData.ContainsKey(eventID))
            {
                _eventData[eventID] = new List<Action<object>>();
            }
            _eventData[eventID].Add(callback);
        }

        /// <summary>
        /// Unregister an event.
        /// </summary>
        public static void Unregister(int eventID)
        {
            if (_eventData.ContainsKey(eventID))
            {
                _eventData.Remove(eventID);
            }
        }

        /// <summary>
        /// Unregister a callback.
        /// </summary>
        public static void Unregister(int eventID, Action<object> callback)
        {
            if (_eventData.ContainsKey(eventID))
            {
                _eventData[eventID].Remove(callback);
            }
        }

        /// <summary>
        /// Unregister all events.
        /// </summary>
        public static void UnregisterAll()
        {
            _eventData.Clear();
        }

        /// <summary>
        /// Call all callback functions of an event.
        /// </summary>
        public static void Emit(int eventID, object param)
        {
            if (_eventData.ContainsKey(eventID))
            {
                List<Action<object>> actions = _eventData[eventID];
                int length = actions.Count;
                for (int i = 0; i < length; i++)
                {
                    actions[i](param);
                }
            }
        }

        #endregion
    }
}