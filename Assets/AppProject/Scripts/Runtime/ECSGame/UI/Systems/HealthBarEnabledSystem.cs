using ECSCore;

namespace ECSGame.UI
{
    public sealed class HealthBarEnabledSystem : IEntityEnabledSystem
    {
        public void OnIEntityEnabled(IEntity entity, IContext context)
        {
            if (!entity.TryGet(out HealthBarComponent health_bar)) return;
            if (!entity.TryGet(out HealthComponent health)) return;
            
            health_bar.bar.SetPercentage(health.currentHealth / health.maxHealth);
        }
    };
}
