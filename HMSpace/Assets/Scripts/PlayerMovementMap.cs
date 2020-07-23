using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerMovementMap : MonoBehaviour
{
//System.IO.StreamWriter file = new StreamWriter("levelMem.txt", false);
//System.IO.StreamReader sr = new StreamReader("levelMem.txt");
  public GameObject portal1;
  public GameObject portal2;
  static int numPressed =0;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(numPressed);
        //numPressed = Int32.Parse(sr.ReadLine());
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Return)){
          numPressed++;
          Debug.Log("Return key was pressed.");
          //controller.Fly(40f * Time.fixedDeltaTime);
          if (numPressed==1){
          GetComponent<Transform>().position=Vector2.MoveTowards(GetComponent<Transform>().position, portal1.GetComponent<Transform>().position, 5f);
          //file.WriteLine("1");

          //portal1.SetActive(false);
        }
          else if(numPressed==2){
            GetComponent<Transform>().position=Vector2.MoveTowards(GetComponent<Transform>().position, portal2.GetComponent<Transform>().position, 5f);
            //portal2.SetActive(false);
            //file.WriteLine("2");
          }
          }

          if (Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene("Menu");
          }
        }
          // file.Close();
          // sr.Close();

}
