using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossBody : MonoBehaviour
{
    float value;
    float a;
    public bool ops;
    ParticleSystem ps;
    public boss boss;
    AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ps = GetComponent<ParticleSystem>();
    }
    public void Update()
    {
        if (value != 0)
        {
            audioSource.pitch = 1 / value;  
        }
        audioSource.mute = value == 0;
        if (ops)
        {
            value += Time.deltaTime;
        }
        else
        {
            value -= Time.deltaTime;
        }
        value = Mathf.Clamp01(value);
        ps.startColor = (Color.red * value) + (Color.white * (1 - value));
        if (value >= 1)
        {
            ps.startColor = Color.red;
            Destroy(this);
            audioSource.mute = true;
            boss.RandomGenerate();
        }
        a += Time.deltaTime;
        if (a > 0.2f)
        {
            ops = false;
        }
    }
    public void open()
    {
        if (!ops)
        {
            ops = true;
            a = 0;
        }

    }
}
