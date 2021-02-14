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

    public List<int> levelBreaks;

    public bool forceUnlock;

    private void Start()
    {
        bool previousLevelCompleted = true;

        float count = 0;

        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            //Debug.Log(SceneManager.GetSceneByBuildIndex(i).name);

            if (forceUnlock || LevelCompleted(sceneName) || previousLevelCompleted)
            {
                GameObject spawnedButton = Instantiate(levelButtonPrefab, transform);
                LevelSelectionButton button = spawnedButton.GetComponent<LevelSelectionButton>();

                button.textName.text = i.ToString();
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

            count += 2;

            if (levelBreaks.Contains(i))
            {
                //Lets add a split in here
                for (int b = 0; b < 3; b++)
                {
                    Instantiate(fillerPrefab, transform);
                    count++;
                }
            }

        }


        //Lets adjust the size of the area


        GridLayoutGroup layoutGroup = GetComponent<GridLayoutGroup>();

        Vector2 size = GetComponent<RectTransform>().sizeDelta;

        Vector2 cellSize = layoutGroup.cellSize;

        Vector2 spacing = layoutGroup.spacing;

        count = count / 6;
        count = count * 2;

        count = Mathf.Ceil(count);

        size.y = (cellSize.y + spacing.y) * count;

        size.y += layoutGroup.padding.bottom;

        size.y += layoutGroup.padding.top;

        size.y += 50;

        GetComponent<RectTransform>().sizeDelta = size;

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
