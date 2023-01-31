using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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

        if (nextPos.x + transform.localScale.x / 2 > _bounds.Right ||
            nextPos.x - transform.localScale.x / 2 < _bounds.Left)
            _dir.x *= -1;

        if (nextPos.y + transform.localScale.y / 2 > _bounds.Top ||
            nextPos.y - transform.localScale.y / 2 < _bounds.Bottom)
            _dir.y *= -1;

        _closestLineInfo = LineData.Instance.GetClosest(transform.position);

        if (_closestLineInfo is not null &&
            Vector3.Distance(transform.position, _closestLineInfo.Value.point) <= transform.localScale.x / 2)
            HandleLineCollision();

        transform.Translate(_dir * (speed * Time.deltaTime));
    }

    private void HandleLineCollision()
    {
        if (_closestLineInfo is null)
            return;

        Vector2 normal = _closestLineInfo.Value.associativelineData.Normal;
        Vector2 along = _closestLineInfo.Value.associativelineData.Along;


        Vector2 refrNorm = Vector3.Dot(_dir, normal) * normal;
        Vector2 refrAlong = Vector3.Dot(_dir, along) * along;

        _dir = (-normal + refrAlong).normalized;
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
        arrow.BaseLength = _tracker.Speed / 3;

        if (_closestLineInfo is null) return;

        var normal = _closestLineInfo.Value.associativelineData.Normal;
        print(normal);
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