using System.Collections.Generic;
using ECSCore;

namespace ECSGame
{
    public sealed class CharacterAttackPromise : IPromise
    {
        public bool IsFulfilled { get; set; }
        public List<IEvent> Resolve { get; } = new();

        public float damage;
        public readonly List<IEntity> targets = new();
    };
}
