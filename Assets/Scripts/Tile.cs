using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool canMoveInto = true;
    public bool isSelected = false;
    public bool isTarget = false;
    public bool isSelectable = false;

    public List<Tile> neighbors = new List<Tile>();

    public bool hasVisited = false;
    public Tile parent = null;
    public int distance = 0;

    void Start()
    {

    }

    void Update()
    {
        //Color coding
        if (isSelected)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
        else if (isTarget)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (isSelectable)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else
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

        hasVisited = false;
        parent = null;
        distance = 0;
    }

    public void FindNeighbors()
    {
        Reset();

        //Check all for tiles in all directions
        CheckTile(Vector3.forward);
        CheckTile(Vector3.back);
        CheckTile(Vector3.right);
        CheckTile(Vector3.left);
    }

    public void CheckTile(Vector3 direction)
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
                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 10))
                {
                    //If nothing is found, add tile to neighbors
                    neighbors.Add(tile);
                }
            }
        }
    }
}
