using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{

    private StateMachine _fsm;
    private Hunter _hunter;

    private Coroutine patrolCoroutine;
    public PatrolState(StateMachine fsm, Hunter h)
    {
        _fsm = fsm;
        _hunter = h;
    }


    public void OnStart()
    {
        Debug.Log("HUNTER -> Patrolling...");
        patrolCoroutine = _hunter.StartCoroutine(_hunter.Patrol());
    }

    public void OnUpdate()
    {
        _hunter.energy -= Time.deltaTime;
        
        if (_hunter.energy <= 0)
            {
                _fsm.ChangeState(StatesEnum.Rest);
            }
        if (_hunter.boidDetected)
        {
            _fsm.ChangeState(StatesEnum.Chase);
        }
    }

    public void OnExit()
    {
        _hunter.StopCoroutine(patrolCoroutine);
        Debug.Log("HUNTER -> Stop Patrolling");
    }


}
