using UnityEngine;
using ECSCore;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ECSGame
{
    [DisallowMultipleComponent]
    public sealed class CharacterComponent : EntityActorComponent
    {
        public float damageEventDelay = 0.5f;
        public float defenceEventDelay = 0.5f;
        public float healEventDelay = 0.5f;
    };
    
#if UNITY_EDITOR
    [CustomEditor(typeof(CharacterComponent)), CanEditMultipleObjects]
    public sealed class CharacterComponentEditor : Editor
    {
        public new CharacterComponent target;
        
        void OnEnable()
        {
            target = (CharacterComponent)base.target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10f);
            GUI.enabled = Application.isPlaying;
            
            
            
            GUI.enabled = true;
        }
    };
#endif
}
