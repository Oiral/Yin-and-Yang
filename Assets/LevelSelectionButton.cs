using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionButton : MonoBehaviour
{
    public Text textName;
    public int levelNum;

    public void LoadLevel()
    {
        GameObject.FindWithTag("LevelManager").GetComponent<LevelManagerScript>().LoadLevel(levelNum);
    }
}
