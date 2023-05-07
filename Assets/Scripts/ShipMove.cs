using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

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

        if (!moving)
        {
            FindSelectableTiles();
            CheckMouse();
        } else if (currentAction == action.moving)
        {
            Move();
        }
        if (currentAction == action.attacking)
        {
            CheckMouse();
        }
        
    }

    #region attack functions
    public void AttackAction(Tile t)
    {
        Debug.Log(this + " Attacked! Tile Attacked: " + t);

        TacticalMovement attacked;

        //Find ship attacked, if it exists
        RaycastHit hit;
        if (Physics.Raycast(t.transform.position, Vector3.up, out hit, 10))
        {
            attacked = hit.collider.GetComponent<TacticalMovement>();
            Debug.Log(attacked + " attacked");

            System.Random rand = new System.Random();
            int attackRoll = rand.Next(1, 20);
            if (attackRoll < attackPower)
            {
                int dodgeRoll = rand.Next(1, 100);
                if (dodgeRoll < attacked.dodge)
                {
                    Debug.Log(attacked + " dodged!");
                } else
                {
                    damageEnemy(attacked);
                }
            } else
            {
                Debug.Log("Miss!");
            }
        }
        EndAttack();
    }

    public void damageEnemy(TacticalMovement attacked)
    {
        Debug.Log("Attack Hit! Damage: " + damage);
        if (attacked.armor > 0)
        {
            int overflow = attacked.armor - damage;
            attacked.health -= overflow;
        } else
        {
            attacked.health -= damage;
            Debug.Log("Remaining health: " + attacked.health);
        }
        
        if (attacked.health <= 0)
        {
            attacked.shipDeath();
        }
    }

    public void EndAttack()
    {
        RemoveSelectableTiles();

        currentAction = action.moving;

        TurnManager.EndTurn();
    }
    #endregion

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
                    if (t.isSelectable || t.isTargetable)
                    {
                        if (currentAction == action.moving)
                        {
                            MoveToTile(t);
                        } else if (currentAction == action.attacking)
                        {
                            AttackAction(t);
                        }
                    }
                }
            }
        }
    }
}
