using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

    public GameObject musicIcon, soundIcon;

    public static Settings instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        if (IsSoundOn())
        {
            soundIcon.GetComponent<Animator>().SetTrigger("IsActive");
        }
        else
        {
            soundIcon.GetComponent<Animator>().SetTrigger("NotActive");
        }

        if (IsMusicOn())
            musicIcon.GetComponent<Animator>().SetTrigger("Active");
        else
            musicIcon.GetComponent<Animator>().SetTrigger("NotActive");
    }

    public void SoundButtonPush()
    {
        if (IsSoundOn())
            SwitchSoundOff();
        else
            SwitchSoundOn();
    }

    public void MusicButtonPush()
    {
        if (IsMusicOn())
            SwitchMusicOff();
        else
            SwitchMusicOn();
    }

    void SwitchMusicOn()
    {
        PlayerPrefs.SetString("Music", "On");
        musicIcon.GetComponent<Animator>().SetTrigger("On");
        SoundManager.instance.StartMusic();
    }

    void SwitchMusicOff()
    {
        PlayerPrefs.SetString("Music", "Off");
        musicIcon.GetComponent<Animator>().SetTrigger("Off");
        SoundManager.instance.StopMusic();
    }

    void SwitchSoundOn()
    {
        PlayerPrefs.SetString("Sound", "On");
        soundIcon.GetComponent<Animator>().SetTrigger("On");
    }

    void SwitchSoundOff()
    {
        PlayerPrefs.SetString("Sound", "Off");
        soundIcon.GetComponent<Animator>().SetTrigger("Off");
    }

    public bool IsMusicOn()
    {
        if (PlayerPrefs.GetString("Music") == "Off")
            return false;
        else
            return true;
    }

    public bool IsSoundOn()
    {
        if (PlayerPrefs.GetString("Sound") == "Off")
            return false;
        else
            return true;
    }
}
