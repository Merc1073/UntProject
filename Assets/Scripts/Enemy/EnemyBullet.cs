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

    public MainPlayer player;

    private void Start()
    {
        player = FindObjectOfType<MainPlayer>();
    }

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

        if(other.gameObject.tag == "Player")
        {

            player.DecreasePlayerHealth(1f);

            var em = particles.emission;
            var dur = particles.main.duration;

            em.enabled = true;

            transform.parent.position = transform.position;

            particles.Play();

            particOnce = false;

            Destroy(mesh);
            Invoke(nameof(DestroyObj), 0);
        }

        if ((other.gameObject.tag == "Wall" || other.gameObject.tag == "Ground") && particOnce)
        {

            player.DecreasePlayerHealth(1f);

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
