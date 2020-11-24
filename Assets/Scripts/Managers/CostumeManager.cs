using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CostumeManager : MonoBehaviour, ISerializationCallbackReceiver
{
    string playerMaterialKey = "PlayerMaterial";
    string playerModelKey = "PlayerModel";

    public string selectedMaterial;
    [SerializeField]
    public Dictionary<string, PlayerMaterial> playerColours = new Dictionary<string, PlayerMaterial>();

    public string selectedModel;
    [SerializeField]
    public Dictionary<string, PlayerModel> playerModels = new Dictionary<string, PlayerModel>();

    List<string> unlockedMaterials = new List<string>();
    List<string> unlockedModels = new List<string>();

    void ChangePlayerMaterial(string materialName)
    {
        SaveToPrefs();

        Material mat = playerColours[materialName].material;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player == null)
            {
                Debug.Log("Could not find a mesh renderer");
                return;
            }

            foreach (MeshRenderer foundRenderer in player.GetComponentsInChildren<MeshRenderer>(false))
            {
                if (foundRenderer == null || foundRenderer.enabled == false)
                {
                    Debug.Log("Could not find a mesh renderer");
                    return;
                }

                foundRenderer.sharedMaterial = mat;
            }

        }

    }

    void ChangePlayerModels(string modelName)
    {
        SaveToPrefs();

        GameObject prefabToSpawn = playerModels[modelName].modelPrefab;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player == null)
            {
                Debug.Log("Could not find a mesh renderer");
                return;
            }
            //Lets change out the player model and then lets change the player material
            //TODO
            player.GetComponent<PlayerCostume>().UpdateCostume(prefabToSpawn);
            ChangePlayerMaterial(selectedMaterial);
        }
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


        if (instance == this)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            GetFromPrefs();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("Test");
        GetSelected();
    }

    private void OnDisable()
    {
        if (instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion

    #region player Prefs Saving Stuffs
    public void UnlockMaterial(string materialName)
    {
        GetFromPrefs();

        if (unlockedMaterials.Contains(materialName)) 
            return;

        unlockedMaterials.Add(materialName);

        SaveToPrefs();
    }

    public void UnlockModel(string modelName)
    {
        GetFromPrefs();

        if (unlockedModels.Contains(modelName))
            return;

        unlockedMaterials.Add(modelName);

        SaveToPrefs();
    }

    void GetFromPrefs()
    {
        string savedString = "";

        //Set the saved string to what has been saved
        if (PlayerPrefs.HasKey(playerMaterialKey))
        {
            savedString = PlayerPrefs.GetString(playerMaterialKey);
        }

        unlockedMaterials = new List<string>();
        foreach (string matName in savedString.Split('/'))
        {
            unlockedMaterials.Add(matName);
        }

        if (PlayerPrefs.HasKey(playerModelKey))
        {
            savedString = PlayerPrefs.GetString(playerModelKey);
        }

        unlockedMaterials = new List<string>();
        foreach (string modelName in savedString.Split('/'))
        {
            unlockedModels.Add(modelName);
        }

        GetSelected();
    }

    public void GetSelected()
    {
        string matSelected = "";
        string modelSelected = "";

        //Set the current tile to what has been set
        if (PlayerPrefs.HasKey(playerMaterialKey + "Selected"))
        {
            matSelected = PlayerPrefs.GetString(playerMaterialKey + "Selected");
        }

        //Set the current tile to what has been set
        if (PlayerPrefs.HasKey(playerModelKey + "Selected"))
        {
            modelSelected = PlayerPrefs.GetString(playerModelKey + "Selected");
        }

        SetSelectedMaterial(matSelected);
        SetSelectedModel(modelSelected);
    }

    public void SetSelectedMaterial(string matName)
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

    public void SetSelectedModel(string modelName)
    {
        if (playerModels.ContainsKey(modelName))
        {
            //If the saved materials contains this name, We can save it
            selectedModel = modelName;
        }
        else
        {
            //Otherwise lets set it to the first in the order
            selectedModel = "Default";
        }

        ChangePlayerModels(selectedModel);
    }

    void SaveToPrefs()
    {
        //Save the unlocked items
        string namesToSave = "";

        foreach (string item in unlockedMaterials)
        {
            namesToSave += item + "/";
        }
        PlayerPrefs.SetString(playerMaterialKey, namesToSave);

        namesToSave = "";
        foreach (string item in unlockedModels)
        {
            namesToSave += item + "/";
        }
        PlayerPrefs.SetString(playerModelKey, namesToSave);


        //Save the selected item
        PlayerPrefs.SetString(playerMaterialKey + "Selected", selectedMaterial);

        PlayerPrefs.SetString(playerModelKey + "Selected", selectedModel);
    }

    void GetUnlocked()
    {

    }

    #endregion

    #region Serialise Dictionary
    //We need to do this because unity doesn't automatically serialize dictionarys
    //[SerializeField]
    [HideInInspector]
    public List<string> _materialKeys = new List<string>();

    [HideInInspector]
    public List<string> _modelKeys = new List<string>();
    //[SerializeField]
    [HideInInspector]
    public List<PlayerMaterial> _materialValues = new List<PlayerMaterial>();
    [HideInInspector]
    public List<PlayerModel> _modelValues = new List<PlayerModel>();

    public void OnBeforeSerialize()
    {
        _materialKeys.Clear();
        _materialValues.Clear();

        foreach (var kvp in playerColours)
        {
            _materialKeys.Add(kvp.Key);
            _materialValues.Add(kvp.Value);
        }

        _modelKeys.Clear();
        _modelValues.Clear();

        foreach (var kvp in playerModels)
        {
            _modelKeys.Add(kvp.Key);
            _modelValues.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        playerColours = new Dictionary<string, PlayerMaterial>();

        for (int i = 0; i != Math.Min(_materialKeys.Count, _materialValues.Count); i++)
            playerColours.Add(_materialKeys[i], _materialValues[i]);


        playerModels = new Dictionary<string, PlayerModel>();

        for (int i = 0; i != Math.Min(_modelKeys.Count, _modelValues.Count); i++)
            playerModels.Add(_modelKeys[i], _modelValues[i]);
    }
    #endregion

}

[System.Serializable]
public class PlayerMaterial
{
    public Material material;
    public bool unlocked;
    public int cost;

    public PlayerMaterial(Material mat, int newCost)
    {
        material = mat;
        cost = newCost;
    }
    public PlayerMaterial()
    {

    }
}

[System.Serializable]
public class PlayerModel
{
    public GameObject modelPrefab;
    public bool unlocked;
    public int cost;

    public PlayerModel(GameObject prefab, int newCost)
    {
        modelPrefab = prefab;
        cost = newCost;
    }
    public PlayerModel()
    {

    }
}
