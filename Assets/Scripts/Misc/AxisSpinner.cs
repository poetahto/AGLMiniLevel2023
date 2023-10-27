using UnityEngine;

public class AxisSpinner : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How fast the object should spin, in degrees per second.")]
    private float spinSpeed = 45.0f;

    [SerializeField]
    private Vector3 axis = Vector3.up;


    private void Update()
    {
        transform.Rotate(axis, spinSpeed * Time.deltaTime);
    }
}
