using ECSCore;

namespace ECSGame
{
    public abstract class DeathSystem : IUpdateSystem
    {
        public void OnUpdate(IContext context)
        {
            foreach (var entity in context)
            {
                if (!entity.Has<DeathEvent>()) continue;
                
                if (entity.TryGet(out HealthComponent health))
                {
                    health.currentHealth = 0f;
                }
                    
                OnDie(entity);
            }
        }

        protected abstract void OnDie(IEntity entity);
    };
}
