using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBasicSpell : Spell
{
    public int ShieldPower;

    public override void Effect()
    {
        // TODO
    }

    public override string GetLocalizedDescription()
    {
        return context.LocalizationManager.GetLocalizedString( Description , ShieldPower.ToString() );
    }
}
