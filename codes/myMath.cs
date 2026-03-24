using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myMath : MonoBehaviour
{
    public static Vector2 DirFromRot(float rot)
    {
        float x = Mathf.Cos(rot * Mathf.Deg2Rad);
        float y = Mathf.Sin(rot * Mathf.Deg2Rad);
        return new Vector2(x, y);
    }
}
