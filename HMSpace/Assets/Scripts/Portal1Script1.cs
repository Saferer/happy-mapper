using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal1Script1 : MonoBehaviour
{

    bool alreadyCollide = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("In function");
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colliding with portal");
            alreadyCollide = true;
            MapState.Location = other.gameObject.GetComponent<Transform>().position;
            MapState.CurrentLevel = 1;
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            Debug.Log("Not colliding with portal");

        }
    }
}
