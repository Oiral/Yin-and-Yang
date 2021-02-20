using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorTextureMover : MonoBehaviour
{
    public Material conveyorMat;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = conveyorMat.mainTextureOffset;

        offset.y += Time.deltaTime * speed;

        if (offset.y > 1)
        {
            offset.y -= 1;
        }

        if (offset.y < -1)
        {
            offset.y += 1;
        }

        conveyorMat.mainTextureOffset = offset;
    }
}
