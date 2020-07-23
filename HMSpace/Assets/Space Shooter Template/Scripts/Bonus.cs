using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script defines to which bonus type belongs the object. When colliding ‘Player’ it sends to ‘Player’ script a command to perform a certain action.
/// </summary>

//enumerating of bonuses types
public enum BonusType
{
    levelup, short_Lazer, rocket, /*swirling,*/ ray
}

public class Bonus : MonoBehaviour {

    public BonusType bonusType;

    //when colliding with another object, if another objct is 'Player', sending command to the 'Player'
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            switch (bonusType)
            {
                //if 'weapon power' not more than 'max weapon power', increase weapon power level
                case BonusType.levelup:
                    if (PlayerShooting.instance.weaponPower < PlayerShooting.instance.maxweaponPower)
                    {
                        PlayerShooting.instance.weaponPower++;
                    }
                    SoundManager.instance.PlaySound("powerUp");
                    break;
                //changing 'Player's' shooting mode depending on bonus type
                case BonusType.short_Lazer:
                    PlayerShooting.instance.activeShootingMode = ActiveShootingMode.Short_Lazer;
                    SoundManager.instance.PlaySound("powerUp");
                    break;
                case BonusType.rocket:
                    PlayerShooting.instance.activeShootingMode = ActiveShootingMode.Rocket;
                    SoundManager.instance.PlaySound("powerUp");
                    break;
                /*case BonusType.swirling:
                    PlayerShooting.instance.activeShootingMode = ActiveShootingMode.Swirling;
                    SoundManager.instance.PlaySound("powerUp");
                    break;*/
                case BonusType.ray:
                    SoundManager.instance.PlaySound("ray");     //play sound from 'SoundManager'
                    PlayerShooting.instance.activeShootingMode = ActiveShootingMode.Ray;
                    break;
            }
            //if getting a bonus of any type updating the Player's sprite and destroying the bonus
            Player.instance.UpdateSkin();
            Destroy(gameObject);
        }
    }
}
