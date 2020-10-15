using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public GameObject levelButtonPrefab;

    public LevelManagerScript levelManager;

    private void Start()
    {
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            GameObject spawnedButton = Instantiate(levelButtonPrefab, transform);
            LevelSelectionButton button = spawnedButton.GetComponent<LevelSelectionButton>();

            button.textName.text = "Level " + i;
            button.levelNum = i;
        }

    }
}
