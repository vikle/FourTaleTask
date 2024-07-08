using ECSCore;

namespace ECSGame.UI
{
    public sealed class HealthBarInitializeSystem : IEntityInitializeSystem
    {
        public void OnAfterEntityCreated(IContext context, IEntity entity)
        {
            if (!entity.TryGet(out HealthBarComponent health_bar)) return;

            var bar = health_bar.Bar;

            bar.SetActive(true);
            
            if (entity.TryGet(out HealthComponent health))
            {
                bar.SetHealthBarPercentage(health.currentHealth / health.maxHealth);
            }
            
            bar.SetDefenceBarPercentage(0f);
        }
    };
}
