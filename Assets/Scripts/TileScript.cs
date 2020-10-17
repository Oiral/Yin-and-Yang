using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { Default,Teleport,Goal,Block,Ice,Jelly,Button}

public class TileScript : MonoBehaviour {
    public TileConnectionsScript topPoint;
    //public TileConnectionsScript bottomPoint;

    public TileType Type = TileType.Default;

    public GameObject visual;

    public GameObject tileAdditionalVisual;

    /*
    private void Start()
    {
        PickCostume();
    }

    void PickCostume()
    {
        CostumeManager costumes = CostumeManager.instance;

        GameObject skin = GetComponentsInChildren<Transform>()[3].gameObject;
        Destroy(skin);
        switch (Type)
        {
            case TileType.Default:
                Instantiate(costumes.DefaultTileCostumes[Random.Range(0,costumes.DefaultTileCostumes.Count)],transform.position, Quaternion.identity, transform);
                break;
            case TileType.Hole:
                Instantiate(costumes.HoleTileCostumes[Random.Range(0, costumes.HoleTileCostumes.Count)], transform.position, Quaternion.identity, transform);
                break;
            case TileType.Goal:
                Instantiate(costumes.GoalTileCostumes[Random.Range(0, costumes.GoalTileCostumes.Count)], transform.position, Quaternion.identity, transform);
                break;
            case TileType.Block:
                Instantiate(costumes.BlockTileCostumes[Random.Range(0, costumes.BlockTileCostumes.Count)], transform.position, Quaternion.identity, transform);
                break;
            default:
                break;
        }
    }
    */
}
