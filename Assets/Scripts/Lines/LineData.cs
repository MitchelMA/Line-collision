using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

public class LineData : GenericSingleton<LineData>
{
    private readonly HashSet<AssociativeLineData> _lineData = new();

    public void Add(PqrForm formula)
    {
        LinePart linePart = LinePart.AsLine(formula);
        AssociativeLineData associativeData = new AssociativeLineData(linePart);
        _lineData.Add(associativeData);
    }

    public void Add(Vector2 a, Vector2 b)
    {
        LinePart linePart = LinePart.AsLinePart(a, b);
        AssociativeLineData associativeData = new AssociativeLineData(linePart);
        _lineData.Add(associativeData);
    }

    public (AssociativeLineData data, Vector2 refPoint) GetClosest(Vector3 fromPosition)
    {
        AssociativeLineData[] arr = _lineData.ToArray();
        (AssociativeLineData data, Vector2 point) currentClosest = (arr[0], Vector2.zero);

        int l = arr.Length;
        for (int i = 0; i < l; i++)
        {
            AssociativeLineData current = arr[i];
            if (current.LinePart.IsLinePart)
            {
                CheckLinePart(current, fromPosition);
            }
            else
            {
            }
        }

        return currentClosest;
    }

    private static (bool allowed, Vector2 intersect) CheckLinePart(AssociativeLineData associativeLineData,
        Vector3 from)
    {
        PqrForm formula = associativeLineData.LinePart.Formula;
        PqrForm perpendicular = formula.Perpendicular(from);

        var (_, intersection) = PqrForm.Intersect(formula, perpendicular, 0.01d);
        if (intersection is null)
            throw new Exception(
                "This Exception shouldn't be possible since it's a intersection between perpendicular lines");
        
        
    }
}