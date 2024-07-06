using ECSCore;

namespace ECSGame
{
    public sealed class CharacterSystem : IUpdateSystem
    {
        public void OnUpdate(IContext context)
        {
            foreach (var entity in context)
            {
                if (!entity.TryGet(out CharacterComponent character)) continue;

                if (entity.TryGet(out CharacterHealEvent chr_heal_event))
                {
                    var heal_promise = entity.Then<CharacterHealPromise>();
                    var heal_event = FragmentFactory.GetInstance<HealEvent>();
                    heal_event.value = chr_heal_event.value;
                    heal_promise.Resolve.Add(heal_event);
                    heal_promise.eventTriggerTime = (TimeData.Time + character.healEventDelay);
                }

                if (entity.TryGet(out CharacterHealPromise chr_heal_promise))
                {
                    chr_heal_promise.IsFulfilled = (chr_heal_promise.eventTriggerTime <= TimeData.Time);
                }
                
                
                
            }
        }
    };
}
