using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EditingActions {Add, Remove, Edit , HeightUp, HeightDown};

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
    public EditingActions currentAction;

    #region Action Controls
    public void ChangeEditingAction(EditingActions newAction)
    {
        if (newAction == currentAction) { return; }

        currentAction = newAction;
        if (newAction == EditingActions.Add)
        {
            UpdateGhosts();
        }
        else
        {
            RemoveGhosts();
        }
    }
    public void ChangeEditingTile(TileType type)
    {
        editingType = type;
    }

    public void ChangeEditingTile(int type)
    {
        ChangeEditingTile((TileType)type);
    }

    public void ChangeEditingAction(int newAction)
    {
        ChangeEditingAction((EditingActions)newAction);
    }

    #endregion

    private void Start()
    {
        SpawnInBoard(Vector2Int.zero, ghostPrefab);
        newBoard[Vector2Int.zero].type = TileType.Default;
        newBoard[Vector2Int.zero].UpdateVisuals();

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

    

    public void RemoveGhosts()
    {
        List<EditorTile> toRemove = new List<EditorTile>();
        foreach (EditorTile tile in newBoard.Values)
        {
            if (tile.type == TileType.Ghost)
            {
                toRemove.Add(tile);
            }
        }

        while (toRemove.Count > 0)
        {
            RemoveFromBoard(toRemove[0]);
            toRemove.RemoveAt(0);
        }
    }

    public void UpdateGhosts()
    {
        List<EditorTile> tempTileList = new List<EditorTile>(newBoard.Values);

        foreach (EditorTile tile in tempTileList)
        {
            Vector2Int tilePos = PosToKey(tile.transform.position);

            UpdateGhosts(tilePos);
        }
    }

    public void UpdateGhosts(Vector2Int pos)
    {
        if (newBoard[pos].type == TileType.Ghost)
        {
            bool hasConnectingTile = false;

            //If this is a ghost, Lets check if there is a normal block next to this
            foreach (Vector2Int dir in vector2Directions)
            {
                //If there is something in the way, Next
                if (CheckValue(pos, dir) == false) { continue; }

                if (newBoard[pos + dir].type != TileType.Ghost)
                {
                    hasConnectingTile = true;
                    break;
                }
            }

            if (hasConnectingTile == false)
            {
                //Lets remove this tile
                RemoveFromBoard(pos);
            }
        }
        else
        {

            foreach (Vector2Int dir in vector2Directions)
            {
                //If there is something in the way, Next
                if (CheckValue(pos, dir)) { continue; }

                SpawnInBoard(pos + dir, ghostPrefab);
            }
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
        newBoard[pos].UpdateVisuals();
    }
    public void RemoveFromBoard(Vector2Int pos)
    {
        EditorTile tile = newBoard[pos];

        newBoard.Remove(pos);
        Destroy(tile.gameObject);
    }
    public void RemoveFromBoard(EditorTile tile)
    {
        RemoveFromBoard(PosToKey(tile.transform.position));
    }

    public void TileClicked(EditorTile tile)
    {
        Vector2Int tileKey = PosToKey(tile.transform.position);

        switch (currentAction)
        {
            case EditingActions.Add:
                if (tile.type == TileType.Ghost)
                {
                    tile.type = TileType.Default;
                }
                UpdateGhosts(tileKey);
                break;
            case EditingActions.Remove:
                RemoveFromBoard(tile);
                break;
            case EditingActions.Edit:
                tile.type = editingType;
                break;
            case EditingActions.HeightDown:
                tile.UpdateHeight(-1);
                break;
            case EditingActions.HeightUp:
                tile.UpdateHeight(1);
                break;
            default:
                break;
        }
    }

}
