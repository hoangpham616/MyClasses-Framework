/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MySerializableMonoDictionary (version 1.0)
 */

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyClasses
{
    public class MySerializableMonoDictionary<TKey, TValue> : MonoBehaviour, ISerializationCallbackReceiver
    {
        #region ----- Internal Class -----

        [Serializable]
        public class KeyValueEntry
        {
            public TKey Key;
            public TValue Value;
        }

        #endregion

        #region ----- Variable -----

        [SerializeField]
        private List<KeyValueEntry> _entries;

        private Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();
        public Dictionary<TKey, TValue> Dictionary
        {
            get { return _dictionary; }
        }

        #endregion

        #region ----- ISerializationCallbackReceiver Implementation -----

        /// <summary>
        /// OnBeforeSerialize.
        /// </summary>
        public void OnBeforeSerialize()
        {
            if (_entries == null)
            {
                return;
            }

#if UNITY_EDITOR
            List<TKey> keys = new List<TKey>();
            for (int i = 0, count = _entries.Count; i < count; i++)
            {
                keys.Add(_entries[i].Key);
            }

            var result = keys.GroupBy(x => x)
                            .Where(g => g.Count() > 1)
                            .Select(x => new { Element = x.Key, Count = x.Count() })
                            .ToList();
            if (result.Count > 0)
            {
                var duplicates = string.Join(", ", result);

                Debug.LogError($"[{typeof(MySerializableMonoDictionary<TKey, TValue>).Name}] OnBeforeSerialize(): {GetType().FullName} keys has duplicates {duplicates}");
            }
#endif
        }

        /// <summary>
        /// OnAfterDeserialize.
        /// </summary>
        public void OnAfterDeserialize()
        {
            _dictionary.Clear();
            for (int i = 0, count = _entries.Count; i < count; i++)
            {
                KeyValueEntry entry = _entries[i];
                _dictionary.Add(entry.Key, entry.Value);
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set entries. It should only be called once to create a prefab.
        /// </summary>
        public void SetEntries(List<MySerializableMonoDictionary<TKey, TValue>.KeyValueEntry> entries)
        {
            _entries = entries;

            _dictionary.Clear();
            for (int i = 0, count = _entries.Count; i < count; i++)
            {
                KeyValueEntry entry = _entries[i];
                _dictionary.Add(entry.Key, entry.Value);
            }
        }

        #endregion
    }
}