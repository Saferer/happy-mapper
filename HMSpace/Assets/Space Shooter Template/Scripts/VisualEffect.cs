﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script attaches to ‘VisualEffect’ objects. It destroys or deactivates them after the defined time.
/// </summary>
public class VisualEffect : MonoBehaviour {

    [Tooltip("the time after object will be destroyed")]
    public float destructionTime;

    [Tooltip("whether 'pooling' is used or not")]
    public bool isPooled;

    private void OnEnable()
    {
        StartCoroutine(Destruction()); //launching the timer of destruction
    }

    IEnumerator Destruction() //wait for the estimated time, and destroying or deactivating the object
    {
        yield return new WaitForSeconds(destructionTime); 
        if (isPooled)                       
            gameObject.SetActive(false); 
        else
            Destroy(gameObject);
    }
}
