using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class KeyScript : MonoBehaviour
{

    bool alreadyCollide = false;
    public GameObject youWin;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      if(alreadyCollide == true && Input.GetKeyDown(KeyCode.Return)){
                    SceneManager.LoadScene("Menu");
      }

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("In function");
        if (other.gameObject.CompareTag("Player"))
        {
            MapState.Location = gameObject.GetComponent<Transform>().position;
            MapState.CurrentLevel = 0;
            Debug.Log("Colliding with Key");
            alreadyCollide = true;
            youWin.gameObject.transform.localScale = new Vector3(1,1,1);
        }
        else
        {
            Debug.Log("Not colliding with Key");

        }
    }
}
