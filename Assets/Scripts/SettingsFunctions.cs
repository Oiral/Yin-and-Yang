using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsFunctions : MonoBehaviour
{
    public Sprite soundOnImage;
    public Sprite soundOffImage;

    public Image musicButtonImage;
    public Image sfxButtonImage;

    private void Start()
    {
        ToggleMusic();
        ToggleMusic();

        ToggleSFX();
        ToggleSFX();
    }

    public void ToggleMusic()
    {
        if (AudioManager.instance == null)
            return;

        bool state = !AudioManager.instance.MusicAudio;

        AudioManager.instance.MusicAudio = state;

        musicButtonImage.sprite = state ? soundOnImage : soundOffImage;
    }

    public void ToggleSFX()
    {
        //If there is not instance manager found, We don't want to continue
        if (AudioManager.instance == null)
            return;

        //Lets change the audio state of the SFX
        bool state = !AudioManager.instance.SFXAudio;

        AudioManager.instance.SFXAudio = state;

        //We need to change the visuals of the button
        sfxButtonImage.sprite = state ? soundOnImage : soundOffImage;
    }
}
