using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class AsteroidSpawner : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("How quickly the asteroids spawn, in asteroids-per-minute")]
        private float spawnRate = 10.0f;

        [SerializeField]
        private Asteroid asteroidPrefab;

        [FormerlySerializedAs("moverPrefab")] [SerializeField]
        private AsteroidPath pathPrefab;

        [SerializeField]
        private Transform[] targetPositions;

        [SerializeField]
        [Tooltip("How far from the player can asteroids strike.")]
        private float strikingRange = 5.0f;

        [SerializeField]
        [Tooltip("How long it takes an asteroid to strike the surface")]
        private float impactTime = 5;

        [SerializeField]
        private Transform strikingVisualizer;

        private GameState _gameState;
        private float _timeSinceSpawn;
        private ObjectPool<AsteroidPath> _pathPool;
        private ObjectPool<Asteroid> _asteroidPool;

        private void OnValidate()
        {
            strikingVisualizer.localScale = new Vector3(strikingRange, strikingRange, strikingRange);
        }

        private void Start()
        {
            _gameState = FindAnyObjectByType<GameState>();

            var pathPoolParent = new GameObject("Path Pool");
            _pathPool = new ObjectPool<AsteroidPath>(() =>
            {
                AsteroidPath instance = Instantiate(pathPrefab, pathPoolParent.transform);
                instance.OnLifetimeEnd += () => _pathPool.Release(instance);
                return instance;
            });

            var asteroidPoolParent = new GameObject("Asteroid Pool");
            _asteroidPool = new ObjectPool<Asteroid>(() =>
            {
                Asteroid instance = Instantiate(asteroidPrefab, asteroidPoolParent.transform);
                instance.OnLifetimeEnd += () => _asteroidPool.Release(instance);
                return instance;
            });
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

            // Get a random target (the points around the planet)
            int randomIndex = Random.Range(0, targetPositions.Length);
            Transform randomTarget = targetPositions[randomIndex];

            // Create the mover that drives asteroid paths
            AsteroidPath asteroidPath = _pathPool.Get();
            asteroidPath.Spline.Clear();
            Spline spline = asteroidPath.Spline;
            spline.Add(new BezierKnot(transform.position), TangentMode.AutoSmooth, 1);
            spline.Add(new BezierKnot(randomTarget.position), TangentMode.AutoSmooth, 1);
            asteroidPath.Initialize(playerTransform, strikingRange, impactTime + 1);

            // Create the asteroid itself, and assign it to follow the path
            Asteroid asteroidInstance = _asteroidPool.Get();
            asteroidInstance.IntactView.SetActive(true);
            asteroidInstance.SplineAnimate.Container = asteroidPath.Container;
            asteroidInstance.SplineAnimate.Duration = impactTime;
            asteroidInstance.SplineAnimate.Restart(true);
        }
    }
}
