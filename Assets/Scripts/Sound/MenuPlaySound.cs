using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlaySound : MonoBehaviour
{
    public AudioSource src;

    public AudioClip gameStartSound;
    public AudioClip selectOptionSound;

    public bool canPlayGameStartSound = false;
    public bool canPlaySelectOptionSound = false;

    public bool hasSoundPlayed = false;

    void Start()
    {
        src = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (canPlayGameStartSound == true && hasSoundPlayed == false)
        {
            src.volume = 0.4f;
            src.PlayOneShot(gameStartSound);
            hasSoundPlayed = true;
            //canPlayGameStartSound = false;
        }

        if (canPlaySelectOptionSound == true && hasSoundPlayed == false)
        {
            src.volume = 1f;
            src.PlayOneShot(selectOptionSound);
            hasSoundPlayed = true;
            //canPlaySelectOptionSound = false;
        }
    }

    public void PlayStartSound()
    {
        src.volume = 0.4f;
        src.PlayOneShot(gameStartSound);
    }

    public void PlayOptionSelectSound()
    {
        src.volume = 1f;
        src.PlayOneShot(selectOptionSound);
    }

}
