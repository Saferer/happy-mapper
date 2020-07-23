using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{

    private Vector3 backPos;
    private float X;
    private float Y;


       void OnBecameInvisible()
    {
        //calculate current position
        backPos = GetComponent<Transform>().position;
        Debug.Log(backPos);
        //calculate new position
        X = backPos.x + GetComponent<SpriteRenderer>().sprite.bounds.size.x/2;
        Y = backPos.y;
        //move to new position when invisible
        GetComponent<Transform>().position = new Vector3(X, Y, 0f);
        Debug.Log(GetComponent<Transform>().position);
    }

}

