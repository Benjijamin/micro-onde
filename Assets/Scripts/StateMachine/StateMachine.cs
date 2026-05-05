using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private State startState;

    private State currentState;
    private Dictionary<Type, State> typeToStateMap = new Dictionary<Type, State>();

    private void Awake()
    {
        State[] states = GetComponentsInChildren<State>();
        foreach (State state in states)
        {
            typeToStateMap.Add(state.GetType(), state);
            state.InitializeStateMachine(this);
        }
    }

    private void Start()
    {
        SetState(startState, true);
    }

    private void Update()
    {
        currentState?.StateUpdate();
    }

    public void SetState(State nextState, bool forceReset)
    {
        if(currentState != nextState || forceReset)
        {
            currentState?.StateExit();
            currentState = nextState;
            currentState?.StateEnter();
        }
    }

    public void SetState(Type nextState, bool forceReset)
    {
        SetState(typeToStateMap[nextState], forceReset);
    }
}
