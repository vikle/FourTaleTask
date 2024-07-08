using ECSCore;

namespace ECSGame.UI
{
    public sealed class HealthBarSystem : IUpdateSystem
    {
        public void OnUpdate(IContext context)
        {
            foreach (var entity in context)
            {
                HealthBarProcess(entity);
                DefenceBarProcess(entity);
            }
        }

        private static void HealthBarProcess(IEntity entity)
        {
            if (!entity.Has<HealthChangedEvent>()) return;
            if (!entity.TryGet(out HealthBarComponent health_bar)) return;

            if (entity.Has<DeathEvent>())
            {
                health_bar.Bar.SetActive(false);
                health_bar.Bar.SetHealthBarPercentage(0f);
                return;
            }

            if (entity.TryGet(out HealthComponent health))
            {
                health_bar.Bar.SetHealthBarPercentage(health.currentHealth / health.maxHealth);
            } 
        }

        private static void DefenceBarProcess(IEntity entity)
        {
            if (!entity.Has<DefenceBuffChangedEvent>()) return;
            if (!entity.TryGet(out HealthBarComponent health_bar)) return;

            if (entity.Has<RemoveDefenceBuffEvent>())
            {
                health_bar.Bar.SetDefenceBarPercentage(0f);
                return;
            }
                
            if (entity.TryGet(out DefenceBuff defence_buff))
            {
                health_bar.Bar.SetDefenceBarPercentage(defence_buff.currentValue / defence_buff.maxValue);
            }
        }
    };
}
