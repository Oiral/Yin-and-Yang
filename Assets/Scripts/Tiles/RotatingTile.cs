using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingTile : MonoBehaviour
{
    public float rotateAmount = 90;

    public float moveTime = 0.3f;

    [SerializeField]
    public GameObject topRotatePrefab;
    public GameObject bottomRotatePrefab;

    public bool showTopDisplay;
    bool moving;

    private void Start()
    {
        SpawnTrackPrefab();
    }

    public void ButtonSteppedOn()
    {
        if (moving)
            return;

        StartCoroutine(RotateTile());
    }

    IEnumerator RotateTile()
    {
        moving = true;
        PlayerController.instance.ToggleMovement(false);

        yield return new WaitForSeconds(0.2f);

        Vector3 startRotation = transform.rotation.eulerAngles;
        Vector3 rotation = startRotation;


        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            rotation = new Vector3(rotation.x,
                Mathf.LerpAngle(0 + startRotation.y, rotateAmount + startRotation.y, elapsedTime / moveTime),
                rotation.z);

            transform.rotation = Quaternion.Euler(rotation);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(startRotation);
        transform.Rotate(new Vector3(0, rotateAmount, 0));

        BoardManager.instance.UpdateBoard();
        LevelController.instance.CheckAllKeysOnPlayer();
        moving = false;
        PlayerController.instance.ToggleMovement(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.5f, 0.2f);
    }

    void SpawnTrackPrefab()
    {
        GameObject prefabToSpawn = showTopDisplay ? topRotatePrefab : bottomRotatePrefab;

        if (prefabToSpawn == null)
            return;

        //Vector3 offset = Vector3.up * 0.4f;


        GameObject spawnedPrefab = Instantiate(prefabToSpawn, transform.position, Quaternion.identity, BoardManager.instance.transform);
        //spawnedPrefab.transform.LookAt(endPos + offset);

        GetComponent<TileScript>().tileAdditionalVisual = spawnedPrefab;

        /*
        Vector3 scale = spawnedPrefab.transform.localScale;
        scale.y = Vector3.Distance(startingPos, endPos);

        spawnedPrefab.transform.localScale = scale;
        */
    }
}
