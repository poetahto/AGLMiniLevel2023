using UnityEngine;
using UnityEngine.Pool;

namespace DefaultNamespace
{
    // Responsible for the timing of asteroid spawners.
    // Only one of these should ever exist (probably)
    public class AsteroidCoordinator : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("How quickly the asteroids spawn, in asteroids-per-minute")]
        private float spawnRate = 10.0f;

        [SerializeField]
        private WeightedItem<AsteroidFactory>[] weightedFactories;

        [SerializeField]
        private AsteroidLauncher[] launchers;

        [SerializeField]
        private AsteroidPath pathPrefab;

        private int _launcherIndex;
        private float _timeSinceSpawn;
        private GameState _gameState;
        private ObjectPool<AsteroidPath> _pathPool;

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
        }

        private void Update()
        {
            if (_gameState.TryGetPlayer(out GameObject playerInstance))
            {
                _timeSinceSpawn += Time.deltaTime;

                if (_timeSinceSpawn > 60.0f / spawnRate)
                {
                    Asteroid asteroid = SpawnRandomAsteroid();
                    asteroid.IntactView.SetActive(true);
                    _launcherIndex = (_launcherIndex + 1) % launchers.Length;
                    AsteroidPath asteroidPath = _pathPool.Get();
                    asteroidPath.Spline.Clear();
                    launchers[_launcherIndex].LaunchAsteroid(asteroidPath, asteroid, playerInstance.transform);
                    _timeSinceSpawn = 0;
                }
            }
        }

        private Asteroid SpawnRandomAsteroid()
        {
            AsteroidFactory selectedFactory = weightedFactories.GetRandom();
            return selectedFactory.SpawnAsteroid();
        }
    }
}
