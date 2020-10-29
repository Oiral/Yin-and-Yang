using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TileButton : MonoBehaviour
{
    //public UnityEvent steppedOnEvent = new UnityEvent();

    public List<MovingTile> movingTiles;
    public List<RotatingTile> rotatingTiles;

    public void SteppedOn()
    {
        //steppedOnEvent.Invoke();
        for (int m = 0; m < movingTiles.Count; m++)
        {
            movingTiles[m].ButtonSteppedOn();
        }

        for (int r = 0; r < rotatingTiles.Count; r++)
        {
            rotatingTiles[r].ButtonSteppedOn();
        }
    }

    public Material buttonMaterial;

    private void Start()
    {
        if (buttonMaterial == null)
            return;

        for (int m = 0; m < movingTiles.Count; m++)
        {
            movingTiles[m].GetComponent<TileScript>().tileAdditionalVisual.GetComponentInChildren<MeshRenderer>().sharedMaterial = buttonMaterial;
        }

        for (int r = 0; r < rotatingTiles.Count; r++)
        {
            rotatingTiles[r].GetComponent<TileScript>().tileAdditionalVisual.GetComponentInChildren<MeshRenderer>().sharedMaterial = buttonMaterial;
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //Lets draw a line in the direction that this will go
        for (int m = 0; m < movingTiles.Count; m++)
        {
            DrawLine(movingTiles[m].gameObject.transform.position);
        }

        for (int r = 0; r < rotatingTiles.Count; r++)
        {
            DrawLine(rotatingTiles[r].gameObject.transform.position);
        }

    }


    private void DrawLine(Vector3 pos)
    {
        var p1 = transform.position;
        var p2 = pos;
        var thickness = 3;
        Handles.DrawBezier(p1, p2, p1, p2, Color.black, null, thickness);
    }



#endif

}
