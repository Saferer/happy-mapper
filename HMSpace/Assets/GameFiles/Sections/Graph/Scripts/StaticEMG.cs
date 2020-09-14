using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using I4HUSB;
using System.Threading;
using SYS = System.Diagnostics;


public class StaticEMG : MonoBehaviour
{
    private static StaticEMG instance = null;
    private EMGReader emg = null;
    private static bool running = false;
    private static Thread childThread;

    public bool debugMode = false;
    public double debugValue = 0;

    public List<float> debugRecorded;
    public double debugMaxGoalValue = 0;
    private long debugTimer;

    private BoolWrapper signal;
    private StaticEMG()
    {

    }

    public static StaticEMG Instance
    {
        get
        {
            return instance;
        }
    }
    public EMGReader EMG { get { return emg; } }

    private void Awake()
    {
        if (emg == null)
        {
            emg = new EMGReader(debugMode);
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

    public void StartRecord(int timeSeconds, BoolWrapper signal)
    {
        if (!debugMode)
            EMG.StartRecord(timeSeconds, signal);
        Debug.Log("Started Debug Record");
        debugRecorded = new List<float>();
        debugTimer = timeSeconds * 1000;
        this.signal = signal;
        Thread debugThread = new Thread(DebugThreadRun);
        debugThread.Start();
    }

    public List<float> GetRecordedValues()
    {
        if (!debugMode)
            return EMG.GetRecordedValues();

        return new List<float>(debugRecorded);
    }

    public double GetPercentage()
    {
        if (!debugMode)
            return EMG.getPercentage();
        return debugValue / debugMaxGoalValue;
    }

    public void OnApplicationQuit()
    {
        this.EMG.close();
    }

    private void DebugThreadRun()
    {
        SYS.Stopwatch stopwatch = new SYS.Stopwatch();
        stopwatch.Reset();
        stopwatch.Start();
        while (stopwatch.ElapsedMilliseconds < debugTimer)
        {
            Debug.Log(debugValue + ":" + StaticEMG.Instance.signal.value);
            debugRecorded.Add((float)debugValue);
            Thread.Sleep(50);
        }
        StaticEMG.Instance.signal.value = false;
    }


}
