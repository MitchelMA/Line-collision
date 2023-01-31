using UnityEngine;
using Utils;

public class AssociativeLineData
{
    private LineInfo _lineInfo;

    public LineInfo LineInfo
    {
        set
        {
            _lineInfo = value;
            Update();
        }
        get
        {
            Update();
            return _lineInfo;
        }
    }

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

            Vector2 c = new Vector2(-1, _lineInfo.Formula.GetY(-1));
            Vector2 d = new Vector2(1, _lineInfo.Formula.GetY(1));
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

    private void Update()
    {
        if (!_lineInfo.IsSegment) return;

        _lineInfo.Formula.WithPoints((Vector2) _lineInfo.LineBounds?.a, (Vector2) _lineInfo.LineBounds?.b);
    }
}