using UnityEngine;

namespace DefaultNamespace
{
    public class AsteroidSpawner : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("How quickly the asteroids spawn, in asteroids-per-minute")]
        private float spawnRate = 10.0f;

        [SerializeField]
        private GameObject asteroidPrefab;

        [SerializeField]
        private Transform spawnPosition;

        [SerializeField]
        [Tooltip("How far from the player can asteroids strike.")]
        private float strikingRange = 5.0f;

        [SerializeField]
        private Transform strikingVisualizer;

        private GameState _gameState;
        private float _timeSinceSpawn;

        private void OnValidate()
        {
            strikingVisualizer.localScale = new Vector3(strikingRange, strikingRange, strikingRange);
        }

        private void Start()
        {
            _gameState = FindAnyObjectByType<GameState>();
            strikingVisualizer.localScale = new Vector3(strikingRange, strikingRange, strikingRange);
        }

        private void Update()
        {
            bool playerExists = _gameState.TryGetPlayer(out GameObject playerInstance);

            if (playerExists)
            {
                // Update the visualizer transform
                strikingVisualizer.transform.position = playerInstance.transform.position;
                strikingVisualizer.forward = playerInstance.transform.up * -1;

                _timeSinceSpawn += Time.deltaTime;

                if (_timeSinceSpawn > 60.0f / spawnRate)
                {
                    // spawn an asteroid
                    SpawnAsteroid(playerInstance.transform);
                    _timeSinceSpawn = 0;
                }
            }

            strikingVisualizer.gameObject.SetActive(playerExists);
        }

        private void SpawnAsteroid(Transform playerTransform)
        {
            Debug.Log("Spawned an asteroid.");

            RaycastHit spawnPos = PhysicsUtil.GetRandomPointOnGroundNear(
                -playerTransform.up,
                playerTransform.position,
                strikingRange
            );

            Instantiate(asteroidPrefab, spawnPos.point, Quaternion.identity, transform);
        }
    }
}
