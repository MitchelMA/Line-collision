using UnityEngine;
using Utils;

public struct AssociativeLineData
{
    public LinePart LinePart { get; }
    public Vector2 Normal { get; private set; }
    public Vector2 Along { get; private set; }
    public LineRenderer LineRenderer { get; private set; }

    public AssociativeLineData(LinePart linePart) : this()
    {
        LinePart = linePart;
        Update();
    }

    public AssociativeLineData(LinePart linePart, LineRenderer lineRenderer) : this()
    {
        LinePart = linePart;
        LineRenderer = lineRenderer;
        Update();
    }

    public void Update()
    {
        if (LinePart.IsLinePart)
        {
            UpdateLineAsPart((Vector2)LinePart.LineBounds?.a, (Vector2)LinePart.LineBounds?.b);
        }
        else
        {
            UpdateLineAsInfinite(LinePart.Formula);
        }
        
    }

    private void UpdateLineAsPart(Vector2 a, Vector2 b)
    {
        Along = (b - a).normalized;
        Normal = Vector3Extensions.RotatedBy(Along, 90 * Mathf.Deg2Rad, 'z');
    }

    private void UpdateLineAsInfinite(PqrForm form)
    {
        if (float.IsInfinity(form.Slope))
        {
            Normal = Vector2.right;
            Along = Vector2.up;
            return;
        }

        Vector2 a = new Vector2(-1, form.GetY(-1));
        Vector2 b = new Vector2(1, form.GetY(1));
        UpdateLineAsPart(a, b);
    }
}