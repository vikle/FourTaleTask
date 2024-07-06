using ECSCore;

namespace ECSGame
{
    public sealed class AnimationSystem : IUpdateSystem
    {
        public void OnUpdate(IContext context)
        {
            foreach (var entity in context)
            {
                if (!entity.TryGet(out PlayAnimationEvent play_anim_evt)) continue;
                if (!entity.TryGet(out AnimationComponent anim_comp)) continue;
                
                anim_comp.animator.Play(play_anim_evt.stateNameHash);
            }
        }
    };
}
