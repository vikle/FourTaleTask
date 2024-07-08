using UnityEngine;
using ECSCore;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ECSGame
{
    [DisallowMultipleComponent]
    public sealed class AnimationComponent : EntityActorComponent
    {
        [Space]
        public Animator animator;

        [Space]
        public string idleName = "Idle";
        public string attackName = "Attack";
        public string defenceName = "Defence";
        public string healName = "Heal";
        public string deathName = "Death";
    };
    
#if UNITY_EDITOR
    [CustomEditor(typeof(AnimationComponent)), CanEditMultipleObjects]
    public sealed class AnimationComponentEditor : Editor
    {
        public new AnimationComponent target;
        IEntity m_targetEntity;
        
        void OnEnable()
        {
            target = (AnimationComponent)base.target;
            m_targetEntity = target.Actor.Entity;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10f);
            GUI.enabled = Application.isPlaying;

            if (GUILayout.Button("Idle"))
            {
                PlayAnim("Idle");
            }
            if (GUILayout.Button("Attack"))
            {
                PlayAnim("Attack");
            }
            if (GUILayout.Button("Defence"))
            {
                PlayAnim("Defence");
            }
            if (GUILayout.Button("Heal"))
            {
                PlayAnim("Heal");
            }
            if (GUILayout.Button("Death"))
            {
                PlayAnim("Death");
            }

            GUI.enabled = true;
        }

        private void PlayAnim(string animName)
        {
            var play_anim_evt = m_targetEntity.Trigger<PlayAnimationEvent>();
            m_targetEntity.TryGet(out AnimationDataComponent anim_data);

            play_anim_evt.stateNameHash = animName switch
            {
                "Idle" => anim_data.IdleHash, 
                "Attack" => anim_data.AttackHash, 
                "Defence" => anim_data.DefenceHash, 
                "Heal" => anim_data.HealHash, 
                "Death" => anim_data.DeathHash, 
                _=> 0
            };
        }
    };
#endif
}
