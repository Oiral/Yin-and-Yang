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
    public GameObject fillerPrefab;

    public LevelManagerScript levelManager;


    private void Start()
    {
        bool previousLevelCompleted = true;

        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            //Debug.Log(SceneManager.GetSceneByBuildIndex(i).name);

            if (LevelCompleted(sceneName) || previousLevelCompleted)
            {
                GameObject spawnedButton = Instantiate(levelButtonPrefab, transform);
                LevelSelectionButton button = spawnedButton.GetComponent<LevelSelectionButton>();

                button.textName.text = "Level " + i;
                button.levelNum = i;
                previousLevelCompleted = LevelCompleted(sceneName);
                button.moveCountNum.text = GetMoveCount(sceneName);
            }
            else
            {
                GameObject spawnedButton = Instantiate(lockedButtonPrefab, transform);
                LevelSelectionButton button = spawnedButton.GetComponent<LevelSelectionButton>();

                button.textName.text = i.ToString();
                previousLevelCompleted = false;
            }

            Instantiate(fillerPrefab, transform);

        }

    }

    public bool LevelCompleted(string sceneName)
    {
        //Debug.Log(sceneName);
        if (PlayerPrefs.HasKey(sceneName))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public string GetMoveCount(string sceneName)
    {
        if (PlayerPrefs.HasKey(sceneName))
        {
            return PlayerPrefs.GetInt(sceneName).ToString();
        }
        else
        {
            return "- -";
        }
    }
}
