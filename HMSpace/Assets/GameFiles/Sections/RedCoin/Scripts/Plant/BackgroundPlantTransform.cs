using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BackgroundPlantTransform : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Sprite> spriteList;
    public float lowerSpawnOffset;
    private float speed = -2f;
    void Start()
    {
        System.Random random = new System.Random();
        this.GetComponent<SpriteRenderer>().sprite = spriteList[random.Next(0, spriteList.Count - 1)];
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (Time.deltaTime) * speed;
        transform.position = new Vector3(transform.position.x + distance, lowerSpawnOffset + this.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2, transform.position.z);

        if (transform.position.x < -18f)
        {
            Destroy(this.gameObject);
        }
    }
}
