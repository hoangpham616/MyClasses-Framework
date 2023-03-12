/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUtilities.Find (version 1.3)
 */

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MyClasses
{
    public static partial class MyUtilities
    {
        #region ----- GameObject -----

        /// <summary>
        /// Return an object in root by name.
        /// </summary>
        public static GameObject FindObjectInRoot(string name)
        {
            GameObject[] objs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

            if (objs != null)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].name.Equals(name))
                    {
                        return objs[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Return an object in all layers by name.
        /// </summary>
        public static GameObject FindObjectInAllLayers(GameObject root, string name)
        {
            if (root != null)
            {
                GameObject tmp, tmp2;

                int count = root.transform.childCount;
                for (int i = 0; i < count; i++)
                {
                    tmp = root.transform.GetChild(i).gameObject;
                    if (tmp.name.Equals(name))
                    {
                        return tmp.gameObject;
                    }

                    tmp2 = FindObjectInAllLayers(tmp, name);
                    if (tmp2 != null)
                    {
                        return tmp2;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Return an object in the first layer by name.
        /// </summary>
        public static GameObject FindObjectInFirstLayer(GameObject root, string name)
        {
            if (root != null)
            {
                GameObject tmp;

                int count = root.transform.childCount;
                for (int i = 0; i < count; i++)
                {
                    tmp = root.transform.GetChild(i).gameObject;
                    if (tmp.name.Equals(name))
                    {
                        return tmp;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Return an object by path.
        /// </summary>
        public static GameObject FindObject(GameObject root, string path)
        {
            if (root != null)
            {
                string[] layers = path.Split('/');

                int beginIndex = layers.Length > 0 && layers[0] != string.Empty ? 0 : 1;
                int endIndex = layers.Length - 1;
                for (int i = beginIndex; i < endIndex; i++)
                {
                    string name = layers[i];
                    int count = root.transform.childCount;
                    for (int j = 0; j < count; j++)
                    {
                        GameObject tmp = root.transform.GetChild(j).gameObject;
                        if (tmp.name.Equals(name))
                        {
                            root = tmp;
                            break;
                        }
                    }
                }

                if (endIndex >= 0)
                {
                    return FindObjectInFirstLayer(root, layers[endIndex]);
                }
            }

            return null;
        }

        /// <summary>
        /// Return activate child objects.
        /// </summary>
        public static GameObject[] FindActiveChildObjects(GameObject root)
        {
            List<GameObject> listObj = new List<GameObject>();

            if (root != null)
            {
                foreach (Transform child in root.transform)
                {
                    if (child.gameObject.activeSelf)
                    {
                        listObj.Add(child.gameObject);
                    }
                }
            }

            return listObj.ToArray();
        }

        /// <summary>
        /// Return child objects.
        /// </summary>
        public static T[] FindChildObjects<T>(Transform parent)
        {
            List<T> listObj = new List<T>();

            if (parent != null)
            {
                foreach (Transform child in parent)
                {
                    listObj.Add(child.GetComponent<T>());
                }
            }

            return listObj.ToArray();
        }

        #endregion

        #region ----- Type -----

        /// <summary>
        /// Return an type array by class name.
        /// </summary>
        public static Type[] FindTypesByName(string className)
        {
            List<Type> types = new List<Type>();

            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] assemblyTypes = a.GetTypes();
                for (int j = 0; j < assemblyTypes.Length; j++)
                {
                    if (assemblyTypes[j].Name == className)
                    {
                        types.Add(assemblyTypes[j]);
                    }
                }
            }

            return types.ToArray();
        }

        #endregion
    }
}