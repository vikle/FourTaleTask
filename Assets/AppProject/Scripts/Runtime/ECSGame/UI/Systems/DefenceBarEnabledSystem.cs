using ECSCore;

namespace ECSGame.UI
{
    public sealed class DefenceBarEnabledSystem : IEntityEnabledSystem
    {
        public void OnIEntityEnabled(IEntity entity, IContext context)
        {
            if (!entity.TryGet(out DefenceBarComponent defence_bar)) return;
            
            defence_bar.bar.SetPercentage(0f);
        }
    };
}
