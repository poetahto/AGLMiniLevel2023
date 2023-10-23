using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    // Responsible for launching random asteroids when requested.
    // Needs one for each planet / place things should spawn.
    public class AsteroidLauncher : MonoBehaviour
    {
        [SerializeField]
        private AsteroidPath pathPrefab;

        [SerializeField]
        private Transform[] targetPositions;

        [SerializeField]
        [Tooltip("How long it takes an asteroid to strike the surface")]
        private float impactTime = 5;

        [SerializeField]
        [Tooltip("How far from the player can asteroids strike.")]
        private float strikingRange = 5.0f;

        [SerializeField]
        private Transform strikingVisualizer;

        private ObjectPool<AsteroidPath> _pathPool;
        private GameState _gameState;

        public void LaunchAsteroid(Asteroid asteroid, Transform target)
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
            asteroidPath.Initialize(target, strikingRange, impactTime + 1);

            // Create the asteroid itself, and assign it to follow the path
            asteroid.IntactView.SetActive(true);
            asteroid.SplineAnimate.Container = asteroidPath.Container;
            asteroid.SplineAnimate.Duration = impactTime;
            asteroid.SplineAnimate.Restart(true);
        }

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
        }

        private void Update()
        {
            bool playerExists = _gameState.TryGetPlayer(out GameObject playerInstance);

            if (playerExists)
            {
                // Update the target visualizer transform
                strikingVisualizer.transform.position = playerInstance.transform.position;
                strikingVisualizer.forward = playerInstance.transform.up * -1;
            }

            strikingVisualizer.gameObject.SetActive(playerExists);
        }
    }
}
