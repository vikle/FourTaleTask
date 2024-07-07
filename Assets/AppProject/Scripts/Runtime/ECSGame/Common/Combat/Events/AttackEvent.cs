using System.Collections.Generic;
using ECSCore;

namespace ECSGame
{
    public sealed class AttackEvent : IEvent
    {
        public float damage;
        public readonly List<IEntity> targets = new();
    };
}
