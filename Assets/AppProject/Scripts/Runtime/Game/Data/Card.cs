using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
                var effect_target = effects[i].target;
                if (effect_target == ECardEffectTarget.SelectedOpponent) return true;
            }

            return false;
        }
    };

#if UNITY_EDITOR
    [CustomEditor(typeof(Card)), CanEditMultipleObjects]
    public sealed class CardEditor : Editor
    {
        public new Card target;

        void OnEnable()
        {
            target = (Card)base.target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (target.background != null)
            {
                DrawTexture(target.background.texture);
            }
        }

        private static void DrawTexture(Texture texture)
        {
            var tex_rect = EditorGUILayout.GetControlRect();

            tex_rect.x += ((tex_rect.width / 2f) - 128f);
            tex_rect.y += 24f;

            tex_rect.width = 240f;
            tex_rect.height = 320f;

            GUI.DrawTexture(tex_rect, texture, ScaleMode.ScaleToFit);
        }
    };
#endif
}
