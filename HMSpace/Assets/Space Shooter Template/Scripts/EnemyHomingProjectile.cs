using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script rotates attached projectile in ‘Player’s’ direction.
/// </summary>
public class EnemyHomingProjectile : Projectile {

    public float speed;

    //rotating the object in 'Player's' direction
    private void OnEnable() 
    {
        if (Player.instance != null)
            transform.up = Player.instance.transform.position - transform.position;
    }
    
}
