using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TacticalMovement : Action
{
    [SerializeField] public Attack attack;

    Stack<Tile> movementPath = new Stack<Tile>();

    [HideInInspector] public bool moving = false;
    
    [SerializeField] public float moveSpeed = 2;

    Vector3 velocity = new Vector3();
    Vector3 charcterDirection = new Vector3();

    float halfHeight = 0;

    [HideInInspector] public Tile actualTargetTile;

    public void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("tile");

        halfHeight = GetComponent<Collider>().bounds.extents.y;

        TurnManager.AddUnit(this);
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

    public void Move()
    {
        if (movementPath.Count > 0)
        {
            Tile t = movementPath.Peek();
            Vector3 target = t.transform.position;

            //Calculate unit position on top of target tile
            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(transform.position, target) >= 0.5f)
            {
                calculateCharDirection(target);
                SetHorizontalVelocity();

                transform.right = -charcterDirection;
                transform.position += velocity * Time.deltaTime;
            } else
            {
                //Tile center reached
                transform.position = target;
                movementPath.Pop();
            }

        } else
        {
            RemoveSelectableTiles();
            moving = false;

            //After done moving, begin attack phase
            attack.StartAttack(actualTargetTile);
            if (tag == "NPCShips")
            {
                TurnManager.EndTurn();
            }
        }
    }

    public void calculateCharDirection(Vector3 target)
    {
        charcterDirection = target - transform.position;
        charcterDirection.Normalize();
    }

    public void SetHorizontalVelocity()
    {
        velocity = charcterDirection * moveSpeed;
    }

    public Tile FindLowestF(List<Tile> tiles)
    {
        Tile lowest = tiles[0];
        foreach (Tile t in tiles)
        {
            if (t.f < lowest.f)
            {
                lowest = t;
            }
        }

        tiles.Remove(lowest);

        return lowest;
    }

    public Tile FindEndTile(Tile t)
    {
        Stack<Tile> tempPath = new Stack<Tile>();

        Tile next = t.parent;
        while (next != null)
        {
            tempPath.Push(next);
            next = next.parent;
        }

        if (tempPath.Count <= moveDistance) 
        {
            return t.parent;
        }

        Tile endTile = null;
        for (int i = 0; i <= moveDistance; i++)
        {
            endTile = tempPath.Pop();
        }

        return endTile;
    }

    public void FindPath(Tile target)
    {
        ComputeNeighborsList(target);
        GetCurrentTile();

        List<Tile> openList = new List<Tile>();
        List<Tile> closeList = new List<Tile>();

        openList.Add(currentTile);
        currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
        currentTile.f = currentTile.h;
        
        while (openList.Count > 0)
        {
            Tile t = FindLowestF(openList);

            closeList.Add(t);

            if (t == target)
            {
                actualTargetTile = FindEndTile(t);
                MoveToTile(actualTargetTile);
                return;
            }

            foreach (Tile tile in t.neighbors)
            {
                if (closeList.Contains(tile))
                {
                    //Already processed
                } 
                else if (openList.Contains(tile))
                {
                    float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);

                    if (tempG < tile.g)
                    {
                        tile.parent = t;

                        tile.g = tempG;
                        tile.f = tile.g + tile.h;
                    }
                } else
                {
                    tile.parent = t;

                    tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    tile.h = Vector3.Distance(tile.transform.position, target.transform.position);
                    tile.f = tile.g + tile.h;

                    openList.Add(tile);
                }
            }


        }
    }

    public void BeginTurn()
    {
        turn = true;
        this.currentAction = action.moving;
    }

    public void EndTurn()
    {
        turn = false;
    }
}
