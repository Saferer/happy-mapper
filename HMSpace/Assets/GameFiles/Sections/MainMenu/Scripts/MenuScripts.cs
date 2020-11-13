using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScripts : MonoBehaviour
{
    private void Start()
    {

    }

    private void Update()
    {
        Debug.Log("MenuScript: Update: Current Goal is " + StaticEMG.Instance.EMG.getGoal());
    }

    public void LoadGraph()
    {
        SceneManager.LoadScene("Graph");
    }

    public void LoadRed()
    {
        SceneManager.LoadScene("Level1");
    }

    public void LoadSpace2()
    {
        SceneManager.LoadScene("Level_1");
    }
    public void LoadSpace1()
    {
        SceneManager.LoadScene("Level_2");
    }

}
