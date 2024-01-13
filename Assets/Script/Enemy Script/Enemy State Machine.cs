using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    
    public EnemyState currentState { get; private set; }
    public void Initialize(EnemyState _startState)
    {
        currentState = _startState;
    }
    public void changeState(EnemyState _nextState)
    {
        currentState.Exit();
        currentState = _nextState;
        currentState.Enter();
    }
}
