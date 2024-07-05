using System;
using ECSCore;

namespace ECSGame
{
    public sealed class HealingSystem : IUpdateSystem
    {
        public void OnUpdate(IContext context)
        {
            foreach (var entity in context)
            {
                if (!entity.TryGet(out HealEvent heal)) continue;
                if (!entity.TryGet(out HealthComponent health)) continue;
                
                health.currentHealth = MathF.Min(health.currentHealth + heal.value, health.maxHealth);
                
                entity.Trigger<HealthChangedEvent>();
            }
        }
    };
}
