using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    #region Singleton
    public static LevelEditor instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public GameObject ghostPrefab;

    public Vector2Int[] vector2Directions = new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };


    //Use this to store the board tiles
    public Dictionary<Vector2Int, EditorTile> newBoard = new Dictionary<Vector2Int, EditorTile>();

    

    #region Dictionary Functions
    public bool CheckValue(Vector2Int value, Vector2Int direction)
    {
        return newBoard.ContainsKey(value + direction);
    }

    public bool CheckValue(Vector2Int value)
    {
        return newBoard.ContainsKey(value);
    }

    public static Vector3 KeyToPos(Vector2Int pos)
    {
        return new Vector3(pos.x, 0, pos.y);
    }
    public static Vector3 KeyToPos(Vector2Int pos, EditorTile tile)
    {
        return new Vector3(pos.x, tile.height, pos.y);
    }
    public static Vector2Int PosToKey(Vector3 pos)
    {
        return new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z));
    }
    #endregion

    public Vector2Int startingSize = new Vector2Int(2, 5);

    public Transform boardTransform;
    public CostumeSO costume;

    public TileType editingType = TileType.Default;

    private void Start()
    {
        SpawnInBoard(Vector2Int.zero, ghostPrefab);
        newBoard[Vector2Int.zero].type = TileType.Default;

        /*
        for (int x = 0; x < startingSize.x; x++)
        {
            for (int y = 0; y < startingSize.y; y++)
            {
                SpawnInBoard(x, y, ghostPrefab);
            }
        }
        */
    }

    public void UpdateGhosts()
    {
        foreach (EditorTile tile in newBoard.Values)
        {
            Vector2Int tilePos = PosToKey(tile.transform.position);

            //If it is a ghost tile, We don't want any more
            if (tile.type == TileType.Ghost) { continue; }

            UpdateGhosts(tilePos);
        }
    }

    public void UpdateGhosts(Vector2Int pos)
    {
        foreach (Vector2Int dir in vector2Directions)
        {
            //If there is something in the way, Next
            if (CheckValue(pos, dir)) { continue; }

            SpawnInBoard(pos + dir, ghostPrefab);
        }
    }

    public void SpawnInBoard(int x, int y, GameObject toBeAdded)
    {
        Vector2Int pos = new Vector2Int(x, y);
        SpawnInBoard(pos, toBeAdded);
    }

    public void SpawnInBoard(Vector2Int pos, GameObject toBeAdded)
    {
        if (CheckValue(pos))
        {
            Destroy(newBoard[pos]);
        }
        newBoard[pos] = Instantiate(toBeAdded, KeyToPos(pos), Quaternion.identity, boardTransform).GetComponent<EditorTile>();
    }



}
