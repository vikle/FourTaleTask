using System;
using UnityEngine;

namespace Game.UI
{
    public sealed class UIGameHudPanel : MonoBehaviour
    {
        public HandArea handArea;
        public UIIntText drawDeckCountText;
        public UIIntText discardDeckCountText;
        public UIIntText energyCountText;
        public GameObject endTurnButton;

        void Awake()
        {
            EventBus.Register<CardGameTable>(EventHooks.k_OnCardGameTablePlayerTurn, OnCardGameTablePlayerTurn);
            EventBus.Register<CardGameTable>(EventHooks.k_OnCardGameTableEnemiesTurn, OnCardGameTableEnemiesTurn);
            
            EventBus.Register<CardGameTable>(EventHooks.k_OnCardGameTableCardsUpdated, CardGameTableOnCardsUpdated);
            EventBus.Register<int>(EventHooks.k_OnCardGamePlayerEnergyUpdated, OnCardGamePlayerEnergyUpdated);

            endTurnButton.SetActive(true);
        }

        private void OnCardGameTablePlayerTurn(CardGameTable table)
        {
            endTurnButton.SetActive(true);
        }
        private void OnCardGameTableEnemiesTurn(CardGameTable table)
        {
            endTurnButton.SetActive(false);
        }
        
        private void CardGameTableOnCardsUpdated(CardGameTable table)
        {
            drawDeckCountText.Set(table.DrawDeck.Count);
            discardDeckCountText.Set(table.DiscardDeck.Count);
        }

        private void OnCardGamePlayerEnergyUpdated(int energy)
        {
            energyCountText.Set(energy);
        }

        public void _OnEndTurn()
        {
            EventBus.Trigger(EventHooks.k_OnGameControllerNextTurn);
        }
    };
}
