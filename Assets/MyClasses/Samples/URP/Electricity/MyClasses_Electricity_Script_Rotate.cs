using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyClasses_Electricity_Script_Rotate : MonoBehaviour
{
    #region ----- Variable -----

    [SerializeField]
    private float _speed = 5;

    #endregion

    #region ----- MonoBehaviour Implementation -----

    private void Update()
    {
        Vector3 euler = transform.localEulerAngles;
        euler.y += Time.deltaTime * _speed;
        transform.localEulerAngles = euler;
    }

    #endregion
}
