using UnityEngine;
using ECSCore;
using Game;

namespace ECSGame
{
    [DisallowMultipleComponent]
    public sealed class ECSGameBootstrap : MonoBehaviour, IECSBootstrap
    {
        public CardGameTable cardGameTable;
        
        
        public void OnBootstrap(IContextBinding context)
        {
            context
                .BindSystem<HealthComponentInitializeSystem>()
                ;
            
            context
                .BindSystem<AnimationDataInitializeSystem>()
                ;
            
            context
                .BindSystem<CharacterBattleSystem>()
                .BindSystem<HealingSystem>()
                .BindSystem<CombatSystem>()
                .BindSystem<DamageSystem>()
                .BindSystem<DefenceBuffAddSystem>()
                .BindSystem<DefenceBuffRemoveSystem>()
                .BindSystem<PlayerDeathSystem>()
                .BindSystem<EnemyDeathSystem>()
                ;

            context
                .BindSystem<CharacterAnimationSystem>()
                .BindSystem<AnimationSystem>()
                ;
                
            context
                .BindSystem<UI.HealthBarInitializeSystem>()
                .BindSystem<UI.HealthBarSystem>()
                ;
            
            context
                .BindEvent<AddDefenceBuffEvent>()
                .BindEvent<RemoveDefenceBuffEvent>()
                .BindEvent<DefenceBuffChangedEvent>()
                .BindEvent<HealEvent>()
                .BindEvent<AttackEvent>()
                .BindEvent<DamageEvent>()
                .BindEvent<DeathEvent>()
                .BindEvent<CharacterHealEvent>()
                .BindEvent<CharacterAttackEvent>()
                .BindEvent<CharacterDefenceEvent>()
                ;
            
            context
                .BindEvent<PlayAnimationEvent>()
                ;
            
            context
                .BindPromise<CharacterHealPromise>()
                .BindPromise<CharacterAttackPromise>()
                .BindPromise<CharacterDefencePromise>()
                ;


            context
                .Inject(cardGameTable)
                ;
        }
    };
}
