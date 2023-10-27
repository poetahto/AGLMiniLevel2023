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
        private AsteroidSpawnSettings defaultSettings;

        [SerializeField]
        private AsteroidPath pathPrefab;

        [SerializeField]
        private float difficultyIncreaseRate = 5;

        private int _launcherIndex;
        private float _timeSinceSpawn;
        private GameState _gameState;
        private ObjectPool<AsteroidPath> _pathPool;

        public AsteroidSpawnSettings Settings { get; set; }

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

            if (_gameState.TryGetPlayer(out GameObject playerInstance))
            {
                _timeSinceSpawn += Time.deltaTime;

                if (_timeSinceSpawn > 60.0f / Settings.spawnRate)
                {
                    Asteroid asteroid = SpawnRandomAsteroid();
                    asteroid.IntactView.SetActive(true);
                    _launcherIndex = (_launcherIndex + 1) % Settings.launchers.Length;
                    AsteroidPath asteroidPath = _pathPool.Get();
                    asteroidPath.Spline.Clear();
                    Settings.launchers[_launcherIndex].LaunchAsteroid(asteroidPath, asteroid, playerInstance.transform);
                    _timeSinceSpawn = 0;
                }
            }
        }

        private Asteroid SpawnRandomAsteroid()
        {
            AsteroidFactory selectedFactory = Settings.weightedFactories.GetRandom();
            return selectedFactory.SpawnAsteroid();
        }
    }
}
