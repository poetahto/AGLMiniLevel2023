using System;
using UnityEngine;

namespace DefaultNamespace
{
    // Plays an animated effect where an asteroid breaks into small pieces.
    // The pieces grow smaller over time, until disappearing completely.
    public class AsteroidDebris : MonoBehaviour
    {
        [SerializeField]
        private float lifetime = 10.0f;

        private float _remainingTime;
        private Vector3[] _originalScales;
        private Vector3[] _originalPositions;
        private bool _isActive;

        public event Action OnLifetimeEnd;

        private void Awake()
        {
            Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
            _originalPositions = new Vector3[rigidbodies.Length];
            _originalScales = new Vector3[rigidbodies.Length];

            for (int i = 0; i < rigidbodies.Length; i++)
            {
                _originalPositions[i] = rigidbodies[i].transform.localPosition;
                _originalScales[i] = rigidbodies[i].transform.localScale;
            }
        }

        public void Initialize(Vector3 velocity)
        {
            gameObject.SetActive(true);
            _remainingTime = lifetime;
            _isActive = true;

            Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

            for (int i = 0; i < rigidbodies.Length; i++)
            {
                Rigidbody rb = rigidbodies[i];
                rb.angularVelocity = Vector3.zero;
                rb.velocity = velocity;
                rb.transform.localScale = _originalScales[i];
                rb.transform.localPosition = _originalPositions[i];
            }
        }

        private void Update()
        {
            if (_isActive)
            {
                _remainingTime -= Time.deltaTime;
                Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

                for (int i = 0; i < rigidbodies.Length; i++)
                    rigidbodies[i].transform.localScale = Vector3.Lerp(_originalScales[i], Vector3.zero, (lifetime - _remainingTime) / lifetime);

                if (_remainingTime <= 0)
                {
                    OnLifetimeEnd?.Invoke();
                    gameObject.SetActive(false);
                    _isActive = false;
                    _remainingTime = 0;
                }
            }
        }
    }
}
