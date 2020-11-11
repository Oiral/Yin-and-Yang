using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    public static DontDestroyOnLoad instance;

    public bool OnlyOne = true;

    private void Awake()
    {
        if (OnlyOne)
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Debug.LogWarning("Found an additional Dont destroy on load");
                Destroy(gameObject);
            }
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

    }
}
