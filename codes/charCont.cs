using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class charCont : MonoBehaviour
{
    public LayerMask groundMask;
    bool isDead;
    Rigidbody2D rb;
    public float speed;
    public bool isGrounded;
    public float jumpAmount;
    ParticleSystem jumpParticle;
    public light myLight;
    AudioSource audioSource;
    Animator Animator;
    void Start()
    {
        jumpParticle = transform.Find("jumpParticle").GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    float rot;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(waitDeath());
        }
        if (Physics2D.OverlapCircleAll(transform.position, 0.1f, groundMask).Length > 0)
        {
            if (!isGrounded)
            {
                jumpParticle.Play();
            }
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x,jumpAmount);
            audioSource.Play();
            jumpParticle.Play();
        }
        float x = Input.GetAxis("Horizontal");
        Animator.SetBool("walk", (x != 0 && isGrounded));
        rb.velocity= new Vector2(x*speed,rb.velocity.y);



        myLight.degree = 30;
        myLight.distance = 20;
        Vector2 dist = Camera.main.ScreenToWorldPoint(Input.mousePosition) - myLight.transform.position;
        rot = Mathf.Atan2(dist.y, dist.x) * Mathf.Rad2Deg;
        myLight.transform.rotation = Quaternion.Lerp(myLight.transform.rotation, Quaternion.Euler(0, 0, rot), Time.deltaTime * 10);
        if (Input.GetMouseButton(0))
        {
            myLight.degree = 30;
            myLight.distance = 20;

        }
        else if (Input.GetMouseButton(1))
        {

            myLight.degree = 4;
            myLight.distance = 60;
        }
        else
        {
            myLight.distance = 0;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Death") && !isDead)
        {
            StartCoroutine(waitDeath());
            isDead = true;
        }
        if ((collision.tag == "Next"))
        {
            StartCoroutine(waitNext());

        }
    }

    IEnumerator waitDeath()
    {
        GameObject.FindGameObjectWithTag("mc").GetComponent<Animator>().SetTrigger("fin");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void next()
    {
        StartCoroutine(waitNext());

    }
    IEnumerator waitNext()
    {
        GameObject.FindGameObjectWithTag("mc").GetComponent<Animator>().SetTrigger("fin");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
}
