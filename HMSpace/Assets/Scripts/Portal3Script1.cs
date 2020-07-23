using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Portal3Script1 : MonoBehaviour
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
            MapState.Location = other.gameObject.GetComponent<Transform>().position;
            MapState.CurrentLevel = 3;
            Debug.Log("Colliding with portal");
            alreadyCollide = true;
            SceneManager.LoadScene("Level3");
        }
        else
        {
            Debug.Log("Not colliding with portal");

        }
    }
}
