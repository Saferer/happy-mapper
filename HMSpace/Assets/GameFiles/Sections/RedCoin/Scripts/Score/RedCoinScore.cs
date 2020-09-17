using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SysDiag = System.Diagnostics;

public class RedCoinScore : MonoBehaviour
{
    public int Score = 0;

    private SysDiag.Stopwatch chestTimer;


    public static RedCoinScore _instance;
    public static RedCoinScore Instance

    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<RedCoinScore>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("RedCoinScore");
                    _instance = container.AddComponent<RedCoinScore>();
                }
            }

            return _instance;
        }
    }
    void Start()
    {
        chestTimer = new SysDiag.Stopwatch();

    }

    // Update is called once per frame
    void Update()
    {
        if (Instance.chestTimer.ElapsedMilliseconds > 3000)
        {
            Instance.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            Instance.chestTimer.Stop();
            Instance.chestTimer.Reset();
        }
    }
    public void increment()
    {
        Instance.Score++;
        Instance.gameObject.GetComponent<Text>().text = "Score:\n" + RedCoinScore.Instance.Score;
        if (Instance.Score % 30 == 0 && Instance.Score != 0)
        {
            Instance.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            Instance.chestTimer.Reset();
            Instance.chestTimer.Start();
        }
    }
}
