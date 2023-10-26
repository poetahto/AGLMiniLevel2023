using UnityEngine;

namespace AGL.Misc
{
    public static class ColliderExtensions
    {
        public static GameObject GetRoot(this Collider collider)
        {
            if (collider.attachedRigidbody != null)
                return collider.attachedRigidbody.gameObject;

            return collider.gameObject;
        }

        public static bool TryGetComponentOnRoot<T>(this Collider collider, out T result)
        {
            GameObject root = collider.GetRoot();
            return root.TryGetComponent(out result);
        }
    }
}
