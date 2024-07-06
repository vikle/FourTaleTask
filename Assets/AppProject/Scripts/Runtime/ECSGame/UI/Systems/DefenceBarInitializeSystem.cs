using ECSCore;

namespace ECSGame.UI
{
    public sealed class DefenceBarInitializeSystem : IEntityInitializeSystem
    {
        public void OnAfterEntityCreated(IContext context, IEntity entity)
        {
            if (!entity.TryGet(out DefenceBarComponent defence_bar)) return;
            
            defence_bar.bar.SetPercentage(0f);
        }
    };
}
