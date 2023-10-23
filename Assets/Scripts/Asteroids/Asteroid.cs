using System;
using UnityEngine;
using UnityEngine.Splines;

namespace DefaultNamespace
{
    // Some object that can fall from the sky and crash into the ground.
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

            // We don't want to crash into other asteroids (or asteroid debris)
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
