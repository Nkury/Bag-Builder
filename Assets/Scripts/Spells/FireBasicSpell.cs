using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBasicSpell : Spell
{
    private int _damage;

    public override void Effect()
    {
        Context.context.EnemyManager.DealDamage( _damage );
    }
}
