using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTile : MonoBehaviour
{
    public bool atStart;

    Vector3 startingPos;
    public Vector3 endPos;

    public float moveTime = 0.3f;

    public GameObject displayTrackPrefab;

    private void Start()
    {
        startingPos = transform.position;
        SpawnTrackPrefab();
    }

    public void ButtonSteppedOn()
    {
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
        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(start, end, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = end;

        BoardManager.instance.UpdateBoard();


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(endPos, 0.2f);

        Gizmos.DrawLine(transform.position, endPos);
    }

    void SpawnTrackPrefab()
    {
        if (displayTrackPrefab == null)
            return;

        GameObject spawnedPrefab = Instantiate(displayTrackPrefab, transform.position, Quaternion.identity,BoardManager.instance.transform);
        spawnedPrefab.transform.LookAt(endPos);

        Vector3 scale = spawnedPrefab.transform.localScale;
        scale.y = Vector3.Distance(startingPos, endPos);

        spawnedPrefab.transform.localScale = scale;
    }
}
