using ECSCore;

namespace ECSGame.UI
{
    public sealed class HealthBarSystem : IUpdateSystem
    {
        public void OnUpdate(IContext context)
        {
            foreach (var entity in context)
            {
                if (!entity.Has<HealthChangedEvent>()) continue;
                if (!entity.TryGet(out HealthBarComponent health_bar)) return;

                if (entity.Has<DeathEvent>())
                {
                    health_bar.bar.SetPercentage(0f);
                    continue;
                }

                if (entity.TryGet(out HealthComponent health))
                {
                    health_bar.bar.SetPercentage(health.currentHealth / health.maxHealth);
                } 
            }
        }
    };
}
