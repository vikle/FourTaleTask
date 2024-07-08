using System.Collections.Generic;
using UnityEngine;
using ECSCore;
using ECSGame;

namespace Game
{
    [CreateAssetMenu(fileName = "CardDefenceEffect", menuName = "CardGame/Effect/Defence", order = 51)]
    public sealed class CardDefenceEffect : CardEffect
    {
        public override void Apply(IEntity owner, List<IEntity> opponents)
        {
            var defence_event = owner.Trigger<CharacterDefenceEvent>();
            defence_event.value = effectValue;
        }
    };
}
