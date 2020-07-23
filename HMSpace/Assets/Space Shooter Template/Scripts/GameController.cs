using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// This script counts the amount of points and coins and shows it on the screen.
/// </summary>
[System.Serializable]
public class MenuIcons
{
    public GameObject short_lazerMenuIcon, rocketMenuIcon, swirlingMenuIcon, rayMenuIcon;
    public Sprite short_lazerMenuIconColor, rocketMenuIconColor, swirlingMenuIconColor, rayMenuIconColor;
    public Sprite short_lazerMenuIconWhite, rocketMenuIconWhite, swirlingMenuIconWhite, rayMenuIconWhite;
}

public class GameController : MonoBehaviour {

    public GameObject coinPrefab;
    [Range(0,100)]
    public int chanceForCoin = 10;

    Text scoreText;     //text displayed in 'Score' field to 'Main Canvas'
    Text coinsText;
    Text timeText;

    int score;
    int coins;
    int mins;
    int secs;
    float CurrentTime;

    public GameObject gameOverScreen;
    public Text finalScoreText;

    [HideInInspector] public bool gameIsActive;

    public static GameController instance;  //static reference for access from other classes

    #region menu fields

    public bool ChooseShipAtStart = false;
    public GameObject backPanel;
    public GameObject mainMenuPanel;
    public GameObject frontPanelShip;
    public GameObject frontPanelPause;
    public GameObject pauseButton;
    public MenuIcons menuShipIcons;

    bool settingsAreOpen = false;

#endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;
        scoreText = GameObject.FindWithTag("Score").GetComponent<Text>();
        coinsText = GameObject.FindWithTag("CoinsText").GetComponent<Text>();
        timeText = GameObject.FindWithTag("TimeText").GetComponent<Text>();
        gameOverScreen.SetActive(false);

        if (ChooseShipAtStart)
            OpenChooseShipMenu();
        else
            CloseMenuPanel();
    }

    private void Start()
    {
        score = 0;
        scoreText.text = score.ToString();
        coins = 0;
        coinsText.text = coins.ToString();
        CurrentTime = 0f;
        timeText.text = "0:0";


    }

    public void Update()
    {
        float t = CurrentTime += 1 * Time.deltaTime;

        mins = ((int)t / 60);
        secs = Mathf.RoundToInt(t % 60);
        if (secs == 60)
            secs = 0;
        timeText.text = mins.ToString() + ":" + secs.ToString();
    }

    public void AddScore(int count)     //updating 'Score' and display on 'Main Canvas'
    {
        score += count;
        scoreText.text = score.ToString();
    }

    public void AddCoin()
    {
        coins++;
        coinsText.text = coins.ToString();
        SoundManager.instance.PlaySound("coin");
    }

#region Menu Controlling
    public void SettingsButtonActivation()
    {
        if (!settingsAreOpen)
        {
            backPanel.GetComponent<Animator>().SetTrigger("On");
            settingsAreOpen = true;
        }
        else
        {
            backPanel.GetComponent<Animator>().SetTrigger("Off");
            settingsAreOpen = false;
        }
    }

    void OpenChooseShipMenu()
    {
        mainMenuPanel.SetActive(true);
        frontPanelShip.SetActive(true);
        frontPanelPause.SetActive(false);
        StopTheGame();
    }

    public void OpenPauseMenu()
    {
        mainMenuPanel.SetActive(true);
        frontPanelShip.SetActive(false);
        frontPanelPause.SetActive(true);
        StopTheGame();
    }

    void StopTheGame()
    {
        Time.timeScale = 0;
        PlayerMoving.instance.controlIsActive = false;
        pauseButton.SetActive(false);
    }

    public void CloseMenuPanel()
    {
        mainMenuPanel.SetActive(false);
        //PlayerMoving.instance.controlIsActive = true; //Removed in order to stop the player moving with the mouse pointer
        Time.timeScale = 1;
        pauseButton.SetActive(true);
        gameIsActive = true;
    }

    public void SetStartSkin(string mode)
    {
        switch (mode)
        {
            case "short_lazer":
                PlayerShooting.instance.activeShootingMode = ActiveShootingMode.Short_Lazer;
                DisableMenuIcons();
                menuShipIcons.short_lazerMenuIcon.GetComponent<Image>().sprite = menuShipIcons.short_lazerMenuIconColor;
                break;
            case "rocket":
                PlayerShooting.instance.activeShootingMode = ActiveShootingMode.Rocket;
                DisableMenuIcons();
                menuShipIcons.rocketMenuIcon.GetComponent<Image>().sprite = menuShipIcons.rocketMenuIconColor;
                break;
            case "ray":
                PlayerShooting.instance.activeShootingMode = ActiveShootingMode.Ray;
                DisableMenuIcons();
                menuShipIcons.rayMenuIcon.GetComponent<Image>().sprite = menuShipIcons.rayMenuIconColor;
                break;
            case "swirling":
                PlayerShooting.instance.activeShootingMode = ActiveShootingMode.Swirling;
                DisableMenuIcons();
                menuShipIcons.swirlingMenuIcon.GetComponent<Image>().sprite = menuShipIcons.swirlingMenuIconColor;
                break;
        }
        Player.instance.UpdateSkin();
    }

    void DisableMenuIcons()
    {
        menuShipIcons.short_lazerMenuIcon.GetComponent<Image>().sprite = menuShipIcons.short_lazerMenuIconWhite;
        menuShipIcons.rocketMenuIcon.GetComponent<Image>().sprite = menuShipIcons.rocketMenuIconWhite;
        menuShipIcons.rayMenuIcon.GetComponent<Image>().sprite = menuShipIcons.rayMenuIconWhite;
        menuShipIcons.swirlingMenuIcon.GetComponent<Image>().sprite = menuShipIcons.swirlingMenuIconWhite;
    }
    #endregion

    public void GameOver()
    {
        StartCoroutine(GameOverCor());
    }

    IEnumerator GameOverCor()
    {
        yield return new WaitForSeconds(0.5f);
        gameOverScreen.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        finalScoreText.text = score + "\n"+"scores";
        if (Services.ServicesAreEnable)
            Services.instance.ReportToLeaderboard(score);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenStartScreen()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
