using UnityEngine;
using Utils;

public class LineInfo
{
    private (Vector2 a, Vector2 b)? _linebounds;
    private PqrForm _formula;

    public PqrForm Formula
    {
        get => _formula;
        set
        {
            IsSegment = false;
            _formula = value;
        }
    }
    public bool IsSegment { get; private set; }

    public (Vector2 a, Vector2 b)? LineBounds
    {
        get => _linebounds;
        set
        {
            IsSegment = true;
            _linebounds = value;
        }
    }

    public static LineInfo AsLineSegment(Vector2 a, Vector2 b) =>
        new LineInfo
        {
            Formula = new PqrForm(a, b),
            LineBounds = (a, b)
        };

    public static LineInfo AsLine(PqrForm formula) =>
        new LineInfo
        {
            IsSegment = false,
            Formula = formula,
        };
}