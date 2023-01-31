using System;
using UnityEngine;
using Utils;

public class FormController : MonoBehaviour 
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    private PqrForm _pointsFormula;
    private AssociativeLineData _associativeLineData;


    // Start is called before the first frame update
    private void Start()
    {
        _pointsFormula = new PqrForm(pointA.position, pointB.position);
        _associativeLineData = LineData.Instance.Add(_pointsFormula, true);
    }

    // Update is called once per frame
    private void Update()
    {
        _pointsFormula.WithPoints(pointA.position, pointB.position);
    }
}