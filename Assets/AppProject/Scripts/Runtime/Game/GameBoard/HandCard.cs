using UnityEngine;

namespace Game
{
    public sealed class HandCard : MonoBehaviour
    {
        [Space]
        public float moveSpeed = 20f;
        public float moveRotateSpeed = 30f;
        public float moveMaxRotate = 5f;
        public float focusYOffset = 120f;
        public float focusScale = 1.33f;
        public float sightModeYOffset = 120f;

        public Card Card { get; set; }
        public int Index { get; set; }
        public Vector2 DeckPosition { get; set; }
        public float DeckAngle { get; set; }
        public RectTransform HandTransform { get; set; }
        public HandArea HandCardArea { get; set; }

        public float Width { get; private set; }
        public float WorldPositionX { get; private set; }

        public static int SelectedIndex { get; set; }
        public static int DraggedIndex { get; set; }
        public static bool IsDragMode { get; set; }

        float m_focusTimer;
        bool m_isFocus;
        bool m_isDrag;
        bool m_isMouseEnter;
        bool m_isSightActive;
        bool m_isCardInGame;
        RectTransform m_transform;
        float m_startWidth;
        Vector2 m_localMousePos;
        Vector3 m_startScale;
        Vector3 m_currentRotate;
        Vector3 m_targetRotate;
        Vector3 m_prevPos;

        void Awake()
        {
            m_transform = (RectTransform)transform;
            m_startScale = m_transform.localScale;
            m_startWidth = m_transform.rect.width;
        }

        public void OnUpdate(float deltaTime)
        {
            if (m_focusTimer < 1f) m_focusTimer += deltaTime;

            var cur_pos = m_transform.position;
            WorldPositionX = cur_pos.x;

            bool not_focus_or_drag = true;
            var target_position = DeckPosition;

            if (IsFocus())
            {
                not_focus_or_drag = false;
                target_position.y = focusYOffset;
            }

            if (m_isDrag)
            {
                not_focus_or_drag = false;
                target_position = GetTargetPosition();

                if (m_isCardInGame) DrawSight();

                var dir = (cur_pos - m_prevPos);
                var add_rot = new Vector3(dir.y * 90f, -dir.x * 90f, 0f);

                m_targetRotate += ((add_rot * (deltaTime * moveRotateSpeed)));

                m_targetRotate.x = Mathf.Clamp(m_targetRotate.x, -moveMaxRotate, moveMaxRotate);
                m_targetRotate.y = Mathf.Clamp(m_targetRotate.y, -moveMaxRotate, moveMaxRotate);
                m_targetRotate.z = 0f;

                m_currentRotate = Vector3.Lerp(m_currentRotate, m_targetRotate, deltaTime * moveRotateSpeed);
            }
            else
            {
                m_targetRotate = new(0f, 0f, not_focus_or_drag ? DeckAngle : 0f);
                m_currentRotate = new(0f, 0f, not_focus_or_drag ? DeckAngle : 0f);
            }

            var target_size = not_focus_or_drag
                ? m_startScale
                : (m_startScale * focusScale);

            float move_step = (deltaTime * moveSpeed);
            float scale_step = (deltaTime * 4f);

            if (!not_focus_or_drag)
            {
                move_step *= 2f;
                scale_step *= 2f;
            }

            m_transform.anchoredPosition = Vector2.Lerp(m_transform.anchoredPosition, target_position, move_step);
            m_transform.localRotation = Quaternion.Slerp(m_transform.localRotation, Quaternion.Euler(m_currentRotate), move_step);
            m_transform.localScale = Vector3.Lerp(m_transform.localScale, target_size, scale_step);
            m_prevPos = Vector3.Lerp(m_prevPos, m_transform.position, deltaTime);

            float target_width = not_focus_or_drag
                ? m_startWidth
                : (m_startWidth * focusScale);

            Width = Mathf.Lerp(Width, target_width, scale_step);
        }

        public bool IsFocus()
        {
            return (m_isFocus && !m_isDrag && (m_focusTimer > 0f));
        }

        private Vector2 GetTargetPosition()
        {
            var rt = HandTransform;
            var world_pos = rt.position;
            var world_scale = rt.lossyScale;
            var mouse_pos = Input.mousePosition;
            Vector2 anchor_pos = (mouse_pos - world_pos);

            anchor_pos.x = (anchor_pos.x / world_scale.x);
            anchor_pos.y = (anchor_pos.y / world_scale.y);

            m_localMousePos = anchor_pos;

            bool is_card_in_game = !rt.rect.Contains(anchor_pos);
            m_isCardInGame = is_card_in_game;

            bool is_sight_active = (is_card_in_game && Card.IsRequireTarget() && CardGameTable.Instance.IsMoreOneTarget);
            
            if (is_sight_active)
            {
                anchor_pos = new(anchor_pos.x * 0.016f, sightModeYOffset);
            }
            
            SetSightActive(is_sight_active);

            return anchor_pos;
        }

        private void SetSightActive(bool value)
        {
            if (m_isSightActive == value) return;
            m_isSightActive = value;
            HandCardArea.handFingerLine.SetActive(value);
            CardGameTable.Instance.handSightPointer.SetActive(value);
        }

        private void DrawSight()
        {
            HandCardArea.handFingerLine.DrawLine(m_transform.anchoredPosition, m_localMousePos);
        }

        public void SetTransformSiblingIndex(int index)
        {
            m_transform.SetSiblingIndex(index);
        }

        public void _OnPointerEnter()
        {
            m_isMouseEnter = true;
            if (IsDragMode) return;
            Focus();
            Select();
        }

        public void _OnPointerExit()
        {
            m_isMouseEnter = false;
            if (IsDragMode) return;
            UnFocus();
            UnSelect();
        }

        public void _OnPointerDown()
        {
            if (IsDragMode) return;

            DraggedIndex = Index;
            IsDragMode = true;
            m_isDrag = true;
            Select();
        }

        public void _OnPointerUp()
        {
            if (IsDragMode && DraggedIndex != Index)
            {
                return;
            }

            IsDragMode = false;
            DraggedIndex = -1;
            m_isDrag = false;

            if (m_isCardInGame)
            {
                TryPlayCard();
            }

            UnSelect();
            if (!m_isMouseEnter) UnFocus();
        }

        private void Focus()
        {
            m_isFocus = true;
        }

        private void UnFocus()
        {
            m_isFocus = false;
            m_focusTimer = -0.2f;
        }

        private void Select()
        {
            SelectedIndex = Index;
            HandCardArea.ToForeground(this);
        }

        private void UnSelect()
        {
            SelectedIndex = -1;
            m_isCardInGame = false;

            HandCardArea.SortCards();

            if (m_isMouseEnter)
            {
                HandCardArea.ToForeground(this);
            }
        }

        private void TryPlayCard()
        {
            if (!CardGameTable.Instance.TryPlayCard(Card)) return;
            HandCardArea.DiscardCard(this);
            m_isMouseEnter = false;
            SetSightActive(false);
        }
    };
}
