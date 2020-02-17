using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// defines how frequently and what damage the ray deals. It adds visual effect when the weapon power rises.
/// When colliding with the ‘Player’ it sends the command: ‘receive damage’.
/// </summary>

//video effects for different stages of ray power
[System.Serializable]

public class PlayerRay : MonoBehaviour {

    [Tooltip("how often 'Enemy' receives damage from the ray in seconds")]
    public float frequency;

    [Tooltip("ray power on the first stage")]
    public int damage;

    private void Start()
{
    }

    //when colliding with the 'Enemy' sending the command 'receive damage'
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().GetIndestructibleDamage(frequency, damage*PlayerShooting.instance.weaponPower);
        }
    }

}
