using System.Collections.Generic;
using UnityEngine;
using ECSCore;

namespace Game
{
    // [CreateAssetMenu(fileName = "CardEffect", menuName = "CardGame/CardEffect", order = 51)]
    public abstract class CardEffect : ScriptableObject
    {
        public bool isRequireTarget;
        
        public abstract void Apply(IEntity owner, List<IEntity> opponents);
    };
}
