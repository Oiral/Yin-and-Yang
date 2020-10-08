using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour {

    public PlayerMovement selectedMoveScript;
    public PlayerMovement otherMoveScript;

    public RotateBoard rotateScript;

    public UnityEvent onMove;

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
        if (selectedMoveScript.MovePlayer(dir, true))
        {
            //Debug.Log("Move Other Player");
            otherMoveScript.MovePlayer(dir, false);
            onMove.Invoke();
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

    void FlipSelected()
    {
        PlayerMovement temp = selectedMoveScript;
        selectedMoveScript = otherMoveScript;
        otherMoveScript = temp;

        onMove.Invoke();
    }

    
}
