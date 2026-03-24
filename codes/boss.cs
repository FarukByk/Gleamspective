using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class boss : MonoBehaviour
{
    public Transform hand1, hand2;
    public Transform arm1,arm2;
    public Transform target;
    public List<ParticleSystem> list;
    public int health = 5;
    int sa;
    void Start()
    {
        StartCoroutine(wait());
        GetParticleObjects(transform);
        StartCoroutine(randomGenerate());


    }
    public void RandomGenerate()
    {
        health--;
        GetComponent<AudioSource>().Play();
        GetComponent<Animator>().SetTrigger("damage");
        if (health < 0)
        {
            target.GetComponent<charCont>().next();
        }
        StartCoroutine(randomGenerate());
    }
    
    void Update()
    {
        setPos(arm1.GetChild(0), Vector2.Distance(hand1.position, arm1.position) / 4);
        setPos(arm2.GetChild(0), Vector2.Distance(hand2.position, arm2.position) / 4);
        lookAt(arm1, hand1);
        lookAt(arm2, hand2);



        if (sa == -1)
        {
            hand1.parent.position = Vector3.Lerp(hand1.parent.position, target.position, Time.deltaTime * 3);
            hand2.parent.position = Vector3.Lerp(hand2.parent.position, new Vector3(10, 1, 0), Time.deltaTime * 3);
        }
        else if (sa == 1)
        {
            hand2.parent.position = Vector3.Lerp(hand2.parent.position, target.position, Time.deltaTime * 3);
            hand1.parent.position = Vector3.Lerp(hand1.parent.position, new Vector3(-10, 1, 0), Time.deltaTime * 3);
        }
        else if (sa == 0)
        {
            hand1.parent.position = Vector3.Lerp(hand1.parent.position, new Vector3(-10, 1, 0), Time.deltaTime * 3);
            hand2.parent.position = Vector3.Lerp(hand2.parent.position, new Vector3(10, 1, 0), Time.deltaTime * 3);
        }
    }

    void setPos(Transform a , float dist)
    {
        a.localPosition = new Vector3(dist, 0, 0);
        if (a.childCount != 0)
        {
            setPos(a.GetChild(0), dist);
        }
    }
    void lookAt(Transform a, Transform b)
    {
        float rot = Mathf.Atan2((b.position.y-a.position.y),(b.position.x-a.position.x)) * Mathf.Rad2Deg;
        a.transform.rotation = Quaternion.Euler(0,0,rot);   
    }



    void GetParticleObjects(Transform transform)
    {
        foreach (Transform t in transform)
        {
            ParticleSystem ps;
            if (t.TryGetComponent<ParticleSystem>(out ps))
            {
                list.Add(ps);
            }
            if (t.childCount != 0)
            {
                GetParticleObjects(t);
            }
        }
    }


    IEnumerator wait()
    {
        yield return new WaitForSeconds(5);
        sa = 1;
        yield return new WaitForSeconds(3);
        hand2.parent.GetComponent<Animator>().SetTrigger("hit");
        sa = 2;
        yield return new WaitForSeconds(1);
        sa = 0;
        yield return new WaitForSeconds(2);
        sa = -1;
        yield return new WaitForSeconds(3);
        hand1.parent.GetComponent<Animator>().SetTrigger("hit");
        sa = 2;
        yield return new WaitForSeconds(1);
        sa = 0;
        StartCoroutine(wait());
    }

    IEnumerator randomGenerate()
    {
        yield return new WaitForSeconds(3);
        int rand = Random.Range(0, list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            if (rand == i)
            {
                list[i].AddComponent<bossBody>().boss = this;
                list[i].GetComponent<CircleCollider2D>().enabled = true;
            }
            else
            {
                list[i].GetComponent<CircleCollider2D>().enabled = false;
            }
        }
    }
}
