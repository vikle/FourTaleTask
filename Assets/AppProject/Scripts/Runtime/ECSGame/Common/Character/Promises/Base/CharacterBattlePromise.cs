using System.Collections.Generic;
using ECSCore;

namespace ECSGame
{
    public abstract class CharacterBattlePromise : IPromise
    {
        public bool IsFulfilled { get; set; }
        public List<IEvent> Resolve { get; } = new();
        
        public float eventTriggerTime;
    };
}
