using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script defines which sprite the 'Player" uses and its health.
/// </summary>

//Sprites for every kind of ship
[System.Serializable]
public class ShipSkins
{
    public Sprite short_LazerShip, rocketShip, swirlingShip, rayShip;
}

//Images for every stage of health bar on Main Canvas
[System.Serializable]
/*public class HealthIndicators
{
    public Sprite health_0, health_1, health_2, health_3;
}*/

public class Player : MonoBehaviour
{
    #region FIELDS

    int health;

    [Tooltip("time after receiving the damage when 'Player' will be non-vulnerable to the new damage")]
    public float damageFrequency;


    public ShipSkins shipSkins;
    //public HealthIndicators healthIndicatorsSprites;

    [Tooltip("Object 'Shield' of the 'Player' located in his hierarchy")]
    public GameObject shield;

    [Tooltip("VFX's prefab emerging after the Player is destroyed")]
    public GameObject destructionFX;

    Image healthIndicatorImage;
    float nextDamage;
    SpriteRenderer playerSprite;

    public static Player instance; 
#endregion

    private void Awake()
    {
        if (instance == null) 
            instance = this;
        playerSprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        /*healthIndicatorImage = GameObject.FindWithTag("Health").GetComponent<Image>();  //set the health to 3 and update the health bar
        health = 3;
        UpdateHealthIndicator();
        UpdateSkin();*/
    }

    //method for damage proceccing by 'Player'
    /*public void GetDamage(int damage)   
    {
        if (Time.time > nextDamage)     //checking if the time comes for the new damage, and if it does, decreasing health and setting the new time
        {
            health -= damage;
            UpdateHealthIndicator();
            nextDamage = Time.time + damageFrequency;
            if (health <= 0)        //if health decreses to zero, destroying the 'Player'
                Destruction();
            else
            {
                shield.SetActive(false); //deactivate and activate Shield to set up animation
                shield.SetActive(true);
            }
        }
    }*/
    
    //depending on 'Player's' health, refreshing health bar for a needed sprite
    /*void UpdateHealthIndicator()
    {
        switch (health)
        {
            case 0:
                healthIndicatorImage.sprite = healthIndicatorsSprites.health_0;
                break;
            case 1:
                healthIndicatorImage.sprite = healthIndicatorsSprites.health_1;
                break;
            case 2:
                healthIndicatorImage.sprite = healthIndicatorsSprites.health_2;
                break;
            case 3:
                healthIndicatorImage.sprite = healthIndicatorsSprites.health_3;
                break;
        }
    }*/

    //'Player's' destruction procedure
    void Destruction()
    {
        Instantiate(destructionFX, transform.position, Quaternion.identity); //generating destruction visual effect and destroying the 'Player' object
        GameController.instance.GameOver();
        Destroy(gameObject);
    }

    //depending on active shooting mode, refreshing 'Player's' skin
    public void UpdateSkin()
    {
        switch (PlayerShooting.instance.activeShootingMode)
        {
            case ActiveShootingMode.Short_Lazer:
                playerSprite.sprite = shipSkins.short_LazerShip;
                break;
            case ActiveShootingMode.Rocket:
                playerSprite.sprite = shipSkins.rocketShip;
                break;
            case ActiveShootingMode.Swirling:
                playerSprite.sprite = shipSkins.swirlingShip;
                break;
            case ActiveShootingMode.Ray:
                playerSprite.sprite = shipSkins.rayShip;
                break;
        }
    }
}
















