using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class SpriteProgressBar : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        [Range(0f, 1f)]public float percentage = 0.7f;

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
        void Awake()
        {
            m_material = spriteRenderer.material;
        }

        public void SetPercentage(float value)
        {
            percentage = value;
            m_material.SetFloat(sr_percentage, value);
        }
    };
}


