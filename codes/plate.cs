using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plate : MonoBehaviour
{
    public LayerMask mask;
    public Sprite s2;
    Sprite s1;
    SpriteRenderer sr;
    public bool opening;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        s1 = sr.sprite;
    }
    private void Update()
    {
        opening = Physics2D.OverlapBoxAll(transform.position + (Vector3)GetComponent<BoxCollider2D>().offset, GetComponent<BoxCollider2D>().size, 0, mask).Length > 0;

        sr.sprite = opening ? s2 : s1;

    }
}
