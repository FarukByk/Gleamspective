using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorCode : MonoBehaviour
{
    public objects obj;
    public plate plate;
    public Transform targetPos;
    Vector2 basePos,tPos;
    public bool fixedDoor;
    bool sabit;
    void Start()
    {
        basePos = transform.position;
        tPos = targetPos.position;
    }

    
    void Update()
    {
        if (obj != null && plate == null)
        {
            
            if (fixedDoor)
            {
                if (obj.openning)
                {
                    sabit = true;
                }
                transform.position = Vector2.Lerp(transform.position, sabit ? tPos : basePos, Time.deltaTime * 5);
            }
            else
            {
                transform.position = Vector2.Lerp(transform.position, obj.openning ? tPos : basePos, Time.deltaTime * 5);
            }
        }
        if (plate != null && obj == null)
        {

            if (fixedDoor)
            {
                if (plate.opening)
                {
                    sabit = true;
                }
                transform.position = Vector2.Lerp(transform.position, sabit ? tPos : basePos, Time.deltaTime * 5);
            }
            else
            {
                transform.position = Vector2.Lerp(transform.position, plate.opening ? tPos : basePos, Time.deltaTime * 5);
            }
        }
        if (plate != null && obj != null)
        {

            if (fixedDoor)
            {
                if (plate.opening && obj.openning)
                {
                    sabit = true;
                }
                if (!sabit)
                {
                    transform.position = Vector2.Lerp(transform.position, plate.opening || obj.openning ? tPos : basePos, Time.deltaTime * 5);
                }
                else
                {
                    transform.position = Vector2.Lerp(transform.position, sabit ? tPos : basePos, Time.deltaTime * 5);
                }
                
            }
            else
            {
                transform.position = Vector2.Lerp(transform.position, plate.opening || obj.openning ? tPos : basePos, Time.deltaTime * 5);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine( basePos, tPos );
    }
}
