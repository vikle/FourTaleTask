using ECSCore;

namespace ECSGame
{
    public sealed class DamageSystem : IUpdateSystem
    {
        public void OnUpdate(IContext context)
        {
            foreach (var entity in context)
            {
                if (!entity.TryGet(out DamageEvent damage_evt)) continue;
                if (!entity.TryGet(out HealthComponent health)) continue;

                float damage_value = damage_evt.value;

                if (entity.TryGet(out DefenceBuff defence_buff))
                {
                    float defence_value = defence_buff.currentValue;
                    ref float defence_value_ref = ref defence_buff.currentValue;

                    defence_value_ref -= damage_value;
                    
                    entity.Trigger<DefenceBuffChangedEvent>();
                    
                    if (defence_value_ref <= 0f)
                    {
                        defence_value_ref = 0f;
                        entity.Trigger<RemoveDefenceBuffEvent>();
                    }
                    
                    damage_value -= defence_value;

                    if (damage_value <= 0f)
                    {
                        continue;
                    }
                }
                
                ref float health_value = ref health.currentHealth;
                health_value -= damage_value;

                if (health_value <= 0f)
                {
                    health_value = 0f;
                    entity.Trigger<DeathEvent>();
                }
                
                entity.Trigger<HealthChangedEvent>();
            }
        }
    };
}
