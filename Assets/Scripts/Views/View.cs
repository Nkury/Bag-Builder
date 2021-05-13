using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    protected IContext _context; 

    // Start is called before the first frame update
    public virtual void InstantiateView( IContext context )
    {
        _context = context;
        RegisterEvents();
    }

    protected virtual void RegisterEvents()
    {

    }

    protected virtual void DeregisterEvents()
    {

    }

    private void OnDestroy()
    {
        DeregisterEvents();
    }
}
