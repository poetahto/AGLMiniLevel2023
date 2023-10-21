using UnityEngine;

namespace DefaultNamespace
{
    // Static generic helper methods for some physics stuff.
    public static class PhysicsUtil
    {
        private static RaycastHit[] _hits = new RaycastHit[30];

        // Calculates a random point on the ground, within some range of a transform.
        public static RaycastHit GetRandomPointOnGroundNear(Vector3 down, Vector3 point, float radius)
        {
            Vector2 randomOffset2d = Random.insideUnitCircle * (radius * 2);
            Vector3 randomOffset = new Vector3(randomOffset2d.x, randomOffset2d.y, 0);
            randomOffset = Quaternion.LookRotation(-down) * randomOffset;
            Vector3 unsnappedRandomPosition = point + randomOffset;
            return SnapToGround(down, unsnappedRandomPosition);
        }

        // Snaps a point to the closest collider, given some downwards direction.
        public static RaycastHit SnapToGround(Vector3 down, Vector3 point)
        {
            Ray ray = new Ray(point, down);
            int hits = Physics.RaycastNonAlloc(ray, _hits);

            if (hits == 0)
                return default;

            // Find the closest result.
            int resultIndex = 0;

            for (int i = 0; i < hits; i++)
            {
                if (_hits[i].distance < _hits[resultIndex].distance)
                    resultIndex = i;
            }

            return _hits[resultIndex];
        }
    }
}
