using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBasicSpell : Spell
{
    public int Damage;

    public override void Effect()
    {
        if( isEnemy )
        {
            context.PlayerManager.DealDamage( Damage );
        }
        else
        {
            context.EnemyManager.DealDamage( Damage );
        }        
    }

    public override string GetLocalizedDescription()
    {
        return context.LocalizationManager.GetLocalizedString( Description , Damage.ToString() );
    }
}
