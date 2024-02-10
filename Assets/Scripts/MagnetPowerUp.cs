using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class MagnetPowerUp : MonoBehaviour
{

    private GenericPlaySound soundPlay;

    public ParticleSystem particles;

    public MeshRenderer mesh;

    public bool particOnce = true;

    GameScript gameScript;

    [SerializeField] float fadeDuration;


    void Start()
    {

        soundPlay = GetComponentInParent<GenericPlaySound>();

        gameScript = FindObjectOfType<GameScript>();

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {

        Color color = mesh.material.color;
        float targetAlpha = 1f;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            if(mesh)
            {
                color.a = Mathf.Lerp(0f, targetAlpha, t / fadeDuration);
                mesh.material.color = color;
                yield return null;
            }
            

        }

        if(mesh)
        {
            color.a = targetAlpha;
            mesh.material.color = color;
        }
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {

            gameScript.ActivateMagnetPowerUp();

            soundPlay.canPlaySound = true;

            var em = particles.emission;

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
