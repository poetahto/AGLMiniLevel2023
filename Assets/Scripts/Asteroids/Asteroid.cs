using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;

namespace DefaultNamespace
{
    // Some object that can fall from the sky and crash into the ground.
    public class Asteroid : MonoBehaviour
    {
        [SerializeField]
        private WeightedItem<GameObject>[] itemDropTable;

        [SerializeField]
        private Vector3 itemSpawnPosition;

        [SerializeField]
        private AudioSource crashAudio;

        // todo: impl
        [SerializeField]
        private float damage = 1;

        [SerializeField]
        private ParticleSystem particleEffect;

        [SerializeField]
        private GameObject intactView;

        [SerializeField]
        private AsteroidDebris debrisView;

        [SerializeField]
        private SplineAnimate splineAnimate;

        private Vector3 _previousPosition;
        private ItemDropManager _itemDropManager;

        public SplineAnimate SplineAnimate => splineAnimate;
        public GameObject IntactView => intactView;
        public AsteroidDebris DebrisView => debrisView;

        public event Action OnLifetimeEnd;

        private void HandleDebrisEnd()
        {
            // todo: I'll occasionally get errors in the pooling that propagate through here - I think something is releasing twice
            OnLifetimeEnd?.Invoke();
        }

        private void OnEnable()
        {
            debrisView.OnLifetimeEnd += HandleDebrisEnd;
        }

        private void OnDisable()
        {
            debrisView.OnLifetimeEnd -= HandleDebrisEnd;
        }

        private void Start()
        {
            _itemDropManager = FindAnyObjectByType<ItemDropManager>();
        }

        private void Update()
        {
            _previousPosition = transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            // We keep track of the velocity so the debris can be launched smoothly.
            Vector3 velocity = (transform.position - _previousPosition) / Time.deltaTime;

            // We don't want to crash into other asteroids (or asteroid debris)
            if (!other.CompareTag("Asteroid"))
            {
                // At some point, we might want to move this into a factory for possible pooling.
                if (itemDropTable.TryGetRandom(out GameObject randomItemPrefab) && _itemDropManager.ShouldDrop(randomItemPrefab))
                    Instantiate(randomItemPrefab, transform.position + itemSpawnPosition, Quaternion.identity);

                print("explode");
                particleEffect.Play();
                splineAnimate.Pause();
                intactView.SetActive(false);
                debrisView.StartPlaying(velocity);
                crashAudio.Play();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(transform.position + itemSpawnPosition, 1);
        }
    }
}
