using System.Collections.Generic;
using ECSCore;
using ECSGame;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "CardDamageEffect", menuName = "CardGame/Effect/Damage", order = 51)]
    public sealed class CardDamageEffect : CardEffect
    {
        public override void Apply(IEntity owner, List<IEntity> opponents)
        {
            var attack_event = owner.Trigger<CharacterAttackEvent>();
            attack_event.value = effectValue;
            
            var attack_targets = attack_event.targets;
            attack_targets.Clear();
            
            switch (target)
            {
                case ECardEffectTarget.SelectedOpponent:
                    if (opponents.Count > 1)
                    {
                        var pointer = Table.handSightPointer;

                        if (pointer.IsHaveHit && pointer.HitObject.TryGetComponent(out EntityActor enemy_actor))
                        {
                            attack_targets.Add(enemy_actor.Entity);
                        }
                    }
                    else
                    {
                        attack_targets.AddRange(opponents);
                    }
                    break;
                case ECardEffectTarget.RandomOpponent:
                    int random_index = Random.Range(0, opponents.Count);
                    attack_targets.Add(opponents[random_index]);
                    break;
                case ECardEffectTarget.AllOpponents: 
                    attack_targets.AddRange(opponents);
                    break;
                
                default: break;
            }
        }
    };
}
