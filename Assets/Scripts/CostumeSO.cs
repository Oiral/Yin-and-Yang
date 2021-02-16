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

    public Material GetMaterial(TileType type)
    {
        switch (type)
        {
            case TileType.Default:
                return defaultTile;

            case TileType.Goal:
                return goalClosed;

            case TileType.Block:
                return defaultTile;

            case TileType.Ice:
                return ice;

            case TileType.Jelly:
                return jelly;

            case TileType.Button:
                return buttonType1;

            case TileType.Conveyor:
                return conveyor;

            case TileType.Ghost:
                return editorGhost;

            default:
                return null;
        }
    }

}
