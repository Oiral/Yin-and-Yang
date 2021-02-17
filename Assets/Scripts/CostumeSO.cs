using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Costume", menuName = "Scriptable Objects/Tile Material Costume", order = 1)]
public class CostumeSO : ScriptableObject
{
    [Header("Tiles")]
    public Material defaultTile;
    public Material goalOpen;
    public Material goalClosed;
    public Material ice;
    public Material jelly;
    public Material conveyor;

    [Header("Buttons")]
    public Material buttonType1;
    public Material buttonType2;

    [Header("Misc Materials")]
    public Material wallMaterial;

    [Header("Editor")]
    public Material editorGhost;

    [Header("Prefabs")]
    public GameObject teleporterPrefab;

    public Material[] GetMaterial(TileType type)
    {
        //The 0th is the walls of the tile - The 1st is the Top of the tile
        Material[] toReturn = new Material[] {defaultTile, defaultTile};

        switch (type)
        {
            case TileType.Goal:
                toReturn[1] = goalClosed;
                break;

            case TileType.Block:
                toReturn[0] = null;
                break;

            case TileType.Ice:
                toReturn[0] = toReturn[1] = ice;
                break;

            case TileType.Jelly:
                toReturn[0] = toReturn[1] = jelly;
                break;

            case TileType.Button:
                toReturn[1] = buttonType1;
                break;

            case TileType.Conveyor:
                toReturn[1] = conveyor;
                break;

            case TileType.Ghost:
                toReturn[0] = toReturn[1] = editorGhost;
                break;
        }
        return toReturn;
    }

}
