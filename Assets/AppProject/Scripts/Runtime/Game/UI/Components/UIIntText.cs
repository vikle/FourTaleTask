using TMPro;
using UnityEngine;

namespace Game.UI
{
    public sealed class UIIntText : MonoBehaviour
    {
        public TMP_Text text;

        public void Set(int value)
        {
            text.text = value.ToString();
        }
    };
}
