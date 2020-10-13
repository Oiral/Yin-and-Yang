using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum TileSide {Top, Bottom };

public class TileConnectionsScript : MonoBehaviour {

    public List<List<GameObject>> grids = new List<List<GameObject>>() {};

    public List<TileConnectionsScript> connections = new List<TileConnectionsScript>();

    //public TileSide side = TileSide.Top;

    public float distanceCheck = 0.5f;

    public LayerMask layerMask;

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

        UpdateConnections();
    }

    private Vector3 Average(Vector3 pos1, Vector3 pos2)
    {
        return Vector3.Lerp(pos1,pos2,0.4f);
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

            switch (GetComponentInParent<TileScript>().Type)
            {
                case TileType.Jelly:
                    connections.Add(tileCon);
                    tileCon.connections.Add(this);
                    break;

                default:
                    if (Vector3.Distance(tileCon.gameObject.transform.position, this.transform.position) > distanceCheck)
                    {
                        //If the tile is not in the correct distance
                        if (CheckInHoriRange(this, tileCon) && CheckInVertRange(this, tileCon))
                        {
                            connections.Add(tileCon);
                        }
                        continue;
                    }

                    connections.Add(tileCon);
                    tileCon.connections.Add(this);

                    break;
            }

        }

        for (int i = connections.Count-1; i >= 0; i--)
        {
            if (connections[i] == null)
            {
                connections.RemoveAt(i);
                continue;
            }
            switch (connections[i].GetComponentInParent<TileScript>().Type)
            {
                case TileType.Jelly:
                    //If it is jelly, we don't want to remove it due to height differences
                    break;

                default:
                    //Remove conneciton if piece is higher and should ne be moved to
                    if (Vector3.Distance(connections[i].gameObject.transform.position, this.transform.position) > distanceCheck)
                    {
                        if (CheckInHoriRange(this, connections[i]) && CheckInVertRange(this, connections[i]))
                        {
                            continue;
                        }

                        if (connections[i].connections.Contains(this))
                        {
                            connections[i].connections.Remove(this);
                        }
                        connections.Remove(connections[i]);
                        continue;
                    }
                    break;
            }

            //Ray cast to see if there is a barrier in the way of this connection
            if (Physics.Linecast(transform.position, connections[i].transform.position, layerMask))
            {
                Debug.Log("Something blocked the connection");
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

        return (horizontalDist < distanceCheck);
    }

    bool CheckInVertRange(TileConnectionsScript myTile, TileConnectionsScript otherTile)
    {
        Vector3 myPos = myTile.transform.position;
        Vector3 theirPos = otherTile.gameObject.transform.position;

        float heightDif = myPos.y - theirPos.y;

        if (heightDif > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
