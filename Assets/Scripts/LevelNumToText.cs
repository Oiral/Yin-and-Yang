using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelNumToText : MonoBehaviour
{
    private void Start()
    {
        //Debug.Log(SceneManager.GetActiveScene().buildIndex);

        int levelNum = SceneManager.GetActiveScene().buildIndex;

        GetComponent<Text>().text = levelNum.ToString();
    }
}
