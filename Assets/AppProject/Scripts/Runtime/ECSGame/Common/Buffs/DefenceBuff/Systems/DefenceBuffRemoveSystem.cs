using ECSCore;

namespace ECSGame
{
    public sealed class DefenceBuffRemoveSystem : IUpdateSystem
    {
        public void OnUpdate(IContext context)
        {
            foreach (var entity in context)
            {
                if (!entity.Has<RemoveDefenceBuffEvent>()) continue;
                
                if (entity.TryGet(out DefenceBuff defence_buff))
                {
                    defence_buff.currentValue = 0f;
                    defence_buff.maxValue = 0f;
                }
                
                entity.Remove<DefenceBuff>();
            }
        }
    };
}
