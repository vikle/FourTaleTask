using ECSCore;

namespace ECSGame.UI
{
    public sealed class DefenceBarSystem : IUpdateSystem
    {
        public void OnUpdate(IContext context)
        {
            foreach (var entity in context)
            {
                if (!entity.Has<DefenceBuffChangedEvent>()) continue;
                if (!entity.TryGet(out DefenceBarComponent defence_bar)) continue;

                if (entity.Has<RemoveDefenceBuffEvent>())
                {
                    defence_bar.bar.SetPercentage(0f);
                    continue;
                }
                
                if (entity.TryGet(out DefenceBuff defence_buff))
                {
                    defence_bar.bar.SetPercentage(defence_buff.currentValue / defence_buff.maxValue);
                }
            }
        }
    };
}
