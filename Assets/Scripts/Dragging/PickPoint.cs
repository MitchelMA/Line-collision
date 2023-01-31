using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PickPoint : MonoBehaviour
{
    private bool _dragged;

    public bool Dragged
    {
        get => _dragged;
        set => _dragged = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_dragged)
        {
            // get the mouse position as a 2d vector
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            transform.position = mousePos2D;
            float height = 2f * Camera.main.orthographicSize;
            float width = height * Camera.main.aspect;
            
            // Debug.Log($"height: {height}; width: {width}" );
        }
    }
    
}
