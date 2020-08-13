using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using I4HUSB;
using System.Threading;


public class StaticEMG : MonoBehaviour
{
    private static StaticEMG instance = null;
    private EMGReader emg = null;
    private static bool running = false;
    private static Thread childThread;

    public double debugValue = 0;

    public double debugMaxGoalValue = 0;
    private StaticEMG()
    {

    }

    public static StaticEMG Instance { get { return instance; } }
    public EMGReader EMG { get { return emg; } }

    private void Awake()
    {
        if (emg == null)
        {
            emg = new EMGReader(true);
        }
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Update()
    {
        Instance.EMG.RunningAverage = debugValue;
        Instance.EMG.setGoal(debugMaxGoalValue);
    }

    public static void Run()
    {
        if (!running)
        {
            Instance.EMG.setFlag(true);
            childThread = new Thread(Instance.EMG.run);
            childThread.Start();
            running = true;
        }
    }

    public static void Stop()
    {
        if (running)
        {
            Instance.EMG.setFlag(false);
            running = false;
            childThread.Join();

        }
    }


    public void OnApplicationQuit()
    {
        this.EMG.close();
    }
}
