using UnityEngine;
using ECSCore;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ECSGame
{
    [DisallowMultipleComponent]
    public sealed class HealthComponent : EntityActorComponent
    {
        [Range(0f, 1000f)]public float maxHealth = 1000f;
        [Range(0f, 1000f)]public float currentHealth = 1000f;
    };

#if UNITY_EDITOR
    [CustomEditor(typeof(HealthComponent)), CanEditMultipleObjects]
    public sealed class HealthComponentEditor : Editor
    {
        public new HealthComponent target;
        IEntity m_targetEntity;
        
        void OnEnable()
        {
            target = (HealthComponent)base.target;
            m_targetEntity = target.actor.Entity;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10f);
            GUI.enabled = Application.isPlaying;

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Damage_40%"))
            {
                var dmg_evt = m_targetEntity.Trigger<DamageEvent>();
                dmg_evt.value = (target.maxHealth * 0.4f);
            }
            if (GUILayout.Button("Healing_40%"))
            {
                var heal_evt = m_targetEntity.Trigger<HealEvent>();
                heal_evt.value = (target.maxHealth * 0.4f);
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Buff_30%"))
            {
                var add_buff_evt = m_targetEntity.Trigger<AddDefenceBuffEvent>();
                add_buff_evt.initialValue = (target.maxHealth * 0.3f);
            }
            if (GUILayout.Button("DeBuff"))
            {
                m_targetEntity.Trigger<RemoveDefenceBuffEvent>();
                m_targetEntity.Trigger<DefenceBuffChangedEvent>();
            }
            EditorGUILayout.EndHorizontal();

            GUI.enabled = true;
        }
    };
#endif
}
