using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/*
#if UNITY_EDITOR
using UnityEditor;
#endif
*/

public class TileButton : MonoBehaviour
{
    public UnityEvent steppedOnEvent = new UnityEvent();

    public void SteppedOn()
    {
        steppedOnEvent.Invoke();
    }




    /*
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //Lets draw a line in the direction that this will go

        foreach (var item in steppedOnEvent.GetPersistentTarget)
        {

        }

        var p1 = transform.position;
        var p2 = 
        var thickness = 3;
        Handles.DrawBezier(p1, p2, p1, p2, Color.black, null, thickness);


    }
#endif
*/
}
