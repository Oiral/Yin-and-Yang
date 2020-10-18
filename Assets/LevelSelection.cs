using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public GameObject lockedButtonPrefab;

    public LevelManagerScript levelManager;



    private void Start()
    {
        int highestLevel = 1;

        if (PlayerPrefs.HasKey("Level"))
        {
            highestLevel = PlayerPrefs.GetInt("Level");
            Debug.Log("Test");
        }

        Debug.Log(highestLevel);


        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (i <= highestLevel)
            {
                GameObject spawnedButton = Instantiate(levelButtonPrefab, transform);
                LevelSelectionButton button = spawnedButton.GetComponent<LevelSelectionButton>();

                button.textName.text = "Level " + i;
                button.levelNum = i;
            }
            else
            {
                GameObject spawnedButton = Instantiate(lockedButtonPrefab, transform);
                LevelSelectionButton button = spawnedButton.GetComponent<LevelSelectionButton>();

                button.textName.text = i.ToString();
            }

            
        }

    }
}
