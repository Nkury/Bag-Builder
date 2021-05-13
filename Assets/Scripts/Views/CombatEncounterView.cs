using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CombatEncounterView : View
{
    #region DELEGATES
    public delegate void OnDrawOrb();
    #endregion

    #region EVENTS
    public static event OnDrawOrb DrawOrbClickEvent;
    #endregion

    [SerializeField]
    private Image _playerAvatar;

    [SerializeField]
    private Image _enemyAvatar;

    [SerializeField]
    private Slider _playerHealthSlider;

    [SerializeField]
    private TextMeshProUGUI _playerHealthText;

    [SerializeField]
    private Slider _playerManaSlider;

    [SerializeField]
    private TextMeshProUGUI _playerManaText;

    [SerializeField]
    private TextMeshProUGUI _playerSkullLimitText;

    [SerializeField]
    private TextMeshProUGUI _playerBagLimitText;

    [SerializeField]
    private Slider _enemyHealthSlider;

    [SerializeField]
    private TextMeshProUGUI _enemyHealthText;

    [SerializeField]
    private TextMeshProUGUI _enemyNameText;

    [SerializeField]
    private GameObject[] _playerDrawnOrbPositions;

    private OrbDraggable[] _playerOrbPositionsOccupied;

    [SerializeField]
    private GameObject[] _enemyDrawnOrbPositions;

    private OrbDraggable[] _enemyOrbPositionsOccupied;

    [SerializeField]
    private GameObject[] _playerSpellPositions;

    [SerializeField]
    private SpellView _spellViewPrefab;

    [SerializeField]
    private OrbDraggable _orbDraggablePrefab;

    [SerializeField]
    private CombatController _controller;

    public override void InstantiateView( IContext context )
    {
        base.InstantiateView( context );
    }

    public void Setup( List<Spell> spells )
    {
        SetupPlayerSpells( spells );
        _enemyOrbPositionsOccupied = new OrbDraggable[ _enemyDrawnOrbPositions.Length ];
        _playerOrbPositionsOccupied = new OrbDraggable[ _playerDrawnOrbPositions.Length ];
    }

    public void SetupCombatController( IContext context )
    {
        _controller.Setup( context );
    }

    protected override void RegisterEvents()
    {
        CombatController.PlayEnemySpellEvent += RemoveEnemyOrbsFromDrawnOrbs;
        BagManager.PlayerDrawOrbEvent += SetPlayerDrawnOrb;
        BagManager.EnemyDrawOrbEvent += SetEnemyDrawnOrb;
    }

    protected override void DeregisterEvents()
    {
        CombatController.PlayEnemySpellEvent -= RemoveEnemyOrbsFromDrawnOrbs;
        BagManager.PlayerDrawOrbEvent -= SetPlayerDrawnOrb;
        BagManager.EnemyDrawOrbEvent -= SetEnemyDrawnOrb;
    }

    private void SetupPlayerSpells( List<Spell> spells )
    {
        int numSpells = spells.Count;
        for(int spellIndex = 0; spellIndex < numSpells; spellIndex++ )
        {
            if( !spells[ spellIndex ].IsBasic ) // if it's not a basic spell, then display in spell area
            {
                SpellView spellView = GameObject.Instantiate<SpellView>( _spellViewPrefab , _playerSpellPositions[ spellIndex ].transform );
                spellView.Setup( spells[ spellIndex ] );
            }
        }
    }

    public void OnDrawOrbClickHandler()
    {
        DrawOrbClickEvent();
    }

    #region ADD_ORBS
    public void SetPlayerDrawnOrb( Orb orb )
    {
        if( orb != null )
        {
            for( int orbIndex = 0; orbIndex < _playerOrbPositionsOccupied.Length; orbIndex++ )
            {
                OrbDraggable orbDraggable = _playerOrbPositionsOccupied[ orbIndex ];

                if( orbDraggable == null )
                {
                    orbDraggable = GameObject.Instantiate<OrbDraggable>( _orbDraggablePrefab , _playerDrawnOrbPositions[ orbIndex ].transform );
                    orbDraggable.Setup( _context );
                    _playerOrbPositionsOccupied[ orbIndex ] = orbDraggable;
                }

                if( orbDraggable.OrbType == OrbType.NONE )
                {
                    orbDraggable.SetOrb( orb );
                    break;
                }                
            }
        }
    }

    public void SetEnemyDrawnOrb( Orb orb )
    {
        if( orb != null )
        {
            for( int orbIndex = 0; orbIndex < _enemyOrbPositionsOccupied.Length; orbIndex++ )
            {
                OrbDraggable orbDraggable = _enemyOrbPositionsOccupied[ orbIndex ];

                if( orbDraggable == null )
                {
                    orbDraggable = GameObject.Instantiate<OrbDraggable>( _orbDraggablePrefab , _enemyDrawnOrbPositions[ orbIndex ].transform );
                    orbDraggable.Setup( _context );
                    _enemyOrbPositionsOccupied[ orbIndex ] = orbDraggable;
                }

                if( orbDraggable.OrbType == OrbType.NONE )
                {
                    orbDraggable.SetOrb( orb );
                    break;
                }
            }
        }
    }
    #endregion
    
    #region REMOVE_ORBS
    private void RemovePlayerOrbsFromDrawnOrbs( List<OrbInfo> orbInfo )
    {
        for( int orbInfoIndex = 0; orbInfoIndex < orbInfo.Count; orbInfoIndex++ )
        {
            RemovePlayerOrb( orbInfo[ orbInfoIndex ].orb_name , orbInfo[ orbInfoIndex ].quantity );
        }
    }

    private void RemoveEnemyOrbsFromDrawnOrbs( List<OrbInfo> orbInfo )
    {
        for( int orbInfoIndex = 0; orbInfoIndex < orbInfo.Count; orbInfoIndex++ )
        {
            RemoveEnemyOrb( orbInfo[ orbInfoIndex ].orb_name, orbInfo[ orbInfoIndex ].quantity );
        }
    }

    private void RemovePlayerOrb( string orb_name, int quantity )
    {
        for( int orbIndex = 0; orbIndex < _playerOrbPositionsOccupied.Length; orbIndex++ )
        {
            if( _playerOrbPositionsOccupied[ orbIndex ].OrbName == orb_name )
            {
                // remove player orb draggable
                _playerOrbPositionsOccupied[ orbIndex ].ClearOrbDraggable();
                quantity--;
                if( quantity <= 0 )
                {
                    break;
                }
            }
        }
    }

    private void RemoveEnemyOrb( string orb_name, int quantity )
    {
        for( int orbIndex = 0; orbIndex < _enemyOrbPositionsOccupied.Length; orbIndex++ )
        {
            if( _enemyOrbPositionsOccupied[ orbIndex ] != null &&
                _enemyOrbPositionsOccupied[ orbIndex ].OrbName  == orb_name )
            {
                // remove player orb draggable
                _enemyOrbPositionsOccupied[ orbIndex ].ClearOrbDraggable();
                quantity--;
                if( quantity <= 0 )
                {
                    break;
                }
            }
        }
    }
    #endregion

}
