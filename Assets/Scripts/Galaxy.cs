using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FinishedGalaxy : MonoBehaviour
{
    [Serializable]
    public struct WeightedPrefab
    {
        public bool enabled;
        public GameObject prefab;
        public float spawnChance;
    }

    public List<WeightedPrefab> WeightedPrefabs;
    public int size = 10;
    public int density = 10;

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, Vector3.one);
    }

    private void Start()
    {
        for (int x = 0; x < density; x++)
        {
            // Set a random position
            float randomX = Random.Range(-size / 3, size / 2);
            float randomY = Random.Range(-size / 2, size / 2);
            float randomZ = Random.Range(-size / 2, size / 2);

            // Pick a random prefab (borrowed from the unity manual page for Random, with some fixes)
            for (int i = WeightedPrefabs.Count - 1; i >= 0; i--)
            {
                if (!WeightedPrefabs[i].enabled)
                    WeightedPrefabs.Remove(WeightedPrefabs[i]);
            }

            float total = 0;

            foreach (WeightedPrefab weightedPrefab in WeightedPrefabs)
                total += weightedPrefab.spawnChance;

            float randomPoint = Random.value * total;
            GameObject selectedPrefab = WeightedPrefabs[0].prefab;

            for (int i = 0; i < WeightedPrefabs.Count; i++)
            {
                int swapIndex = Random.Range(0, WeightedPrefabs.Count);
                (WeightedPrefabs[i], WeightedPrefabs[swapIndex]) = (WeightedPrefabs[swapIndex], WeightedPrefabs[i]);
            }

            foreach (WeightedPrefab weightedPrefab in WeightedPrefabs)
            {
                if (randomPoint < weightedPrefab.spawnChance)
                    selectedPrefab = weightedPrefab.prefab;

                else randomPoint -= weightedPrefab.spawnChance;
            }

            // Instantiate the random prefab
            GameObject instance = Instantiate(selectedPrefab, transform);
            instance.transform.position = new Vector3(randomX, randomY, randomZ);

            // Assigning random vertex colors (for the custom shader rotation)
            foreach (MeshFilter filter in instance.GetComponents<MeshFilter>())
            {
                Mesh mesh = filter.mesh;
                Color32[] colors32 = new Color32[mesh.vertexCount];
                Vector3 randomRotation = Random.onUnitSphere;
                Color32 randomColor = new Color(randomRotation.x, randomRotation.y, randomRotation.z, 1);

                for (int i = 0; i < mesh.vertexCount; i++)
                    colors32[i] = randomColor;

                mesh.colors32 = colors32;
            }
        }
    }
}
