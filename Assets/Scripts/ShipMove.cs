using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShipMove : TacticalMovement
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (!turn)
        {
            return;
        }

        if (!moving && currentAction == action.moving)
        {
            FindSelectableTiles();
            CheckMouse();
        } else if (currentAction == action.moving)
        {
            Move();
        }
    }

    void CheckMouse()
    {
        //This checks for move target
        if (Input.GetMouseButtonUp(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "tile") 
                {
                    Tile t = hit.collider.GetComponent<Tile>();
                    if (t.isSelectable)
                    {
                        MoveToTile(t);
                    }
                }
            }
        }
    }
}
