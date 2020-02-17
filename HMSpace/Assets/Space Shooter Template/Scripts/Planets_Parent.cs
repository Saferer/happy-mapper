using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script translates the planets parent with defined speed and delete the parent if all planets were destroyed
/// </summary>
public class Planets_Parent : MonoBehaviour {

    [HideInInspector] public float speed;

    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        if (transform.childCount == 0)
            Destroy(gameObject);
    }
}
