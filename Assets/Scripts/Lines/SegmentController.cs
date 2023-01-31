using System;
using UnityEngine;

public class SegmentController : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    private AssociativeLineData _associativeLineData;

    private void Awake()
    {
        _associativeLineData = LineData.Instance.Add(pointA.position, pointB.position, true);
    }

    private void Update()
    {
        _associativeLineData.LineInfo.LineBounds =
            (pointA.position, pointB.position);
    }
}