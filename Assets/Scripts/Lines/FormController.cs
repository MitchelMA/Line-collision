using System;
using UnityEngine;
using Utils;

public class FormController : GenericSingleton<FormController>
{
    [SerializeField] private LineRenderer pointsLr;
    [SerializeField] private LineRenderer perpLr;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Transform movingPoint;
    [SerializeField] private GameObject intersectPoint;

    private PqrForm _pointsFormula;
    private PqrForm _perpendicularForm;

    public PqrForm PointsFormula => _pointsFormula;
    public PqrForm Perpendicular => _perpendicularForm;
    public Vector2 Intersect => intersectPoint.transform.position;

    // Start is called before the first frame update
    private void Start()
    {
        _pointsFormula = new PqrForm(pointA.position, pointB.position);
        _perpendicularForm.PerpendicularToThrough(_pointsFormula, movingPoint.position);
    }

    // Update is called once per frame
    private void Update()
    {
        _pointsFormula.WithPoints(pointA.position, pointB.position);
        _perpendicularForm.PerpendicularToThrough(_pointsFormula, movingPoint.position);
        
        DrawLine(_pointsFormula, pointsLr);
        DrawLine(_perpendicularForm, perpLr);
        
        DrawIntersect(_pointsFormula, _perpendicularForm, intersectPoint);
    }

    private static void DrawLine(PqrForm form, LineRenderer lr)
    {
        if (form.Q == 0)
        {
            float x = form.GetX(0);
            lr.SetPosition(0, new Vector2(x, -20));
            lr.SetPosition(1, new Vector2(x, 20));
            return;
        }

        lr.SetPosition(0, new Vector2(-10, form.GetY(-10)));
        lr.SetPosition(1, new Vector2(10, form.GetY(10)));
    }

    private static void DrawIntersect(PqrForm a, PqrForm b, GameObject intersectVisual)
    {
        var (iType, intersect) = PqrForm.Intersect(a, b, 0.01d);
        
        if (!iType.HasFlag(IntersectionType.Once))
        {
            intersectVisual.SetActive(false);
            return;
        }

        intersectVisual.SetActive(true);
        intersectVisual.transform.position = (Vector2) intersect;
    }
}