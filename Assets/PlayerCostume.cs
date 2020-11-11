using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCostume : MonoBehaviour
{
    public GameObject spawnedCostume;
    public Transform parentForCostume;

    public void UpdateCostume(GameObject prefab)
    {
        Destroy(spawnedCostume);
        spawnedCostume = Instantiate(prefab, parentForCostume);
    }
}
