using System.Collections.Generic;
using UnityEngine;
using ECSCore;
using ECSGame;

namespace Game
{
    [CreateAssetMenu(fileName = "CardHealEffect", menuName = "CardGame/Effect/Heal", order = 51)]
    public sealed class CardHealEffect : CardEffect
    {
        public override void Apply(IEntity owner, List<IEntity> opponents)
        {
            var heal_event = owner.Trigger<CharacterHealEvent>();
            heal_event.value = effectValue;
        }
    };
}
