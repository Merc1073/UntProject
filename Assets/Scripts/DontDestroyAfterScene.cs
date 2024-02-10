using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyAfterScene : MonoBehaviour
{

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


}
