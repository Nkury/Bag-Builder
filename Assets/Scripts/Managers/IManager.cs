using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManager
{
    void Setup ( IContext context );
    void Teardown (); 
}