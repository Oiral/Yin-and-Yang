using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCenter : MonoBehaviour
{
    public Vector3 offset;

    Vector3 pos;

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

            averageY += item.transform.localPosition.y;
        }

        averageY /= manager.tiles.Count;
        pos = new Vector3((maxX + minX) / 2, averageY, (maxZ + minZ) / 2);

    }

    private void Update()
    {
        transform.position = pos + offset;
    }
}
