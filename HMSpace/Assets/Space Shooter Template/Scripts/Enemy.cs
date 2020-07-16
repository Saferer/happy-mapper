using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script defines 'Enemy's' health and behavior. 
/// </summary>
public class Enemy : MonoBehaviour {

    #region FIELDS
    [Tooltip("Health points in integer")]
    public int health;

    [Tooltip("Enemy's projectile prefab")]
    public GameObject Projectile;

    [Tooltip("VFX prefab generating after destruction")]
    public GameObject destructionVFX;
    public GameObject hitRayEffect;

    [Tooltip("Object is pooled or not. Mark if pooling of the object is added to Pooling_Controller")]
    public bool isPooled;

    public int scoreReward; //points the player gets after destroying this enemy


    [HideInInspector] public int shotChance; //probability of 'Enemy's' shooting during tha path
    [HideInInspector] public float shotTimeMin, shotTimeMax; //max and min time for shooting from the beginning of the path
    float nextRayDamage;                                //time for receiving new damage of player's weapon ray
    int startingHealth;
    #endregion

    //if 'Enemy' was activated after deactivation, setting start health
    private void Awake()
    {
        startingHealth = health; 
    }

    private void OnEnable()
    {
        health = startingHealth; 
    }

    //coroutine making a shot
    public IEnumerator ActivateShooting() 
    {
        yield return new WaitForSeconds(Random.Range(shotTimeMin,shotTimeMax)); //waiting for random time between 'shotTimeMin' and 'shotTimeMax'     
        if (Random.value < (float)shotChance / 100)                             //if random value less than shot probability, making a shot
        {
            if (Projectile.GetComponent<Projectile>() != null && Projectile.GetComponent<Projectile>().isPooled)    //if 'Enemy's' projectile uses pooling, pooling the object
            {
                GameObject obj = PoolingController.instance.GetPoolingObject(Projectile);       //
                obj.transform.position = transform.position;
                obj.SetActive(true);
            }
            else
            {                
                Instantiate(Projectile,  gameObject.transform.position, Quaternion.identity); 
            }
        }
    }

    //method of getting damage for the 'Enemy'
    public void GetDamage(int damage) 
    {
        health -= damage;           //reducing health for damage value, if health is less than 0, starting destruction procedure
        if (health <= 0) 
            Destruction();
    }

    //method of getting damage, if damage is received from weapon 'Ray'
    public void GetIndestructibleDamage(float frequency, int damage) 
    {
        if (Time.time>nextRayDamage)    //if comes time for the next damage, reducing health
        {
            health -= damage;           //if health less than 0, starting destruction procedure; if not, generating 'hit effect' and setting time for the next damage
            if (health <= 0)
                Destruction();
            else
            {
                GameObject newHitFx = PoolingController.instance.GetPoolingObject(hitRayEffect);
                newHitFx.transform.position = transform.position;
                newHitFx.transform.parent = transform;
                newHitFx.SetActive(true);
                nextRayDamage = Time.time + frequency;
            }
        }
    }

    //if 'Enemy' collides 'Player', 'Player' gets the damage equal to projectile's damage value
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (collision.tag == "Player")
        {
            if (Projectile.GetComponent<Projectile>() != null)
                Player.instance.GetDamage(Projectile.GetComponent<Projectile>().damage);
            else
                Player.instance.GetDamage(1);
        }*/
        if (collision.tag == "Boundary")
        {
            Debug.Log("touched");
            Destruction();
        }
    }

    //method of destroying the 'Enemy'
    void Destruction()                           
    {
        GameController.instance.AddScore(scoreReward);              //adding 'Player's' points
        if (destructionVFX.GetComponent<VisualEffect>().isPooled) //generating destruction visual effect
        {
            GameObject newVfx = PoolingController.instance.GetPoolingObject(destructionVFX);
            newVfx.transform.position = transform.position;
            newVfx.SetActive(true);
        }
        else
        {
            Instantiate(destructionVFX, transform.position, Quaternion.identity); 
        }
        if (Random.value < (float)GameController.instance.chanceForCoin / 100)
        {
            GameObject newCoin = PoolingController.instance.GetPoolingObject(GameController.instance.coinPrefab);
            newCoin.transform.position = transform.position;
            newCoin.SetActive(true);
        }
        SoundManager.instance.PlaySound("explosion"); //playing sound
        if (isPooled)                                  //if 'Enemy' uses pooling, deactivating the 'Enemy', if not, destroying the 'Enemy'
            gameObject.SetActive(false);
        else
            Destroy(gameObject);
    }
}
