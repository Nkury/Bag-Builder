using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : State
{ 
    private static readonly string PREFAB_NAME = "CombatEncounterView";

    private CombatEncounterView _combatEncounterView;

    public override void Setup( IContext context )
    {
        base.Setup( context );
    }

    public override void Enter()
    {
        CreateCombatView();
    }

    private void CreateCombatView()
    {
        _context.UIManager.InstantiateViewOnDefaultCanvas( PREFAB_NAME, gameObj =>
        {
            if( gameObj != null )
            {
                CombatEncounterView view = gameObj.GetComponent<CombatEncounterView>();
                if( view != null )
                {
                    _combatEncounterView = view;
                    _combatEncounterView.InstantiateView( _context );
                    _combatEncounterView.Setup( _context.SpellManager.GetPlayerSpells() );
                    _combatEncounterView.SetupCombatController( _context );
                }
            }
        } );        
    }
}

