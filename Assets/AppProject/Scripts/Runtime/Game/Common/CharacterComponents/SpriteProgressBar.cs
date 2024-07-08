using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class SpriteProgressBar : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        [Range(0f, 1f)]public float percentage = 0.7f;

        bool m_inited;
        Material m_material;
        static readonly int sr_percentage = Shader.PropertyToID("_Percentage");

#if UNITY_EDITOR
        void OnValidate()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
            if (m_material == null) m_material = spriteRenderer.sharedMaterial;
            m_material.SetFloat(sr_percentage, percentage);
        }
#endif
        public void SetPercentage(float value)
        {
            InitMaterial();
            percentage = value;
            m_material.SetFloat(sr_percentage, value);
        }

        private void InitMaterial()
        {
            if (m_inited) return;
            m_inited = true;
            m_material = spriteRenderer.material;
        }
    };
}


