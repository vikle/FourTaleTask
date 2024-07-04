using ECSCore;

namespace ECSGame
{
    public sealed class PlayerDeathSystem : DeathSystem
    {
        protected override void OnDie(IEntity entity)
        {
            if (entity.Has<PlayerMarker>() == false) return;
            
            
        }
    };
}
