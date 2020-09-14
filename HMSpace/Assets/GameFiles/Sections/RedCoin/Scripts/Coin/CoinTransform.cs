using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTransform : MonoBehaviour
{
    public float Speed = -0.02f;

    public float LastFrameTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        LastFrameTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (Time.time - LastFrameTime) * Speed;
        LastFrameTime = Time.time;
        transform.position = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);

        if (transform.position.x < -15f)
        {
            Destroy(this.gameObject);
        }
    }

}
