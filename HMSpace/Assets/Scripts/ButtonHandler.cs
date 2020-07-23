using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
  bool pressed = false;

  public void changeScene(GameObject percentage){
    if (pressed == false){StaticEMG.Instance.EMG.setMax(45);}
    MapState.CurrentLevel = 0;
    MapState.Location = new Vector2(-2.1f, 0.89f);
    MapState.NumPressed = 0;
    SceneManager.LoadScene("map");
    StreamWriter F = new StreamWriter("./levelMem.txt");
    F.WriteLine(percentage.GetComponent<UnityEngine.UI.Slider>().value);
    //F.WriteLine("It worked");
    F.Close();
  }

  public void calibrate(){
    pressed = true;
    StaticEMG.Stop();
    StaticEMG.Instance.EMG.calibrateMax();
    StaticEMG.Run();
  }

  public void quitGame(){
    // UnityEditor.EditorApplication.isPlaying = false;
    Application.Quit();
  }
}
