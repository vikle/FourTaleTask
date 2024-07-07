using System.Collections.Generic;
using ECSCore;
using UnityEngine;

namespace Game
{
    public sealed class CardGameTable : MonoBehaviour
    {
        public EntityActor playerActor;
        public List<EntityActor> enemyActors;
        
        public IEntity CurrentPlayer { get; private set; }
        public List<IEntity> CurrentOpponents { get; } = new();
        
        
        
    };
}
