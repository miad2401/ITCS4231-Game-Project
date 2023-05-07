using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool canMoveInto = true;
    public bool isSelected = false;
    public bool isTarget = false;
    public bool isSelectable = false;
    public bool isTargetable = false;

    public List<Tile> neighbors = new List<Tile>();

    //BFS for player movement
    public bool hasVisited = false;
    public Tile parent = null;
    public int distance = 0;

    //A* for enemy movement
    public float f = 0;
    public float g = 0;
    public float h = 0;

    void Start()
    {

    }

    void Update()
    {
        //Color coding
        if (isSelected)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        } else if (isTarget) 
        {
            GetComponent<Renderer>().material.color = Color.blue;
        } else if (isSelectable) 
        {
            GetComponent<Renderer>().material.color = Color.green;
        } else if (isTargetable)
        {
            GetComponent<Renderer>().material.color = Color.red;
        } else
        {
            GetComponent<Renderer>().material.color = Color.gray;
        }
    }

    //Set all values to base values
    public void Reset()
    {
        neighbors.Clear();

        isSelectable = false;
        isTarget = false;
        isSelectable = false;
        isTargetable = false;

        hasVisited = false;
        parent = null;
        distance = 0;

        f = g = h = 0;
    }

    public void FindNeighbors(Tile target)
    {
        Reset();

        //Check all for tiles in all directions
        CheckTile(Vector3.forward, target);
        CheckTile(Vector3.back, target);
        CheckTile(Vector3.right, target);
        CheckTile(Vector3.left, target);
    }

    public void CheckTile(Vector3 direction, Tile target)
    {
        //Find overlapping colliders
        Vector3 halfExtents = new Vector3(9f, 0.25f, 9f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders)
        {
            //Check to see if object is actually a tile
            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.canMoveInto)
            {
                
                RaycastHit hit;
                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 10) || tile == target)
                {
                    //Nothing found, so add to neighbors list
                    neighbors.Add(tile);
                } else if (Physics.Raycast(tile.transform.position, Vector3.up, out hit, 10))
                {
                    //Something is on the tile
                    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

                    //if that something is not a player and the current player is attacking, allow tile to neighbors list
                    if (hit.collider.gameObject.tag != "Player")
                    {
                        foreach (GameObject obj in players)
                        {
                            if (obj.GetComponent<TacticalMovement>().currentAction == Action.action.attacking)
                            {
                                neighbors.Add(tile);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
