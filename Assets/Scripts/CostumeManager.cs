using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CostumeManager : MonoBehaviour, ISerializationCallbackReceiver
{
    string playerPrefsKey = "PlayerMaterial";

    public string selectedMaterial;
    [SerializeField]
    public Dictionary<string, PlayerMaterial> playerColours = new Dictionary<string, PlayerMaterial>();

    List<string> unlockedMaterials = new List<string>();

    void ChangePlayerMaterial(string materialName)
    {
        SaveToPrefs();

        Material mat = playerColours[materialName].material;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.Log("Could not find a mesh renderer");
            return;
        }

        MeshRenderer renderer = player.GetComponentInChildren<MeshRenderer>(false);

        if (renderer == null)
        {
            Debug.Log("Could not find a mesh renderer");
            return;
        }

        renderer.sharedMaterial = mat;
    }

    #region On Scene Loading & Singleton
    public static CostumeManager instance;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        GetFromPrefs();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("Test");
        ChangePlayerMaterial(selectedMaterial);
    }

    private void OnDisable()
    {
        if (instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion

    #region player Prefs Saving Stuffs
    public void SaveUnlocked(string materialName)
    {
        GetFromPrefs();

        if (playerColours.ContainsKey(materialName)) 
            return;

        unlockedMaterials.Add(materialName);

        SaveToPrefs();
    }

    void GetFromPrefs()
    {
        string savedString = "";

        //Set the saved string to what has been saved
        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            savedString = PlayerPrefs.GetString(playerPrefsKey);
        }

        unlockedMaterials = new List<string>();
        foreach (string matName in savedString.Split('/'))
        {
            unlockedMaterials.Add(matName);
        }

        //Set the current tile to what has been set
        if (PlayerPrefs.HasKey(playerPrefsKey + "Selected"))
        {
            SetSelected(PlayerPrefs.GetString(playerPrefsKey + "Selected"));
        }
        else
        {
            SetSelected("");
        }
    }

    public void SetSelected(string matName)
    {
        if (playerColours.ContainsKey(matName))
        {
            //If the saved materials contains this name, We can save it
            selectedMaterial = matName;
        }
        else
        {
            //Otherwise lets set it to the first in the order
            selectedMaterial = "Grey";
        }

        ChangePlayerMaterial(selectedMaterial);
    }

    void SaveToPrefs()
    {
        string namesToSave = "";

        foreach (string item in unlockedMaterials)
        {
            namesToSave += item + "/";
        }

        PlayerPrefs.SetString(playerPrefsKey, namesToSave);

        PlayerPrefs.SetString(playerPrefsKey + "Selected", selectedMaterial);
    }

    void GetUnlocked()
    {

    }

    #endregion

    #region Serialise Dictionary
    //We need to do this because unity doesn't automatically serialize dictionarys
    //[SerializeField]
    [HideInInspector]
    public List<string> _keys = new List<string>();
    //[SerializeField]
    [HideInInspector]
    public List<PlayerMaterial> _values = new List<PlayerMaterial>();

    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();

        foreach (var kvp in playerColours)
        {
            _keys.Add(kvp.Key);
            _values.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        playerColours = new Dictionary<string, PlayerMaterial>();

        for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
            playerColours.Add(_keys[i], _values[i]);
    }
    #endregion

}

[System.Serializable]
public class PlayerMaterial
{
    public Material material;
    public bool unlocked;

    public PlayerMaterial(Material mat)
    {
        material = mat;
    }
    public PlayerMaterial()
    {

    }
}
