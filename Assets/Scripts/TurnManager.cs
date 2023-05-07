using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    static Dictionary<string, List<TacticalMovement>> units = new Dictionary<string, List<TacticalMovement>>();
    static Queue<string> turnKey = new Queue<string>();
    static Queue<TacticalMovement> turnTeam = new Queue<TacticalMovement>();

    

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (turnTeam.Count == 0) 
        { 
            InitTeamQueue();
        }
    }

    public static void InitTeamQueue()
    {
        List<TacticalMovement> teamlist = units[turnKey.Peek()];

        foreach(TacticalMovement unit in teamlist) 
        {
            turnTeam.Enqueue(unit);
        }

        StartTurn();
    }

    public static void StartTurn()
    {
        if (turnTeam.Count > 0)
        {
            turnTeam.Peek().BeginTurn();
        }
    }

    public static void EndTurn()
    {
        TacticalMovement unit = turnTeam.Dequeue();
        unit.currentAction = Action.action.moving;
        unit.EndTurn();

        if (turnTeam.Count > 0)
        {
            StartTurn();
        } else
        {
            string team = turnKey.Dequeue();
            turnKey.Enqueue(team);
            InitTeamQueue();
        }
    }

    public static void AddUnit(TacticalMovement unit)
    {
        List<TacticalMovement> list;

        if (!units.ContainsKey(unit.tag)) 
        {
            list = new List<TacticalMovement>();
            units[unit.tag] = list;

            if (!turnKey.Contains(unit.tag))
            {
                turnKey.Enqueue(unit.tag);
            }
        } else
        {
            list = units[unit.tag];
        }

        list.Add(unit);
    }

    public void attackClicked()
    {
        TacticalMovement unit = turnTeam.Peek();
        unit.StartAttack();
    }

    public void skipClicked()
    {
        TacticalMovement unit = turnTeam.Peek();
    }
}
