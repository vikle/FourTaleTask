using System;
using UnityEngine;
using UnityEngine.UI;

[HelpURL("https://github.com/Radishmouse22/UILineRenderer")]
[RequireComponent(typeof(CanvasRenderer))]
public sealed class UILineRenderer : Graphic
{
    public Sprite sprite;
    public Vector2[] points = Array.Empty<Vector2>();

    public float thickness = 10f;
    public bool center = true;
    
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (points.Length < 2)
            return;

        for (int i = 0; i < points.Length - 1; i++)
        {
            // Create a line segment between the next two points
            CreateLineSegment(points[i], points[i + 1], vh);

            int index = i * 5;

            // Add the line segment to the triangles array
            vh.AddTriangle(index, index + 1, index + 3);
            vh.AddTriangle(index + 3, index + 2, index);

            // These two triangles create the beveled edges
            // between line segments using the end point of
            // the last line segment and the start points of this one
            if (i != 0)
            {
                vh.AddTriangle(index, index - 1, index - 3);
                vh.AddTriangle(index + 1, index - 1, index - 2);
            }
        }
    }

    private void CreateLineSegment(Vector3 point1, Vector3 point2, VertexHelper vh)
    {
        Vector3 offset = center ? (rectTransform.sizeDelta / 2f) : Vector2.zero;

        // Create vertex template
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        // Create the start of the segment
        Quaternion point1Rotation = Quaternion.Euler(0f, 0f, RotatePointTowards(point1, point2) + 90f);
        vertex.position = point1Rotation * new Vector3(-thickness / 2f, 0f);
        vertex.position += point1 - offset;
        vh.AddVert(vertex);
        vertex.position = point1Rotation * new Vector3(thickness / 2f, 0f);
        vertex.position += point1 - offset;
        vh.AddVert(vertex);

        // Create the end of the segment
        Quaternion point2Rotation = Quaternion.Euler(0f, 0f, RotatePointTowards(point2, point1) - 90f);
        vertex.position = point2Rotation * new Vector3(-thickness / 2f, 0f);
        vertex.position += point2 - offset;
        vh.AddVert(vertex);
        vertex.position = point2Rotation * new Vector3(thickness / 2f, 0f);
        vertex.position += point2 - offset;
        vh.AddVert(vertex);

        // Also add the end point
        vertex.position = point2 - offset;
        vh.AddVert(vertex);
    }

    private float RotatePointTowards(Vector2 vertex, Vector2 target)
    {
        return (Mathf.Atan2(target.y - vertex.y, target.x - vertex.x) * (180f / Mathf.PI));
    }
};
