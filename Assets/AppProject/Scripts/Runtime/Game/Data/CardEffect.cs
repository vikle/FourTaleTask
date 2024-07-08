using System.Collections.Generic;
using UnityEngine;
using ECSCore;

namespace Game
{
    public enum ECardEffectTarget
    {
        None,
        Owner,
        SelectedOpponent, 
        RandomOpponent, 
        AllOpponents,
    };
    
    // [CreateAssetMenu(fileName = "CardEffect", menuName = "CardGame/CardEffect", order = 51)]
    public abstract class CardEffect : ScriptableObject
    {
        public ECardEffectTarget target;
        public float effectValue = 300f;
        
        public abstract void Apply(IEntity owner, List<IEntity> opponents);
    };
}
