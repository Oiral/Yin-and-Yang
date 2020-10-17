using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform);
        }else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    #endregion


    public AudioSource musicSource;
    public AudioSource sfxSource;


    [Header("Sound effects")]
    public AudioClip moveSFX;
    public AudioClip cannotMoveSFX;
    public AudioClip teleportSFX;
    public AudioClip slimeSFX;
    public AudioClip winSFX;

    public void PlaySound(string soundName)
    {
        switch (soundName)
        {
            case "Move":
                PlaySound(moveSFX);
                break;

            case "Not Move":
                PlaySound(cannotMoveSFX);
                break;
            case "Teleport":
                PlaySound(teleportSFX);
                break;

            case "Slime":
                PlaySound(slimeSFX);
                break;
            case "Win":
                PlaySound(winSFX);
                break;

            default:
                break;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null || sfxSource == null)
            return;

        sfxSource.clip = clip;
        sfxSource.Play();
    }
}
