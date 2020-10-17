using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

    PlayerController controller;

    public BoardManager boardManager;

    public bool goalOpen;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        OnPlayerMove();
    }

    public void WinLevel()
    {

    }

    public void OnPlayerMove()
    {
        goalOpen = CheckAllKeysOnPlayer();

        boardManager.ChangeGoalMaterial(goalOpen);
    }

    bool CheckAllKeysOnPlayer()
    {
        //if all keys are in the same spot as the player

        TileScript playerTile = controller.playerMoveScript.currentTile;

        foreach (PlayerMovement key in controller.keyMoveScripts)
        {
            if (key.currentTile != playerTile)
            {
                //If any key is not on the same tile this should return false
                return false;
            }
        }

        //We only get here is all the keys are in the same place as the player
        return true;

    }

    /*
    public void ChangeGoalMaterial(Material mat)
    {
        foreach (TileScript tile in boardManager.GetTiles(TileType.Goal))
        {
            Material[] rendererMats = tile.GetComponent<MeshRenderer>().materials;

            rendererMats[0] = mat;

            tile.GetComponent<MeshRenderer>().materials = rendererMats;
        } 
    }*/
}
