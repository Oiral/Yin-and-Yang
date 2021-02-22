using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorPlayer : MonoBehaviour
{
    public bool isPlayer;

    EditorTile targetTile;

    Vector3 targetPos;

    public float moveSpeed = 5f;

    public float heightAboveTile = 1f;
    public float nonPlacedHeight = 20f;

    public bool hasBeenPlaced;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, nonPlacedHeight, transform.position.y);
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update() {

        MoveToTarget();

        if (hasBeenPlaced == false) return;

        Vector2Int targetTileLocation;

        if (isPlayer)
        {
            targetTileLocation = LevelEditor.instance.playerLocation;
        }
        else
        {
            targetTileLocation = LevelEditor.instance.ringLocation;
        }

        if (LevelEditor.instance.newBoard.ContainsKey(targetTileLocation))
        {
            targetTile = LevelEditor.instance.newBoard[targetTileLocation];
        }
        else
        {
            targetTile = null;
            hasBeenPlaced = false;
            targetPos = new Vector3(transform.position.x, nonPlacedHeight, transform.position.z);
            return;
        }

        targetPos = targetTile.transform.position + Vector3.up * heightAboveTile;// + Vector3.up * targetTile.height;

    }

    void MoveToTarget()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }
}
