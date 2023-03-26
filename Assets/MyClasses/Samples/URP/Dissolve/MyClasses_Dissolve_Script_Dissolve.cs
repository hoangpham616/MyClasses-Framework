using UnityEngine;

public class MyClasses_Dissolve_Script_Dissolve : MonoBehaviour
{
    #region ----- Variable -----

    [SerializeField]
    private const string _ALPHA_CLIP_THRESHOLD = "_Alpha_Clip_Threshold";

    #endregion

    #region ----- Variable -----

    [SerializeField]
    private float _duration = 1;

    private Material _material;
    private float _time;

    #endregion

    #region ----- MonoBehaviour Implementation -----

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        _material = renderer.material;
    }

    private void Update()
    {
        _time = (_time + Time.deltaTime ) % _duration;
        _material.SetFloat(_ALPHA_CLIP_THRESHOLD, _time / _duration);
    }

    #endregion
}
