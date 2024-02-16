using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlaySound : MonoBehaviour
{
    public AudioSource src;
    public AudioClip enemyExplosion;

    public bool canPlaySound = false;
    public bool hasSoundPlayed = false;

    void Start()
    {
        src = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (canPlaySound == true && hasSoundPlayed == false)
        {
            src.pitch = Random.Range(0.9f, 1.1f);
            src.volume = 0.4f;
            src.PlayOneShot(enemyExplosion);
            hasSoundPlayed = true;
        }
    }
}
