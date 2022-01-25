using System;
using System.Collections;
using UnityEngine;

using Characters;
using Characters.Enemies.States;
using System.Collections.Generic;

public class StateMachine
{
    public Stack<State> _overlapStates;

    public StateMachine()
    {
        CurrentState = new NullState();
        _overlapStates = new Stack<State>();
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

    public void OverlapState(AttackState overlapState)
    {
        _overlapStates.Push(CurrentState);

        overlapState.Enter();
        CurrentState = overlapState;
    }

    public void QuitFromOverlap()
    {
        if(_overlapStates.Count < 0)
        {
            return;
        }

        CurrentState = _overlapStates.Pop();
    }
}
