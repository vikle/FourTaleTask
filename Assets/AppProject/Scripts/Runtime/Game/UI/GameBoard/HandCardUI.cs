using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.UI
{
    public sealed class HandCardUI : MonoBehaviour
    {
        public HandCard handCard;
        public Image background;
        public TMP_Text description;

        static readonly StringBuilder sr_textBuilder = new(128);
        
        public void Init()
        {
            var card = handCard.Card;
            background.sprite = card.background;

            var tb = sr_textBuilder;
            tb.Clear();

            var effects = card.effects;

            for (int i = 0, i_max = effects.Length; i < i_max; i++)
            {
                var effect = effects[i];
                if (i > 0) tb.AppendLine();
                tb.Append(effect.ToString());
            }
            
            description.text = tb.ToString();
        }
    };
}
