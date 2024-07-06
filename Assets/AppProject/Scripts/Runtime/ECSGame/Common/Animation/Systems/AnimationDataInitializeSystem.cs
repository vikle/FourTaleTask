using ECSCore;
using UnityEngine;

namespace ECSGame
{
    public sealed class AnimationDataInitializeSystem : IEntityInitializeSystem
    {
        public void OnAfterEntityCreated(IContext context, IEntity entity)
        {
            if (!entity.TryGet(out AnimationComponent anim_comp)) return;

            var anim_data = entity.Add<AnimationDataComponent>();
            
            anim_data.IdleHash = Animator.StringToHash(anim_comp.idleName);
            anim_data.AttackHash = Animator.StringToHash(anim_comp.attackName);
            anim_data.DefenceHash = Animator.StringToHash(anim_comp.defenceName);
            anim_data.HealHash = Animator.StringToHash(anim_comp.healName);
            anim_data.DeathHash = Animator.StringToHash(anim_comp.deathName);
        }
    };
}
