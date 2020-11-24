using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputScript : MonoBehaviour {


    Vector2 mouseInitialPos;
    Vector2 mouseReleasePos;

    PlayerController charController;

    public float touchDeadZone = 100f;

    private void Awake()
    {
        charController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (AdManager.instance != null && AdManager.instance.IsAdRunning())
        {
            //We want to not take any inputs if there is an ad running
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            mouseInitialPos = Input.mousePosition;
        }else if (Input.GetMouseButtonUp(0))
        {
            //Check transform
            mouseReleasePos = Input.mousePosition;

            if (Vector2.Distance(mouseInitialPos, mouseReleasePos) > touchDeadZone)
            {
                //Debug.Log(mouseReleasePos);
                charController.Move(CheckDirection());
            }
        }
    }

    Direction CheckDirection()
    {
        Vector2 differencePos = mouseInitialPos - mouseReleasePos;

        differencePos.Normalize();

        float angleDirection = Mathf.Atan2(differencePos.y, differencePos.x) * Mathf.Rad2Deg;

        //Debug.Log(angleDirection);

        /*
        if (Mathf.Abs(differencePos.x) > Mathf.Abs(differencePos.y)){
            //If the x axis is bigger
            //Debug.Log("X axis");
            if (differencePos.x > 0)
            {
                return Direction.West;
            }
            else
            {
                return Direction.East;
            }
        }
        else
        {
            //If the Y axis is bigger
            //Debug.Log("Y axis");
            if (differencePos.y > 0)
            {
                return Direction.South;
            }
            else
            {
                return Direction.North;
            }
        }
        */


        if (angleDirection > 65 && angleDirection < 155)
        {
            //If less than 45
            return Direction.South;
        }else if (angleDirection > -115 && angleDirection < -25)
        {
            return Direction.North;
        }else if (angleDirection > -25 && angleDirection < 65)
        {
            return Direction.West;
        }
        else
        {
            return Direction.East;
        }

        /*
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
        */
    }
}
