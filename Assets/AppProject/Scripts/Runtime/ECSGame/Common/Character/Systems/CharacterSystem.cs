using ECSCore;
using Game;

namespace ECSGame
{
    public sealed class CharacterSystem : IUpdateSystem
    {
        public void OnUpdate(IContext context)
        {
            foreach (var entity in context)
            {
                if (!entity.TryGet(out CharacterComponent character)) continue;
                
                if (entity.TryGet(out CharacterAttackEvent attack_event))
                {
                    var resolve_event = FragmentFactory.GetInstance<AttackEvent>();
                    resolve_event.damage = attack_event.value;
                    var attack_targets = resolve_event.targets;
                    attack_targets.Clear();
                    attack_targets.AddRange(attack_event.targets);
                    ThenPromise<CharacterAttackPromise>(entity, character.damageEventDelay, resolve_event);
                }
                
                if (entity.TryGet(out CharacterDefenceEvent defence_event))
                {
                    var resolve_event = FragmentFactory.GetInstance<AddDefenceBuffEvent>();
                    resolve_event.initialValue = defence_event.value;
                    ThenPromise<CharacterDefencePromise>(entity, character.defenceEventDelay, resolve_event);
                }
                
                if (entity.TryGet(out CharacterHealEvent heal_event))
                {
                    var resolve_event = FragmentFactory.GetInstance<HealEvent>();
                    resolve_event.value = heal_event.value;
                    ThenPromise<CharacterHealPromise>(entity, character.healEventDelay, resolve_event);
                }

                PromiseProcess<CharacterAttackPromise>(entity);
                PromiseProcess<CharacterDefencePromise>(entity);
                PromiseProcess<CharacterHealPromise>(entity);
            }
        }

        private static void ThenPromise<T>(IEntity entity, float eventDelay, IEvent resolveEvent) where T : CharacterBattlePromise
        {
            var promise = entity.Then<T>();
            promise.Resolve.Add(resolveEvent);
            promise.eventTriggerTime = (TimeData.Time + eventDelay);
        }

        private static void PromiseProcess<T>(IEntity entity) where T : CharacterBattlePromise
        {
            if (!entity.TryGet(out T promise)) return;
            if (TimeData.Time < promise.eventTriggerTime) return;
            promise.State = EPromiseState.Fulfilled;
        }
    };
}
