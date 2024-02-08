using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float bulletSpeed;
    float timer;

    float appearingSpeed;

    public bool particOnce = true;

    public ParticleSystem particles;
    public MeshRenderer mesh;

    private void Start()
    {
        transform.localScale = Vector3.zero;
    }

    void Update()
    {

        if (appearingSpeed < 3)
        {
            appearingSpeed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 1f, 1f), appearingSpeed);
        }

        transform.position += transform.forward * bulletSpeed * Time.deltaTime;

        timer += Time.deltaTime;

        if(timer >= 2f)
        {
            Destroy(this.gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if((other.gameObject.tag == "Enemy" || other.gameObject.tag == "Wall" || other.gameObject.tag == "Ground") && particOnce)
        {

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
