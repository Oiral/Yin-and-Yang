using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    public EditorTile[] levelTiles;
    public Vector2Int playerPos;
    public Vector2Int ringPos;

    public string name;
    public string date;
}
