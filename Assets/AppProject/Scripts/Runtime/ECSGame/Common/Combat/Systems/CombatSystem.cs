using ECSCore;

namespace ECSGame
{
    public sealed class CombatSystem : IUpdateSystem
    {
        public void OnUpdate(IContext context)
        {
            foreach (var entity in context)
            {
                if (!entity.TryGet(out AttackEvent attack_evt)) continue;

                var attack_targets = attack_evt.targets;
                float damage_value = attack_evt.damage;
                
                for (int i = 0, i_max = attack_targets.Count; i < i_max; i++)
                {
                    var target = attack_targets[i];
                    var damage_evt = target.Trigger<DamageEvent>();
                    damage_evt.value = damage_value;
                }
            }
        }
    };
}
