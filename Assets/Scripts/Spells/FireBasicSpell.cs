﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBasicSpell : Spell
{
    public int Damage;

    public override void Effect()
    {
        Context.context.EnemyManager.DealDamage( Damage );
    }
}
