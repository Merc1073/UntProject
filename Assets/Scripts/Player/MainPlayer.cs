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
    public bool canMove = true;

    public ParticleSystem particles;
    public MeshRenderer mesh;

    public Transform canvasTransform;

    GameScript gameScript;

    AudioSource src;
    public AudioClip gameOverNoise;

    private HealthBar playerHealthBar, magnetPowerBar, tripleBulletPowerBar;

    private FollowMouse aimReticle;
    private BulletPoint bulletReticle;

    Rigidbody rb;

    void Start()
    {

        rb = GetComponent<Rigidbody>();

        playerHealthBar = GetComponentInChildren<HealthBar>();
        magnetPowerBar = GetComponentInChildren<HealthBar>();
        tripleBulletPowerBar = GetComponentInChildren<HealthBar>();

        aimReticle = FindObjectOfType<FollowMouse>();
        bulletReticle = FindObjectOfType<BulletPoint>();
        gameScript = FindObjectOfType<GameScript>();

        src = FindObjectOfType<AudioSource>();

        currentHealth = maxHealth;

        if(playerHealthBar)
        {
            playerHealthBar.UpdateHealthBar(maxHealth, currentHealth);
        }

    }

    void Update()
    {

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

        if(transform.position.y > 1.0f)
        {
            transform.position += new Vector3(0, -0.1f, 0) * Time.deltaTime * 100;
        }

        //if (transform.position.y < 0.9f)
        //{
        //    transform.position += new Vector3(0, +0.1f, 0) * Time.deltaTime * 100;
        //}

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        if(currentHealth == 0)
        {

            src.pitch = 1;
            src.volume = 0.5f;
            src.PlayOneShot(gameOverNoise);

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

        //if(Input.GetMouseButton(0))
        //{

        //    RaycastHit hit;

        //    if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, groundMask))
        //    {

        //        Vector3 directionToMouse = transform.position - hit.point;
        //        directionToMouse = directionToMouse.normalized * forceMultiplier;

        //        //clone = Instantiate(clone, hit.point + tranDif, rotation);
        //        rb.AddForce(-directionToMouse + tranDif * Time.deltaTime);
        //    }
        //}

        if(canMove == true)
        {
            rb.AddForce(movement * forceMultiplier * Time.deltaTime);
        }

    }

    private void LateUpdate()
    {
        canvasTransform.position = transform.position + new Vector3(0, 4, 2);
        canvasTransform.rotation = Quaternion.Euler(90, 0, 0);
        //canvasTransform.LookAt(transform.position + Camera.main.transform.forward);
    }

    public void DecreasePlayerHealth(float health)
    {
        currentHealth -= health;

        if(playerHealthBar)
        {
            playerHealthBar.UpdateHealthBar(maxHealth, currentHealth);
        }
    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }

    public void AddCoins(int coins)
    {
        coinCount += coins;
        gameScript.scoreMultiplier += 0.1f;
        gameScript.coinCount += coins;
    }

}
