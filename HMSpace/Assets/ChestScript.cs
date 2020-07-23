using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
  public GameObject scoreIn;
  string scoreText;
  int score;
  string[] scoreArray;

  //string[] separator = {" ", ";"};
    // Start is called before the first frame update
    void Start()
    {
      gameObject.transform.localScale = new Vector3(0,0,0);

    }

    // Update is called once per frame
    void Update()
    {

      scoreText = scoreIn.GetComponent<UnityEngine.UI.Text>().text;
      Debug.Log(scoreText);
      scoreArray = scoreText.Split(' ');
      score = int.Parse(scoreArray[1]);
      Debug.Log(score);
      if (score%25>=0 && score%25<=5 && score > 9){//(score%10==0){
      Debug.Log("chest activate");
      gameObject.transform.localScale = new Vector3(1,1,1);
    } else {
      gameObject.transform.localScale = new Vector3(0,0,0);
    }

    }
}
