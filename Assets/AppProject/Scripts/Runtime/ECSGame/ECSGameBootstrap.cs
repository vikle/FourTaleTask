using UnityEngine;
using ECSCore;

namespace ECSGame
{
    [DisallowMultipleComponent]
    public sealed class ECSGameBootstrap : MonoBehaviour, IECSBootstrap
    {
        
        
        
        public void OnBootstrap(IContextBinding context)
        {
            context
                .BindSystem<HealthComponentInitializeSystem>()
                ;
            
            context
                .BindSystem<AnimationDataInitializeSystem>()
                ;
            
            context
                .BindSystem<CharacterSystem>()
                .BindSystem<HealingSystem>()
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
                .BindSystem<UI.DefenceBarInitializeSystem>()
                .BindSystem<UI.DefenceBarSystem>()
                ;
            
            context
                .BindEvent<AddDefenceBuffEvent>()
                .BindEvent<RemoveDefenceBuffEvent>()
                .BindEvent<DefenceBuffChangedEvent>()
                .BindEvent<HealEvent>()
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
            
            
            
        }
    };
}
