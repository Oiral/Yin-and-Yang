using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum Direction { North, East, South, West };

public static class DirectionHelper
{
    public static Direction CheckDirection(Vector3 startingPos, Vector3 checkingPos)
    {
        if (startingPos.x < checkingPos.x)
        {
            return Direction.East;
        }
        else if (startingPos.x > checkingPos.x)
        {
            return Direction.West;
        }
        else if (startingPos.z < checkingPos.z)
        {
            return Direction.North;
        }
        else if (startingPos.z > checkingPos.z)
        {
            return Direction.South;
        }
        else
        {
            Debug.LogError("Can't find Direction - Defaulting to North");
            return Direction.North;
        }

    }

    public static Vector3 VectorFromDir(Direction dir)
    {
        Vector3 vector = Vector3.zero;

        switch (dir)
        {
            case Direction.North:
                return Vector3.forward;

            case Direction.South:
                return Vector3.back;

            case Direction.East:
                return Vector3.right;

            case Direction.West:
                return Vector3.left;
        }
        return vector;
    }
}
