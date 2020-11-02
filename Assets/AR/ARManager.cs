using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARManager : MonoBehaviour
{
    public Camera cam;
    public LayerMask mask;

    public void MoveAR()
    {
        //Raycast from camera
        RaycastHit hit;

        Ray forwardRay = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(forwardRay, out hit, 20, mask)) 
        {
            transform.position = Vector3.zero - hit.point;
        }
    }
}
