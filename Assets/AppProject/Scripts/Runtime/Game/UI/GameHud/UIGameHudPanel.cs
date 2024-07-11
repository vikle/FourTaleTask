using System.Collections;
using UnityEngine;

namespace Game.UI
{
    public sealed class UIGameHudPanel : MonoBehaviour
    {
        public UIIntText drawDeckCountText;
        public UIIntText discardDeckCountText;
        public UIIntText energyCountText;
        public GameObject outOfEnergyText;
        public GameObject endTurnButton;

        void Awake()
        {
            EventBus.Register<(CardGameTable, Card)>(EventHooks.k_OnCardGameTableCardDiscarded, OnCardGameTableCardDiscarded);
            EventBus.Register<CardGameTable>(EventHooks.k_OnCardGameTablePlayerTurn, OnCardGameTablePlayerTurn);
            EventBus.Register<CardGameTable>(EventHooks.k_OnCardGameTableEnemiesTurn, OnCardGameTableEnemiesTurn);
            EventBus.Register<int>(EventHooks.k_OnCardGamePlayerEnergyUpdated, OnCardGamePlayerEnergyUpdated);
            EventBus.Register(EventHooks.k_OnCardGameTableOutOfEnergyMessage, OnCardGameTableOutOfEnergyMessage);

            outOfEnergyText.SetActive(false);
            endTurnButton.SetActive(true);
        }

        private void OnCardGameTableCardDiscarded((CardGameTable, Card) tuple)
        {
            UpdateDecksTexts(tuple.Item1);
        }
        
        private void OnCardGameTablePlayerTurn(CardGameTable table)
        {
            outOfEnergyText.SetActive(false);
            endTurnButton.SetActive(true);
            UpdateDecksTexts(table);
        }
        private void OnCardGameTableEnemiesTurn(CardGameTable table)
        {
            endTurnButton.SetActive(false);
            UpdateDecksTexts(table);
        }

        private void UpdateDecksTexts(CardGameTable table)
        {
            drawDeckCountText.Set(table.DrawDeck.Count);
            discardDeckCountText.Set(table.DiscardDeck.Count);
        }
        
        private void OnCardGamePlayerEnergyUpdated(int energy)
        {
            energyCountText.Set(energy);
        }

        private void OnCardGameTableOutOfEnergyMessage()
        {
            StartCoroutine(IEFlickerErrorText());
        }
        
        private IEnumerator IEFlickerErrorText()
        {
            float elapsed_time = 0f;
            while (elapsed_time < 1f)
            {
                float current_delay = Mathf.Lerp(0.5f, 0.05f, elapsed_time / 1f);
                outOfEnergyText.SetActive(!outOfEnergyText.activeSelf);

                for (float t = 0; t < current_delay; t+= Time.deltaTime)
                {
                    yield return null;
                }
                
                elapsed_time += current_delay;
            }
            
            outOfEnergyText.SetActive(false);
        }
        
        public void _OnEndTurn()
        {
            EventBus.Trigger(EventHooks.k_OnGameControllerNextTurn);
        }
    };
}
