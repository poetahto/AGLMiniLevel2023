using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    [Serializable]
    public class WeightedItem<T>
    {
        public T item;
        public float probability;
    }

    public static class WeightedItemUtil
    {
        public static T GetRandom<T>(this IList<WeightedItem<T>> list)
        {
            float total = 0;

            foreach (WeightedItem<T> weightedFactory in list)
                total += weightedFactory.probability;

            float randomPoint = Random.value * total;
            T result = list[0].item;

            for (int i = 0; i < list.Count; i++)
            {
                int swapIndex = Random.Range(0, list.Count);
                (list[i], list[swapIndex]) = (list[swapIndex], list[i]);
            }

            foreach (WeightedItem<T> weightedPrefab in list)
            {
                if (randomPoint < weightedPrefab.probability)
                    result = weightedPrefab.item;

                else randomPoint -= weightedPrefab.probability;
            }

            return result;
        }
    }
}
