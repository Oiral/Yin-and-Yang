using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class LevelManagerScript : MonoBehaviour {

	public int currentLevel = 0;

    //public static LevelManagerScript instance;

    public float waitTime = 2f;

    public static string PrefsCurrentLevelKey = "CurrentLevel";
    /*
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    */

    private void Start()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;

        if (PlayerController.instance == null)
        {
            return;
        }
        int highScore = 0;

        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name))
        {
            highScore = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name);
        }

        //We want to set our highscore
        PlayerController.instance.bestMoveCount = highScore;
    }

    public void NextLevel()
    {
        //Debug.Log("Scene loading");

        //UIScript.instance.SetMainMenu(false);
        //UIScript.instance.SetPauseMenu(false);

        if (currentLevel == 0)
        {
            SceneManager.LoadScene(currentLevel += 1);
        }else
        {
            StartCoroutine(WaitForNextLevel());

        }
        
    }

    IEnumerator WaitForNextLevel()
    {
        AudioManager.instance.PlaySound("Win");
        yield return new WaitForSeconds(waitTime * 0.1f);

        PlayerController.instance.MassAnimation("Win");

        yield return new WaitForSeconds(waitTime * 0.7f);

        SaveScore(PlayerController.instance.moveCount);
        //FindBoard
        if (GameObject.FindGameObjectWithTag("MainCamera") != null)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<RotateBoard>().targetHeight = 15;
        }

        PlayerPrefs.SetInt(PrefsCurrentLevelKey, currentLevel + 1);
        //PlayerController.instance.moveCount



        Analytics.CustomEvent("Level Complete", new Dictionary<string, object>() {

            {"LevelIndex", SceneManager.GetActiveScene().buildIndex},
            {"LevelName", SceneManager.GetActiveScene().name },
            {"Move",  PlayerController.instance.moveCount},
            {"Best Move", PlayerPrefs.GetInt(SceneManager.GetActiveScene().name)
    }
        });

        

        if (currentLevel + 1 == SceneManager.sceneCountInBuildSettings)
        {
            yield return new WaitForSeconds(waitTime * 0.2f);
            PlayerPrefs.DeleteKey(PrefsCurrentLevelKey);
            LoadMainMenu();
            yield break;
        }

        AdManager.instance.RunAd();

        yield return new WaitForSeconds(waitTime * 0.2f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(currentLevel += 1);

        

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        //Reaches this point when its loaded
        //SceneManager.UnloadSceneAsync(currentLevel - 1);

    }

    public void LoadMainMenu()
    {
        currentLevel = 0;
        SceneManager.LoadScene(0);
        //UIScript.instance.SetMainMenu(true);
        //UIScript.instance.SetPauseMenu(false);
        //UIScript.instance.SetWinMenu(false);
    }

    public void LoadLevel(int num)
    {
        AdManager.instance.ClearAdTimer();
        SceneManager.LoadScene(num);
    }

    public void Quit()
    {
        Debug.Log("<color=red>Quit Game</color>");
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(currentLevel);
        //UIScript.instance.SetPauseMenu(false);
    }

    public void SaveScore(int score)
    {
        //Debug.Log(SceneManager.GetActiveScene().name);
        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name) == false)
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, score);
        }
        else if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name) > score)
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, score);
        }
    }

    /// <summary>
    /// Only call this if the player really wants to reset - please double check
    /// </summary>
    public void ResetEverything()
    {
        Analytics.CustomEvent("Reset");
        PlayerPrefs.DeleteAll();
        LoadMainMenu();
    }

    public void Contine()
    {
        AdManager.instance.ClearAdTimer();

        int sceneNum = 1;

        if (PlayerPrefs.HasKey(PrefsCurrentLevelKey))
        {
            sceneNum = PlayerPrefs.GetInt(PrefsCurrentLevelKey);
        }

        SceneManager.LoadScene(sceneNum);
    }
}
