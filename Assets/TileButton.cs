using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileButton : MonoBehaviour
{
    public UnityEvent steppedOnEvent = new UnityEvent();

    public void SteppedOn()
    {
        steppedOnEvent.Invoke();
    }
}
