using System;
using ECSCore;

namespace ECSGame
{
    public sealed class DefenceBuffAddSystem : IUpdateSystem
    {
        public void OnUpdate(IContext context)
        {
            foreach (var entity in context)
            {
                if (!entity.TryGet(out AddDefenceBuffEvent add_defence)) continue;
                if (!entity.TryGet(out HealthComponent health)) continue;
                
                var defence_buff = entity.Add<DefenceBuff>();
                
                float max_value = health.maxHealth;
                defence_buff.currentValue = MathF.Min(defence_buff.currentValue + add_defence.initialValue, max_value);
                defence_buff.maxValue = max_value;
                
                entity.Trigger<DefenceBuffChangedEvent>();
            }
        }
    };
}
