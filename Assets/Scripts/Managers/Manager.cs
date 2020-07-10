using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : IManager
{
    protected IContext context;

    public virtual void Setup( IContext context )
    {
        this.context = context;
    }

    public virtual void Teardown ( )
    {

    }
}
