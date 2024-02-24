using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiEnemyBulletPoint : NetworkBehaviour
{
    public float fireRate;
    public float fireRateCooldown;

    public float rotateVelocity;
    public float rotateSpeedMovement;

    [SerializeField]
    private GameObject playerDetection;

    [SerializeField]
    private GameObject enemyBullet;

    public bool canFire = false;

    [SerializeField]
    private List<GameObject> spawnedEnemyBullets = new List<GameObject>();

    private void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {

        if(!IsOwner) return;

        if (playerDetection.GetComponent<MultiPlayerDetection>().targetPlayer != null)
        {

            if (playerDetection.GetComponent<MultiPlayerDetection>().targetPlayer.GetComponent<MultiMainPlayer>().isAlive.Value == false)
            {
                playerDetection.GetComponent<MultiPlayerDetection>().player.Remove(playerDetection.GetComponent<MultiPlayerDetection>().targetPlayer);
                playerDetection.GetComponent<MultiPlayerDetection>().playerObject.Remove(playerDetection.GetComponent<MultiPlayerDetection>().targetPlayer.gameObject);
                Debug.Log("player removed");
            }

            fireRate += Time.deltaTime;

            if (fireRate >= fireRateCooldown)
            {
                canFire = true;
                fireRate = fireRateCooldown;
            }

            if (canFire == true)
            {
                fireRate = 0;

                CreateEnemyBulletServerRpc();

                canFire = false;
            }
        }
    }

    private void LateUpdate()
    {
        transform.position = transform.parent.position;
        //transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    [ServerRpc(RequireOwnership = false)]
    private void CreateEnemyBulletServerRpc()
    {

        Vector3 playerObject = playerDetection.GetComponent<MultiPlayerDetection>().targetPlayer.transform.position;

        Quaternion rotationToLookAt = Quaternion.LookRotation(playerObject - transform.position);
        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref rotateVelocity, rotateSpeedMovement * (Time.deltaTime * 5));
        transform.eulerAngles = new Vector3(0, rotationY, 0);

        GameObject clone = Instantiate(enemyBullet, transform.position, rotationToLookAt);

        spawnedEnemyBullets.Add(clone);

        clone.GetComponentInChildren<MultiEnemyBullet>().parent = this;
        clone.GetComponent<NetworkObject>().Spawn();

    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyEnemyBulletServerRpc()
    {
        GameObject toDestroy = spawnedEnemyBullets[0];

        if (toDestroy)
        {
            toDestroy.GetComponent<NetworkObject>().Despawn();
            spawnedEnemyBullets.Remove(toDestroy);
            Destroy(toDestroy);
        }
    }

}
