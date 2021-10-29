using UnityEngine;

public class EvadeState : IState
{

    private StateMachine _fsm;
    private Boid _boid;

    private Coroutine evadeCoroutine;
    public EvadeState(StateMachine fsm, Boid h)
    {
        _fsm = fsm;
        _boid = h;
    }


    public void OnStart()
    {
        Debug.Log("Evading...");
        evadeCoroutine = _boid.StartCoroutine(_boid.EvadeCoroutine());
    }

    public void OnUpdate()
    {

        if (!_boid.nearbyEnemy)
        {
            _fsm.ChangeState(StatesEnum.Flocking);
        }
    }

    public void OnExit()
    {
        _boid.StopCoroutine(evadeCoroutine);
        Debug.Log("Stop Evade");
    }


}