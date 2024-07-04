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
                .BindSystem<DamageSystem>()
                .BindSystem<PlayerDeathSystem>()
                .BindSystem<EnemyDeathSystem>()
                ;

            context
                .BindEvent<DamageEvent>()
                .BindEvent<DeathEvent>()
                ;
            
            
            
        }
    };
}
