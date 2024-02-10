using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    MainPlayer player;
    GameScript gameScript;
    GenericPlaySound soundPlay;

    private TrailRenderer trailRenderer;

    public float bulletSpeed;
    public float bulletDuration;
    float timer;

    public float shrinkSpeed;

    public bool particOnce = true;

    public ParticleSystem particles;
    public MeshRenderer mesh;

    [SerializeField] float growthDuration;
    [SerializeField] float fadeDuration;

    public Vector3 originalTransformScale;
    public Vector3 targetGrowthScale;

    

    private void Start()
    {
        player = FindObjectOfType<MainPlayer>();
        gameScript = FindObjectOfType<GameScript>();
        soundPlay = GetComponentInParent<GenericPlaySound>();

        StartCoroutine(Grow());
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


        timer += Time.deltaTime;

        AdjustTrailWidth();

        transform.position += transform.forward * bulletSpeed * Time.deltaTime;

        if (timer >= 4.0f)
        {
            StartCoroutine(ShrinkObject());
        }

        if (timer >= bulletDuration)
        {
            Destroy(this.gameObject);
        }

    }

    private IEnumerator Grow()
    {

        Vector3 originalSize = Vector3.zero;

        for (float t = 0; t < growthDuration; t += Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(originalSize, targetGrowthScale, t / growthDuration);
            yield return null;
        }

        transform.localScale = targetGrowthScale;

    }

    void AdjustTrailWidth()
    {
        float averageScale = (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3f;

        trailRenderer.widthMultiplier = averageScale;
    }

    IEnumerator ShrinkObject()
    {
        while (transform.localScale.x > 0 && transform.localScale.y > 0 && transform.localScale.z > 0)
        {
            Vector3 newScale = transform.localScale - new Vector3(shrinkSpeed, shrinkSpeed, shrinkSpeed) * Time.deltaTime;

            newScale = Vector3.Max(newScale, Vector3.zero);

            transform.localScale = newScale;

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.tag == "Player")
        {

            if(gameScript.isPlayerInvincible == false)
            {
                player.DecreasePlayerHealth(1f);
            }

            var em = particles.emission;
            var dur = particles.main.duration;

            em.enabled = true;

            transform.parent.position = transform.position;

            soundPlay.canPlaySound = true;
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
