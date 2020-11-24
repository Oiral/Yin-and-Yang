using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

    PlayerController controller;

    public BoardManager boardManager;

    public bool goalOpen;



    public static LevelController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogWarning("An additional Board Manager has been found, Destroying", gameObject);
            Destroy(this);
        }
    }

    private void Start()
    {
        controller = PlayerController.instance;
        OnPlayerMove();
    }

    public void WinLevel()
    {

    }

    public void OnPlayerMove()
    {
        goalOpen = CheckAllKeysOnPlayer();
    }

   public bool CheckAllKeysOnPlayer()
    {
        

        //if all keys are in the same spot as the player

        TileScript playerTile = controller.playerMoveScript.currentTile;

        //If the player is on the goal, This should return true
        if (playerTile.Type == TileType.Goal)
        {
            return true;
        }

        bool allKeysOnPlayer = true;

        foreach (PlayerMovement key in controller.keyMoveScripts)
        {
            if (key.currentTile != playerTile)
            {
                //If any key is not on the same tile this should return false
                allKeysOnPlayer = false;
            }
        }

        boardManager.ChangeGoalMaterial(allKeysOnPlayer);

        //We only get here is all the keys are in the same place as the player
        return allKeysOnPlayer;

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
