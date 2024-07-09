﻿using System.Collections.Generic;
using UnityEngine;
using ECSCore;
using ECSGame;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game
{
    [DefaultExecutionOrder(-1), DisallowMultipleComponent]
    public sealed class CardGameTable : MonoBehaviour
    {
        public static CardGameTable Instance { get; private set; }
        
        [Space]
        public HandSightPointer handSightPointer;
        [Space]
        public EntityActor playerActor;
        public List<EntityActor> enemyActors;
        
        [Space]
        public int startupCardsCount = 5;
        public int maxCardsCount = 10;
        public Card[] allAvailableCards;
        
        public List<IEntity> AllPlayers { get; } = new();
        public IEntity CurrentPlayer { get; private set; }
        public List<IEntity> CurrentOpponents { get; } = new();

        public List<Card> DrawDeck { get; } = new();
        public List<Card> HandDeck { get; } = new();
        public List<Card> DiscardDeck { get; } = new();
        
        public bool IsMoreOneTarget => (CurrentOpponents.Count > 1);
        
        void Awake()
        {
            Instance = this;
            
            InitActors();
            InitPlayers();

            InitDeckCards();
            InitHandCards();
        }

        private void InitActors()
        {
            playerActor.InitEntity();
            enemyActors.ForEach(actor => actor.InitEntity());
        }

        private void InitPlayers()
        {
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
        
        private void InitHandCards()
        {
            var draw_deck = DrawDeck;
            var hand_deck = HandDeck;
        
            for (int i = 0; i < startupCardsCount; i++)
            {
                int random_index = Random.Range(0, draw_deck.Count);
                var random_card = draw_deck[random_index];
                hand_deck.Add(random_card);
                draw_deck.Remove(random_card);
            }
        }
        
        public void NextTurn()
        {
            int current_player_index = AllPlayers.IndexOf(CurrentPlayer);
            int next_player_index = (++current_player_index % AllPlayers.Count);
            CurrentPlayer = AllPlayers[next_player_index];

            CurrentOpponents.Clear();
            
            if (CurrentPlayer.Has<PlayerMarker>())
            {
                CurrentOpponents.AddRange(AllPlayers);
                CurrentOpponents.RemoveAt(next_player_index);
            }
            else if (CurrentPlayer.Has<EnemyMarker>())
            {
                var real_player = AllPlayers.Find(player => player.Has<PlayerMarker>());
                CurrentOpponents.Add(real_player);
            }
        }
        
        public bool TryPlayCard(Card card)
        {
            if (card.IsRequireTarget() && IsMoreOneTarget)
            {
                if (!handSightPointer.IsHaveHit)
                {
                    return false;
                }
            }
            
            var card_effects = card.effects;

            for (int i = 0, i_max = card_effects.Length; i < i_max; i++)
            {
                card_effects[i].Apply(CurrentPlayer, CurrentOpponents);
            }
        
            HandDeck.Remove(card);
            return true;
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
