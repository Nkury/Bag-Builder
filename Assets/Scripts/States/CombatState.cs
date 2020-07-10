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
        GameObject view = _context.UIManager.InstantiateViewOnDefaultCanvas( PREFAB_NAME );
        _combatEncounterView = view.GetComponent<CombatEncounterView>();
        _combatEncounterView.Setup( _context.SpellManager.GetPlayerSpells() );
    }
}

