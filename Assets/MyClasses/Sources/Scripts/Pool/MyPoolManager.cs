/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyPoolManager (version 2.13)
 */

using UnityEngine;
using System.Collections.Generic;

namespace MyClasses
{
    public class MyPoolManager : MonoBehaviour
    {
        #region ----- Define -----

        private const string GROUP_FREE = "Free";
        private const string GROUP_OCCUPIED = "Occupied";

        #endregion

        #region ----- Variable -----

        private Dictionary<string, MyPool> _dictionaryPooler = new Dictionary<string, MyPool>();

        #endregion

        #region ----- Singleton -----

        private static object _singletonLock = new object();
        private static MyPoolManager _instance;

        public static MyPoolManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_singletonLock)
                    {
                        _instance = (MyPoolManager)FindObjectOfType(typeof(MyPoolManager));
                        if (_instance == null)
                        {
                            GameObject obj = new GameObject(typeof(MyPoolManager).Name);
                            _instance = obj.AddComponent<MyPoolManager>();
                            DontDestroyOnLoad(obj);
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Provide an available object. If no objects are present in the pool, a new object is created and returned.
        /// </summary>
        /// <param name="isForceCreate">always create new object</param>
        public GameObject Use(string path, bool isForceCreate = false)
        {
            if (_dictionaryPooler.ContainsKey(path))
            {
                if (isForceCreate)
                {
                    return _dictionaryPooler[path].Create();
                }

                return _dictionaryPooler[path].Use();
            }

            GameObject root = MyUtilities.FindObjectInFirstLayer(gameObject, path);
            if (root == null)
            {
                root = new GameObject(path);
                root.transform.SetParent(transform, false);
            }

            GameObject rootFree = MyUtilities.FindObjectInFirstLayer(root, GROUP_FREE);
            if (rootFree == null)
            {
                rootFree = new GameObject(GROUP_FREE);
                rootFree.transform.SetParent(root.transform, false);
            }

            GameObject rootOccupied = MyUtilities.FindObjectInFirstLayer(root, GROUP_OCCUPIED);
            if (rootOccupied == null)
            {
                rootOccupied = new GameObject(GROUP_OCCUPIED);
                rootOccupied.transform.SetParent(root.transform, false);
            }

            _dictionaryPooler.Add(path, new MyPool(path, rootFree, rootOccupied));

            return Use(path, isForceCreate);
        }

        /// <summary>
        /// Provide an available object. If no objects are present in the pool, a new object is created and returned.
        /// </summary>
        public GameObject Use(GameObject prefab)
        {
            if (_dictionaryPooler.ContainsKey(prefab.name))
            {
                _dictionaryPooler[prefab.name].SetTemplate(prefab);
                return _dictionaryPooler[prefab.name].Use();
            }

            GameObject root = MyUtilities.FindObjectInFirstLayer(gameObject, prefab.name);
            if (root == null)
            {
                root = new GameObject(prefab.name);
                root.transform.SetParent(transform, false);
            }

            GameObject rootFree = MyUtilities.FindObjectInFirstLayer(root, GROUP_FREE);
            if (rootFree == null)
            {
                rootFree = new GameObject(GROUP_FREE);
                rootFree.transform.SetParent(root.transform, false);
            }

            GameObject rootOccupied = MyUtilities.FindObjectInFirstLayer(root, GROUP_OCCUPIED);
            if (rootOccupied == null)
            {
                rootOccupied = new GameObject(GROUP_OCCUPIED);
                rootOccupied.transform.SetParent(root.transform, false);
            }

            _dictionaryPooler.Add(prefab.name, new MyPool(prefab.name, rootFree, rootOccupied));

            _dictionaryPooler[prefab.name].SetTemplate(prefab);
            return Use(prefab.name, false);
        }

        /// <summary>
        /// Add an object back into the pool.
        /// </summary>
        public void Return(GameObject obj)
        {
            if (obj != null)
            {
                MyPooledObject poolObject = obj.GetComponent<MyPooledObject>();
                if (poolObject != null)
                {
                    if (_dictionaryPooler.ContainsKey(poolObject.Pool))
                    {
                        _dictionaryPooler[poolObject.Pool].Return(obj);
                    }
                }
            }
        }

        #endregion
    }

    public class MyPool
    {
        #region ----- Variable -----

        private string _path;
        private string _objectName;
        private GameObject _template;
        private GameObject _free;
        private GameObject _occupied;
        private List<GameObject> _listFreeObject;
        private List<GameObject> _listOccupiedObject;

        #endregion

        #region ----- Property -----

        public string Name
        {
            get { return _path; }
        }

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyPool(string path, GameObject rootFree, GameObject rootOccupied)
        {
            _path = path;
            _objectName = path.Substring(path.LastIndexOf("/") + 1);
            _free = rootFree;
            _occupied = rootOccupied;
            _listFreeObject = new List<GameObject>();
            _listOccupiedObject = new List<GameObject>();
        }

        #endregion

        #region ----- Public Method -----
        
        /// <summary>
        /// Set the template.
        /// </summary>
        public void SetTemplate(GameObject template)
        {
            _template = template;
        }

        /// <summary>
        /// Provide an available object. If no objects are present in the pool, a new object is created and returned.
        /// </summary>
        public GameObject Use()
        {
            if (_listFreeObject != null)
            {
                GameObject oldObj;

                int count = _listFreeObject.Count;
                for (int i = 0; i < count; i++)
                {
                    oldObj = _listFreeObject[i];
                    if (!oldObj.activeInHierarchy)
                    {
                        oldObj.transform.SetParent(_occupied.transform, false);
                        oldObj.SetActive(true);

                        if (!_listOccupiedObject.Contains(oldObj))
                        {
                            _listOccupiedObject.Add(oldObj);
                        }

                        _listFreeObject.Remove(oldObj);

                        return oldObj;
                    }
                }
            }

            return Create();
        }

        /// <summary>
        /// Add an object back into the pool.
        /// </summary>
        public void Return(GameObject obj)
        {
            if (obj != null)
            {
                obj.transform.SetParent(_free.transform, false);
                obj.SetActive(false);

                if (!_listFreeObject.Contains(obj))
                {
                    _listFreeObject.Add(obj);
                }

                _listOccupiedObject.Remove(obj);
            }
        }

        /// <summary>
        /// Create a new object.
        /// </summary>
        public GameObject Create()
        {
            if (_template == null)
            {
                _template = Resources.Load(_path) as GameObject;
                if (_template == null)
                {
                    Debug.LogError("[" + typeof(MyPool).Name + "] Create(): Could not find file \"" + _path + "\"");
                    return null;
                }
            }

            GameObject newObj = GameObject.Instantiate(_template);
            newObj.SetActive(true);
            newObj.name = _objectName;
            newObj.transform.SetParent(_occupied.transform, false);

            MyPooledObject poolObject = newObj.GetComponent<MyPooledObject>();
            if (poolObject == null)
            {
                poolObject = newObj.AddComponent<MyPooledObject>();
            }
            poolObject.SetPool(_path);

            _listOccupiedObject.Add(newObj);

            return newObj;
        }

        #endregion
    }
}