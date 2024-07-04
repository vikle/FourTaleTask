using ECSCore;

namespace ECSGame
{
    public sealed class DamageSystem : IUpdateSystem
    {
        public void OnUpdate(IContext context)
        {
            foreach (var entity in context)
            {
                if (!entity.TryGet(out DamageEvent damage)) continue;
                if (!entity.TryGet(out HealthComponent health)) continue;
                
                ref float health_value = ref health.currentHealth;
                health_value -= damage.value;

                if (health_value <= 0f)
                {
                    entity.Trigger<DeathEvent>();
                }
            }
        }
    };
}
