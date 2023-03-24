using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Action
{

    [SerializeField] public int attackDistance = 3;

    bool attacking = false;

    Tile attackTile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!turn)
        {
            return;
        }

        if (!attacking && currentAction == action.attacking)
        {
            FindSelectableTiles();
            CheckMouse();
            if (tag == "NPCShips")
            {
                PrepAttack(attackTile);
                AttackAction();
            }

        } else if (currentAction == action.attacking)
        {
            AttackAction();
        }
    }

    public void StartAttack(Tile t)
    {
        currentAction = action.attacking;
        attackTile = t;
        if (tag == "Player")
        {
            turn = true;
        }
    }

    public void AttackAction()
    {
        Debug.Log("Attacked!");
        EndAttack();
    }

    public void EndAttack()
    {
        RemoveSelectableTiles();
        currentAction = action.moving;
        
        TurnManager.EndTurn();
    }

    void PrepAttack(Tile t)
    {
        attacking = true;
        attackTile = t;
        t.isTarget = true;
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
                        PrepAttack(t);
                    }
                }
            }
        }
    }
}
