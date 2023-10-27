using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomSpinner : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How fast the object should spin, in degrees per second.")]
    private float spinSpeed = 45.0f;

    private Vector3 _rotationAxis;

    private void Start()
    {
        _rotationAxis = Random.insideUnitSphere;
        transform.forward = _rotationAxis;
    }

    private void Update()
    {
        transform.Rotate(_rotationAxis, spinSpeed * Time.deltaTime);
    }
}
