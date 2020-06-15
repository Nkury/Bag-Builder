using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Context : IContext
{
    public BagManager BagManager { get; set; }
    public PlayerManager PlayerManager { get; set; }
    public SpellManager SpellManager { get; set; }
    public EnemyManager EnemyManager { get; set; }

    public static Context context;

    private List<IManager> _managerList;

    public Context ( )
    {
        _managerList = new List<IManager>();
        context = this;
    }

    public void AddManagers( params IManager[] managers )
    {
        int numOfManagers = managers.Length;

        for( int i = 0; i < numOfManagers; i++ )
        {
            IManager managerToAdd = AssignManager( managers [ i ] );
            _managerList.Add( managerToAdd );
        }
    }

    public void SetupManagers ( )
    {
        int numOfManagers = _managerList.Count;

        for( int i = 0; i < numOfManagers; i++ )
        {
            _managerList [ i ].Setup( );
        }
    }

    public void TeardownManagers()
    {
        int numOfManagers = _managerList.Count;

        for(int i = 0; i < numOfManagers; i++ )
        {
            _managerList[ i ].Teardown();
        }
    }

    private IManager AssignManager( IManager manager )
    {
        if(manager is BagManager )
        {
            BagManager = ( BagManager ) manager;
            return BagManager;
        } else if(manager is PlayerManager )
        {
            PlayerManager = ( PlayerManager ) manager;
            return PlayerManager;
        } else if(manager is SpellManager )
        {
            SpellManager = ( SpellManager ) manager;
            return SpellManager;
        } else if(manager is EnemyManager )
        {
            EnemyManager = ( EnemyManager ) manager;
            return EnemyManager;
        }

        return null;
    }
}
