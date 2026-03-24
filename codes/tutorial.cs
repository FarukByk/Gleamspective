using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial : MonoBehaviour
{
    public LayerMask layer;
    public GameObject tut;
    void Update()
    {

        tut.SetActive(Physics2D.OverlapCircleAll(transform.position, .5f, layer).Length > 0);
    }
}
