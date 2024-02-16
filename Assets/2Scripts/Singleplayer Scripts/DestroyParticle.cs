using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{

    [SerializeField] private float timer;
    [SerializeField] private float timeUntilDestroyed;

    void Update()
    {
        if(transform.childCount <= 0)
        {
            timer += Time.deltaTime;

            if(timer >= timeUntilDestroyed)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
