using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputScript : MonoBehaviour {


    Vector2 mouseInitialPos;
    Vector2 mouseReleasePos;

    PlayerController charController;

    private void Awake()
    {
        charController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseInitialPos = Input.mousePosition;
        }else if (Input.GetMouseButtonUp(0))
        {
            //Check transform
            mouseReleasePos = Input.mousePosition;
            charController.Move(CheckDirection());
        }
    }

    Direction CheckDirection()
    {
        Vector2 differencePos = mouseInitialPos - mouseReleasePos;
        if (differencePos.x > 0 && differencePos.y > 0)
        {
            return Direction.South;
        }
        else if (differencePos.x > 0 && differencePos.y < 0)
        {
            return Direction.West;
        }
        else if (differencePos.x < 0 && differencePos.y > 0)
        {
            return Direction.East;
        }
        else
        {
            return Direction.North;
        }
    }
}
