using ECSCore;

namespace ECSGame
{
    public sealed class CharacterAnimationSystem : IUpdateSystem
    {
        public void OnUpdate(IContext context)
        {
            foreach (var entity in context)
            {
                if (!entity.Has<CharacterComponent>()) continue;
                if (!entity.TryGet(out AnimationDataComponent anim_data)) continue;

                if (entity.Has<CharacterHealEvent>())
                {
                    var play_anim_evt = entity.Trigger<PlayAnimationEvent>();
                    play_anim_evt.stateNameHash = anim_data.DefenceHash;
                }
                
                
                
                
                
            }
        }
    };
}
