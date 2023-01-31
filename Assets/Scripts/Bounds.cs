using System;
using Unity.VisualScripting;
using UnityEngine;

public class Bounds : GenericSingleton<Bounds>
{
    [SerializeField] private float width = 5f;
    [SerializeField] private float height = 5f;
    [SerializeField] private Vector2 centre = Vector2.zero;
    
    private AssociativeLineData _topLine;
    private AssociativeLineData _rightLine;
    private AssociativeLineData _bottomLine;
    private AssociativeLineData _leftLine;

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

    private void Awake()
    {
        LineData lineData = LineData.Instance;
        _topLine = lineData.Add(new Vector2(Left, Top), new Vector2(Right, Top), true);
        _rightLine = lineData.Add(new Vector2(Right, Top), new Vector2(Right, Bottom), true);
        _bottomLine = lineData.Add(new Vector2(Left, Bottom), new Vector2(Right, Bottom), true);
        _leftLine = lineData.Add(new Vector2(Left, Top), new Vector2(Left, Bottom), true);
    }

    private void FixedUpdate()
    {
       // DrawOutsides(); 
       SetBoundLines();
    }

    private void SetBoundLines()
    {
        _topLine.LineInfo.LineBounds = (new Vector2(Left, Top), new Vector2(Right, Top));
        _rightLine.LineInfo.LineBounds = (new Vector2(Right, Top), new Vector2(Right, Bottom));
        _bottomLine.LineInfo.LineBounds = (new Vector2(Left, Bottom), new Vector2(Right, Bottom));
        _leftLine.LineInfo.LineBounds = (new Vector2(Left, Top), new Vector2(Left, Bottom));
        
    }
}