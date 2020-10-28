using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCenter : MonoBehaviour
{
    public Vector3 offset;

    Vector3 pos;

    private void Update()
    {
        transform.position = pos;// + offset;

        SetCameraPos();
        SetCameraWidth();
    }


    void SetCameraWidth()
    {
        if (smallesPos == null && biggestPos == null)
            return;

        float distance = Vector3.Distance(smallesPos.transform.position, biggestPos.transform.position);

        distance += 2f;

        distance = Mathf.Max(distance, 3f);


        float screenAspect = (float)Screen.width / (float)Screen.height;

        float camHalfWidth = distance * 0.5f;
        float camHalfHeight = camHalfWidth / screenAspect;
        GetComponentInChildren<Camera>().orthographicSize = camHalfHeight;
    }

    void SetCameraPos()
    {
        BoardManager manager = BoardManager.instance;

        float averageY = 0f;

        smallesPos = manager.tiles[0].transform;
        biggestPos = manager.tiles[0].transform;

        foreach (TileScript item in manager.tiles)
        {
            if (GetPositionScore(item.transform.position) <= GetPositionScore(smallesPos.position))
            {
                smallesPos = item.transform;
            }

            if (GetPositionScore(item.transform.position) >= GetPositionScore(biggestPos.position))
            {
                biggestPos = item.transform;
            }
            averageY += item.transform.localPosition.y;
        }

        averageY /= manager.tiles.Count;
        pos = new Vector3((smallesPos.position.x + biggestPos.position.x) / 2, averageY, (smallesPos.position.z + biggestPos.position.z) / 2);

    }

    float GetPositionScore(Vector3 checkPos)
    {
        return (checkPos.x - checkPos.z);
    }

    Transform smallesPos;
    Transform biggestPos;
         
    /*
    private void OnDrawGizmos()
    {
        if (testingObject1 != null && testingObject2 != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(testingObject1.position, testingObject2.position);
            //Debug.Log(Vector3.Distance(testingObject1.transform.position, testingObject2.transform.position));
        }
    }
    */
}
