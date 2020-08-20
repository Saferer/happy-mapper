using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To the ‘SwirlingProjectileCenter’ object, ‘PlayerSwirlingProjectiles’ are attached. It spins them with the defined speed and range.
/// </summary>

public class Photon : MonoBehaviour
{

    [Tooltip("speed with which projectiles are moving up")]
    public float speed;

    //current speed and radius
    float radius;

    GameObject firstProjectile, secondProjectile;


    private void Start()
    {
        int power = PlayerShooting.instance.weaponPower;
        firstProjectile = transform.GetChild(0).gameObject;
        secondProjectile = transform.GetChild(1).gameObject;
        radius = 0;
    }

    private void Update()
    {
        if (transform.childCount == 0)         //if projectiles are destroyed destroying the object
            Destroy(gameObject);

        transform.position += Vector3.up * speed * Time.deltaTime; //moving the projectiles up with the defined speed
    }
}
