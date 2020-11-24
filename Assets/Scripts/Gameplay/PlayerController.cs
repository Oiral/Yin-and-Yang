using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public PlayerMovement playerMoveScript;
    public List<PlayerMovement> keyMoveScripts = new List<PlayerMovement>();

    public RotateBoard rotateScript;

    public int moveCount = 0;
    public int bestMoveCount = 0;

    public UIManager userInterface;

    public static PlayerController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogWarning("An additional Board Manager has been found, Destroying");
            Destroy(this);
        }

        moveCount = 0;
        UpdateMoveCount(0);
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetButtonDown("up"))
		{
            Move(Direction.North);
        }
		if (Input.GetButtonDown("down"))
        {
            Move(Direction.South);
        }
		if (Input.GetButtonDown("left"))
        {
            Move(Direction.West);
        }
		if (Input.GetButtonDown("right"))
        {
            Move(Direction.East);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            /*
            selectedMoveScript.DisableMovementInputOnRotation(rotateScript.waitTime + rotateScript.framesOfRotation * 0.02f);
            rotateScript.BeginRotation();
            FlipSelected();
            */           
        }
    }

    public void Move(Direction dir)
    {
        if (playerMoveScript.MovePlayer(dir, true))
        {
            UpdateMoveCount(1);
            //Debug.Log("Move Other Player");
            //keyMoveScripts.MovePlayer(dir, false);
            moveKeys(dir);

            GetComponent<LevelController>().OnPlayerMove();
        }
    }

    public void TouchInputs(string dir)
    {
        switch (dir)
        {
            case "North":
                Move(Direction.North);
                break;
            case "South":
                Move(Direction.South);
                break;
            case "East":
                Move(Direction.East);
                break;
            case "West":
                Move(Direction.West);
                break;
        }
    }

    /*
    void FlipSelected()
    {
        PlayerMovement temp = selectedMoveScript;
        selectedMoveScript = keyMoveScripts;
        keyMoveScripts = temp;

        onMove.Invoke();
    }
    */

    void moveKeys(Direction dir)
    {
        foreach (PlayerMovement key in keyMoveScripts)
        {
            key.MovePlayer(dir, false);
        }
    }

    void UpdateMoveCount(int changeNum)
    {
        moveCount += changeNum;

        userInterface.UpdateMoveCount(moveCount);
    }

    public void ToggleMovement(bool toggle){

        if (toggle)
        {
            playerMoveScript.canMove = toggle;

            foreach (PlayerMovement key in keyMoveScripts)
            {
                key.canMove = toggle;
            }
        }
        else
        {
            StartCoroutine(DisableMovement());
        }


    }

    IEnumerator DisableMovement()
    {
        yield return 0;
        playerMoveScript.canMove = false;

        foreach (PlayerMovement key in keyMoveScripts)
        {
            key.canMove = false;
        }
    }

    public void MassAnimation(string animationName)
    {
        playerMoveScript.Animate(animationName);

        foreach (PlayerMovement key in keyMoveScripts)
        {
            key.Animate(animationName);
        }
    }
}
