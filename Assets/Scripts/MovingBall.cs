using System;
using UnityEngine;
using Utils;

[RequireComponent(typeof(MovementTracker))]
public class MovingBall : MonoBehaviour
{
    private MovementTracker _tracker;
    private Vector3 _rotation = Vector3.zero;

    [SerializeField] private Arrow arrow;
    [SerializeField] private Vector2 startDir;
    [SerializeField] private float speed = 1f;
    [SerializeField] private GameObject normalArrow;
    [SerializeField] private GameObject AlongPerpArrow;
    [SerializeField] private GameObject intersectionPoint;
    [SerializeField] private LineRenderer perpendicular;

    private Vector3 _dir;
    private Bounds _bounds;
    private (AssociativeLineData associativelineData, Vector2 point)? _closestLineInfo;

    // Start is called before the first frame update
    private void Start()
    {
        _bounds = Bounds.Instance;
        startDir.Normalize();
        _tracker = GetComponent<MovementTracker>();
        _dir = startDir;
    }

    // Update is called once per frame
    private void Update()
    {
        // calculate the normal

        _tracker.SetLastPos();
        Movement();
        _tracker.CalcMovement();

        DrawIntersection();
        DrawPerpendicular();
        DrawVectors();
    }

    private void Movement()
    {
        Vector3 nextPos = transform.position + _dir * (speed * Time.deltaTime);

        _closestLineInfo = LineData.Instance.GetClosest(nextPos);

        if (_closestLineInfo is not null &&
            Vector3.Distance(nextPos, _closestLineInfo.Value.point) <= transform.localScale.x / 2)
            HandleLineCollision();

        transform.Translate(_dir * (speed * Time.deltaTime));
    }

    private void HandleLineCollision()
    {
        if (_closestLineInfo is null)
            return;

        Vector2 normal = _closestLineInfo.Value.associativelineData.Normal;
        Vector2 along = _closestLineInfo.Value.associativelineData.Along;


        Vector2 resNorm = Vector3.Dot(_dir, normal) * normal;
        Vector2 resAlong = Vector3.Dot(_dir, along) * along;

        _dir = (-resNorm + resAlong).normalized;
    }

    private void DrawIntersection()
    {
        if (_closestLineInfo is null)
        {
            intersectionPoint.SetActive(false);
            return;
        }

        intersectionPoint.SetActive(true);
        intersectionPoint.transform.position = _closestLineInfo.Value.point;
    }

    private void DrawVectors()
    {
        Vector3 dir = _tracker.UDir;
        _rotation = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        arrow.transform.eulerAngles = _rotation;
        // logarithmically lengthen the arrow with base e such that a speed of 3 equals a length of 1
        arrow.BaseLength = (float)Math.Log(_tracker.Speed * Math.E/3d);

        if (_closestLineInfo is null) return;

        var normal = _closestLineInfo.Value.associativelineData.Normal;
        var along = _closestLineInfo.Value.associativelineData.Along;

        Vector3 normAngles = new Vector3(0, 0, Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg);
        Vector3 alongAngles = new Vector3(0, 0, Mathf.Atan2(along.y, along.x) * Mathf.Rad2Deg);

        normalArrow.transform.eulerAngles = normAngles;
        AlongPerpArrow.transform.eulerAngles = alongAngles;
    }

    private void DrawPerpendicular()
    {
        if (_closestLineInfo is null)
            return;

        PqrForm perp = _closestLineInfo.Value.associativelineData.LineInfo.Formula.Perpendicular(transform.position);

        if (perp.Q == 0)
        {
            float x = perp.GetX(0);
            perpendicular.SetPosition(0, new Vector2(x, -20));
            perpendicular.SetPosition(1, new Vector2(x, 20));
            return;
        }

        perpendicular.SetPosition(0, new Vector2(-10, perp.GetY(-10)));
        perpendicular.SetPosition(1, new Vector2(10, perp.GetY(10)));
    }
}