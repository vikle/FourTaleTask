using ECSCore;

namespace ECSGame
{
    public class HealthComponentEnabledSystem : IEntityEnabledSystem
    {
        public void OnIEntityEnabled(IEntity entity, IContext context)
        {
            if (!entity.TryGet(out HealthComponent health)) return;

            health.currentHealth = health.maxHealth;
        }
    }
}
