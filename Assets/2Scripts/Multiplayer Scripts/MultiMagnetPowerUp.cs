using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiMagnetPowerUp : NetworkBehaviour
{
    //private GenericPlaySound soundPlay;

    //public ParticleSystem particles;

    public GameObject particles;

    public MeshRenderer mesh;

    public bool particOnce = true;

    //MultiGameScript multiGameScript;

    [SerializeField] float fadeDuration;


    void Start()
    {
        //soundPlay = GetComponentInParent<GenericPlaySound>();

        //multiGameScript = FindObjectOfType<MultiGameScript>();

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {

        Color color = mesh.material.color;
        float targetAlpha = 1f;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            if (mesh)
            {
                color.a = Mathf.Lerp(0f, targetAlpha, t / fadeDuration);
                mesh.material.color = color;
                yield return null;
            }
        }

        if (mesh)
        {
            color.a = targetAlpha;
            mesh.material.color = color;
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MultiMainPlayer>())
        {
            other.GetComponent<MultiMainPlayer>().multiBulletPoint.GetComponent<MultiBulletPoint>().isTripleBulletPowerUpActive = true;

            CreateParticlesServerRpc();

            DespawnMagnetServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnMagnetServerRpc()
    {
        GetComponentInParent<NetworkObject>().Despawn();

        Destroy(mesh);
        Destroy(gameObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void CreateParticlesServerRpc()
    {
        GameObject coinParticle = Instantiate(particles, transform.position, Quaternion.identity);
        coinParticle.GetComponent<NetworkObject>().Spawn();
    }

}
