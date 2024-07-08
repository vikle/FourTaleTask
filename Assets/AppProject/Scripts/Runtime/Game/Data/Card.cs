using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "NewCard", menuName = "CardGame/Card", order = 51)]
    public sealed class Card : ScriptableObject
    {
        [Header("Settings")]
        public int cost = 1;
        public CardEffect[] effects;

        [Header("UI")]
        public Sprite background;
    
        public bool IsRequireTarget()
        {
            for (int i = 0, i_max = effects.Length; i < i_max; i++)
            {
                if (effects[i].isRequireTarget) return true;
            }
        
            return false;
        }
    };
}
