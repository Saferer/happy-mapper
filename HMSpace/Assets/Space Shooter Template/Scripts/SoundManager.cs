using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores a list of different sounds and plays them when requested.
/// </summary>
public class SoundManager : MonoBehaviour {

    //audioclips used in the game
    [Tooltip("A music begins immediately after the start of the game")]
    public AudioClip backgroundMusic;

    [Tooltip("This sound is playing when the player ship makes a lazer shot")]
    public AudioClip shortLazer;
    [Tooltip("This sound is playing when the player ship makes a rocket shot")]
    public AudioClip rocket;
    [Tooltip("This sound is playing when the player ship makes a swirling projectile shot")]
    public AudioClip swirling;
    [Tooltip("This sound is playing when the player ship activates the ray")]
    public AudioClip ray;
    [Tooltip("This sound is playing when the enemy is destroyed")]
    public AudioClip explosion;
    [Tooltip("This sound is playing when the player ship picks up a coin")]
    public AudioClip coin;
    [Tooltip("This sound is playing when the player ship picks up a bonus")]
    public AudioClip bonus;

    AudioSource audioSource;
    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartMusic();
    }

    public void StartMusic()
    {
        if (backgroundMusic != null && Settings.instance.IsMusicOn())
        {
            audioSource.clip = backgroundMusic;
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (backgroundMusic != null)
        {
            audioSource.Stop();
        }
    }

    public void PlaySound(string sound)     //playing the chosen sound. use the method to play the sound
    {
        if (Settings.instance.IsSoundOn())
        {
            switch (sound)
            {
                case "shortLazer":
                    if (shortLazer != null)
                        audioSource.PlayOneShot(shortLazer);
                    break;
                case "rocket":
                    if (rocket != null)
                        audioSource.PlayOneShot(rocket);
                    break;
                case "swirling":
                    if (swirling != null)
                        audioSource.PlayOneShot(swirling);
                    break;
                case "ray":
                    if (ray != null)
                        audioSource.PlayOneShot(ray);
                    break;
                case "explosion":
                    if (explosion != null)
                        audioSource.PlayOneShot(explosion);
                    break;
                case "coin":
                    if (coin != null)
                        audioSource.PlayOneShot(coin);
                    break;
                case "powerUp":
                    if (bonus != null)
                        audioSource.PlayOneShot(bonus);
                    break;
                default:
                    break;
            }
        }
    }
}
