using System;
using UnityEngine;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

namespace AGL.Asteroids
{
    public class AsteroidPath : MonoBehaviour
    {
        [SerializeField]
        private SplineContainer splineContainer;

        public Spline Spline => splineContainer.Spline;
        public SplineContainer Container => splineContainer;

        public event Action OnLifetimeEnd;

        private float _remainingTime;

        public void Initialize(Transform targetTransform, float strikingRange, float lifetime)
        {
            _remainingTime = lifetime;

            RaycastHit spawnPos = PhysicsUtil.GetRandomPointOnGroundNear(
                -targetTransform.up,
                targetTransform.position,
                strikingRange / 2
            );

            if (spawnPos.point == Vector3.zero)
                spawnPos.point = targetTransform.position;

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
            Spline.Add(new BezierKnot(strikePoint), TangentMode.AutoSmooth, 1);
            Spline.Add(new BezierKnot(playerPos, -vectorToPlayer, vectorToPlayer));
        }

        private void Update()
        {
            _remainingTime -= Time.deltaTime;

            if (_remainingTime <= 0)
            {
                OnLifetimeEnd?.Invoke();
                _remainingTime = float.PositiveInfinity;
            }
        }
    }
}
