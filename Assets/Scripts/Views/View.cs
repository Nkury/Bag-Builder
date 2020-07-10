using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    protected State _viewState;
    protected IContext _context; 

    // Start is called before the first frame update
    public virtual void InstantiateView( State state, Context context )
    {
        _viewState = state;
        _context = context;
    }
}
