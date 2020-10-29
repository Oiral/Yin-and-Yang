using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public TileConnectionsScript targetTile;
    TileConnectionsScript startingTile;
    public float respawnTime = 2;

    public TileScript currentTile;

    public GameObject splashPrefab;
    public GameObject winParticlePrefab;

    public Animator turtleAnimator;

    public LevelManagerScript levelManager;

    PlayerController controller;

    public bool canMove = true;

    public float moveSpeed = 0.2f;

    public bool isMainPlayer;

    public Coroutine conveyorRoutine;

    private void Start()
    {
        controller = PlayerController.instance;
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

        return ForceMove(dir, primary);

    }

    bool ForceMove(Direction dir, bool primary)
    {

        //targetTile.UpdateConnections();
        foreach (TileConnectionsScript tile in targetTile.connections)
        {

            if (DirectionHelper.CheckDirection(targetTile.transform.position, tile.transform.position) == dir)
            {
                //DisableMovementInput(0.23f);
                switch (tile.GetComponentInParent<TileScript>().Type)
                {
                    case TileType.Default:
                        MovePlayer(tile);
                        return true;

                    case TileType.Teleport:
                        MovePlayer(tile);
                        StartCoroutine(Respawn());
                        return true;

                    case TileType.Goal:
                        if (FindObjectOfType<LevelController>().goalOpen)
                        {
                            MovePlayer(tile);
                            if (winParticlePrefab != null)
                            {
                                Instantiate(winParticlePrefab, targetTile.transform.position, targetTile.transform.rotation, null);
                            }
                            //Play the win Animation
                            //turtleAnimator.SetTrigger("Win");
                            //SoundManager.instance.PlaySound("win");
                            if (isMainPlayer)
                            {
                                levelManager.NextLevel();
                            }
                            return true;
                        }
                        else
                        {
                            LookAtConnection(dir);
                            Animate("Attempt");
                            PlaySound("Not Move");
                            return false;
                        }

                    case TileType.Ice:
                        MovePlayer(tile);
                        ForceMove(dir, primary);
                        return true;

                    case TileType.Jelly:
                        if (targetTile.transform.position.y < tile.transform.position.y)
                        {
                            LookAtConnection(tile);
                            Animate("Denied");
                            PlaySound("Slime");
                            return true;
                        }
                        else
                        {
                            MovePlayer(tile);
                            return true;
                        }

                    case TileType.Button:
                        MovePlayer(tile);
                        if (tile.GetComponentInParent<TileButton>() != null)
                        {
                            tile.GetComponentInParent<TileButton>().steppedOnEvent.Invoke();
                        }
                        else
                            Debug.LogError("Missing Tile button on Tile", tile.gameObject);

                        return true;

                    case TileType.Conveyor:
                        MovePlayer(tile);

                        if (conveyorRoutine == null)
                        {
                            conveyorRoutine = StartCoroutine(MoveOnConveyor(tile, primary));
                        }
                        return true;

                    default:
                        LookAtConnection(dir);
                        Animate("Attempt");
                        PlaySound("Not Move");
                        return false;
                }
            }
        }

        //If we cannot move
        LookAtConnection(dir);
        Animate("Attempt");
        PlaySound("Not Move");
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
        PlaySound("Move");
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
        Vector3 offset = DirectionHelper.VectorFromDir(dir);

        Vector3 lookPos = transform.position + offset;

        transform.LookAt(lookPos,Vector3.up);
    }

   public void Animate(string movement)
    {
        if (turtleAnimator != null)
        {
            turtleAnimator.SetTrigger(movement);
        }
    }

    IEnumerator Respawn()
    {
        yield return 0;

        PlayerController.instance.ToggleMovement(false);

        yield return new WaitForSeconds(0.2f);

        //Spawn respawn Particle
        //Instantiate(splashPrefab, targetTile.transform.position, transform.rotation, null);
        //SoundManager.instance.PlaySound("holeFall");

        Animate("Teleport");
        PlaySound("Teleport");

        yield return new WaitForSeconds(respawnTime);
        currentTile = startingTile.GetComponentInParent<TileScript>();
        targetTile = startingTile;
        transform.position = startingTile.transform.position;
        LevelController levelController = GameObject.FindWithTag("GameController").GetComponent<LevelController>();
        levelController.OnPlayerMove();

        PlayerController.instance.ToggleMovement(true);

    }

    void DisableMovementInput(float time)
    {
        PlayerController.instance.ToggleMovement(false);
        StartCoroutine(ReenableMovement(time));
    }

    IEnumerator ReenableMovement(float time)
    {
        yield return new WaitForSeconds(time);
        PlayerController.instance.ToggleMovement(true);
    }

    void PlaySound(string soundName)
    {
        if (AudioManager.instance != null && isMainPlayer)
        {
            AudioManager.instance.PlaySound(soundName);
        }
    }

    IEnumerator MoveOnConveyor(TileConnectionsScript tile, bool primary)
    {
        PlayerController.instance.ToggleMovement(false);
        //Move to each conveyor tile
        //We want to recursivly call this function
        
        yield return new WaitForSeconds(0.2f);

        while (currentTile.Type == TileType.Conveyor &&
            ForceMove(DirectionHelper.CheckDirection(currentTile.transform.forward), primary))
        {
            //Debug.Log(DirectionHelper.CheckDirection(currentTile.transform.forward));
            LevelController.instance.OnPlayerMove();
            yield return new WaitForSeconds(0.2f);
        }
        conveyorRoutine = null;

        PlayerController.instance.ToggleMovement(true);

    }
}

