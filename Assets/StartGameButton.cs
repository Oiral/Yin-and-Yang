using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StartGameButton : MonoBehaviour
{
    public Text startButtonText;
    public string StartText = "Start";
    public string ContinueText = "Continue";

    private void Start()
    {
        if (PlayerPrefs.HasKey(LevelManagerScript.PrefsCurrentLevelKey))
        {
            startButtonText.text = "Continue";
        }
        else
        {
            startButtonText.text = "Start";
        }
    }
}
