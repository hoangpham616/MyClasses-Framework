/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyCameraRotateAround (version 1.1)
 */

using UnityEngine;

namespace MyClasses
{
    public class MyCameraRotateAround : MonoBehaviour
    {
        #region ----- Variable -----

        [SerializeField]
        private Transform _camera;
        [SerializeField]
        private Transform _target;

        [SerializeField]
        private Vector2 _limitXAxis = new Vector2(int.MinValue, int.MaxValue);
        [SerializeField]
        private Vector2 _limitYAxis = new Vector2(int.MinValue, int.MaxValue);

        [SerializeField]
        private Vector2 _cameraOriginalEulerAngles;
        [SerializeField]
        private Vector2 _cameraOriginalOffset;

        #endregion

        #region ----- Property -----

        public Transform Camera
        {
            get { return _camera; }
        }

        public Transform Target
        {
            get { return _target; }
        }

        public Vector2 LimitXAxis
        {
            get { return _limitXAxis; }
        }

        public Vector2 LimitYAxis
        {
            get { return _limitYAxis; }
        }

        public Vector2 CameraOriginalEulerAngles
        {
            get { return _cameraOriginalEulerAngles; }
        }

        public Vector2 CameraOriginalOffset
        {
            get { return _cameraOriginalOffset; }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Setup camera & target.
        /// </summary>
        public void Setup(Transform camera, Transform target, bool isKeepCameraOffset = true)
        {
            _camera = camera;
            _target = target;
            _cameraOriginalEulerAngles = _camera.eulerAngles;
            _cameraOriginalOffset = isKeepCameraOffset ? _camera.position - _target.position : Vector3.zero;
        }

        /// <summary>
        /// Set limit of vertical axis.
        /// </summary>
        public void SetVerticalAxisLimit(float min, float max)
        {
            _limitXAxis.x = min;
            _limitXAxis.y = max;
        }

        /// <summary>
        /// Set limit of horizontal axis.
        /// </summary>
        public void SetHorizontalAxisLimit(float min, float max)
        {
            _limitYAxis.x = min;
            _limitYAxis.y = max;
        }

        /// <summary>
        /// Rotate the camera around the target.
        /// You should call this in Update method.
        /// </summary>
        public void Rotate(float deltaX, float deltaY)
        {
            Vector3 cameraPosition = _camera.position;
            cameraPosition.x -= _cameraOriginalOffset.x;
            cameraPosition.y -= _cameraOriginalOffset.y;
            float distance = Vector3.Magnitude(cameraPosition - _target.position);

            _cameraOriginalEulerAngles.x += deltaY;
            _cameraOriginalEulerAngles.x = _ClampAngle(_cameraOriginalEulerAngles.x, _limitXAxis.x, _limitXAxis.y);

            _cameraOriginalEulerAngles.y += deltaX;
            _cameraOriginalEulerAngles.y = _ClampAngle(_cameraOriginalEulerAngles.y, _limitYAxis.x, _limitYAxis.y);

            Quaternion rotation = Quaternion.Euler(_cameraOriginalEulerAngles.x, _cameraOriginalEulerAngles.y, 0);
            Vector3 negativeDistance = Vector3.zero;
            negativeDistance.z = -distance;

            Vector3 position = rotation * negativeDistance;
            position.x += _cameraOriginalOffset.x;
            position.y += _cameraOriginalOffset.y;

            _camera.rotation = rotation;
            _camera.position = position;
        }

        #endregion

        private float _ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            else if (angle > 360F)
            {
                angle -= 360F;
            }
            return Mathf.Clamp(angle, min, max);
        }
    }
}