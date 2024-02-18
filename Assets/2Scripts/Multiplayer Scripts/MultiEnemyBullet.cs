using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class MultiEnemyBullet : NetworkBehaviour
{

    [HideInInspector]
    public MultiEnemyBulletPoint parent;

    MainPlayer player;
    //GameScript gameScript;
    //GenericPlaySound soundPlay;

    public GameObject particles1;
    public GameObject particles2;

    private TrailRenderer trailRenderer;

    public float bulletSpeed;
    public float bulletDuration;
    float timer;

    public float shrinkSpeed;

    public bool despawnInstructionSent = false;
    public bool particOnce = true;

    //public ParticleSystem particles;
    public MeshRenderer mesh;

    [SerializeField] float growthDuration;
    [SerializeField] float fadeDuration;

    public Vector3 targetGrowthScale;



    private void Start()
    {
        player = FindObjectOfType<MainPlayer>();
        //gameScript = FindObjectOfType<GameScript>();
        //soundPlay = GetComponentInParent<GenericPlaySound>();

        trailRenderer = GetComponent<TrailRenderer>();

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

        if (timer >= bulletDuration - 1f)
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

        if (other.GetComponent<MultiMainPlayer>())
        {

            if (!IsOwner) return;

            other.GetComponent<MultiMainPlayer>().GetComponentInParent<MultiHealthState>().DecreaseHealthClientRpc(1f);

            CreateParticlesServerRpc();

            if (!despawnInstructionSent)
            {
                parent.DestroyEnemyBulletServerRpc();
                despawnInstructionSent = true;
            }

            Destroy(mesh);
            Destroy(gameObject);
        }

        if ((other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Ground")) && particOnce)
        {

            if (!IsOwner) return;

            CreateParticlesMuteServerRpc();

            if (!despawnInstructionSent)
            {
                parent.DestroyEnemyBulletServerRpc();
                despawnInstructionSent = true;
            }

            Destroy(mesh);
            Destroy(gameObject);
        }

    }

    //[ServerRpc(RequireOwnership = false)]
    //public void DespawnEnemyBulletServerRpc()
    //{
    //    GetComponentInParent<NetworkObject>().Despawn();

    //    Destroy(mesh);
    //    Destroy(gameObject);
    //}

    [ServerRpc(RequireOwnership = false)]
    private void CreateParticlesServerRpc()
    {
        GameObject coinParticle = Instantiate(particles1, transform.position, Quaternion.identity);
        coinParticle.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void CreateParticlesMuteServerRpc()
    {
        GameObject coinParticle = Instantiate(particles2, transform.position, Quaternion.identity);
        coinParticle.GetComponent<NetworkObject>().Spawn();
    }

}
