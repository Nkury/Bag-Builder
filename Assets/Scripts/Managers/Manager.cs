using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Manager : IManager
{
    protected IContext context;

    public virtual async Task Setup( IContext context )
    {
        this.context = context;
        RegisterEvents();        
    }

    public virtual void Teardown ( )
    {
        DeregisterEvents();
    }

    protected virtual void RegisterEvents()
    {

    }

    protected virtual void DeregisterEvents()
    {

    }
}
