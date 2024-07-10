using ECSCore;
using Game;

namespace ECSGame
{
    public sealed class EnemyIntentionSystem : IUpdateSystem
    {
        readonly CardGameTable m_table;
        
        public void OnUpdate(IContext context)
        {
            foreach (var entity in context)
            {
                if (!entity.Has<EnemyMarker>()) continue;

                EnemyIntentionViewComponent intention_view;
                
                if (entity.TryGet(out EnemyAddIntentionEvent add_enemy_intention))
                {
                    var added_intention = entity.Add<EnemyIntentionComponent>();
                    added_intention.effect = add_enemy_intention.effect;

                    if (entity.TryGet(out intention_view))
                    {
                        intention_view.text.text = add_enemy_intention.effect.ToString();
                    }
                    
                    continue;
                }
                
                if (!entity.Has<EnemyPerformIntentionEvent>()) continue;
                if (!entity.TryGet(out EnemyIntentionComponent enemy_intention)) continue;
                
                m_table.ApplyEffect(enemy_intention.effect);

                if (entity.TryGet(out intention_view))
                {
                    intention_view.text.text = string.Empty;
                }
                
                entity.Remove(enemy_intention);
            }
        }
    };
}
