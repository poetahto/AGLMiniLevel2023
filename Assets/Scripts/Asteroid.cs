using System;
using UnityEngine;
using UnityEngine.Splines;

namespace DefaultNamespace
{
    public class Asteroid : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem particleEffect;

        [SerializeField]
        private GameObject intactView;

        [SerializeField]
        private AsteroidDebris debrisView;

        [SerializeField]
        private SplineAnimate splineAnimate;

        private Vector3 _previousPosition;

        public SplineAnimate SplineAnimate => splineAnimate;
        public GameObject IntactView => intactView;
        public AsteroidDebris DebrisView => debrisView;

        public event Action OnLifetimeEnd;

        private void Start()
        {
            debrisView.OnLifetimeEnd += () =>
            {
                OnLifetimeEnd?.Invoke();
            };
        }

        private void Update()
        {
            _previousPosition = transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            Vector3 velocity = (transform.position - _previousPosition) / Time.deltaTime;

            if (!other.CompareTag("Asteroid"))
            {
                particleEffect.Play();
                splineAnimate.Pause();
                intactView.SetActive(false);
                debrisView.Initialize(velocity);
            }
        }
    }
}
