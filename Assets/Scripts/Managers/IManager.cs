﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public interface IManager
{
    Task Setup ( IContext context );
    void Teardown (); 
}