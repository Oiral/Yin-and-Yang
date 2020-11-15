using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class UpdateBoard
{
    [MenuItem("Custom/Update Board %#g")]
    public static void UpdateTheBoard()
    {
        GameObject.FindGameObjectWithTag("Board").GetComponent<BoardManager>().UpdateBoard();
    }

    [MenuItem("Custom/Revert Mesh In Tiles")]
    public static void RevertMeshInTiles()
    {
        foreach (var tile in GameObject.FindObjectsOfType<TileScript>())
        {
            PrefabUtility.RevertObjectOverride(tile.GetComponentInChildren<MeshFilter>(), InteractionMode.UserAction);
        }
    }

}
