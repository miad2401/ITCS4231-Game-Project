using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TacticalMovement : MonoBehaviour
{
    List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

    Stack<Tile> movementPath = new Stack<Tile>();
    Tile currentTile;

    public bool moving = false;
    [SerializeField] public int moveDistance = 1;
    [SerializeField] public float moveSpeed = 2;

    Vector3 velocity = new Vector3();
    Vector3 charcterDirection = new Vector3();

    float halfHeight = 0;

    public void init()
    {
        tiles = GameObject.FindGameObjectsWithTag("tile");

        halfHeight = GetComponent<Collider>().bounds.extents.y;


    }

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.isSelected = true;
    }

    public Tile GetTargetTile(GameObject target) 
    { 
        RaycastHit hit;
        Tile tile = null;

        if(Physics.Raycast(target.transform.position, Vector3.down, out hit, 10))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }

    public void ComputeNeighborsList()
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors();
        }
    }

    public void FindSelectableTiles()
    {
        ComputeNeighborsList();
        GetCurrentTile();


        //Find selectable tiles using BFS
        Queue<Tile> process = new Queue<Tile>();
        process.Enqueue(currentTile);
        currentTile.hasVisited = true;
        //Set tile's parent here, leave as null for now

        //While there are legal tiles to process
        while (process.Count > 0)
        {
            //Get current tile
            Tile t = process.Dequeue();

            //Add tile to be selectable and set it to be selectable
            selectableTiles.Add(t);
            t.isSelectable = true;

            //Only process tiles that are within the move distance
            if (t.distance < moveDistance)
            {
                //Get all the tile's neighbors
                foreach (Tile tile in t.neighbors)
                {
                    //We only care about tiles that havent been visited by the algorithm yet
                    if (!tile.hasVisited)
                    {
                        tile.parent = t;
                        tile.hasVisited = true;
                        tile.distance = 1 + t.distance;

                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    public void MoveToTile(Tile tile)
    {
        movementPath.Clear();
        tile.isTarget = true;
        moving = true;

        Tile next = tile;
        while (next != null)
        {
            movementPath.Push(next);
            next = next.parent;
        }
    }
}
