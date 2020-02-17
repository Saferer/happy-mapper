using UnityEngine;
#if USE_SERVICES && UNITY_ANDROID
using GooglePlayGames;
#endif

public class Services : MonoBehaviour {

    public string leaderboardID;

    //Create a singelton object
    public static Services instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);        
    }

    public static bool ServicesAreEnable
    {
        get
        {
            if (PlayerPrefs.GetString("LeaderboardIsEnable") == "true")
                return true;
            else
                return false;
        }
        set
        {
            if (value == true)
            {
                PlayerPrefs.SetString("LeaderboardIsEnable", "true");                
            }
            else
                PlayerPrefs.SetString("LeaderboardIsEnable", "false");
            PlayerPrefs.Save();
        }
    }

#if USE_SERVICES    
    private void Start()
    {
        ToAuthenticate();
    }

    void ToAuthenticate()
    {
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log("Login Sucess");
            }
            else
            {
                Debug.Log("Login failed");
            }
        });
    }
#endif
   

    public void ReportToLeaderboard(int score)
    {
#if USE_SERVICES
        Social.ReportScore(score, leaderboardID, (bool success) => {
        });
#endif
    }

    public void ShowLeaderboard()
    {
#if USE_SERVICES
        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(leaderboardID);
#else
        Debug.LogError("Services are not enabled");
#endif
    }

}
