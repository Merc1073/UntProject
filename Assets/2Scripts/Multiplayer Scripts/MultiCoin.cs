using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiCoin : NetworkBehaviour
{

    //MainPlayer playerScript;
    //BulletPoint bulletReticle;

    private MultiGameScript multiGameScript;

    public GameObject playerDetection;

    public GameObject particles;

    Rigidbody rb;

    //private GenericPlaySound soundPlay;

    //public ParticleSystem particles;
    public MeshRenderer mesh;

    public float forceMultiplier;
    public float explosionForce;
    public float speed;
    public float fireRateToIncrease;

    public float coinMagnetSpeed;

    public float distanceToPlayer;
    public float maxDistanceToPlayer;

    public bool distanceTriggered = false;

    void Start()
    {

        multiGameScript = FindObjectOfType<MultiGameScript>();

        rb = GetComponent<Rigidbody>();

        //playerObject = GameObject.FindGameObjectWithTag("MultiPlayer");

        //playerScript = FindObjectOfType<MainPlayer>();
        //bulletReticle = FindObjectOfType<BulletPoint>();

        rb.AddForce(transform.forward * explosionForce, ForceMode.Impulse);

        //soundPlay = GetComponentInParent<GenericPlaySound>();

    }

    void Update()
    {

        if (transform.position.y < 0.52)
        {
            transform.position += new Vector3(0, 0.1f, 0);
        }

        if (transform.position.y > 0.52)
        {
            transform.position += new Vector3(0, -0.1f, 0);
        }

        if (playerDetection.GetComponent<MultiPlayerDetection>().targetPlayer != null)
        {

            if (playerDetection.GetComponent<MultiPlayerDetection>().targetPlayer.GetComponent<MultiMainPlayer>().isAlive.Value == false)
            {
                playerDetection.GetComponent<MultiPlayerDetection>().player.Remove(playerDetection.GetComponent<MultiPlayerDetection>().targetPlayer);
                playerDetection.GetComponent<MultiPlayerDetection>().playerObject.Remove(playerDetection.GetComponent<MultiPlayerDetection>().targetPlayer.gameObject);
                Debug.Log("player removed");
            }

            Vector3 playerObject = playerDetection.GetComponent<MultiPlayerDetection>().targetPlayer.transform.position;

            distanceToPlayer = Vector3.Distance(transform.position, playerObject);

            //Debug.Log(distanceToPlayer);

            if (distanceToPlayer <= maxDistanceToPlayer)
            {
                distanceTriggered = true;
            }

            if (distanceTriggered == true)
            {
                speed += coinMagnetSpeed;
                transform.position = Vector3.MoveTowards(transform.position, playerObject, speed * Time.deltaTime);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<MultiMainPlayer>())
        {

            //soundPlay.canPlaySound = true;

            //var em = particles.emission;

            //em.enabled = true;

            //transform.parent.position = transform.position;

            //particles.Play();

            //particOnce = false;

            //playerScript.AddCoins(1);
            //bulletReticle.IncreaseFireRate(fireRateToIncrease);

            //if (!IsOwner) return;

            other.GetComponent<MultiMainPlayer>().UpdateScoreServerRpc();
            other.GetComponent<MultiMainPlayer>().multiBulletPoint.GetComponent<MultiBulletPoint>().ChangeFireRateServerRpc(fireRateToIncrease);

            //Debug.Log(other.GetComponent<MultiMainPlayer>().multiBulletPoint.GetComponent<MultiBulletPoint>().fireRateMultiplier);

            CreateParticlesServerRpc();

            DespawnCoinServerRpc();

        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnCoinServerRpc()
    {
        GetComponentInParent<NetworkObject>().Despawn();

        Destroy(mesh);
        Destroy(gameObject);

        multiGameScript.GetComponent<MultiCoinCount>().allCoins.Remove(gameObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void CreateParticlesServerRpc()
    {
        GameObject coinParticle = Instantiate(particles, transform.position, Quaternion.identity);
        coinParticle.GetComponent<NetworkObject>().Spawn();
    }

    //[ClientRpc]
    //private void FireRateIncreasedClientRpc(Collider whoHit)
    //{
    //    whoHit.GetComponent<MultiMainPlayer>().multiBulletPoint.GetComponent<MultiBulletPoint>().IncreaseFireRate(fireRateToIncrease);
    //}

}
