using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Arrow : MonoBehaviour
{
    private MeshFilter _mf;
    private MeshRenderer _mr;
    private Mesh _mesh;

    private Vector3[] _vertices;
    private int[] _triangles;

    [SerializeField] private Vector2 arrowheadDimensions = new(0.4f, 0.4f);
    [SerializeField] private float arrowWidth = 0.2f;
    [SerializeField] private float baseLength = 2f;

    public Mesh Mesh
    {
        get => _mesh;
        private set
        {
            _mesh = value;

            if (_mf == null)
                _mf = GetComponent<MeshFilter>();

            _mf.mesh = _mesh;
        }
    }

    public float BaseLength
    {
        get => baseLength;
        set => baseLength = value;
    }

    // Start is called before the first frame update
    private void Start()
    {
        _mr = GetComponent<MeshRenderer>();
        Mesh = new Mesh();
        CreateShape();
        UpdateMesh();
    }

    // Update is called once per frame
    private void Update()
    {
        CreateShape();
        UpdateMesh();
    }

    private void CreateShape()
    {
        _vertices = new[]
        {
            new Vector3(0, arrowWidth, 0),
            new Vector3(baseLength, arrowWidth, 0),
            new Vector3(baseLength, arrowheadDimensions.y, 0),

            new Vector3(baseLength + arrowheadDimensions.x, 0, 0),

            new Vector3(baseLength, -arrowheadDimensions.y, 0),
            new Vector3(baseLength, -arrowWidth, 0),
            new Vector3(0, -arrowWidth, 0),
        };

        _triangles = new[]
        {
            0, 1, 6,
            1, 5, 6,
            2, 3, 4
        };
    }

    private void UpdateMesh()
    {
        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
    }
}