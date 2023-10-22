using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace DefaultNamespace
{
    public class PoolRegistry : MonoBehaviour
    {
        private Dictionary<GameObject, ObjectPool<GameObject>> _poolLookup;

        private void Awake()
        {
            _poolLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();
        }

        public ObjectPool<GameObject> Get(GameObject prefab)
        {
            if (!_poolLookup.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                var poolParent = new GameObject($"{prefab.name} Pool");
                pool = new ObjectPool<GameObject>(() => Instantiate(prefab, poolParent.transform));
                _poolLookup.Add(prefab, pool);
            }

            return pool;
        }
    }
}
