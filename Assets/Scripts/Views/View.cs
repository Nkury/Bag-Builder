using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    private State _viewState;

    // Start is called before the first frame update
    public virtual void InstantiateView( State state )
    {
        _viewState = state;
    }
}
