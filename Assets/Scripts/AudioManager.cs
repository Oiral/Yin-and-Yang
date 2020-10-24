using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager instance;

    string sfxPrefName = "sfxPlaying";
    string musicPrefName = "musicPlaying";

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

        if (PlayerPrefs.HasKey(musicPrefName))
            MusicAudio = (PlayerPrefs.GetInt(musicPrefName) == 1);
        if (PlayerPrefs.HasKey(sfxPrefName))
            SFXAudio = (PlayerPrefs.GetInt(sfxPrefName) == 1);
    }

    #endregion


    public AudioSource musicSource;
    public AudioSource sfxSource;

    public bool MusicAudio
    {
        get
        {
            return !musicMuted;
        }
        set
        {
            PlayerPrefs.SetInt(musicPrefName, value ? 1 : 0);
            musicSource.volume = value ? 1 : 0;
            musicMuted = !value;
        }
    }

    public bool SFXAudio
    {
        get
        {
            return !sfxMuted;
        }
        set
        {
            PlayerPrefs.SetInt(sfxPrefName, value ? 1 : 0);
            sfxSource.volume = value ? 1 : 0;
            sfxMuted = !value;
        }
    }

    bool musicMuted;
    bool sfxMuted;

    public AudioClip[] musicTrack;

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

    public void Update()
    {
        Debug.Log(musicSource.isPlaying);
        if (musicSource.isPlaying == false)
        {
            musicSource.clip = musicTrack[Random.Range(0, musicTrack.Length - 1)];
            musicSource.Play();
        }
    }
}
