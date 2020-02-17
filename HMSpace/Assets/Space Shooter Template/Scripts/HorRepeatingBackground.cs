using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorRepeatingBackground : MonoBehaviour {

    [Tooltip("horizontal size of the sprite in the world space. Attach box collider2D to get the exact size")]
    public float horizontalSize;

    private void Update()
    {
        if (transform.position.x < -horizontalSize) 
        {
            RepositionBackground();
        }
    }

    void RepositionBackground()
    {
        Vector2 groundOffSet = new Vector2(horizontalSize * 2f, 0);
        transform.position = (Vector2)transform.position + groundOffSet;
    }
}
