using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CostumeManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ChangePlayerMaterial(playerColours["Blue"]);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public Dictionary<string, Material> playerColours = new Dictionary<string, Material>();

    public void ChangePlayerMaterial(Material mat)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return;

        MeshRenderer renderer = player.GetComponentInChildren<MeshRenderer>();

        if (renderer == null)
            return;

        renderer.sharedMaterial = mat;
    }

}
