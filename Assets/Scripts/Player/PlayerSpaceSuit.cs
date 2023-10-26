using UnityEngine;
using UnityEngine.Serialization;

namespace AGL.Player
{
    public class PlayerSpaceSuit : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private float maxOxygen;

        [SerializeField]
        private float maxIntegrity;

        [SerializeField]
        private float maxOxygenDepletionRate;

        [FormerlySerializedAs("oxygenDepletionRate")]
        [SerializeField]
        [Tooltip("How quickly oxygen leaves the suit, depending on percent integrity.")]
        private AnimationCurve oxygenDepletionScaling;

        public float Integrity { get; set; }
        public float IntegrityMax => maxIntegrity;
        public float IntegrityPercent => Integrity / IntegrityMax;

        public float Oxygen { get; set; }
        public float OxygenMax => maxOxygen;
        public float OxygenPercent => Oxygen / OxygenMax;

        public bool IsDead { get; set; }

        private void Awake()
        {
            Integrity = maxIntegrity;
            Oxygen = maxOxygen;
        }

        public void Damage(DamageEvent eventData)
        {
            print($"damaged {eventData.Amount}!");
            Integrity = Mathf.Max(Integrity - eventData.Amount, 0); // Make sure you can't go below 0 integrity.
            // todo: sfx, vfx, animator
        }

        private void Update()
        {
            float currentDepletionRate = maxOxygenDepletionRate * oxygenDepletionScaling.Evaluate(IntegrityPercent) * Time.deltaTime;
            Oxygen = Mathf.Max(Oxygen - currentDepletionRate, 0); // make sure you can't go below 0 oxygen

            if (Oxygen == 0 && !IsDead)
            {
                print("you died!");
                IsDead = true;
                // todo: you are dead - game state stuff, sfx, vfx, animator
            }
        }
    }
}
