using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public sealed class AIController : MonoBehaviour
    {

        readonly List<CardEffect> m_effects = new(8);

        
        public void Init(GameController controller)
        {
            var table = controller.cardGameTable;
            var all_cards = table.allAvailableCards;

            InitEffects(all_cards);

        }

        private void InitEffects(Card[] allCards)
        {
            for (int i = 0, i_max = allCards.Length; i < i_max; i++)
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
        
        
        
    };
}
