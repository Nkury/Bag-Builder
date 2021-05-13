using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    #region DELEGATES
    public delegate void PlaySpell( List<OrbInfo> orbInfo );
    #endregion

    #region EVENTS
    public static event PlaySpell PlayEnemySpellEvent;
    #endregion

    [SerializeField]
    private CombatEncounterView _combatEncounterView;

    private IContext _context;
    private Enemy _currentEnemy;
    private Bag _playerBag;
    private Bag _currentEnemyBag;

    private float _seconds = 0;

    public void Setup( IContext context )
    {
        _context = context;
        _currentEnemy = _context.EnemyManager.CurrentEnemy;
        _currentEnemyBag = _context.BagManager.CurrentEnemyBag;
        _playerBag = _context.BagManager.PlayerBag;
        RegisterCallbacks();
    }
    
    private void RegisterCallbacks()
    {
        CombatEncounterView.DrawOrbClickEvent += OnDrawOrbClick;
    }

    public void Teardown()
    {
        DeregisterCallbacks();
    }

    private void DeregisterCallbacks()
    {
        CombatEncounterView.DrawOrbClickEvent += OnDrawOrbClick;
    }

    // Update is called once per frame
    async void Update()
    {
        _seconds += Time.deltaTime;

        if( _currentEnemy.CanDrawOrb() && _seconds > _currentEnemy.GetDrawSpeed() )
        {
            Debug.Log( string.Format( "Enemy draws orb after {0} seconds", _seconds) );
            _seconds = 0;
            DrawEnemyOrb();
            Spell spell = await EnemyAttackIfAble();            
        }
    }

    public void DrawEnemyOrb()
    {
        Orb drawnOrb = _currentEnemyBag.DrawOrb();
        if( drawnOrb != null )
        {
            Debug.Log( string.Format( "Enemy draws a {0} orb" ,
                       _context.LocalizationManager.GetLocalizedString( drawnOrb.orb_name ) ) );
            _currentEnemy.UpdateDrawnOrbInfo( drawnOrb );
        }
    }

    // returns a spell if it can cast one
    // otherwise, returns null
    public async Task<Spell> EnemyAttackIfAble()
    {
        await Task.Delay( 1000 ); // wait a second before attacking if able

        string intent = _currentEnemy.Intent;
        Spell intendedSpell = null;
        if( !string.IsNullOrEmpty( intent ) )
        {
            intendedSpell = _context.SpellManager.GetEnemySpell( intent );
            Debug.Log( string.Format( "Enemy intends to use {0} attack" ,
                                     _context.LocalizationManager.GetLocalizedString( intendedSpell.Name ) ) );
            List<OrbInfo> requiredOrbs;
            if( _currentEnemyBag.DrawnOrbsMeetsRequirements( intendedSpell.OrbRequirements, out requiredOrbs ) )
            {
                Debug.Log( string.Format("Enemy executes a {0} attack",
                                        _context.LocalizationManager.GetLocalizedString( intendedSpell.Name )));
                intendedSpell.Effect();
                _context.BagManager.RemoveOrbToEnemyBag( intendedSpell.OrbRequirements );
                PlayEnemySpellEvent?.Invoke( requiredOrbs );
                return intendedSpell;
            }
        }

        return intendedSpell;
    }
    public void OnDrawOrbClick()
    {

    }
}
