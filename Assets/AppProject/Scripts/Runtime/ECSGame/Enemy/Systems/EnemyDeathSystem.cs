using ECSCore;

namespace ECSGame
{
    public sealed class EnemyDeathSystem : CharacterDeathSystem
    {
        protected override void OnDie(IContext context, IEntity entity)
        {
            base.OnDie(context, entity);
            
            if (entity.Has<EnemyMarker>() == false) return;
            
            
        }
    };
}
