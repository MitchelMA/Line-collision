using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Utils;

public class LineData : GenericSingleton<LineData>
{
    private readonly HashSet<AssociativeLineData> _lineData = new();

    private void Awake()
    {
    }

    public AssociativeLineData Add(PqrForm formula)
    {
        LineInfo lineInfo = LineInfo.AsLine(formula);
        AssociativeLineData associativeData = new AssociativeLineData(lineInfo);
        _lineData.Add(associativeData);
        
        return associativeData;
    }

    public AssociativeLineData Add(Vector2 a, Vector2 b)
    {
        LineInfo lineInfo = LineInfo.AsLineSegment(a, b);
        AssociativeLineData associativeData = new AssociativeLineData(lineInfo);
        _lineData.Add(associativeData);

        return associativeData;
    }

    public (AssociativeLineData data, Vector2 refPoint)? GetClosest(Vector3 fromPosition)
    {
        AssociativeLineData[] arr = _lineData.ToArray();
        int l = arr.Length;
        if (l == 0)
            return null;

        (AssociativeLineData data, Vector2 point)? currentClosest = null;

        for (int i = 0; i < l; i++)
        {
            AssociativeLineData current = arr[i];
            if (current.LineInfo.IsSegment)
            {
                var (allowed, point) = CheckLinePart(current, fromPosition);
                if (!allowed)
                    continue;

                if (currentClosest is null)
                {
                    currentClosest = (current, point);
                    continue;
                }

                if (Vector2.Distance(fromPosition, point) <=
                    Vector2.Distance(fromPosition, (Vector2) currentClosest?.point))
                    currentClosest = (current, point);
            }
            else
            {
                Vector2 intersection = CheckLine(current, fromPosition);

                if (currentClosest is null)
                {
                    currentClosest = (current, intersection);
                    continue;
                }

                if (Vector2.Distance(fromPosition, intersection) <=
                    Vector2.Distance(fromPosition, (Vector2) currentClosest?.point))
                    currentClosest = (current, intersection);
            }
        }

        return currentClosest;
    }

    private static (bool allowed, Vector2 intersect) CheckLinePart(AssociativeLineData associativeLineData,
        Vector3 from)
    {
        if (associativeLineData.LineInfo.LineBounds is null)
            throw new NullReferenceException("LineBounds may not be null at this point");

        PqrForm formula = associativeLineData.LineInfo.Formula;
        PqrForm perpendicular = formula.Perpendicular(from);

        var (_, intersection) = PqrForm.Intersect(formula, perpendicular, 0.01d);
        if (intersection is null)
            throw new Exception(
                "This Exception shouldn't be possible since it's a intersection between perpendicular lines");

        var (a, b) = associativeLineData.LineInfo.LineBounds.Value;
        bool allowed = IsOnSegment(a, b, (Vector2) intersection);
        print($"{intersection} is on segment {a} -> {b}: {allowed}");

        return (allowed, (Vector2) intersection);
    }

    private static bool IsOnSegment(Vector2 a, Vector2 b, Vector2 point)
    {
        //https://stackoverflow.com/a/328122

        float crossProduct = (point.y - a.y) * (b.x - a.x) - (point.x - a.x) * (b.y - a.y);
        float dotProduct = (point.x - a.x) * (b.x - a.x) + (point.y - a.y) * (b.y - a.y);
        double squaredLength = Math.Pow(b.x - a.x, 2) * Math.Pow(b.y - a.y, 2);

        if (Math.Abs(crossProduct) > Math.E)
            return false;

        if (dotProduct < 0)
            return false;

        if (dotProduct > squaredLength)
            return false;

        return true;
    }

    private static Vector2 CheckLine(AssociativeLineData associativeLineData, Vector3 from)
    {
        PqrForm formula = associativeLineData.LineInfo.Formula;
        PqrForm perpendicular = formula.Perpendicular(from);

        var (_, intersection) = PqrForm.Intersect(formula, perpendicular, 0.01d);
        if (intersection is null)
            throw new Exception(
                "This Exception shouldn't be possible since it's a intersection between perpendicular lines");

        return (Vector2) intersection;
    }
}