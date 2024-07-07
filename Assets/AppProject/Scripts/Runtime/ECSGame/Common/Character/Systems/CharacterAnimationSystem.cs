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
                
                if (entity.Has<CharacterAttackEvent>())
                {
                    PlayAnimation(entity, anim_data.AttackHash);
                }
                else if (entity.Has<CharacterDefenceEvent>())
                {
                    PlayAnimation(entity, anim_data.DefenceHash);
                }
                else if (entity.Has<CharacterHealEvent>())
                {
                    PlayAnimation(entity, anim_data.HealHash);
                }
                else if (entity.Has<DeathEvent>())
                {
                    PlayAnimation(entity, anim_data.DeathHash);
                }
            }
        }

        private static void PlayAnimation(IEntity entity, int stateNameHash)
        {
            var play_anim_event = entity.Trigger<PlayAnimationEvent>();
            play_anim_event.stateNameHash = stateNameHash;
        }
    };
}
