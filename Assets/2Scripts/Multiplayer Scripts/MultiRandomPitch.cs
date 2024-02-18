using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiRandomPitch : MonoBehaviour
{

    public bool isTripleBulletActive = false;

    void Start()
    {
        GetComponent<AudioSource>().volume = 0.6f;

        if(isTripleBulletActive)
        {
            GetComponent<AudioSource>().pitch = Random.Range(1.0f, 1.2f);
        }

        else
        {
            GetComponent<AudioSource>().pitch = Random.Range(0.6f, 0.8f);
        }

    }

}
