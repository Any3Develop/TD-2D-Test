namespace Common.Runtime
{
    public interface IDamagable
    {
        void ApplyDamage(DamageContext ctx);
    }
}