using ECSCore;
using Game;

namespace ECSGame
{
    public abstract class CharacterDeathSystem : DeathSystem
    {
        protected readonly CardGameTable m_cardGameTable;
        
        protected override void OnDie(IContext context, IEntity entity)
        {
            entity.Reject<CharacterAttackPromise>();
            entity.Reject<CharacterDefencePromise>();
            entity.Reject<CharacterHealPromise>();
            
            m_cardGameTable.OnPlayerDead(entity);
        }
    }
}
