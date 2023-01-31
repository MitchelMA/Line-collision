using UnityEngine;
using Utils;

public struct LinePart
{
    public PqrForm Formula;
    public bool IsLinePart;
    public (Vector2 a, Vector2 b)? LineBounds { get; private set; }

    public static LinePart AsLinePart(Vector2 a, Vector2 b) =>
        new LinePart
        {
            Formula = new PqrForm(a, b),
            IsLinePart = true,
            LineBounds = (a, b)
        };

    public static LinePart AsLine(PqrForm formula) =>
        new LinePart
        {
            IsLinePart = false,
            Formula = formula,
        };
}