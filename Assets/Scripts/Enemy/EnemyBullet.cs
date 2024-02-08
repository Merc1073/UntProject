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

    [SerializeField] float growthDuration;
    [SerializeField] float fadeDuration;

    public Vector3 originalTransformScale;
    public Vector3 targetGrowthScale;

    MainPlayer player;

    private void Start()
    {
        player = FindObjectOfType<MainPlayer>();

        StartCoroutine(Grow());
    }

    private IEnumerator Grow()
    {

        Vector3 originalSize = originalTransformScale;

        for (float t = 0; t < growthDuration; t += Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(originalSize, targetGrowthScale, t / growthDuration);
            yield return null;
        }

        transform.localScale = targetGrowthScale;

    }

    void Update()
    {

        if (transform.position.y < 1f)
        {
            transform.position += new Vector3(0, 0.1f, 0);
        }

        if (transform.position.y > 1f)
        {
            transform.position += new Vector3(0, -0.1f, 0);
        }

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
