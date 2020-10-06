using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileSide {Top, Bottom };

public class TileConnectionsScript : MonoBehaviour {

    public List<List<GameObject>> grids = new List<List<GameObject>>() {};

    public List<TileConnectionsScript> connections = new List<TileConnectionsScript>();

    public TileSide side = TileSide.Top;

    public float distanceCheck = 0.5f;

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
        foreach (TileConnectionsScript tileCon in FindObjectsOfType(typeof(TileConnectionsScript)))
        {
            if (tileCon == this)
                continue;
            if (connections.Contains(tileCon))
                continue;
            if (Vector3.Distance(tileCon.gameObject.transform.position, this.transform.position) > distanceCheck)
                continue;
            if (tileCon.side != side)
                continue;


            connections.Add(tileCon);
            tileCon.connections.Add(this);
        }

        for (int i = connections.Count-1; i >= 0; i--)
        {
            if (connections[i] == null)
            {
                connections.RemoveAt(i);
                continue;
            }

            if (Vector3.Distance(connections[i].gameObject.transform.position, this.transform.position) > distanceCheck)
            {
                connections[i].connections.Remove(this);
                connections.Remove(connections[i]);

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

}
