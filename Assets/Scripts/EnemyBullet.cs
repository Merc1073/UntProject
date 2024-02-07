using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float bulletSpeed;
    float timer;

    public bool particOnce = true;

    public ParticleSystem particles;
    public MeshRenderer mesh;


    void Update()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;

        timer += Time.deltaTime;

        if (timer >= 2f)
        {
            Destroy(this.gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Player" || other.gameObject.tag == "Wall" || other.gameObject.tag == "Ground") && particOnce)
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
