using System;
using UnityEngine;

namespace Game.UI
{
    public sealed class UIGameHudPanel : MonoBehaviour
    {

        public UIIntText drawDeckCountText;
        public UIIntText discardDeckCountText;
        public UIIntText energyCountText;

        void Awake()
        {
            EventBus.Register<CardGameTable>(EventHooks.k_CardGameTableOnCardsUpdated, CardGameTableOnCardsUpdated);
        }

        private void CardGameTableOnCardsUpdated(CardGameTable table)
        {
            drawDeckCountText.Set(table.DrawDeck.Count);
            discardDeckCountText.Set(table.DiscardDeck.Count);
        }

        
        
    };
}
