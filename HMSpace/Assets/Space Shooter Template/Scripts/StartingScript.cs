using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


/// <summary>
/// Starting script is designed to load the main scene
/// </summary>

public class StartingScript : MonoBehaviour {

    public GameObject playButton;
    public Sprite loadingButtonSprite;
    public GameObject SettingsButton, MusicButton, SoundButton;
    public Sprite MusicOnSprite, MusicOffSprite, SoundOnSprite, SoundOffSprite;
    public Scene SceneToLoad;
    bool settingsAreOpen = false;

    private void Start()
    {
        //check if sound and music are on
        if (PlayerPrefs.GetString("Music") == "Off")
            MusicButton.GetComponent<Image>().sprite = MusicOffSprite;
        else
            MusicButton.GetComponent<Image>().sprite = MusicOnSprite;
        if (PlayerPrefs.GetString("Sound") == "Off")
            SoundButton.GetComponent<Image>().sprite = SoundOffSprite;
        else
            SoundButton.GetComponent<Image>().sprite = SoundOnSprite;
    }

    // Load the scene which is by index 1 in the builded scenes list
    public void LoadMainScene()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject.name);
        if (EventSystem.current.currentSelectedGameObject.name == "Level1"){
            SceneManager.LoadScene("Level_1");
        }
        if (EventSystem.current.currentSelectedGameObject.name == "Level2")
        {
            SceneManager.LoadScene("Level_2");
        }

    }

    public void OpenSettings ()
    {
        if (settingsAreOpen)
            SettingsButton.GetComponent<Animator>().SetTrigger("Close");
        else
            SettingsButton.GetComponent<Animator>().SetTrigger("Open");
        settingsAreOpen = !settingsAreOpen;
    }

    public void SwitchMusic()
    {
        if (PlayerPrefs.GetString("Music") == "Off")
        {
            PlayerPrefs.SetString("Music", "On");
            MusicButton.GetComponent<Image>().sprite = MusicOnSprite;
        }
        else
        {
            PlayerPrefs.SetString("Music", "Off");
            MusicButton.GetComponent<Image>().sprite = MusicOffSprite;
        }
    }

    public void SwitchSound()
    {
        if (PlayerPrefs.GetString("Sound") == "Off")
        {
            PlayerPrefs.SetString("Sound", "On");
            SoundButton.GetComponent<Image>().sprite = SoundOnSprite;
        }
        else
        {
            PlayerPrefs.SetString("Sound", "Off");
            SoundButton.GetComponent<Image>().sprite = SoundOffSprite;
        }
    }
}
