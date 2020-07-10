using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : Manager
{
    private List<State> _stateMachine; // top of the stack is the end of the list
    private State top;

    public override void Setup( IContext context )
    {
        base.Setup( context );
        _stateMachine = new List<State>();
    }

    public override void Teardown()
    {
        base.Teardown();
    }

    public void PushState( State state )
    {
        _stateMachine.Add( state );
        state.Setup( context );
        state.Enter();
        top = state;
    }

    public State PopState()
    {
        int topIndex = _stateMachine.Count - 1;
        if(topIndex < 0 )
        {
            Debug.LogError( "No states exist in the State Machine" );
            return null;
        }

        State removedState = _stateMachine[ topIndex ];

        removedState.Exit();

        _stateMachine.RemoveAt( topIndex );

        if(topIndex - 1 >= 0 )
        {
            top = _stateMachine[ topIndex - 1 ];
        }

        return removedState;
    }
}
