using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid : MonoBehaviour
{
    public bool tileSelected = false;
    UnityEngine.Grid grid;
    private Tile tile;

   // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<UnityEngine.Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.y = 0;
            selectionHandler(mousePos);
        }
    }

    public void selectionHandler(Vector3 mousePos)
    {
        Vector3Int currentTilePos = grid.WorldToCell(mousePos);
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("tile");
        GameObject[] playerShips = GameObject.FindGameObjectsWithTag("playerShips");
        GameObject selectedTile;
        GameObject selectedShip;

        

        foreach(GameObject ship in playerShips) 
        { 
            if (ship.transform.position == currentTilePos)
            {

            }
        }

        if (!tileSelected)
        {
            Debug.Log("searching for potential tile");
            foreach (GameObject potentialTile in tiles)
            {
                Debug.Log("tile postition: " + grid.WorldToCell(potentialTile.transform.position) + " || mouse position: " + currentTilePos);
                if (grid.WorldToCell(potentialTile.transform.position) == currentTilePos)
                {
                    selectedTile = potentialTile;
                    tile = potentialTile.GetComponent<Tile>();
                    

                    tileSelected = true;
                    tile.isSelected = true;

                }
            }
            


        }
    }
}
