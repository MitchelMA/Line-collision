using System;
using Unity.VisualScripting;
using UnityEngine;

public class Bounds : GenericSingleton<Bounds>
{
    [SerializeField] private float width = 5f;
    [SerializeField] private float height = 5f;
    [SerializeField] private Vector2 centre = Vector2.zero;
    
    [Header("LineRenders for sides")] [SerializeField]
    private LineRenderer top;

    [SerializeField] private LineRenderer right;
    [SerializeField] private LineRenderer bottom;
    [SerializeField] private LineRenderer left;

    public Vector4 OuterBounds =>
        new Vector4(
            height / 2f + centre.y,
            width / 2f + centre.x,
            -height / 2f + centre.y,
            -width / 2f + centre.x
        );

    public float Top => centre.y + height / 2;
    public float Right => centre.x + width / 2;
    public float Bottom => centre.y - height / 2;
    public float Left => centre.x - width / 2;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        // draw top-line
        Gizmos.DrawLine(new Vector3(OuterBounds.w, OuterBounds.x),
            new Vector3(OuterBounds.y, OuterBounds.x));

        // draw right-line
        Gizmos.DrawLine(new Vector3(OuterBounds.y, OuterBounds.x),
            new Vector3(OuterBounds.y, OuterBounds.z));

        // draw bottom-line
        Gizmos.DrawLine(new Vector3(OuterBounds.w, OuterBounds.z),
            new Vector3(OuterBounds.y, OuterBounds.z));

        // draw left-line
        Gizmos.DrawLine(new Vector3(OuterBounds.w, OuterBounds.x),
            new Vector3(OuterBounds.w, OuterBounds.z));
    }
#endif // UNITY_EDITOR

    private void FixedUpdate()
    {
       DrawOutsides(); 
    }

    private void DrawOutsides()
    {
        top.SetPosition(0, new Vector3(Left, Top));
        top.SetPosition(1, new Vector3(Right, Top));

        right.SetPosition(0, new Vector3(Right, Top));
        right.SetPosition(1, new Vector3(Right, Bottom));

        bottom.SetPosition(0, new Vector3(Left, Bottom));
        bottom.SetPosition(1, new Vector3(Right, Bottom));

        left.SetPosition(0, new Vector3(Left, Top));
        left.SetPosition(1, new Vector3(Left, Bottom));
    }
}