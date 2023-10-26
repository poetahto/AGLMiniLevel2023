using System;
using UnityEngine;

namespace AGL.Asteroids
{
    // Plays an animated effect where an asteroid breaks into small pieces.
    // The pieces grow smaller over time, until disappearing completely.
    public class AsteroidDebris : MonoBehaviour
    {
        [SerializeField]
        private float lifetime = 10.0f;

        public float _remainingTime;
        private Vector3[] _originalMeshScales;
        private Vector3[] _originalPhysicsPositions;
        private Quaternion[] _originalPhysicsRotations;
        private MeshRenderer[] _meshRenderers;
        private Rigidbody[] _rigidbodies;
        private bool _isActive;

        public event Action OnLifetimeEnd;

        private void Awake()
        {
            _meshRenderers = GetComponentsInChildren<MeshRenderer>(true);
            _originalMeshScales = new Vector3[_meshRenderers.Length];

            _rigidbodies = GetComponentsInChildren<Rigidbody>(true);
            _originalPhysicsPositions = new Vector3[_rigidbodies.Length];
            _originalPhysicsRotations = new Quaternion[_rigidbodies.Length];

            for (int i = 0; i < _rigidbodies.Length; i++)
            {
                _originalPhysicsPositions[i] = _rigidbodies[i].transform.localPosition;
                _originalPhysicsRotations[i] = _rigidbodies[i].transform.localRotation;
            }

            for (int i = 0; i < _meshRenderers.Length; i++)
                _originalMeshScales[i] = _meshRenderers[i].transform.localScale;
        }

        public void StartPlaying(Vector3 velocity)
        {
            gameObject.SetActive(true);
            _remainingTime = lifetime;
            _isActive = true;

            for (int i = 0; i < _rigidbodies.Length; i++)
            {
                Rigidbody rb = _rigidbodies[i];
                rb.angularVelocity = Vector3.zero;
                rb.velocity = velocity;
                rb.transform.SetLocalPositionAndRotation(_originalPhysicsPositions[i], _originalPhysicsRotations[i]);
            }

            for (int i = 0; i < _meshRenderers.Length; i++)
                _meshRenderers[i].transform.localScale = _originalMeshScales[i];
        }

        private void Update()
        {
            if (_isActive)
            {
                _remainingTime -= Time.deltaTime;
                float t = (lifetime - _remainingTime) / lifetime;

                for (int i = 0; i < _meshRenderers.Length; i++)
                    _meshRenderers[i].transform.localScale = Vector3.Lerp(_originalMeshScales[i], Vector3.zero, t);

                if (_remainingTime <= 0)
                {
                    gameObject.SetActive(false);
                    _isActive = false;
                    _remainingTime = 0;
                    OnLifetimeEnd?.Invoke();
                }
            }
        }
    }
}
