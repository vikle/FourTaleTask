using UnityEngine;

namespace Game
{
    public sealed class HandSightPointer : MonoBehaviour
    {
        public Camera m_camera;
        public LayerMask layerMask;
    
        public bool IsHaveHit { get; private set; }
        public GameObject HitObject { get; private set; }
    
        bool m_active;
        int m_hitCollider;
        int m_prevHitCollider;
    
        public void SetActive(bool value)
        {
            m_active = value;
            SetHit(false, null);
        }
    
        void FixedUpdate()
        {
            if (m_active == false) return;
        
            var ray = m_camera.ScreenPointToRay(Input.mousePosition);

            bool is_hit = Physics.Raycast(ray, out var hit, float.PositiveInfinity, layerMask);
        
            m_hitCollider = hit.colliderInstanceID;

            if (m_prevHitCollider != m_hitCollider)
            {
                UpdateHit(in hit, is_hit);
            }
        }

        private void UpdateHit(in RaycastHit hit, bool isHit)
        {
            m_prevHitCollider = m_hitCollider;
            var hit_obj = isHit ? hit.collider.gameObject : null;
            SetHit(isHit, hit_obj);
        }

        private void SetHit(bool isHit, GameObject hitObj)
        {
            IsHaveHit = isHit;
            HitObject = hitObj;
        
            EventBus.Trigger(EventHooks.k_OnHandSightPointerHitUpdated, (isHit, hitObj));
        }
    };
}
