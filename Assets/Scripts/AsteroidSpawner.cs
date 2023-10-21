using UnityEngine;
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
        private GameObject asteroidPrefab;

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

        private void OnValidate()
        {
            strikingVisualizer.localScale = new Vector3(strikingRange, strikingRange, strikingRange);
        }

        private void Start()
        {
            _gameState = FindAnyObjectByType<GameState>();
            strikingVisualizer.localScale = new Vector3(strikingRange, strikingRange, strikingRange);
        }

        private void OnGUI()
        {
            if (_gameState.TryGetPlayer(out GameObject playerInstance))
            {
                if (GUILayout.Button("Spawn Asteroid"))
                    SpawnAsteroid(playerInstance.transform);
            }
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

            Transform randomTarget = GetRandomTargetTransform();
            GameObject asteroidMover = new GameObject("Asteroid Movement Spline");
            var splineContainer = asteroidMover.AddComponent<SplineContainer>();
            Spline spline = splineContainer.Spline;
            spline.Add(new BezierKnot(transform.position), TangentMode.AutoSmooth, 1);
            spline.Add(new BezierKnot(randomTarget.position), TangentMode.AutoSmooth, 1);
            AddLandingKnotsAtPlayer(spline, playerTransform);

            GameObject asteroidInstance = Instantiate(asteroidPrefab, asteroidMover.transform);
            var splineAnimate = asteroidInstance.AddComponent<SplineAnimate>();
            splineAnimate.Container = splineContainer;
            splineAnimate.Duration = impactTime;
            splineAnimate.Easing = SplineAnimate.EasingMode.EaseIn;
            splineAnimate.Play();
        }

        private Transform GetRandomTargetTransform()
        {
            int randomIndex = Random.Range(0, targetPositions.Length);
            return targetPositions[randomIndex];
        }

        private void AddLandingKnotsAtPlayer(Spline spline, Transform playerTransform)
        {
            RaycastHit spawnPos = PhysicsUtil.GetRandomPointOnGroundNear(
                -playerTransform.up,
                playerTransform.position,
                strikingRange / 2
            );

            float angleVariance = 80f;
            float randomXAngle = Random.Range(-angleVariance, angleVariance);
            float randomYAngle = Random.Range(-angleVariance, angleVariance);
            Vector3 randomNormal = Quaternion.Euler(randomXAngle, 0, randomYAngle) * spawnPos.normal;
            Debug.DrawRay(spawnPos.point, spawnPos.normal, Color.red, 10);
            Debug.DrawRay(spawnPos.point, randomNormal, Color.green, 10);

            Vector3 playerPos = spawnPos.point;
            Vector3 strikeVector = randomNormal * 20;
            Vector3 strikePoint = playerPos + strikeVector;
            Vector3 vectorToPlayer = playerPos - strikePoint;
            spline.Add(new BezierKnot(strikePoint), TangentMode.AutoSmooth, 1);
            spline.Add(new BezierKnot(playerPos, -vectorToPlayer, vectorToPlayer));

            // spline.Add(new BezierKnot(spawnPos.point + Vector3.up * 20));
            // spline.Add(new BezierKnot(spawnPos.point, spawnPos.normal, -spawnPos.normal));
        }
    }
}
