using System.Collections;
using System.Collections.Generic;
using SysDiag = System.Diagnostics;
using System;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{

    private Vector3 ScreenTopPosition;
    private Vector3 ScreenBotPosition;

    private float MinimumPositionOffset = 1.3f;
    public GameObject Coin;

    private bool StartSpawn = true;

    private int NumberSpawned = 0;

    private SysDiag.Stopwatch IntervalTimer;

    private SysDiag.Stopwatch SpawnTimer;

    private System.Random random;

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();
        IntervalTimer = new SysDiag.Stopwatch();
        SpawnTimer = new SysDiag.Stopwatch();
        ScreenTopPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        ScreenBotPosition = Camera.main.ScreenToWorldPoint(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        if (StartSpawn)
        {
            StartSpawn = false;
            SpawnTimer.Reset();
            SpawnTimer.Start();
            IntervalTimer.Reset();
            IntervalTimer.Start();
        }

        if (NumberSpawned > 2)
        {
            NumberSpawned = 0;
            IntervalTimer.Stop();
            IntervalTimer.Reset();
        }

        if (IntervalTimer.ElapsedMilliseconds > 400)
        {
            IntervalTimer.Restart();
            GameObject coin = Instantiate(Coin,
            new Vector3(14, (ScreenTopPosition.y - ScreenBotPosition.y - MinimumPositionOffset) * 0.5f + ScreenBotPosition.y + MinimumPositionOffset, 0),
            Coin.transform.rotation);
            coin.SetActive(true);
            NumberSpawned++;
        }

        if (SpawnTimer.ElapsedMilliseconds > random.Next(5000, 7000))
        {
            SpawnTimer.Stop();
            SpawnTimer.Reset();
            StartSpawn = true;
        }
    }
}
