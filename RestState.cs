using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestState : IState
{
    private StateMachine _fsm;
    private Hunter _hunter;

    public const float restTimer = 5f;
    public float timer = 5f;

    public RestState(StateMachine fsm, Hunter h)
    {
        _fsm = fsm;
        _hunter = h;
    }


    public void OnStart()
    {
        Debug.Log("HUNTER -> Resting...");
    }


    public void OnUpdate()
    {
        timer -= Time.deltaTime;
        
        if (timer <= 0)
        {
            timer = restTimer;
            _fsm.ChangeState(StatesEnum.Patrol);
        }
    }


    public void OnExit()
    {
        _hunter.energy = _hunter.maxEnergy;
        Debug.Log("HUNTER -> Stop Rest");
    }

}
