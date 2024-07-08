using UnityEngine;

namespace Game
{
    public sealed class HandFingerLine : MonoBehaviour
    {
        public UILineRenderer lineRenderer;
        public int bezierSegmentsCount = 15;
        public float middleYOffset = 333f;
        public Color normalColor = Color.grey;
        public Color attackColor = Color.grey + Color.red;

        Vector2[] m_points;
        Vector2[] m_bezierPoints;
        Vector2[] m_bezierSegments;

        public void Init()
        {
            m_points = new Vector2[3];
            m_bezierPoints = new Vector2[m_points.Length - 1];
            m_bezierSegments = new Vector2[bezierSegmentsCount];

            EventBus.Register<(bool, GameObject)>(EventHooks.k_OnHandSightPointerHitUpdated, OnHandSightPointerHitUpdated);

            SetActive(false);
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (lineRenderer != null)
            {
                lineRenderer.SetVerticesDirty();
            }
        }
#endif

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public void DrawLine(Vector2 start, Vector2 finish)
        {
            var points = m_points;

            points[0] = start;
            points[1] = new(start.x, finish.y + middleYOffset);
            points[^1] = finish;

            int i = 0;
            float bezier_step = (1f / bezierSegmentsCount);

            for (float t = 0f; t < 1f; t += bezier_step)
            {
                m_bezierSegments[i++] = GetBezierPoint(t);
            }

            m_bezierSegments[^1] = GetBezierPoint(1f);

            UpdateLine(m_bezierSegments);
        }

        public Vector2 GetBezierPoint(float t)
        {
            var points = m_points;
            var bezier_points = m_bezierPoints;
            int points_count = points.Length;

            for (int i = 0, j = 1; j < points_count; i++, j++)
            {
                var i_pos = points[i];
                var j_pos = points[j];
                bezier_points[i] = Vector2.Lerp(i_pos, j_pos, t);
            }

            for (int i = 0, j = 1; j < bezier_points.Length; i++, j++)
            {
                var i_pos = bezier_points[i];
                var j_pos = bezier_points[j];
                bezier_points[j] = Vector2.Lerp(i_pos, j_pos, t);
            }

            return bezier_points[^1];
        }

        public void UpdateLine(Vector2[] points)
        {
            lineRenderer.points = points;
            lineRenderer.SetVerticesDirty();
        }

        private void OnHandSightPointerHitUpdated((bool, GameObject) hitArgs)
        {
            UpdateHitState(hitArgs.Item1);
        }

        private void UpdateHitState(bool isHit)
        {
            lineRenderer.color = !isHit
                ? normalColor
                : attackColor;
        }
    };
}
