using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public List<TileScript> tiles = new List<TileScript>();

    public List<TileConnectionsScript> topTileConnections = new List<TileConnectionsScript>();
    //public List<TileConnectionsScript> bottomTileConnections = new List<TileConnectionsScript>();


    [ContextMenu("Update Board")]
    public void UpdateBoard()
    {
        //Reset all the lists
        tiles = new List<TileScript>();

        topTileConnections = new List<TileConnectionsScript>();

        //bottomTileConnections = new List<TileConnectionsScript>();


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

    private void Awake()
    {
        UpdateBoard();
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

    public Material iceMaterial;
    public Material normalMaterial;
    public Material goalClosedMaterial;
    public Material goalOpenMaterial;

    public void ChangeGoalMaterial(bool isOpen)
    {
        Material mat;

        if (isOpen)
        {
            mat = goalOpenMaterial;
        }
        else
        {
            mat = goalClosedMaterial;
        }

        foreach (TileScript tile in GetTiles(TileType.Goal))
        {

            ChangeTileMaterial(tile, mat);
        }
    }

    public void ChangeTileMaterial (TileScript tile, Material materialToChangeTo)
    {
        if (tile.GetComponent<MeshRenderer>().sharedMaterials[0] == materialToChangeTo)
        {
            return;
        }

        Material[] rendererMats = tile.GetComponent<MeshRenderer>().sharedMaterials;

        rendererMats[0] = materialToChangeTo;

        tile.GetComponent<MeshRenderer>().sharedMaterials = rendererMats;
    }

    public void ChangeTileMaterial(TileScript tile)
    {
        switch (tile.Type)
        {
            case TileType.Ice:
                ChangeTileMaterial(tile, iceMaterial);
                break;

            case TileType.Goal:
                ChangeGoalMaterial(false);
                break;

            default:
                ChangeTileMaterial(tile, normalMaterial);
                break;
        }
    }
}
