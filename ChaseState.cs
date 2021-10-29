using UnityEngine;

public class ChaseState : IState
{

    private StateMachine _fsm;
    private Hunter _hunter;

    private Coroutine patrolCoroutine;
    public ChaseState(StateMachine fsm, Hunter h)
    {
        _fsm = fsm;
        _hunter = h;
    }


    public void OnStart()
    {
        Debug.Log("HUNTER -> Chasing...");
        patrolCoroutine = _hunter.StartCoroutine(_hunter.Chase());

    }

    public void OnUpdate()
    {
        _hunter.energy -= Time.deltaTime * 5;

        if (_hunter.energy <= 0)
        {
            _fsm.ChangeState(StatesEnum.Rest);
        }
        else if (!_hunter.boidDetected)
        {
            _fsm.ChangeState(StatesEnum.Patrol);
        }
    }

    public void OnExit()
    {
        _hunter.StopCoroutine(patrolCoroutine);
        Debug.Log("HUNTER -> Stop Chase");
    }


}
