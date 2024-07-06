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
                .BindSystem<HealingSystem>()
                .BindSystem<DamageSystem>()
                .BindSystem<DefenceBuffAddSystem>()
                .BindSystem<DefenceBuffRemoveSystem>()
                .BindSystem<PlayerDeathSystem>()
                .BindSystem<EnemyDeathSystem>()
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
                ;
            
            
            
        }
    };
}
