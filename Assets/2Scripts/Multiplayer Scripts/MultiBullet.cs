using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiBullet : NetworkBehaviour
{

    [HideInInspector]
    public MultiBulletPoint parent;

    //public ParticleSystem particles;
    public GameObject particles;
    public MeshRenderer mesh;

    public GameObject enemyDetection;

    //GameScript gameScript;
    //GenericPlaySound soundPlay;

    private TrailRenderer trailRenderer;

    public float bulletSpeed;
    public float bulletDuration;
    float timer;

    float appearingSpeed;
    float speed;

    public float magnetSpeed;
    public float magnetDistance;
    public float shrinkSpeed;

    public bool isMagnetPowerUpActive = false;

    public bool despawnInstructionSent = false;
    public bool spawnInstructionSent = false;

    public bool particOnce = true;
    public bool sizeIsMax = false;
    public bool distanceTriggered = false;

    [SerializeField] float growthDuration;
    [SerializeField] float fadeDuration;

    public Vector3 targetGrowthScale;


    private void Start()
    {
        //gameScript = FindObjectOfType<GameScript>();

        //soundPlay = GetComponentInParent<GenericPlaySound>();

        trailRenderer = GetComponent<TrailRenderer>();

        StartCoroutine(Grow());
    }

    void Update()
    {

        timer += Time.deltaTime;

        AdjustTrailWidth();

        if (timer >= bulletDuration - 0.25f)
        {
            StartCoroutine(ShrinkObject());
        }

        if (timer >= bulletDuration)
        {
            if (!IsOwner) return;
            parent.DestroyBulletServerRpc();
        }

        if(!IsOwner) return;

        if (isMagnetPowerUpActive == true)
        {

            transform.position += transform.forward * bulletSpeed * Time.deltaTime;

            if (enemyDetection.GetComponent<MultiEnemyDetection>().targetEnemy != null)
            {
                Vector3 targetEnemy = enemyDetection.GetComponent<MultiEnemyDetection>().targetEnemy.transform.position;
                float distanceToEnemy = Vector3.Distance(transform.position, targetEnemy);


                if (distanceToEnemy <= magnetDistance)
                {
                    distanceTriggered = true;

                }

                else
                {
                    distanceTriggered = false;
                }

                if (distanceTriggered == true)
                {
                    speed += magnetSpeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, targetEnemy, speed);
                }
            }
        }

        else
        {
            transform.position += transform.forward * bulletSpeed * Time.deltaTime;
        }


    }

    void AdjustTrailWidth()
    {
        float averageScale = (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3f;

        trailRenderer.widthMultiplier = averageScale;
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
        if (other.GetComponent<MultiEnemy>())
        {
            if (!IsOwner) return;

            CreateParticlesServerRpc();

            if(!despawnInstructionSent)
            {
                parent.DestroyBulletServerRpc();
                despawnInstructionSent = true;
            }
        }

        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Ground"))
        {
            if (!IsOwner) return;

            CreateParticlesServerRpc();

            if (!despawnInstructionSent)
            {
                parent.DestroyBulletServerRpc();
                despawnInstructionSent = true;
            }
        }

    }

    [ServerRpc(RequireOwnership = false)]
    public void CreateParticlesServerRpc()
    {
        GameObject hitParticle = Instantiate(particles, transform.position, Quaternion.identity);
        hitParticle.GetComponent<NetworkObject>().Spawn();
    }

}
