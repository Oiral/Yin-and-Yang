using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { North, East, South, West };

public class PlayerMovement : MonoBehaviour {

    public TileConnectionsScript targetTile;
    TileConnectionsScript startingTile;
    public float respawnTime = 2;
    public GameObject otherPlayer;

    public TileScript currentTile;

    public GameObject splashPrefab;
    public GameObject winParticlePrefab;

    public Animator turtleAnimator;

    public LevelManagerScript levelManager;

    public bool canMove = true;

    public float moveSpeed = 0.2f;

    private void Start()
    {
        startingTile = targetTile;
        currentTile = targetTile.GetComponentInParent<TileScript>();
        //turtleAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Vector3 targetPos = targetTile.transform.position;
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed);

    }

    public bool MovePlayer(Direction dir,bool primary)
    {
        if (!canMove)
        {
            return false;
        }
        //targetTile.UpdateConnections();
        foreach (TileConnectionsScript tile in targetTile.connections)
        {
            
            if (CheckDirection(targetTile.transform.position, tile.transform.position) == dir)
            {
                //DisableMovementInput(0.23f);
                switch (tile.GetComponentInParent<TileScript>().Type)
                {
                    case TileType.Default:
                        MovePlayer(tile);
                        return true;

                    case TileType.Hole:
                        StartCoroutine(Respawn());
                        MovePlayer(tile);
                        return true;

                    case TileType.Goal:
                        if (FindObjectOfType<LevelController>().goalOpen && primary)
                        {
                            MovePlayer(tile);
                            if (winParticlePrefab != null)
                            {
                                Instantiate(winParticlePrefab, targetTile.transform.position, targetTile.transform.rotation, null);
                            }
                            //Play the win Animation
                            //turtleAnimator.SetTrigger("Win");
                            //SoundManager.instance.PlaySound("win");
                            levelManager.NextLevel();
                            return true;
                        }
                        else if (tile.gameObject.transform.parent == otherPlayer.GetComponent<PlayerMovement>().targetTile.gameObject.transform.parent && !primary)
                        {
                            MovePlayer(tile);
                            if (winParticlePrefab != null)
                            {
                                Instantiate(winParticlePrefab, targetTile.transform.position, targetTile.transform.rotation, null);
                            }
                            //Play the win Animation
                            //turtleAnimator.SetTrigger("Win");
                            //SoundManager.instance.PlaySound("win");
                            return true;
                        }
                        else
                        {
                            LookAtConnection(dir);
                            Animate("Attempt");
                            return false;
                        }

                    case TileType.Ice:
                        MovePlayer(tile);
                        MovePlayer(dir, primary);
                        return true;

                    case TileType.Jelly:
                        if (targetTile.transform.position.y < tile.transform.position.y)
                        {
                            LookAtConnection(tile);
                            Animate("Denied");
                            return true;
                        }
                        else
                        {
                            MovePlayer(tile);
                            return true;
                        }

                    default:
                        LookAtConnection(dir);
                        Animate("Attempt");
                        return false;
                }
            }
        }

        //If we cannot move
        LookAtConnection(dir);
        Animate("Attempt");
        return false;
    }
    
    void MovePlayer(TileConnectionsScript tile)
    {
        targetTile = tile;

        //Set rotation of player

        /*
        Vector3 lookPos = targetTile.transform.position - transform.position;
        lookPos.y = 0;

        Quaternion rotation = Quaternion.LookRotation(lookPos);

        Vector3 myRotation = transform.rotation.eulerAngles;
        myRotation.x = 0;
        myRotation.z = 0;

        transform.rotation = rotation * Quaternion.Euler(myRotation);
        */

        /*Vector3 lookPos = targetTile.transform.position - transform.position;
        lookPos.y = 0;

        Quaternion BoardRotation = GameObject.FindGameObjectWithTag("Board").transform.rotation;

        if (BoardRotation != Quaternion.identity)
        {
            BoardRotation.eulerAngles = new Vector3(180, 180, 0);
        }

        Quaternion rotation = Quaternion.LookRotation(lookPos) * BoardRotation;
        //transform.rotation = rotation;*/


        LookAtConnection(tile);

        //Play the animation
        Animate("Move");
        //SoundManager.instance.PlaySound("walkSummer");

        currentTile = tile.GetComponentInParent<TileScript>();
    }

    void LookAtConnection(TileConnectionsScript tile)
    {
        Vector3 lookPos = tile.transform.position - transform.position;
        lookPos.y = 0;

        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
    }

    void LookAtConnection(Direction dir)
    {
        Vector3 offset = VectorFromDir(dir);

        Vector3 lookPos = transform.position + offset;

        transform.LookAt(lookPos,Vector3.up);
    }

    private Direction CheckDirection(Vector3 startingPos, Vector3 checkingPos)
    {
        if (startingPos.x < checkingPos.x)
        {
            return Direction.East;
        }else if (startingPos.x > checkingPos.x)
        {
            return Direction.West;
        }else if (startingPos.z < checkingPos.z)
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

    private Vector3 VectorFromDir(Direction dir)
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

    void Animate(string movement)
    {
        if (turtleAnimator != null)
        {
            turtleAnimator.SetTrigger(movement);
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(0.2f);

        //Spawn respawn Particle
        Instantiate(splashPrefab, targetTile.transform.position, transform.rotation, null);
        //SoundManager.instance.PlaySound("holeFall");

        yield return new WaitForSeconds(respawnTime);
        targetTile = startingTile;
    }

    public void DisableMovementInputOnRotation(float time)
    {
        DisableMovementInput(time);
    }

    void DisableMovementInput(float time)
    {
        canMove = false;
        StartCoroutine(ReenableMovement(time));
    }

    IEnumerator ReenableMovement(float time)
    {
        yield return new WaitForSeconds(time);
        canMove = true;
    }
}
