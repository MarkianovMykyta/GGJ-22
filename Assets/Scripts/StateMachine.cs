using System;
using System.Collections;
using UnityEngine;

using Characters;

public class StateMachine
{
    public StateMachine()
    {
        CurrentState = new NullState();
    }

    public State CurrentState { get; private set; }

    public void Initialize(State startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
    }

    public void Update()
    {
        if(CurrentState != null)
        {
            CurrentState.Update();
        }
    }

    public void FixedUpdate()
    {
        if(CurrentState != null)
        {
            CurrentState.FixedUpdate();
        }
    }

    public void ChangeState(State newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        newState.Enter();
    }
}
