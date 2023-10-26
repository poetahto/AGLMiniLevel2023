using UnityEngine;

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

        [SerializeField]
        [Tooltip("How quickly oxygen leaves the suit, depending on percent integrity.")]
        private AnimationCurve oxygenDepletionScaling;

        [SerializeField]
        private PlayerDeathEffect deathEffect;

        [SerializeField]
        private PlayerAnimator playerAnimator;

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
            Integrity = Mathf.Max(Integrity - eventData.Amount, 0); // Make sure you can't go below 0 integrity.
            playerAnimator.PlayDamage();
            // todo: sfx, vfx
        }

        private void Update()
        {
            float currentDepletionRate = maxOxygenDepletionRate * oxygenDepletionScaling.Evaluate(IntegrityPercent) * Time.deltaTime;
            Oxygen = Mathf.Max(Oxygen - currentDepletionRate, 0); // make sure you can't go below 0 oxygen

            if (Oxygen == 0 && !IsDead)
            {
                IsDead = true;
                deathEffect.Play();
            }
        }
    }
}
