using ECSCore;

namespace ECSGame
{
    public sealed class EnemyDeathSystem : DeathSystem
    {
        protected override void OnDie(IEntity entity)
        {
            if (entity.Has<EnemyMarker>() == false) return;
            
            
        }
    };
}
