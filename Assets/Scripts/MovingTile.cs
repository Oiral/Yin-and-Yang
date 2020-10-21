using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTile : MonoBehaviour
{
    public bool atStart;

    Vector3 startingPos;
    public Vector3 endPos;

    public float moveTime = 0.3f;

    [SerializeField]
    public GameObject displayTrackPrefab;

    bool moving;


    private void Start()
    {
        startingPos = transform.position;
        SpawnTrackPrefab();
    }

    public void ButtonSteppedOn()
    {
        if (moving)
            return;

        atStart = !atStart;


        if (atStart)
        {
            StartCoroutine(MoveTile(startingPos, endPos));
        }
        else
        {
            StartCoroutine(MoveTile(endPos, startingPos));
        }
    }

    IEnumerator MoveTile(Vector3 start, Vector3 end)
    {
        moving = true;

        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(start, end, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = end;

        BoardManager.instance.UpdateBoard();
        LevelController.instance.CheckAllKeysOnPlayer();
        moving = false;


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(endPos, 0.2f);
        Gizmos.DrawWireSphere(transform.position, 0.2f);

        Gizmos.DrawLine(transform.position, endPos);
    }

    void SpawnTrackPrefab()
    {
        if (displayTrackPrefab == null)
            return;

        Vector3 offset = Vector3.up * 0.4f;

        GameObject spawnedPrefab = Instantiate(displayTrackPrefab, transform.position + offset, Quaternion.identity,BoardManager.instance.transform);
        spawnedPrefab.transform.LookAt(endPos + offset);

        Vector3 scale = spawnedPrefab.transform.localScale;
        scale.y = Vector3.Distance(startingPos, endPos);

        spawnedPrefab.transform.localScale = scale;
    }
}
