using System;
using System.Collections;
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
        public static float GetProbabilitySum<T>(this IEnumerable<WeightedItem<T>> list)
        {
            float result = 0;

            foreach (WeightedItem<T> weightedItem in list)
                result += weightedItem.probability;

            return result;
        }

        public static bool ShouldRandomBeNone<T>(this IEnumerable<WeightedItem<T>> list)
        {
            float sum = list.GetProbabilitySum();
            return 1 - sum >= Random.value;
        }

        public static bool TryGetRandom<T>(this IList<WeightedItem<T>> list, out T result)
        {
            if (!list.ShouldRandomBeNone())
            {
                result = list.GetRandom();
                return true;
            }

            result = default;
            return false;
        }

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
