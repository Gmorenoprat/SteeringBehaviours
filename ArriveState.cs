using UnityEngine;

public class ArriveState : IState
{

    private StateMachine _fsm;
    private Boid _boid;

    private Coroutine arriveCoroutine;
    public ArriveState(StateMachine fsm, Boid b)
    {
        _fsm = fsm;
        _boid = b;
    }


    public void OnStart()
    {
        Debug.Log("Arriving...");
        arriveCoroutine = _boid.StartCoroutine(_boid.ArriveCoroutine());
    }

    public void OnUpdate()
    {

        if (_boid.nearbyEnemy)
        {
            _fsm.ChangeState(StatesEnum.Evade);
        }
        else if (!_boid.nearbyFood)
        {
            _fsm.ChangeState(StatesEnum.Flocking);
        }
    }

    public void OnExit()
    {
        _boid.StopCoroutine(arriveCoroutine);
        Debug.Log("Stop Arrive");
    }


}