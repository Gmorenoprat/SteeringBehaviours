using UnityEngine;

public class FlockingState : IState
{

    private StateMachine _fsm;
    private Boid _boid;

    private Coroutine flockingCoroutine;
    public FlockingState(StateMachine fsm, Boid h)
    {
        _fsm = fsm;
        _boid = h;
    }


    public void OnStart()
    {
        Debug.Log("BOID -> Flocking...");
        flockingCoroutine = _boid.StartCoroutine(_boid.Flocking());
    }

    public void OnUpdate()
    { 
        if (_boid.nearbyEnemy)
        {
            _fsm.ChangeState(StatesEnum.Evade);
        }
        else if (_boid.nearbyFood)
        {
            _fsm.ChangeState(StatesEnum.Arrive);
        }
    }

    public void OnExit()
    {
        _boid.StopCoroutine(flockingCoroutine);
        Debug.Log("BOID -> Stop Flocking");
    }


}