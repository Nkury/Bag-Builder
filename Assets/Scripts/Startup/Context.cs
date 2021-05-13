using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class Context : IContext
{
    #region DELEGATES
    public delegate void OnSetupComplete();
    #endregion

    #region EVENTS
    public static event OnSetupComplete SetupCompleteEvent;
    #endregion

    public OrbManager OrbManager { get; set; }
    public BagManager BagManager { get; set; }
    public PlayerManager PlayerManager { get; set; }
    public SpellManager SpellManager { get; set; }
    public EnemyManager EnemyManager { get; set; }
    public UIManager UIManager { get; set; }
    public StateMachine StateMachine { get; set; }
    public LocalizationManager LocalizationManager { get; set; }
    public AssetManager AssetManager { get; set; }
    private List<IManager> _managerList;

    public Context ( )
    {
        _managerList = new List<IManager>();
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

    public async void SetupManagers ( )
    {
        int numOfManagers = _managerList.Count;

        for( int i = 0; i < numOfManagers; i++ )
        {
            await _managerList [ i ].Setup( this );
        }

        SetupCompleteEvent?.Invoke();
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
        if ( manager is OrbManager )
        {
            OrbManager = ( OrbManager ) manager;
            return OrbManager;
        }
        else if( manager is BagManager )
        {
            BagManager = ( BagManager ) manager;
            return BagManager;
        }
        else if( manager is PlayerManager )
        {
            PlayerManager = ( PlayerManager ) manager;
            return PlayerManager;
        }
        else if( manager is SpellManager )
        {
            SpellManager = ( SpellManager ) manager;
            return SpellManager;
        }
        else if( manager is EnemyManager )
        {
            EnemyManager = ( EnemyManager ) manager;
            return EnemyManager;
        }
        else if( manager is UIManager )
        {
            UIManager = ( UIManager ) manager;
            return UIManager;
        }
        else if( manager is StateMachine )
        {
            StateMachine = ( StateMachine ) manager;
            return StateMachine;
        }
        else if( manager is LocalizationManager )
        {
            LocalizationManager = ( LocalizationManager ) manager;
            return LocalizationManager;
        } else if( manager is AssetManager )
        {
            AssetManager = ( AssetManager ) manager;
            return AssetManager;
        }

        return null;
    }
}
