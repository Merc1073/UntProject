using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPlaySound : MonoBehaviour
{

    public AudioSource src;
    public AudioClip soundEffect;

    public bool canPlaySound = false;
    public bool hasSoundPlayed = false;

    void Start()
    {
        src = GetComponent<AudioSource>();
    }

    void Update()
    {
        //if(canPlaySound == true && hasSoundPlayed == false)
        //{
        //    src.volume = 0.4f;
        //    src.PlayOneShot(soundEffect);
        //    hasSoundPlayed = true;
        //}
    }
}
