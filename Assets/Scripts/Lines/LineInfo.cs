using UnityEngine;
using Utils;

public class LineInfo
{
    private (Vector2 a, Vector2 b)? _lineBounds;
    private PqrForm _formula;

    public PqrForm Formula
    {
        get
        {
            if(IsSegment)
                _formula.WithPoints(LineBounds.Value.a, LineBounds.Value.b);
            
            return _formula;
        }
        set
        {
            IsSegment = false;
            _formula = value;
        }
    }

    public bool IsSegment { get; private set; }

    public (Vector2 a, Vector2 b)? LineBounds
    {
        get => _lineBounds;
        set
        {
            IsSegment = true;
            _lineBounds = value;
        }
    }

    public static LineInfo AsLineSegment(Vector2 a, Vector2 b) =>
        new LineInfo
        {
            LineBounds = (a, b)
        };

    public static LineInfo AsLine(PqrForm formula) =>
        new LineInfo
        {
            Formula = formula,
        };
}