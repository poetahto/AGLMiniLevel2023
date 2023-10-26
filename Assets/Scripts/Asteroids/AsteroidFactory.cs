using UnityEngine;
using UnityEngine.Pool;

namespace DefaultNamespace
{
    // Responsible for creating new asteroids. Probably is pooling behind the scenes.
    // One of these exists for each asteroid type.
    public class AsteroidFactory : MonoBehaviour
    {
        [SerializeField]
        private Asteroid asteroidPrefab;

        private ObjectPool<Asteroid> _asteroidPool;

        private void Start()
        {
            var asteroidPoolParent = new GameObject("Asteroid Pool");
            _asteroidPool = new ObjectPool<Asteroid>(() =>
            {
                Asteroid instance = Instantiate(asteroidPrefab, asteroidPoolParent.transform);
                instance.OnLifetimeEnd += () => _asteroidPool.Release(instance);
                return instance;
            });
        }

        public Asteroid SpawnAsteroid()
        {
            return _asteroidPool.Get();
        }
    }
}
