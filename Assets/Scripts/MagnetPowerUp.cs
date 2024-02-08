using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class MagnetPowerUp : MonoBehaviour
{

    AudioSource src;
    public AudioClip powerupPickupNoise;

    public ParticleSystem particles;

    public MeshRenderer mesh;

    public bool particOnce = true;

    GameScript gameScript;

    void Start()
    {
        src = FindObjectOfType<AudioSource>();

        gameScript = FindObjectOfType<GameScript>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {

            gameScript.ActivateMagnetPowerUp();

            src.pitch = 1;
            //src.clip = explosionSound;
            src.volume = 0.6f;
            src.PlayOneShot(powerupPickupNoise);

            var em = particles.emission;
            var dur = particles.main.duration;

            em.enabled = true;

            transform.parent.position = transform.position;

            particles.Play();

            particOnce = false;

            Destroy(mesh);
            Invoke(nameof(DestroyObj), 0);
        }
    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }

}
