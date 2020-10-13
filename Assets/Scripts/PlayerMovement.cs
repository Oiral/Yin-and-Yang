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
        turtleAnimator = GetComponentInChildren<Animator>();
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
                        }else if (tile.gameObject.transform.parent == otherPlayer.GetComponent<PlayerMovement>().targetTile.gameObject.transform.parent && !primary)
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
                            return false;

                    case TileType.Ice:
                        MovePlayer(tile);
                        MovePlayer(dir, primary);
                        return true;

                    case TileType.Jelly:
                        if (currentTile.transform.position.y < tile.transform.position.y)
                        {
                            return true;
                        }
                        else
                        {
                            MovePlayer(tile);
                            return true;
                        }

                    default:
                        return false;
                }
            }
        }
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
        Vector3 lookPos = targetTile.transform.position - transform.position;
        lookPos.y = 0;

        Quaternion BoardRotation = GameObject.FindGameObjectWithTag("Board").transform.rotation;

        //Holy Jesus Rotation Magic
        Vector3 eulerAngleRotOffset = new Vector3(BoardRotation.eulerAngles.z, BoardRotation.eulerAngles.y, BoardRotation.eulerAngles.x);


        Quaternion rotation = Quaternion.LookRotation(lookPos);
        rotation = Quaternion.Euler(rotation.eulerAngles + eulerAngleRotOffset);
        //transform.rotation = rotation;

        //Play the animation
        //turtleAnimator.SetTrigger("Move");
        //SoundManager.instance.PlaySound("walkSummer");

        currentTile = tile.GetComponentInParent<TileScript>();
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
