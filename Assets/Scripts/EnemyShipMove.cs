using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipMove : TacticalMovement
{
    GameObject target;

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
            FindNearestTarget();
            CalculatePath();
            FindSelectableTiles();
            actualTargetTile.isTarget = true;
            if (currentAction == action.waiting)
            {
                currentAction = action.attacking;
            }
        }
        else if (currentAction == action.moving)
        {
            Move();
        }
        if (currentAction == action.attacking)
        {
            AttackAction();
        }
    }

    public void CalculatePath()
    {
        Tile targetTile = GetTargetTile(target);
        FindPath(targetTile);
    }

    public void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        Debug.Log("Number of targets: " + targets.Length);

        GameObject nearest = null;
        float distance = Mathf.Infinity;

        foreach (GameObject obj in targets)
        {
            float d = Vector3.Distance(transform.position, obj.transform.position);

            if (d < distance)
            {
                distance = d;
                nearest = obj;
            }
        }

        target = nearest;
    }

    public void AttackAction()
    {
        Tile t = actualTargetTile;
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
                }
                else
                {
                    damageEnemy(attacked);
                }
            }
            else
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
        }
        else
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
}
