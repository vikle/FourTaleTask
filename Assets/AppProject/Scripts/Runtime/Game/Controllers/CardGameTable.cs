using System.Collections.Generic;
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
        
        public bool AttackCardsIsRequireTarget => (CurrentOpponents.Count > 1);
        
        void Awake()
        {
            Instance = this;
            InitActors();
            InitPlayers();
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
        
        public void TryPlayCard(Card card)
        {
            var card_effects = card.effects;

            for (int i = 0, i_max = card_effects.Length; i < i_max; i++)
            {
                card_effects[i].Apply(CurrentPlayer, CurrentOpponents);
            }
        
            HandDeck.Remove(card);
        }

        public void OnPlayerDead(IEntity entity)
        {
            AllPlayers.Remove(entity);
            CurrentOpponents.Remove(entity);
        }
        
        public void PlayAttack(float damage)
        {
            var attack_event = CurrentPlayer.Trigger<CharacterAttackEvent>();
            attack_event.value = damage;
            var attack_targets = attack_event.targets;
            attack_targets.Clear();
            attack_targets.AddRange(CurrentOpponents);
        }
        
        public void PlayDefence(float value)
        {
            var defence_event = CurrentPlayer.Trigger<CharacterDefenceEvent>();
            defence_event.value = value;
        }
        
        public void PlayHeal(float value)
        {
            var heal_event = CurrentPlayer.Trigger<CharacterHealEvent>();
            heal_event.value = value;
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
                target.PlayAttack(400f);
            }
            
            if (GUILayout.Button("Defence_300hp"))
            {
                target.PlayDefence(300f);
            }
            
            if (GUILayout.Button("Heal_400hp"))
            {
                target.PlayHeal(400f);
            }
            
            GUI.enabled = true;
        }
    };
#endif
}
