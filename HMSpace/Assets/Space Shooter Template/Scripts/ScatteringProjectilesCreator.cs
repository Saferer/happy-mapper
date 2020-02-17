using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script creates a number of projectiles, which fly apart in a scattering way. It defines the number of projectiles and their speed. 
/// </summary>

public class ScatteringProjectilesCreator : MonoBehaviour {

    [Tooltip("amount of generated projectiles")]
    public int count;

    [Tooltip("speed with which the projectiles are flying apart")]
    public float speed;

    [Tooltip("projectiles prefab")]
    public GameObject projectile;

    float angle;


    void Start()
    {
        float segment = 360 / count; //depending on the amount of projectiles, calculating the evenly distributed directions on the circle
        angle = 270; 

        for (int i =0; i< count; i++) 
        {
           GameObject newProj = PoolingController.instance.GetPoolingObject(projectile); //rotating each projectile in the calculated direction
           

            newProj.transform.position = transform.position;
            newProj.transform.rotation = Quaternion.Euler(0, 0, angle); 
            newProj.GetComponent<DirectMoving>().speed = speed; 
            newProj.SetActive(true); 
            angle += segment; 
        }
        Destroy(gameObject); 
    }
}
