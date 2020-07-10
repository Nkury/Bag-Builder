using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected IContext _context;

    public virtual void Setup( IContext context )
    {
        _context = context;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }
}
