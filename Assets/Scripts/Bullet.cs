using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float bulletSpeed;
    float timer;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;

        timer += Time.deltaTime;

        if(timer >= 2f)
        {
            Destroy(this.gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
    }

}
