using ECSCore;

namespace ECSGame
{
    public abstract class DeathSystem : IUpdateSystem
    {
        public void OnUpdate(IContext context)
        {
            foreach (var entity in context)
            {
                if (entity.Has<DeathEvent>())
                {
                    OnDie(entity);
                }
            }
        }

        protected abstract void OnDie(IEntity entity);
    };
}
