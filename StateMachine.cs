using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{

    IState _currentState = new BlankState();
    Dictionary<StatesEnum, IState> _allStates = new Dictionary<StatesEnum, IState>();

    public void OnUpdate()
    {
        _currentState.OnUpdate();
    }

    public void ChangeState(StatesEnum id)
    {
        if (!_allStates.ContainsKey(id)) return;

        _currentState.OnExit();
        _currentState = _allStates[id];
        _currentState.OnStart();

    }

    public void AddState(StatesEnum id, IState state)
    {
        if (_allStates.ContainsKey(id)) return;
        _allStates.Add(id, state);
    }
}

public enum StatesEnum
{
    Rest,
    Patrol,
    Chase,
    Flocking,
    Arrive,
    Evade
}
