using System;
using UnityEngine;
using UnityEngine.Pool;

namespace AGL.Asteroids
{
    [Serializable]
    public class AsteroidSpawnSettings
    {
        [Tooltip("How quickly the asteroids spawn, in asteroids-per-minute")]
        public float spawnRate;

        [Tooltip("The types of asteroids that can spawn.")]
        public WeightedItem<AsteroidFactory>[] weightedFactories;

        [Tooltip("The potential spawn positions for each asteroid.")]
        public AsteroidLauncher[] launchers;
    }

    // Responsible for the timing of asteroid spawners.
    // Only one of these should ever exist (probably)
    public class AsteroidCoordinator : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("How far from the player can asteroids strike.")]
        private float strikingRange = 5.0f;

        [SerializeField]
        private Transform strikingVisualizer;

        [SerializeField]
        private AsteroidSpawnSettings defaultSettings;

        [SerializeField] private Transform target;

        [SerializeField]
        private AsteroidPath pathPrefab;

        [SerializeField]
        private float difficultyIncreaseRate = 5;

        private int _launcherIndex;
        private float _timeSinceSpawn;
        private GameState _gameState;
        private ObjectPool<AsteroidPath> _pathPool;

        public AsteroidSpawnSettings Settings { get; set; }

        private void OnValidate()
        {
            strikingVisualizer.localScale = new Vector3(strikingRange, strikingRange, strikingRange);
        }

        private void Awake()
        {
            _gameState = FindAnyObjectByType<GameState>();
            Settings = defaultSettings;
        }

        private void Start()
        {
            var pathPoolParent = new GameObject("Path Pool");

            _pathPool = new ObjectPool<AsteroidPath>(() =>
            {
                AsteroidPath instance = Instantiate(pathPrefab, pathPoolParent.transform);
                instance.OnLifetimeEnd += () => _pathPool.Release(instance);
                return instance;
            });
        }

        private void Update()
        {
            Settings.spawnRate += difficultyIncreaseRate * Time.deltaTime;

            if (target != null)
            {
                _timeSinceSpawn += Time.deltaTime;

                if (_timeSinceSpawn > 60.0f / Settings.spawnRate)
                {
                    Asteroid asteroid = SpawnRandomAsteroid();
                    asteroid.IntactView.SetActive(true);
                    _launcherIndex = (_launcherIndex + 1) % Settings.launchers.Length;
                    AsteroidPath asteroidPath = _pathPool.Get();
                    asteroidPath.Spline.Clear();
                    Settings.launchers[_launcherIndex].LaunchAsteroid(asteroidPath, asteroid, target, strikingRange);
                    _timeSinceSpawn = 0;
                }

                strikingVisualizer.position = target.position;
            }
        }

        private Asteroid SpawnRandomAsteroid()
        {
            AsteroidFactory selectedFactory = Settings.weightedFactories.GetRandom();
            return selectedFactory.SpawnAsteroid();
        }
    }
}
