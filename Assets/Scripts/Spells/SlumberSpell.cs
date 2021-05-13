using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlumberSpell : Spell
{
    public int HealPower;
    public int DrawPower;

    public override void Effect()
    {
        if( !isEnemy )
        {
            context.PlayerManager.HealDamage( HealPower );
            for( int i = 0; i < DrawPower; i++ )
            {
                context.BagManager.DrawPlayerBag();
            }
        }
        else
        {
            context.EnemyManager.HealDamage( HealPower );
            context.EnemyManager.ResetDraw();
            context.EnemyManager.OverrideDrawUpdate( true );
            for( int i = 0; i < DrawPower; i++ )
            {
                context.BagManager.DrawEnemyBag();
            }

            context.EnemyManager.OverrideDrawUpdate( false );
        }
    }

    public override string GetLocalizedDescription()
    {
        return context.LocalizationManager.GetLocalizedString( Description , HealPower.ToString(), DrawPower.ToString() );
    }
}
