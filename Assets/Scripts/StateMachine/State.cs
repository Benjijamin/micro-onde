using UnityEngine;

public class State : MonoBehaviour
{
    protected StateMachine stateMachine;

    public void InitializeStateMachine(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void StateEnter()
    {

    }

    public virtual void StateUpdate()
    {

    }

    public virtual void StateExit()
    {

    }
}
