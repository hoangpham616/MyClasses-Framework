/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyShaderLiquidWobble (version 1.0)
 * Requirement: MyShaderGraphLiquid
 * Reference:   https://www.patreon.com/posts/shader-graph-52529253
 */

using UnityEngine;

namespace MyClasses
{
    [RequireComponent(typeof(Renderer))]
    public class MyShaderLiquidWobble : MonoBehaviour
    {
        #region ----- Define -----

        private const string _WOBBLE_X = "_Wobble_X";
        private const string _WOBBLE_Z = "_Wobble_Z";

        #endregion

        #region ----- Variable -----

        [SerializeField]
        [Range(0f, 1f)]
        private float _range = 0.03f;
        [SerializeField]
        [Range(0f, 10f)]
        private float _speed = 1f;
        [SerializeField]
        [Range(0f, 100f)]
        private float _recovery = 1f;

        private Renderer _renderer;
        private Vector3 _velocity;
        private Vector3 _angularVelocity;
        private Vector3 _lastPosition;
        private Vector3 _lastEulerAngle;
        private float _time;
        private float _pulse;
        private float _x;
        private float _z;
        private float _extraX;
        private float _extraZ;

        #endregion

        #region ----- MonoBehaviour Implementation -----
    
        /// <summary>
        /// Start.
        /// </summary>
        void Start()
        {
            _renderer = GetComponent<Renderer>();
        }
        
        /// <summary>
        /// Update.
        /// </summary>
        void Update()
        {
            // decrease wobble over time
            _extraX = Mathf.Lerp(_extraX, 0, Time.deltaTime * _recovery);
            _extraZ = Mathf.Lerp(_extraZ, 0, Time.deltaTime * _recovery);

            // make a sine wave of the decreasing wobble
            _time += Time.deltaTime;
            _pulse = 2 * Mathf.PI * _speed;
            _x = _extraX * Mathf.Sin(_pulse * _time);
            _z = _extraZ * Mathf.Sin(_pulse * _time);

            // send it to the shader
            _renderer.material.SetFloat(_WOBBLE_X, _x);
            _renderer.material.SetFloat(_WOBBLE_Z, _z);

            // velocity
            _velocity = (_lastPosition - transform.position) / Time.deltaTime;
            _angularVelocity = transform.rotation.eulerAngles - _lastEulerAngle;

            // add clamped velocity to wobble
            _extraX += Mathf.Clamp((_velocity.x + (_angularVelocity.z * 0.2f)) * _range, -_range, _range);
            _extraZ += Mathf.Clamp((_velocity.z + (_angularVelocity.x * 0.2f)) * _range, -_range, _range);

            // keep last position
            _lastPosition = transform.position;
            _lastEulerAngle = transform.rotation.eulerAngles;
        }

        #endregion
    }
}