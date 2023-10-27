namespace AGL
{
    public struct DamageEvent
    {
        public float Amount;
    }

    public interface IDamageable
    {
        void Damage(DamageEvent eventData); // pass data as struct, so we can add more data without breaking compatability
    }
}
