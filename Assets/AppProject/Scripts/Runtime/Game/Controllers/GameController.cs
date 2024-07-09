using System.Collections.Generic;
using UnityEngine;
using ECSCore;

namespace Game
{
    [DefaultExecutionOrder(-10), DisallowMultipleComponent]
    public sealed class GameController : MonoBehaviour
    {
        // public static GameController Instance { get; private set; }

        public CardGameTable cardGameTable;
        public AIController aiController;
        
        public struct SubControllers
        {
            public CardGameTable Table { get; set; }
            public AIController AI { get; set; }
        }

        public static SubControllers Controllers => s_controllers;
        static SubControllers s_controllers;
        
        void Awake()
        {
            ref var c = ref s_controllers;
            c.Table = cardGameTable;
            c.AI = aiController;
            
            aiController.Init(this);
        }


        public void NextTurn()
        {
            cardGameTable.NextTurn();
        }
        
        
        
    };
}
