using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines acceleration, max speed, start speed, attack range, damage level and visual effect of the collision.
/// </summary>

public class Rocket : MonoBehaviour {

    [Tooltip("how fast the rocket accelerates")]
    public float acceleration;

    [Tooltip("rocket speed in the beginning")]
    public float startingSpeed;

    [Tooltip("max rocket speed")]
    public float maxSpeed;

    [Tooltip("radius where enemies get damage")]
    public float attackRange;

    [Tooltip("damage from rocket explosion")]
    public int damage;

    [Tooltip("Explosion VFX prefab")]
    public GameObject explosionVFX;

    public float speed;

    private void Awake()
    {
    }

    private void OnEnable()
    {
        speed = startingSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision) //if collides with 'Enemy', launching 'explosion damage'
    {
        if (collision.tag == "Enemy" )
            ExplosionDamage(attackRange);
    }

    private void Update()          //increasing the speed depending on acceleration up to the reaching the max speed and moving the object
    {
      Debug.Log(speed);

        // if (speed < maxSpeed)
        //     speed += acceleration * Time.deltaTime;
        transform.Translate(speed * Vector3.up * Time.deltaTime);
    }

    void ExplosionDamage(float radius)   //find all the objects in the radius and if the object is 'Enemy' dealing damage
    {
        Collider2D [] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);
        for (int i = 0; i<hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject.tag == "Enemy")
                hitColliders[i].gameObject.GetComponent<Enemy>().GetDamage(damage);
        }
        Instantiate(explosionVFX, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }


}
