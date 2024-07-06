using System.Collections.Generic;
using ECSCore;

namespace ECSGame
{
    public sealed class CharacterHealPromise : IPromise
    {
        public bool IsFulfilled { get; set; }
        public List<IEvent> Resolve { get; } = new();

        public float eventTriggerTime;
    };
}
