using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class AsteroidExplodeOnImpact : MonoBehaviour
    {
        [SerializeField]
        private GameObject particleEffect;

        [SerializeField]
        private GameObject brokenMesh;

        [SerializeField]
        private GameObject objectToDestroy;

        private Vector3 _previousPosition;

        private void Update()
        {
            _previousPosition = transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            Vector3 velocity = (transform.position - _previousPosition) / Time.deltaTime;

            if (!other.CompareTag("Asteroid"))
            {
                Instantiate(particleEffect, transform.position, Quaternion.LookRotation(-transform.forward));
                var brokenMeshInstance = Instantiate(brokenMesh, transform.position, transform.rotation);

                foreach (Rigidbody brokenRigidbody in brokenMeshInstance.GetComponentsInChildren<Rigidbody>())
                    brokenRigidbody.velocity = velocity;

                Destroy(objectToDestroy);
            }
        }
    }
}
