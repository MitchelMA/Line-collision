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

    private Vector2 _normal;
    private Vector2 _alongPerp;
    private Vector3 _dir;
    private Bounds _bounds;

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
        _normal = (transform.position - (Vector3)FormController.Instance.Intersect).normalized;
        _alongPerp = new Vector3(_normal.x, _normal.y, 0).RotatedBy(90 * Mathf.Deg2Rad, 'z');
        
        _tracker.SetLastPos();
        Movement();
        _tracker.CalcMovement();

        // set the values of the arrow
        Vector3 dir = _tracker.UDir;
        _rotation = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        arrow.transform.eulerAngles = _rotation;
        arrow.BaseLength = _tracker.Speed / 3;
        
        Vector3 normRot = new Vector3(0, 0, Mathf.Atan2(_normal.y, _normal.x) * Mathf.Rad2Deg);
        normalArrow.transform.eulerAngles = normRot;

        Vector3 alongRot = new Vector3(0, 0, Mathf.Atan2(_alongPerp.y, _alongPerp.x) * Mathf.Rad2Deg);
        AlongPerpArrow.transform.eulerAngles = alongRot;
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
        
        if(Vector3.Distance(nextPos, FormController.Instance.Intersect) <= transform.localScale.x /2)
            HandleLineCollision();

        transform.Translate(_dir * (speed * Time.deltaTime));
    }

    private void HandleLineCollision()
    {
        // vector resolution
        Vector2 normal = Vector3.Dot(_dir, _normal) * _normal;
        Vector2 perp = Vector3.Dot(_dir, _alongPerp) * _alongPerp;

        _dir = (-normal + perp).normalized;
    }
}