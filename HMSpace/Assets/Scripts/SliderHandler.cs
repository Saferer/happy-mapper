using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SliderHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TextChange(GameObject textfeild){
      //Debug.Log(textfeild.GetComponent<UnityEngine.UI.Text>().text);
      //Debug.Log(GetComponent<UnityEngine.UI.Slider>().value);
      float percentage = GetComponent<UnityEngine.UI.Slider>().value*10;
      textfeild.GetComponent<UnityEngine.UI.Text>().text = "Desired percentage: "+percentage.ToString()+"%";
      //Debug.Log(textfeild.GetComponent<UnityEngine.UI.Text>().text);
    }
}
