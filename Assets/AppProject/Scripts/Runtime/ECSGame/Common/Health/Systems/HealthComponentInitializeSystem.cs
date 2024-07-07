using ECSCore;

namespace ECSGame
{
    public sealed class HealthComponentInitializeSystem : IEntityInitializeSystem
    {
        public void OnAfterEntityCreated(IContext context, IEntity entity)
        {
            if (!entity.TryGet(out HealthComponent health)) return;

            health.currentHealth = health.maxHealth;
        }
    }
}
