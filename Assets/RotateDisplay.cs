using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDisplay : MonoBehaviour
{
    public float rotateSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 5f * Time.deltaTime, 0);
    }
}
