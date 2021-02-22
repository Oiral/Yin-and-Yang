﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class BoardManager : MonoBehaviour
{
    public List<TileScript> tiles = new List<TileScript>();

    public List<TileConnectionsScript> topTileConnections = new List<TileConnectionsScript>();
    //public List<TileConnectionsScript> bottomTileConnections = new List<TileConnectionsScript>();

        [Header("Visual Prefabs")]
        [SerializeField]
    public GameObject teleporterVisuals;
    [SerializeField]
    public GameObject jellyVisuals;
    //public GameObject buttonVisuals;


    [ContextMenu("Update Board")]
    public void UpdateBoard()
    {
        //Reset all the lists
        tiles = new List<TileScript>();

        topTileConnections = new List<TileConnectionsScript>();

        //bottomTileConnections = new List<TileConnectionsScript>();
#if UNITY_EDITOR
        if (Application.isPlaying == false)
        {
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
#endif

        foreach (TileScript tile in FindObjectsOfType(typeof(TileScript)))
        {

            tiles.Add(tile);

            topTileConnections.Add(tile.topPoint);
            //bottomTileConnections.Add(tile.bottomPoint);

            tile.topPoint.UpdateConnections();
            //tile.bottomPoint.UpdateConnections();

            ChangeTileMaterial(tile);
        }

    }
    public static BoardManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else if (instance != this)
        {
            Debug.LogWarning("An additional Board Manager has been found, Destroying");
            Destroy(this);
        }

        UpdateBoard();
        UpdateBoardVisuals();
    }

    public List<TileScript> GetTiles(TileType typeToCheck)
    {
        List<TileScript> tilesToReturn = new List<TileScript>();

        foreach (TileScript tile in tiles)
        {
            if (tile.Type == typeToCheck)
            {
                //Debug.Log("Testing 2");
                tilesToReturn.Add(tile);
            }
        }

        return tilesToReturn;
    }

    [Header("Materials")]
    [SerializeField]
    public Material iceMaterial;
    [SerializeField]
    public Material normalMaterial;
    [SerializeField]
    public Material goalClosedMaterial;
    [SerializeField]
    public Material goalOpenMaterial;
    [SerializeField]
    public Material jellyMaterial;
    [SerializeField]
    public Material teleporterMaterial;
    [SerializeField]
    public Material buttonMaterial;
    [SerializeField]
    public Material conveyorMaterial;

    public CostumeSO costume;

    bool goalOpen = false;

    public void ChangeGoalMaterial(bool isOpen)
    {
        goalOpen = isOpen;

        foreach (TileScript tile in GetTiles(TileType.Goal))
        {

            ChangeTileMaterial(tile);
        }
    }
    /*
    public void ChangeTileMaterial (TileScript tile)
    {

        

        
        Material[] rendererMats = tile.visual.GetComponent<MeshRenderer>().sharedMaterials;

        rendererMats[1] = materialToChangeTo;
        rendererMats[0] = normalMaterial;

        tile.visual.GetComponent<MeshRenderer>().sharedMaterials = rendererMats;
        
    }
    */

    /*
    public void ChangeTileMaterial(TileScript tile, Material materialToChangeTo, Material secondaryMaterial)
    {

        Material[] rendererMats = tile.visual.GetComponent<MeshRenderer>().sharedMaterials;

        rendererMats[1] = materialToChangeTo;
        rendererMats[0] = secondaryMaterial;

        tile.visual.GetComponent<MeshRenderer>().sharedMaterials = rendererMats;
    }
    */

    public void ChangeTileMaterial(TileScript tile)
    {

        if (tile.Type == TileType.Goal)
        {
            tile.visual.GetComponent<MeshRenderer>().sharedMaterials = costume.GetGoalMaterial(goalOpen);
        }
        else
        {
            tile.visual.GetComponent<MeshRenderer>().sharedMaterials = costume.GetMaterial(tile.Type);
        }
        /*
        switch (tile.Type)
        {
            case TileType.Ice:
                ChangeTileMaterial(tile, iceMaterial, iceMaterial);
                break;

            case TileType.Goal:
                ChangeGoalMaterial(false);
                break;

            case TileType.Jelly:
                ChangeTileMaterial(tile, jellyMaterial, jellyMaterial);
                break;
            case TileType.Teleport:
                ChangeTileMaterial(tile, teleporterMaterial);
                break;

            case TileType.Button:

                TileButton button = tile.GetComponent<TileButton>();
                if (button != null && button.buttonMaterial != null)
                {
                    ChangeTileMaterial(tile, button.buttonMaterial);
                }
                else
                {
                    ChangeTileMaterial(tile, buttonMaterial);
                }

                break;

            case TileType.Conveyor:
                ChangeTileMaterial(tile, conveyorMaterial);
                break;

            default:
                ChangeTileMaterial(tile, normalMaterial);
                break;

        }
        */
    }

    public void UpdateBoardVisuals()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            switch (tiles[i].Type)
            {

                case TileType.Jelly:
                    UpdateTileVisual(tiles[i], jellyVisuals);
                    break;

                case TileType.Teleport:
                    UpdateTileVisual(tiles[i], teleporterVisuals);
                    break;
                    /*
                case TileType.Button:
                    UpdateTileVisual(tiles[i], buttonVisuals);
                    break;
                    */                   

                default:
                    break;
            }
        }
    }

    void UpdateTileVisual(TileScript tile,GameObject prefab)
    {

        if (prefab == null)
        {
            return;
        }

        if (tile.tileAdditionalVisual != null)
        {
            //Lets destroy the visual
            Destroy(tile.tileAdditionalVisual);
        }

        //Lets create the new visual
        GameObject visual = Instantiate(prefab, tile.transform);
        tile.tileAdditionalVisual = visual;
    }
}
