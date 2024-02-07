using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParticle : MonoBehaviour
{

    private void LateUpdate()
    {
        transform.position = transform.GetChild(0).position;
    }

}
