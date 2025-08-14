using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateMachine<T> where T : UnitBase
{
    private BaseState<T> _currentState;
    private BaseState<T> _nextState;

    public void Update()
    {
        _currentState?.Update();
        
        if (_nextState != null)
        {
            Transition(_nextState);
            _nextState = null;
        }
    }

    public void ChangeState(BaseState<T> newState)
    {
        _currentState = newState;
    }

    private void Transition(BaseState<T> newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }
}
