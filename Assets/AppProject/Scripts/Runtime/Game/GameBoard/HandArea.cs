﻿using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public sealed class HandArea : MonoBehaviour
    {
        [Space]
        public RectTransform handTransform;
        public HandCard cardPrefab;

        [Space]
        public float cardSpacing = 160f;
        public float cardAngle = 5f;
        public float cardOffsetY = 7f;

        [Space]
        public HandFingerLine handFingerLine;

        readonly Stack<HandCard> m_cardsPool = new(5);
        List<HandCard> m_handCards = new(5);
        readonly Dictionary<Card, HandCard> m_handCardsMap = new(5);
        Transform m_cardsParent;

        void Awake()
        {
            m_cardsParent = cardPrefab.transform.parent;
            
            HandCard.SelectedIndex = -1;
            
            handFingerLine.Init();
            m_cardsPool.Push(cardPrefab);

            EventBus.Register<(CardGameTable, Card)>(EventHooks.k_OnCardGameTableCardDiscarded, OnCardGameTableCardDiscarded);
            EventBus.Register<CardGameTable>(EventHooks.k_OnCardGameTablePlayerTurn, OnCardGameTablePlayerTurn);
            EventBus.Register<CardGameTable>(EventHooks.k_OnCardGameTableEnemiesTurn, OnCardGameTableEnemiesTurn);
        }

        private void OnCardGameTableCardDiscarded((CardGameTable, Card) tuple)
        {
            DiscardCard(tuple.Item2);
        }
        
        private void DiscardCard(Card card)
        {
            if (m_handCardsMap.TryGetValue(card, out var hand_card))
            {
                DiscardCard(hand_card);
            }
        }

        private void DiscardCard(HandCard handCard)
        {
            m_handCards.Remove(handCard);
            m_cardsPool.Push(handCard);
            m_handCardsMap.Remove(handCard.Card);
            handCard.SetActive(false);
        }

        private void OnCardGameTablePlayerTurn(CardGameTable table)
        {
            var hand_deck = table.HandDeck;
            
            for (int i = 0, i_max = hand_deck.Count; i < i_max; i++)
            {
                var card = hand_deck[i];
                DrawCard(card, table);
            }
            
            SortCards();
        }

        private void DrawCard(Card card, CardGameTable table)
        {
            var hand_card = GetHandCardInstance();
            
            hand_card.Card = card;
            hand_card.Index = m_handCards.Count;
            hand_card.SetTransformSiblingIndex(hand_card.Index);
            hand_card.HandTransform = handTransform;
            hand_card.HandCardArea = this;
            hand_card.Table = table;
            hand_card.SetActive(true);
            
            m_handCards.Add(hand_card);
            m_handCardsMap[card] = hand_card;
        }

        private HandCard GetHandCardInstance()
        {
            return (m_cardsPool.Count > 0) 
                ? m_cardsPool.Pop() 
                : Instantiate(cardPrefab, m_cardsParent);
        }
        
        private void OnCardGameTableEnemiesTurn(CardGameTable table)
        {
            for (int i = 0, i_max = m_handCards.Count; i < i_max; i++)
            {
                var hand_card = m_handCards[i];
                m_cardsPool.Push(hand_card);
                hand_card.SetActive(false);
            }
            
            m_handCards.Clear();
            m_handCardsMap.Clear();
        }

        void Update()
        {
            float delta_time = Time.deltaTime;

            int cards_count = m_handCards.Count;
            int selected_index = HandCard.SelectedIndex;

            for (int i = 0; i < cards_count; i++)
            {
                var card = m_handCards[i];

                float card_size = (i - (cards_count - 1) / 2f);
                float deck_x = (card_size * cardSpacing);

                if (selected_index != -1 && i != selected_index)
                {
                    var sel_card = m_handCards[selected_index];

                    float width_dif = (sel_card.Width - card.Width);
                    float deck_x_offset = (width_dif + cardSpacing * 0.5f + Mathf.Abs(sel_card.DeckAngle));

                    float card_deck_x = card.DeckPosition.x;
                    float sel_card_deck_x = sel_card.DeckPosition.x;

                    float dist_x = Mathf.Abs(sel_card_deck_x - card_deck_x);
                    deck_x_offset *= (width_dif / dist_x);

                    if (card_deck_x < sel_card_deck_x)
                    {
                        deck_x -= deck_x_offset;
                    }
                    else
                    {
                        deck_x += deck_x_offset;
                    }
                }

                float deck_y = (card_size * card_size * -cardOffsetY);

                card.DeckPosition = new(deck_x, deck_y);
                card.DeckAngle = (card_size * -cardAngle);

                card.OnUpdate(delta_time);
            }
        }

        public void SortCards()
        {
            m_handCards.Sort((a, b) => a.WorldPositionX.CompareTo(b.WorldPositionX));

            for (int i = 0, i_max = m_handCards.Count; i < i_max; i++)
            {
                var card = m_handCards[i];
                card.Index = i;
                card.SetTransformSiblingIndex(i);
            }
        }

        public void ToForeground(HandCard handCard)
        {
            handCard.SetTransformSiblingIndex(m_handCards.Count - 1);
        }
    };
}
