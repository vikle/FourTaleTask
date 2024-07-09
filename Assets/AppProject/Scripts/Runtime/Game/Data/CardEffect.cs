using System.Text;
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
        
        [Header("UI")]
        [TextArea(5, 8)]
        public string description;
        
        public abstract void Apply(IEntity owner, List<IEntity> opponents);

        protected static readonly StringBuilder sr_stringBuilder = new(128);
        
        public override string ToString()
        {
            var sb = sr_stringBuilder;
            sb.Clear();
            
            sb.Append(description);
            sb.Append(' ');
            sb.Append('-');
            sb.Append(' ');
            sb.Append(effectValue);
            sb.Append('.');
            
            return sb.ToString();
        }
    };
}
