using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpBoost : MonoBehaviour
{
    public float jumpAmount;
    public Sprite s1;
    Sprite s2;
    SpriteRenderer sr;
    public LayerMask mask;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        s2 = sr.sprite;
    }


    void Update()
    {
        Collider2D[] c = Physics2D.OverlapBoxAll(transform.position + (Vector3)GetComponent<BoxCollider2D>().offset, GetComponent<BoxCollider2D>().size, 0, mask);
        if (c.Length > 0)
        {
            Rigidbody2D rb;
            foreach (Collider2D c2 in c)
            {
                if (c2.gameObject.TryGetComponent<Rigidbody2D>(out rb))
                {
                    if (rb.velocity.y >= 0)
                    {
                        rb.velocity = new Vector2(rb.velocity.x, jumpAmount);
                        StopAllCoroutines();
                        StartCoroutine(wait());
                    }
                }
            }
        }
    }

    IEnumerator wait()
    {
        sr.sprite = s1;
        yield return new WaitForSeconds(1);
        sr.sprite = s2;
    }
}
