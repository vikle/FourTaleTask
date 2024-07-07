using System.Collections.Generic;
using ECSCore;

namespace ECSGame
{
    public sealed class CharacterAttackEvent : CharacterBattleEvent
    {
        public readonly List<IEntity> targets = new();
    };
}
