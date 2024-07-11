using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECSCore;
using ECSGame;

namespace Game
{
    public sealed class AIController : MonoBehaviour
    {
        public float enemiesAttackDuration = 3f;
        
        readonly List<CardEffect> m_effects = new(8);
        readonly List<IEntity> m_enemies = new(8);

        public void Init(GameController controller)
        {
            EventBus.Register<CardGameTable>(EventHooks.k_OnCardGameTablePlayerTurn, OnCardGameTablePlayerTurn);
            EventBus.Register<CardGameTable>(EventHooks.k_OnCardGameTableEnemiesTurn, OnCardGameTableEnemiesTurn);
            
            var table = controller.cardGameTable;
            var all_cards = table.allAvailableCards;

            InitEffects(all_cards);
        }

        private void InitEffects(IReadOnlyList<Card> allCards)
        {
            for (int i = 0, i_max = allCards.Count; i < i_max; i++)
            {
                var card = allCards[i];
                var card_effects = card.effects;
                
                for (int j = 0, j_max = card_effects.Length; j < j_max; j++)
                {
                    var effect = card_effects[j];
                    
                    if (!m_effects.Contains(effect))
                    {
                        m_effects.Add(effect);
                    }
                }
            }
        }

        private void OnCardGameTablePlayerTurn(CardGameTable table)
        {
            PrepareIntentions(table.CurrentOpponents);
        }
        
        private void OnCardGameTableEnemiesTurn(CardGameTable table)
        {
            StartCoroutine(IEPlayEnemiesTurn(table));
        }
        
        private void PrepareIntentions(IReadOnlyList<IEntity> enemies)
        {
            int effects_count = m_effects.Count;
            
            for (int i = 0, i_max = enemies.Count; i < i_max; i++)
            {
                var enemy = enemies[i];
                var add_event = enemy.Add<EnemyAddIntentionEvent>();
                int random_index = Random.Range(0, effects_count);
                var random_effect = m_effects[random_index];
                add_event.effect = random_effect;
            }
        }

        private IEnumerator IEPlayEnemiesTurn(CardGameTable table)
        {
            m_enemies.Clear();
            m_enemies.AddRange(table.AllPlayers);
            m_enemies.RemoveAll(p => p.Has<PlayerMarker>());
            
            for (float t = 0f; t < 1f; t += Time.deltaTime)
            {
                yield return null;
            }

            for (int i = 0, i_max = m_enemies.Count; i < i_max; i++)
            {
                var enemy_entity = m_enemies[i];
                enemy_entity.Trigger<EnemyPerformIntentionEvent>();
                
                yield return null;
                yield return null;

                // Не успел доделать ожидание продолжительности хода каждого врага.
                // Так как ее калибровка заняла бы много времени, а эффекти был бы примерно такой же
                // Потому просто заглушка =))
                for (float t = 0f; t < enemiesAttackDuration; t += Time.deltaTime)
                {
                    yield return null;
                }
                
                EventBus.Trigger(EventHooks.k_OnGameControllerNextTurn);
            }
        }
    };
}
