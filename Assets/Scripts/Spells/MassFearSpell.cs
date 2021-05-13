using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassFearSpell : Spell
{
    public int Damage;
    public int StunDuration;

    public override void Effect()
    {
        if( isEnemy )
        {
            context.PlayerManager.DealDamage( Damage );
            context.BagManager.DrawEnemyBagAndAttack();
        }
        else
        {
            context.EnemyManager.DealDamage( Damage );
        }
    }

    public override string GetLocalizedDescription()
    {
        return context.LocalizationManager.GetLocalizedString( Description , Damage.ToString() , StunDuration.ToString() );
    }
}
