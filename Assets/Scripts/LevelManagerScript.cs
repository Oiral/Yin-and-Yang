using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagerScript : MonoBehaviour {

	public int currentLevel = 0;

    //public static LevelManagerScript instance;

    public float waitTime = 2f;

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

        if (PlayerPrefs.HasKey("Level"))
        {
            if (currentLevel > PlayerPrefs.GetInt("Level"))
            {
                PlayerPrefs.SetInt("Level", currentLevel);
            }
        }
        else
        {
            PlayerPrefs.SetInt("Level", 1);
        }

    }

    public void NextLevel()
    {
        Debug.Log("Scene loading");

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
        yield return new WaitForSeconds(waitTime * 0.8f);
        //FindBoard
        if (GameObject.FindGameObjectWithTag("Board") != null)
        {
            RotateBoard board = GameObject.FindGameObjectWithTag("Board").GetComponent<RotateBoard>();
            board.targetPos = new Vector3(0, 15, 0);
        }



        yield return new WaitForSeconds(waitTime * 0.2f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(currentLevel += 1);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        //Reaches this point when its loaded
        //SceneManager.UnloadSceneAsync(currentLevel - 1);
        if (currentLevel == SceneManager.sceneCountInBuildSettings-1)
        {
            UIScript.instance.SetWinMenu(true);
        }
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
}
