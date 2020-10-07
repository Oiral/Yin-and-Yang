using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public List<TileScript> tiles = new List<TileScript>();

    public List<TileConnectionsScript> topTileConnections = new List<TileConnectionsScript>();
    public List<TileConnectionsScript> bottomTileConnections = new List<TileConnectionsScript>();


    [ContextMenu("Update Board")]
    public void UpdateBoard()
    {
        foreach (TileScript tile in FindObjectsOfType(typeof(TileScript)))
        {
            tiles.Add(tile);

            topTileConnections.Add(tile.topPoint);
            bottomTileConnections.Add(tile.bottomPoint);

            tile.topPoint.UpdateConnections();
            tile.bottomPoint.UpdateConnections();
        }
    }

    private void Awake()
    {
        UpdateBoard();
    }
}
