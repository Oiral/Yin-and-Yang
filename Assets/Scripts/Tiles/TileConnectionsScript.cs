using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

//public enum TileSide {Top, Bottom };

public class TileConnectionsScript : MonoBehaviour {

    public List<List<GameObject>> grids = new List<List<GameObject>>() { };

    public List<TileConnectionsScript> connections = new List<TileConnectionsScript>();

    //public TileSide side = TileSide.Top;

    public float distanceCheck = 0.5f;

    public LayerMask layerMask;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (connections.Count > 0)
        {
            foreach (TileConnectionsScript neighbor in connections)
            {
                if (neighbor == null)
                {
                    continue;
                }
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(Average(transform.position, neighbor.transform.position), new Vector3(0.2f, 0.2f, 0.2f));
            }
        }

        //Lock to int values for x and z
        //lock to o.25 values for y

        /*
        Vector3 pos = transform.parent.position;

        pos.x = Mathf.Round(pos.x);
        pos.z = Mathf.Round(pos.z);
        pos.y = Mathf.Round(pos.y * 4f) / 4f;

        transform.parent.position = pos;
        */       

        UpdateConnections();

        TileScript tileParent = GetComponentInParent<TileScript>();
        if (tileParent.Type == TileType.Conveyor)
        {
            Gizmos.color = Color.red;
            //Lets draw a line in the direction that this will go

            var p1 = tileParent.transform.position;
            var p2 = tileParent.transform.position +
                DirectionHelper.VectorFromDir(DirectionHelper.CheckDirection(tileParent.transform.forward))
                ;
            var thickness = 3;
            Handles.DrawBezier(p1, p2, p1, p2, Color.red, null, thickness);
        }
    }
#endif

    private Vector3 Average(Vector3 pos1, Vector3 pos2)
    {
        return Vector3.Lerp(pos1, pos2, 0.4f);
        //return (pos1 + pos2) / 2;
    }

    public void UpdateConnections()
    {
        //Find the nearest conneciton points
        foreach (TileConnectionsScript tileCon in FindObjectsOfType(typeof(TileConnectionsScript)))
        {
            if (tileCon == this)
                continue;
            if (connections.Contains(tileCon))
                continue;




            if (CheckInHoriRange(this, tileCon))
            {
                //Only run this switch if were in horizontal range

                switch (GetComponentInParent<TileScript>().Type)
                {
                    case TileType.Jelly:
                        connections.Add(tileCon);
                        tileCon.connections.Add(this);
                        break;

                    default:
                        if (CheckInVertRange(this, tileCon))
                        {
                            connections.Add(tileCon);
                        }
                        else
                        {
                            connections.Add(tileCon);
                            tileCon.connections.Add(this);
                        }
                        break;
                }
            }



        }

        for (int i = connections.Count - 1; i >= 0; i--)
        {
            if (connections[i] == null)
            {
                connections.RemoveAt(i);
                continue;
            }

            //If we are in horizontal range
            //other wise we want to remove
            if (CheckInHoriRange(this, connections[i]))
            {
                //If the connection is to far away
                switch (connections[i].GetComponentInParent<TileScript>().Type)
                {
                    case TileType.Jelly:
                        //If it is jelly, we don't want to remove it
                        break;

                    default:
                        if (CheckInVertRange(this, connections[i]) == false)
                        {
                            //If we cannot connect vertically we want to remove this connection

                            if (connections[i].connections.Contains(this))
                            {
                                connections[i].connections.Remove(this);
                            }
                            connections.Remove(connections[i]);
                            continue;
                        }
                        break;
                }
            }
            else
            {
                if (connections[i].connections.Contains(this))
                {
                    connections[i].connections.Remove(this);
                }
                connections.Remove(connections[i]);
                continue;
            }

            //Ray cast to see if there is a barrier in the way of this connection
            if (Physics.Linecast(transform.position, connections[i].transform.position, layerMask))
            {
                //Debug.Log("Something blocked the connection");
                if (connections[i].connections.Contains(this))
                {
                    connections[i].connections.Remove(this);
                }
                connections.Remove(connections[i]);
                continue;
            }
        }

        /*
        foreach (TileConnectionsScript tileCon in connections)
        {
            if (Vector3.Distance(tileCon.gameObject.transform.position, this.transform.position) > distanceCheck)
            {
                connections.Remove(tileCon);
                tileCon.connections.Remove(this);
            }
        }
        */
    }

    bool CheckInHoriRange(TileConnectionsScript myTile, TileConnectionsScript otherTile)
    {
        Vector3 myPos = myTile.transform.position;
        Vector3 theirPos = otherTile.gameObject.transform.position;

        myPos.y = 0;
        theirPos.y = 0;

        float horizontalDist = Vector3.Distance(myPos, theirPos);

        return (horizontalDist <= distanceCheck);
    }

    bool CheckInVertRange(TileConnectionsScript myTile, TileConnectionsScript otherTile)
    {
        Vector3 myPos = myTile.transform.position;
        Vector3 theirPos = otherTile.gameObject.transform.position;

        float heightDif = myPos.y - theirPos.y;

        if (heightDif >= -0.25)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
