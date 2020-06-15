using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : Manager
{
    private static readonly string _filePath = "Spells";
    Dictionary<string , Spell> playerSpells;
    Dictionary<string , Spell> enemySpells;

    public override void Setup ( )
    {
        playerSpells = new Dictionary<string , Spell>();
        enemySpells = new Dictionary<string , Spell>();
        base.Setup( );
    }

    private Spell CreateSpell(Spell spell)
    {
        switch( spell.GetSpellType() )
        {
            case SpellType.FIREBASICSPELL:
                return ( FireBasicSpell ) spell;
            case SpellType.ICEBASICSPELL:
                // TODO
                break;
            case SpellType.LIGHTNINGBASICSPELL:
                // TODO
                break;
        }

        return null;
    }
}
