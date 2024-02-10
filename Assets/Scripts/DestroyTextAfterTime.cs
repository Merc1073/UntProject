using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTextAfterTime : MonoBehaviour
{
    public float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 3.5f)
        {
            Destroy(this.gameObject);
        }
    }
}
