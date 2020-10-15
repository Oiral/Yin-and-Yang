using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCenter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Debug.LogError("Stopping first");
        BoardManager manager = GameObject.FindWithTag("Board").GetComponent<BoardManager>();
        //Target Position
        float averageY = 0f;

        float maxX = 0f;

        float minX = 0f;

        float maxZ = 0f;

        float minZ = 0f;

        foreach (TileScript item in manager.tiles)
        {
            if (item.transform.localPosition.x > maxX)
            {
                maxX = item.transform.localPosition.x;
            }
            if (item.transform.localPosition.x < minX)
            {
                minX = item.transform.localPosition.x;
            }
            if (item.transform.localPosition.z > maxZ)
            {
                maxZ = item.transform.localPosition.z;
            }
            if (item.transform.localPosition.z < minZ)
            {
                minZ = item.transform.localPosition.z;
            }

            averageY += item.transform.localPosition.y;
        }

        averageY /= manager.tiles.Count;

        transform.position = new Vector3((maxX + minX) / 2, averageY, (maxZ + minZ) / 2);

    }
}
