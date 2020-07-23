using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using I4HUSB;
using System.Threading;




public class SetHeight : MonoBehaviour
{
  string percentage = "Not Retreived";
  float percentageLocation;
  public GameObject background;

    // Start is called before the first frame update
    void Start()
    {
      StreamReader F = new StreamReader("levelMem.txt");
      percentage = F.ReadLine();
      float percentageNum = float.Parse(percentage);
      float bgHeight = background.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
      float bgWidth = background.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
      float sub = bgHeight/2f;
      percentageLocation = bgHeight*(percentageNum*.1f) - sub;
      GetComponent<Transform>().position = new Vector3(background.transform.position.x + 1 +(background.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2), percentageLocation, 0f);
      F.Close();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
