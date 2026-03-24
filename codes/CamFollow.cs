using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public float CamSpeed;
    public Transform target;
    public Vector2 dist;
    Vector2 mousePos;
    void Start()
    {
        
    }

    
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPos;
        if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            targetPos = target.position + new Vector3(0, 0, -10) + (Vector3)dist;
        }
        else
        {
            targetPos = target.position + (((Vector3)mousePos-transform.position).normalized*5) + new Vector3(0, 0, -10) + (Vector3)dist;
        }
        transform.position = Vector3.Lerp(transform.position ,targetPos , CamSpeed*Time.deltaTime);
    }
}
