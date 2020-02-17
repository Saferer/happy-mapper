using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To the ‘SwirlingProjectileCenter’ object, ‘PlayerSwirlingProjectiles’ are attached. It spins them with the defined speed and range.
/// </summary>

public class SwirlingProjectileCenter : MonoBehaviour {

    [Tooltip("speed with which projectiles are moving up")]
    public float speed;

    [Tooltip("speed with which projectiles are rotating if weapon power equals 1")]
    public float basicRotationSpeed;

    [Tooltip("max radius with which projectiles are moving away from each other")]
    public float basicMaxRadius;

    [Tooltip("speed with which projectiles are moving away from each other")]
    public float expansionSpeed;

    //current speed and radius
    float rotationSpeed; 
    float maxRadius; 
    float radius; 

    GameObject firstProjectile, secondProjectile; 


    private void Start()
    {
        int power = PlayerShooting.instance.weaponPower; //setting speed and radious depending on weapon power
        rotationSpeed = basicRotationSpeed * power; 
        maxRadius = basicMaxRadius * power; 
        firstProjectile = transform.GetChild(0).gameObject; 
        secondProjectile = transform.GetChild(1).gameObject;
        radius = 0;
    }

    private void Update()
    {
        if (transform.childCount == 0)         //if projectiles are destroyed destroying the object
            Destroy(gameObject);
        else
        {
            transform.Rotate(Vector3.forward * rotationSpeed * 10 * Time.deltaTime);  //rotating the object with the defined speed
            if (radius < maxRadius)                       
            {
                radius += expansionSpeed * Time.deltaTime;          //distancing projectiles till the max radius is reached
                firstProjectile.transform.localPosition = new Vector3(-radius, 0, 0);
                secondProjectile.transform.localPosition = new Vector3(radius, 0, 0);
            }
        }

        transform.position += Vector3.up * speed * Time.deltaTime; //moving the projectiles up with the defined speed
    }
}
