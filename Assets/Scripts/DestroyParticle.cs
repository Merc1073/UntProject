using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{

    [SerializeField] private float timer;

    void Update()
    {
        if(transform.childCount <= 0)
        {
            timer += Time.deltaTime;

            if(timer >= 3)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
