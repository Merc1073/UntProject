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

    private HealthBar playerHealthBar;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        playerHealthBar = GetComponentInChildren<HealthBar>();

        currentHealth = maxHealth;
        playerHealthBar.UpdateHealthBar(maxHealth, currentHealth);

    }

    void Update()
    {
                
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

}
