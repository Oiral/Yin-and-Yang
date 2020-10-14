using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCenter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BoardManager manager = GameObject.FindWithTag("Board").GetComponent<BoardManager>();
        //Target Position
        float averageY = 0f;

        float maxX = 0f;

        float minX = 0f;

        float maxZ = 0f;

        float minZ = 0f;

        foreach (TileScript item in manager.tiles)
        {
            if (item.transform.position.x > maxX)
            {
                maxX = item.transform.position.x;
            }
            if (item.transform.position.x < minX)
            {
                minX = item.transform.position.x;
            }
            if (item.transform.position.z > maxZ)
            {
                maxZ = item.transform.position.z;
            }
            if (item.transform.position.z < minZ)
            {
                minZ = item.transform.position.z;
            }

            averageY += item.transform.position.y;
        }

        averageY /= manager.tiles.Count;

        transform.position = new Vector3((maxX + minX) / 2, (maxZ + minZ) / 2, averageY);

    }
}
