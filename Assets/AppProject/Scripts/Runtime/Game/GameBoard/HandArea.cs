using System.Collections.Generic;
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

        List<HandCard> m_cards;

        void Awake()
        {
            HandCard.SelectedIndex = -1;
            handFingerLine.Init();
        }

        void Start()
        {
            var cards_parent = cardPrefab.transform.parent;

            var hand_deck = CardGameTable.Instance.HandDeck;

            m_cards = new(hand_deck.Count);

            for (int i = 0, i_max = hand_deck.Count; i < i_max; i++)
            {
                var card = hand_deck[i];

                var hand_card = (i == 0)
                    ? cardPrefab
                    : Instantiate(cardPrefab, cards_parent);

                hand_card.Card = card;
                hand_card.Index = i;
                hand_card.HandCardArea = this;
                hand_card.HandTransform = handTransform;
                hand_card.gameObject.SetActive(true);

                m_cards.Add(hand_card);
            }

            for (int i = 0, i_max = m_cards.Count; i < i_max; i++)
            {
                m_cards[i].GetComponent<UI.HandCardUI>().Init();
            }
        }

        void Update()
        {
            float delta_time = Time.deltaTime;

            int cards_count = m_cards.Count;
            int selected_index = HandCard.SelectedIndex;

            for (int i = 0; i < cards_count; i++)
            {
                var card = m_cards[i];

                float card_size = (i - (cards_count - 1) / 2f);
                float deck_x = (card_size * cardSpacing);

                if (selected_index != -1 && i != selected_index)
                {
                    var sel_card = m_cards[selected_index];

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
            m_cards.Sort((a, b) => a.WorldPositionX.CompareTo(b.WorldPositionX));

            for (int i = 0, i_max = m_cards.Count; i < i_max; i++)
            {
                var card = m_cards[i];
                card.Index = i;
                card.SetTransformSiblingIndex(i);
            }
        }

        public void ToForeground(HandCard card)
        {
            card.SetTransformSiblingIndex(m_cards.Count - 1);
        }
    };
}
