using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayer : MonoBehaviour
{

    public Vector3 tranDif;

    public LayerMask groundMask;

    public float forceMultiplier;

    public float currentHealth;
    public float maxHealth;

    public int coinCount;

    public bool particOnce = true;

    public ParticleSystem particles;
    public MeshRenderer mesh;

    private HealthBar playerHealthBar;
    private FollowMouse aimReticle;
    private BulletPoint bulletReticle;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        playerHealthBar = GetComponentInChildren<HealthBar>();
        aimReticle = FindObjectOfType<FollowMouse>();
        bulletReticle = FindObjectOfType<BulletPoint>();

        currentHealth = maxHealth;
        playerHealthBar.UpdateHealthBar(maxHealth, currentHealth);

    }

    void Update()
    {

        if(currentHealth < 0)
        {
            currentHealth = 0;
        }

        if(currentHealth == 0)
        {
            //src.pitch = 1;
            //src.clip = explosionSound;
            //src.volume = 0.4f;
            //src.PlayOneShot(explosionSound);

            var em = particles.emission;
            var dur = particles.main.duration;

            em.enabled = true;

            transform.parent.position = transform.position;

            particles.Play();

            particOnce = false;

            aimReticle.DestroyObj();
            bulletReticle.DestroyObj();

            Destroy(mesh);
            Invoke(nameof(DestroyObj), 0);
        }

        if(Input.GetMouseButton(0))
        {

            RaycastHit hit;

            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, groundMask))
            {

                Vector3 directionToMouse = transform.position - hit.point;
                directionToMouse = directionToMouse.normalized * forceMultiplier;

                //clone = Instantiate(clone, hit.point + tranDif, rotation);
                rb.AddForce(-directionToMouse + tranDif * Time.deltaTime);
            }
        }
    }

    public void DecreasePlayerHealth(float health)
    {
        currentHealth -= health;
        playerHealthBar.UpdateHealthBar(maxHealth, currentHealth);
    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }

    public void AddCoins(int coins)
    {
        coinCount += coins;
    }

}
