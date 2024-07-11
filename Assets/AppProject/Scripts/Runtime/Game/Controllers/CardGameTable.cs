using System.Collections.Generic;
using UnityEngine;
using ECSCore;
using ECSGame;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game
{
    [DefaultExecutionOrder(1), DisallowMultipleComponent]
    public sealed class CardGameTable : MonoBehaviour
    {
        [Space]
        public HandSightPointer handSightPointer;
        
        [Space]
        public EntityActor playerActor;
        public List<EntityActor> enemyActors;
        
        [Space]
        public int startupCardsCount = 5;
        public int startupEnergyCount = 3;
        public Card[] allAvailableCards;
        
        public List<IEntity> AllPlayers { get; } = new();
        public IEntity CurrentPlayer { get; private set; }
        public List<IEntity> CurrentOpponents { get; } = new();

        public List<Card> DrawDeck { get; set; } = new();
        public List<Card> HandDeck { get; set; } = new();
        public List<Card> DiscardDeck { get; set; } = new();
        
        public bool IsMoreOneTarget => (CurrentOpponents.Count > 1);
        
        public int CurrentPlayerEnergyCount { get; private set; }

        public void Init()
        {
            InitCardsEffects();
            InitDeckCards();
            DrawCardsInHand();
            
            InitActors();
            InitPlayers();
        }

        public void PostInit()
        {
            EventBus.Trigger(EventHooks.k_OnCardGameTablePlayerTurn, this);
            EventBus.Trigger(EventHooks.k_OnCardGamePlayerEnergyUpdated, CurrentPlayerEnergyCount);
        }
        
        private void InitCardsEffects()
        {
            for (int i = 0, i_max = allAvailableCards.Length; i < i_max; i++)
            {
                var card = allAvailableCards[i];
                var card_effects = card.effects;
                
                for (int j = 0, j_max = card_effects.Length; j < j_max; j++)
                {
                    var effect = card_effects[j];
                    effect.Table = this;
                }
            }
        }
        
        private void InitActors()
        {
            playerActor.InitEntity();
            enemyActors.ForEach(actor => actor.InitEntity());
        }

        private void InitPlayers()
        {
            CurrentPlayerEnergyCount = startupEnergyCount;
            
            CurrentPlayer = playerActor.Entity;
            AllPlayers.Add(CurrentPlayer);
            
            for (int i = 0, i_max = enemyActors.Count; i < i_max; i++)
            {
                var entity = enemyActors[i].Entity;
                CurrentOpponents.Add(entity);
                AllPlayers.Add(entity);
            }
        }

        private void InitDeckCards()
        {
            DrawDeck.AddRange(allAvailableCards);
        }
        
        public void NextTurn()
        {
            CurrentPlayerEnergyCount = 0;

            var prev_player = CurrentPlayer;
            
            int current_player_index = AllPlayers.IndexOf(CurrentPlayer);
            int next_player_index = (++current_player_index % AllPlayers.Count);
            CurrentPlayer = AllPlayers[next_player_index];

            CurrentOpponents.Clear();
            
            if (CurrentPlayer.Has<PlayerMarker>())
            {
                CurrentPlayerEnergyCount = startupEnergyCount;
                
                CurrentOpponents.AddRange(AllPlayers);
                CurrentOpponents.RemoveAt(next_player_index);

                DrawCardsInHand();
                
                EventBus.Trigger(EventHooks.k_OnCardGameTablePlayerTurn, this);
            }
            else if (CurrentPlayer.Has<EnemyMarker>())
            {
                var real_player = AllPlayers.Find(player => player.Has<PlayerMarker>());
                CurrentOpponents.Add(real_player);
            }
            
            if (prev_player.Has<PlayerMarker>())
            {
                DiscardHandCards();
                EventBus.Trigger(EventHooks.k_OnCardGameTableEnemiesTurn, this);
            }
            
            EventBus.Trigger(EventHooks.k_OnCardGamePlayerEnergyUpdated, CurrentPlayerEnergyCount);
        }

        private void DiscardHandCards()
        {
            DiscardDeck.AddRange(HandDeck);
            HandDeck.Clear();

            if (DrawDeck.Count >= startupCardsCount) return;
            DrawDeck.AddRange(DiscardDeck);
            DiscardDeck.Clear();
        }
        
        private void DrawCardsInHand()
        {
            for (int i = 0; i < startupCardsCount; i++)
            {
                DrawRandomCard();
            }
        }
        
        private void DrawRandomCard()
        {
            int random_index = Random.Range(0, DrawDeck.Count);
            var random_card = DrawDeck[random_index];
            HandDeck.Add(random_card);
            DrawDeck.Remove(random_card);
        }
        
        public void TryPlayCard(Card card)
        {
            if (card.cost > CurrentPlayerEnergyCount)
            {
                EventBus.Trigger(EventHooks.k_OnCardGameTableOutOfEnergyMessage);
                return;
            }
            
            if (card.IsRequireTarget() && IsMoreOneTarget)
            {
                if (!handSightPointer.IsHaveHit)
                {
                    return;
                }
            }
            
            var card_effects = card.effects;

            for (int i = 0, i_max = card_effects.Length; i < i_max; i++)
            {
                ApplyEffect(card_effects[i]);
            }
        
            HandDeck.Remove(card);
            DiscardDeck.Add(card);
            EventBus.Trigger(EventHooks.k_OnCardGameTableCardDiscarded, (this, card));

            CurrentPlayerEnergyCount -= card.cost;
            EventBus.Trigger(EventHooks.k_OnCardGamePlayerEnergyUpdated, CurrentPlayerEnergyCount);
            
            return;
        }

        public void ApplyEffect(CardEffect effect)
        {
            effect.Apply(CurrentPlayer, CurrentOpponents);
        }
        
        public void OnPlayerDead(IEntity entity)
        {
            AllPlayers.Remove(entity);
            CurrentOpponents.Remove(entity);
        }
    };
    
#if UNITY_EDITOR
    [CustomEditor(typeof(CardGameTable)), CanEditMultipleObjects]
    public sealed class CardGameTableEditor : Editor
    {
        public new CardGameTable target;
        
        void OnEnable()
        {
            target = (CardGameTable)base.target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10f);
            GUI.enabled = Application.isPlaying;

            if (GUILayout.Button("Next Turn"))
            {
                target.NextTurn();
            }
            
            GUILayout.Space(5f);
            
            if (GUILayout.Button("Attack_400hp"))
            {
                PlayAttack(400f);
            }
            
            if (GUILayout.Button("Defence_300hp"))
            {
                PlayDefence(300f);
            }
            
            if (GUILayout.Button("Heal_400hp"))
            {
                PlayHeal(400f);
            }
            
            GUI.enabled = true;
        }
        
        private void PlayAttack(float damage)
        {
            var attack_event = target.CurrentPlayer.Trigger<CharacterAttackEvent>();
            attack_event.value = damage;
            var attack_targets = attack_event.targets;
            attack_targets.Clear();
            attack_targets.AddRange(target.CurrentOpponents);
        }
        
        private void PlayDefence(float value)
        {
            var defence_event = target.CurrentPlayer.Trigger<CharacterDefenceEvent>();
            defence_event.value = value;
        }
        
        private void PlayHeal(float value)
        {
            var heal_event = target.CurrentPlayer.Trigger<CharacterHealEvent>();
            heal_event.value = value;
        }
    };
#endif
}
