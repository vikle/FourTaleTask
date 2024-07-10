using System;
using System.Collections.Generic;
using UnityEngine;
using ECSCore;

namespace Game
{
    [DefaultExecutionOrder(-10), DisallowMultipleComponent]
    public sealed class GameController : MonoBehaviour
    {
        // public static GameController Instance { get; private set; }

        public AIController aiController;
        public CardGameTable cardGameTable;

        public struct SubControllers
        {
            public AIController AI { get; set; }
            public CardGameTable Table { get; set; }
        }

        public static SubControllers Controllers => s_controllers;
        static SubControllers s_controllers;
        
        void Awake()
        {
            ref var c = ref s_controllers;
            
            c.AI = aiController;
            c.Table = cardGameTable;

            aiController.Init(this);
            cardGameTable.Init();
            
            EventBus.Register(EventHooks.k_OnGameControllerNextTurn, NextTurn);
            
        }

        void Start()
        {
            cardGameTable.PostInit();
        }

        public void NextTurn()
        {
            cardGameTable.NextTurn();
        }

        
        
    };
}
