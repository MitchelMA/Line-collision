using UnityEngine;
using Utils;

public class AssociativeLineData
{

    public LineInfo LineInfo { get; private set; }

    public Vector2 Normal => Vector2.Perpendicular(Along);

    public Vector2 Along
    {
        get
        {
            if (LineInfo.IsSegment)
            {
                var (a, b) = LineInfo.LineBounds.Value;
                return (b - a).normalized;
            }

            if (float.IsInfinity(LineInfo.Formula.Slope))
                return Vector2.up;

            Vector2 c = new Vector2(-1, LineInfo.Formula.GetY(-1));
            Vector2 d = new Vector2(1, LineInfo.Formula.GetY(1));
            return (d - c).normalized;
        }
    }

    /// <summary>
    /// Can be null!
    /// </summary>
    public LineRenderer LineRenderer { get; private set; }

    public AssociativeLineData(LineInfo lineInfo)
    {
        LineInfo = lineInfo;
    }

    public AssociativeLineData(LineInfo lineInfo, LineRenderer lineRenderer)
    {
        LineInfo = lineInfo;
        LineRenderer = lineRenderer;
    }
}