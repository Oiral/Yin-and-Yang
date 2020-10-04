using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagerScript : MonoBehaviour {

	public int currentLevel = 0;

    public static LevelManagerScript instance;

    public float waitTime = 0.7f;

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
        yield return new WaitForSeconds(waitTime / 4);
        yield return new WaitForSeconds(waitTime / 4);
        yield return new WaitForSeconds(waitTime / 4);
        //FindBoard
        if (GameObject.FindGameObjectWithTag("Board") != null)
        {
            RotateBoard board = GameObject.FindGameObjectWithTag("Board").GetComponent<RotateBoard>();
            board.targetPos = new Vector3(0, -5, 0);
        }

        yield return new WaitForSeconds(waitTime / 4);

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
        UIScript.instance.SetMainMenu(true);
        UIScript.instance.SetPauseMenu(false);
        UIScript.instance.SetWinMenu(false);
    }

    public void Quit()
    {
        Debug.Log("<color=red>Quit Game</color>");
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(currentLevel);
        UIScript.instance.SetPauseMenu(false);
    }
}
