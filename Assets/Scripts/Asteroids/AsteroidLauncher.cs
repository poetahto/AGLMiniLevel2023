using UnityEngine;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

namespace AGL.Asteroids
{
    // Responsible for launching random asteroids when requested.
    // Needs one for each planet / place things should spawn.
    public class AsteroidLauncher : MonoBehaviour
    {
        [SerializeField]
        private Transform[] targetPositions;

        // todo: this goes w/ asteroid, not launcher
        [SerializeField]
        [Tooltip("How long it takes an asteroid to strike the surface")]
        private float impactTime = 5;

        private GameState _gameState;

        public void LaunchAsteroid(AsteroidPath path, Asteroid asteroid, Transform target, float range)
        {
            // Get a random target (the points around the planet)
            int randomIndex = Random.Range(0, targetPositions.Length);
            Transform randomTarget = targetPositions[randomIndex];

            // Create the mover that drives asteroid paths
            Spline spline = path.Spline;
            spline.Add(new BezierKnot(transform.position), TangentMode.AutoSmooth, 1);
            spline.Add(new BezierKnot(randomTarget.position), TangentMode.AutoSmooth, 1);
            path.Initialize(target, range, impactTime + 1);

            // Create the asteroid itself, and assign it to follow the path
            asteroid.SplineAnimate.Container = path.Container;
            asteroid.SplineAnimate.Duration = impactTime;
            asteroid.SplineAnimate.Restart(true);
        }

        private void Start()
        {
            _gameState = FindAnyObjectByType<GameState>();
        }
    }
}
