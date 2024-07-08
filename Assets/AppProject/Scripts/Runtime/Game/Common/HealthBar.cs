using UnityEngine;

namespace Game
{
    public sealed class HealthBar : MonoBehaviour
    {
        [SerializeField] SpriteProgressBar m_healthBar;
        [SerializeField] SpriteProgressBar m_defenceBar;

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
        
        public void SetHealthBarPercentage(float value)
        {
            m_healthBar.SetPercentage(value);
        }
        
        public void SetDefenceBarPercentage(float value)
        {
            m_defenceBar.SetPercentage(value);
        }
    };
}
