using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorTile : MonoBehaviour
{
    public TileType type {
        get { return _type; }
        set { _type = value;
            UpdateVisuals();
        }
    }

    public Material testMaterial;

    TileType _type = TileType.Ghost;
    public float height = 0f;

    private void OnMouseUpAsButton()
    {
        Debug.Log(LevelEditor.PosToKey(transform.position), gameObject);
        LevelEditor.instance.TileClicked(this);
    }

    public void UpdateVisuals()
    {
        //Lets update the visuals of the tile
        GetComponentInChildren<MeshRenderer>().materials[0] = LevelEditor.instance.costume.GetMaterial(_type);
        GetComponentInChildren<MeshRenderer>().materials[1] = LevelEditor.instance.costume.GetMaterial(_type);
    }

}
