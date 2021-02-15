using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    public GameObject ghostPrefab;

    public GameObject[] board;

    //Use this to store the board tiles
    public Dictionary<Vector2Int, EditorTile> newBoard;

    #region Dictionary Functions
    public bool CheckValue(Vector2Int value, Vector2Int direction)
    {
        return newBoard.ContainsKey(value + direction);
    }

    public bool CheckValue(Vector2Int value)
    {
        return newBoard.ContainsKey(value);
    }
    #endregion

    public Vector2Int startingSize = new Vector2Int(2, 5);

    public Transform boardTransform;

    private void Start()
    {
        board = new GameObject[startingSize.x * startingSize.y];

        for (int x = 0; x < startingSize.x; x++)
        {
            for (int y = 0; y < startingSize.y; y++)
            {
                SpawnInBoard(x, y, ghostPrefab);
            }
        }
    }



    public void UpdateBoard()
    {

    }

    public void SpawnInBoard(int x, int y, GameObject toBeAdded)
    {
        int arrayPos = GetArrayPos(x, y);

        if (board[arrayPos] != null)
        {
            Destroy(board[arrayPos]);
        }
        board[arrayPos] = Instantiate(toBeAdded,new Vector3(x,0,y),Quaternion.identity,boardTransform);
    }


    public int GetArrayPos(int x, int y)
    {
        return (x * startingSize.y) + (y);
    }


}
