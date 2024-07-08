using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game.UI.Events
{
    public sealed class MouseEventsTrigger : MonoBehaviour
                                           , IPointerEnterHandler
                                           , IPointerExitHandler
                                           , IPointerDownHandler
                                           , IPointerUpHandler
    {
        [SerializeField]UnityEvent m_onPointerEnter;
        [SerializeField]UnityEvent m_onPointerExit;
        [SerializeField]UnityEvent m_onPointerDown;
        [SerializeField]UnityEvent m_onPointerUp;
        
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            m_onPointerEnter.Invoke();
        }
        
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            m_onPointerExit.Invoke();
        }
        
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                m_onPointerDown.Invoke();
            }
        }
        
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                m_onPointerUp.Invoke();
            }
        }
    };
}
