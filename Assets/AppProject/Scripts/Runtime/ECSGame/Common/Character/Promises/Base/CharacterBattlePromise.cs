using System.Collections.Generic;
using ECSCore;

namespace ECSGame
{
    public abstract class CharacterBattlePromise : IPromise
    {
        public EPromiseState State { get; set; }
        public List<IEvent> Resolve { get; } = new();
        
        public float eventTriggerTime;
    };
}
