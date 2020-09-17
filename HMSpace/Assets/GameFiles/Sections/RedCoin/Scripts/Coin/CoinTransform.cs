using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTransform : MonoBehaviour
{
    public float Speed = -2f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (Time.deltaTime) * Speed;
        transform.position = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);

        if (transform.position.x < -15f)
        {
            Destroy(this.gameObject);
        }
    }

}
