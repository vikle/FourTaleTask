using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public sealed class HandCardUI : MonoBehaviour
    {
        public HandCard handCard;
        public Image background;

        public void Init()
        {
            background.sprite = handCard.Card.background;
        }
    };
}
