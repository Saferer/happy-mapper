using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void OnBecameInvisible()
    {
        if (gameObject.name.Contains("Clone"))
        {
            Destroy(gameObject);
        }
    }
}
