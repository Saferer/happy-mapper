using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using I4HUSB;
using System.Threading;


public class StaticEMG : MonoBehaviour
{
    private static StaticEMG instance = null;
    private EMGReader emg;
    private static bool running = false;
    private static Thread childThread;

    private StaticEMG()
    {
        emg = new EMGReader();
    }

    public static StaticEMG Instance { get { return instance; } }
    public EMGReader EMG { get { return emg; } }

    private void Awake()
    {
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

    public void OnApplicationQuit(){
      this.EMG.close();
    }
}
