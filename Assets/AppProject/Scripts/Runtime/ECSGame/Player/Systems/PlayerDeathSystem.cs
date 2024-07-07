using ECSCore;

namespace ECSGame
{
    public sealed class PlayerDeathSystem : CharacterDeathSystem
    {
        protected override void OnDie(IContext context, IEntity entity)
        {
            base.OnDie(context, entity);
            
            if (entity.Has<PlayerMarker>() == false) return;
            
            
        }
    };
}
