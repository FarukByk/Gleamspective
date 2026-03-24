
using UnityEngine;


public class objects : MonoBehaviour
{
    [Range(0f, 1f)]
    public float white;
    public bool colorChange = true;
    Color baseColor;
    public bool openning;
    Rigidbody2D rb;
    public float openSecond;
    public bool reverse;
    float a;
    SpriteRenderer sr;
    public bool emici;
    public bool destroy;
    AudioSource audioSource;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        baseColor = sr.color;
        rb.isKinematic = !reverse;
        rb.velocity = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        if (!emici)
        {
            if (colorChange)
            {
                if (audioSource != null)
                {
                    if (white != 0)
                    {
                        audioSource.pitch = 1 / white;
                    }
                    else
                    {
                        audioSource.pitch = 0;
                    }
                    audioSource.mute = white == 0;
                }
                if (reverse)
                {
                    sr.color = baseColor + ((Color.white - baseColor) * (1 - white));

                }
                else
                {
                    sr.color = baseColor + ((Color.white - baseColor) * white);

                }
            }

            
            if (openning)
            {
                white += Time.deltaTime / openSecond;
                if (white >= 1)
                {
                    white = 1;
                    rb.isKinematic = reverse;
                    GetComponent<Collider2D>().enabled = !reverse;
                    Destroy(this);
                    if (audioSource != null)
                    {
                        Destroy(audioSource);
                    }
                    if (destroy)
                    {
                        Destroy(gameObject);
                        Instantiate(Resources.Load<GameObject>("break"),transform.position, Quaternion.identity);
                    }
                }
            }
            else
            {
                white -= Time.deltaTime;
            }
            white = Mathf.Clamp01(white);
        }
        a += Time.deltaTime;
        if (a > 0.2f)
        {
            openning = false;
        }
    }

    public void open()
    {
        if (!openning)
        {
            openning = true;
            a = 0;
        }

    }
}
