using System.Collections;
using System.Collections.Generic;
using SysDiag = System.Diagnostics;
using System;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{

    private Vector3 ScreenTopPosition;
    private Vector3 ScreenBotPosition;

    private float MinimumPositionOffset = 1.2f;
    public GameObject Coin;

    private bool StartSpawn = false;

    private int NumberSpawned = 0;

    private float timer = 3f;

    private float intervalTimer = 0.4f;
    //private SysDiag.Stopwatch IntervalTimer;

    private System.Random random;

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();
        //IntervalTimer = new SysDiag.Stopwatch();
        // SpawnTimer = new SysDiag.Stopwatch();
        ScreenTopPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        ScreenBotPosition = Camera.main.ScreenToWorldPoint(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            StartSpawn = true;
            timer = UnityEngine.Random.Range(5f, 7f);
        }

        if (StartSpawn)
        {
            intervalTimer -= Time.deltaTime;
            if (intervalTimer <= 0)
            {
                intervalTimer = 0.4f;
                GameObject coin = Instantiate(Coin,
                new Vector3(14, (ScreenTopPosition.y - ScreenBotPosition.y - MinimumPositionOffset) * 0.5f + ScreenBotPosition.y + MinimumPositionOffset, 0),
                Coin.transform.rotation);
                coin.SetActive(true);
                NumberSpawned++;
            }
            if (NumberSpawned > 2)
            {
                NumberSpawned = 0;
                StartSpawn = false;
                intervalTimer = 0.4f;
            }
        }
        // if (StartSpawn)
        // {
        //     StartSpawn = false;

        //     // SpawnTimer.Reset();
        //     // SpawnTimer.Start();
        //     IntervalTimer.Reset();
        //     IntervalTimer.Start();
        // }

        // if (NumberSpawned > 2)
        // {
        //     NumberSpawned = 0;
        //     IntervalTimer.Stop();
        //     IntervalTimer.Reset();
        // }

        // if (IntervalTimer.ElapsedMilliseconds > 400)
        // {
        //     IntervalTimer.Restart();
        //     GameObject coin = Instantiate(Coin,
        //     new Vector3(14, (ScreenTopPosition.y - ScreenBotPosition.y - MinimumPositionOffset) * 0.5f + ScreenBotPosition.y + MinimumPositionOffset, 0),
        //     Coin.transform.rotation);
        //     coin.SetActive(true);
        //     NumberSpawned++;
        // }

        // if (SpawnTimer.ElapsedMilliseconds > random.Next(5000, 7000))
        // {
        //     // SpawnTimer.Stop();
        //     // SpawnTimer.Reset();
        //     StartSpawn = true;
        // }
    }
}
