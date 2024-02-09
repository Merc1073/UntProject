using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleBulletPowerUp : MonoBehaviour
{
    AudioSource src;
    public AudioClip powerupPickupNoise;

    public ParticleSystem particles;

    public MeshRenderer mesh;

    public bool particOnce = true;

    GameScript gameScript;

    [SerializeField] float fadeDuration;


    void Start()
    {
        src = FindObjectOfType<AudioSource>();

        gameScript = FindObjectOfType<GameScript>();

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {

        if(mesh)
        {
            Color color = mesh.material.color;
            float targetAlpha = 1f;

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                color.a = Mathf.Lerp(0f, targetAlpha, t / fadeDuration);
                mesh.material.color = color;
                yield return null;

            }

            color.a = targetAlpha;
            mesh.material.color = color;
        }
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            gameScript.ActivateTripleBulletPowerUp();

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
