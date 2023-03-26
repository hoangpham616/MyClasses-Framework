/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyCameraFollow2D (version 1.2)
 */

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace MyClasses
{
    [RequireComponent(typeof(Camera))]
    public class MyCameraFollow2D : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        private Transform _target;

        [SerializeField]
        private float _smoothTimeX = 0.05f;
        [SerializeField]
        private float _smoothTimeY = 0.05f;

        [SerializeField]
        private bool _isBound = false;
        [SerializeField]
        private Vector2 _minPosition = new Vector2(-10, -5);
        [SerializeField]
        private Vector2 _maxPosition = new Vector2(10, 5);

        private Vector2 _velocity;

        #endregion

        #region ----- Property -----

        public Vector2 Velocity
        {
            get { return _velocity; }
        }

#if UNITY_EDITOR

        public Transform Target
        {
            get { return _target; }
            set { _target = value; }
        }

        public float SmoothTimeX
        {
            get { return _smoothTimeX; }
            set { _smoothTimeX = value; }
        }

        public float SmoothTimeY
        {
            get { return _smoothTimeY; }
            set { _smoothTimeY = value; }
        }

        public bool IsBound
        {
            get { return _isBound; }
            set { _isBound = value; }
        }

        public Vector2 MinPosition
        {
            get { return _minPosition; }
            set { _minPosition = value; }
        }

        public Vector2 MaxPosition
        {
            get { return _maxPosition; }
            set { _maxPosition = value; }
        }

#endif

        #endregion

        #region ----- MonoBehaviour Implementation -----

        /// <summary>
        /// LateUpdate.
        /// </summary>
        void LateUpdate()
        {
            if (_target != null)
            {
                Vector3 newPos = transform.position;

                newPos.x = Mathf.SmoothDamp(transform.position.x, _target.transform.position.x, ref _velocity.x, _smoothTimeX);
                newPos.y = Mathf.SmoothDamp(transform.position.y, _target.transform.position.y, ref _velocity.y, _smoothTimeY);

                if (_isBound)
                {
                    newPos.x = Mathf.Clamp(newPos.x, _minPosition.x, _maxPosition.x);
                    newPos.y = Mathf.Clamp(newPos.y, _minPosition.y, _maxPosition.y);
                }

                transform.position = newPos;
            }
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyCameraFollow2D))]
    public class MyCameraFollow2DEditor : Editor
    {
        private MyCameraFollow2D _script;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            _script = (MyCameraFollow2D)target;
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(_script), typeof(MyCameraFollow2D), false);

            _script.Target = (Transform)EditorGUILayout.ObjectField("Target", _script.Target, typeof(Transform), true);

            _script.SmoothTimeX = EditorGUILayout.FloatField("Smooth Time X", _script.SmoothTimeX);
            _script.SmoothTimeY = EditorGUILayout.FloatField("Smooth Time Y", _script.SmoothTimeY);

            _script.IsBound = EditorGUILayout.Toggle("Is Bound", _script.IsBound);
            if (_script.IsBound)
            {
                EditorGUILayout.BeginHorizontal();
                _script.MinPosition = EditorGUILayout.Vector2Field("Min Position", _script.MinPosition);
                if (GUILayout.Button("Set Min Camera Position"))
                {
                    _script.MinPosition = new Vector2(_script.transform.position.x, _script.transform.position.y);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                _script.MaxPosition = EditorGUILayout.Vector2Field("Max Position", _script.MaxPosition);
                if (GUILayout.Button("Set Max Camera Position"))
                {
                    _script.MaxPosition = new Vector2(_script.transform.position.x, _script.transform.position.y);
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Velocity: (" + _script.Velocity.x + ", " + _script.Velocity.y + ")");
        }
    }

#endif
}