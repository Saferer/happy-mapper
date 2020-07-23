using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerMovementMap1 : MonoBehaviour
{
    //System.IO.StreamWriter file = new StreamWriter("levelMem.txt", false);
    //System.IO.StreamReader sr = new StreamReader("levelMem.txt");
    public GameObject portal1;
    public GameObject portal2;
    public GameObject portal3;
    public GameObject key;
    public GameObject youWin;
    // Start is called before the first frame update
    void Start()
    {

      youWin.gameObject.transform.localScale = new Vector3(0,0,0);

        if (MapState.CurrentLevel >= 1)
        {
            portal1.SetActive(false);
        }
        if (MapState.CurrentLevel >= 2)
        {
            portal2.SetActive(false);
        }
        if (MapState.CurrentLevel >= 3)
        {
            portal3.SetActive(false);
        }

        if (MapState.CurrentLevel >= 1)
        {
            this.GetComponent<Transform>().position = MapState.Location;
        }
        Debug.Log(MapState.NumPressed );
        //MapState.NumPressed  = Int32.Parse(sr.ReadLine());
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<Transform>().position = Vector2.Lerp(GetComponent<Transform>().position, portal1.GetComponent<Transform>().position, 8f * Time.fixedDeltaTime);
        if (Input.GetKeyDown(KeyCode.Return))
        {
            MapState.NumPressed ++;
            Debug.Log("Return key was pressed.");
            //controller.Fly(40f * Time.fixedDeltaTime);
            if (MapState.NumPressed  == 1)
            {

                GetComponent<Transform>().position = Vector2.MoveTowards(GetComponent<Transform>().position, portal1.GetComponent<Transform>().position, 5f);
                //file.WriteLine("1");


                //portal1.SetActive(false);
            }
            else if (MapState.NumPressed  == 2)
            {
                GetComponent<Transform>().position = Vector2.MoveTowards(GetComponent<Transform>().position, portal2.GetComponent<Transform>().position, 5f);
                //portal2.SetActive(false);
                //file.WriteLine("2");
            }

            else if (MapState.NumPressed  == 3)
            {
                GetComponent<Transform>().position = Vector2.MoveTowards(GetComponent<Transform>().position, portal3.GetComponent<Transform>().position, 5f);
                //portal2.SetActive(false);
                //file.WriteLine("2");
            }

            else if (MapState.NumPressed  == 4)
            {
                MapState.NumPressed  = 0;
                GetComponent<Transform>().position = Vector2.MoveTowards(GetComponent<Transform>().position, key.GetComponent<Transform>().position, 5f);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }
    // file.Close();
    // sr.Close();

}
