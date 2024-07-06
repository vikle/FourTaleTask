using ECSCore;

namespace ECSGame.UI
{
    public sealed class HealthBarInitializeSystem : IEntityInitializeSystem
    {
        public void OnAfterEntityCreated(IContext context, IEntity entity)
        {
            if (!entity.TryGet(out HealthBarComponent health_bar)) return;
            if (!entity.TryGet(out HealthComponent health)) return;
            
            health_bar.bar.SetPercentage(health.currentHealth / health.maxHealth);
        }
    };
}
