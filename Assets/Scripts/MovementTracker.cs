using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class MovementTracker : MonoBehaviour
{
    private Vector3 _dir;
    private Vector3 _oldPos;
    private Vector3 _pos;

    public Vector3 UDir => _dir.normalized;
    public Vector3 Dir => _dir;
    public float Speed => _dir.magnitude / Time.deltaTime;
    public Vector3 Movement => UDir * Speed;

    // Start is called before the first frame update
    private void Start()
    {
        var position = transform.position;

        _oldPos = position;
        _pos = position;
    }

    public void SetLastPos()
    {
        _oldPos = _pos;
    }

    public void CalcMovement()
    {
        _pos = transform.position;
        _dir = _pos - _oldPos;
    }
}