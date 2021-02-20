using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public Level levelToLoad;

    public void loadLevel(Level lvl)
    {
        foreach (EditorTile tile in lvl.levelTiles)
        {
            GenerateTile(tile);
        }
    }

    public GameObject defaulTile;
    public Transform gameBoard;

    void GenerateTile(EditorTile tile)
    {
        Vector3 pos = LevelEditor.KeyToPos(tile.key);
        pos.y = tile.height;
        GameObject spawnedTile = Instantiate(defaulTile, pos, Quaternion.identity, gameBoard);
        spawnedTile.GetComponent<TileScript>().Type = tile.type;
    }
}
