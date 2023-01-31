using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class ListenMouse : MonoBehaviour 
{
    [SerializeField] private GameObject[] points;
    // Start is called before the first frame update
    void Start()
    {
    }
    

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            // Debug.Log(mousePos2D);
            if (hit.collider == null)
                return;
            
            foreach (GameObject point in points)
            {
                if (hit.collider.gameObject == point)
                {
                    point.GetComponent<PickPoint>().Dragged = true;
                }
            }
        }

        if (!Input.GetMouseButtonUp(0)) return;
        
        foreach (GameObject point in points)
        {
            point.GetComponent<PickPoint>().Dragged = false;
        }
    }
}
