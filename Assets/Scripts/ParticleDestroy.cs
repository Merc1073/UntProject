using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{


    public ParticleSystem particles;


    private void Update()
    {

        if(!particles.isPlaying && particles.time >= particles.main.duration)
        {
            Destroy(gameObject);
        }
    }

}
