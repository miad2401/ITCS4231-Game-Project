using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    [HideInInspector ]public enum action { attacking, moving, waiting};
    [HideInInspector] public bool turn = false;

    [HideInInspector] public GameObject[] tiles;
    [HideInInspector] public List<Tile> selectableTiles = new List<Tile>();
    [HideInInspector] public Tile currentTile;

    [SerializeField] public int moveDistance = 1;
    [SerializeField] public int health = 5;
    [SerializeField] public int dodge = 3;
    [SerializeField] public int armor = 2;
    [SerializeField] public int attackPower = 5;
    [SerializeField] public int damage = 3;

    [HideInInspector] public action currentAction;

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        //currentTile.isSelected = true;
    }

    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;

        if (Physics.Raycast(target.transform.position, Vector3.down, out hit, 10))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }

    public void ComputeNeighborsList(Tile target)
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(target);
        }
    }

    public void FindSelectableTiles()
    {
        ComputeNeighborsList(null);
        GetCurrentTile();


        //Find selectable tiles using BFS
        Queue<Tile> process = new Queue<Tile>();
        process.Enqueue(currentTile);
        currentTile.hasVisited = true;

        //While there are legal tiles to process
        while (process.Count > 0)
        {
            //Get current tile and add tile to be selectable and set it to be selectable
            Tile t = process.Dequeue();

            selectableTiles.Add(t);
            
            if (currentAction == action.attacking)
            {
                t.isTargetable = true;
                t.isSelectable = false;
            } else if (currentAction == action.moving)
            {
                t.isSelectable = true;
                t.isTargetable = false;
            } else
            {
                t.isSelectable = false;
                t.isTargetable = false;
            }
            

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

    public void RemoveSelectableTiles()
    {
        if (currentTile != null)
        {
            currentTile.isSelected = false;
            currentTile = null;
        }

        foreach (Tile t in selectableTiles)
        {
            t.Reset();
        }

        selectableTiles.Clear();
    }

    public void shipDeath()
    {
        Destroy(this);
    }
}
