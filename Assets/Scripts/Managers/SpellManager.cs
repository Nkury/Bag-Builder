using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpellManager : Manager
{
    private static readonly string _filePath = "GameData/Spells";
    Dictionary<string , Spell> allSpells;
    Dictionary<string , Spell> playerSpells;
    Dictionary<string , Spell> enemySpells;

    public override void Setup ( )
    {
        allSpells = new Dictionary<string , Spell>();
        playerSpells = new Dictionary<string , Spell>();
        enemySpells = new Dictionary<string , Spell>();

        string[] files = Directory.GetFiles( _filePath );

        for(int i = 0; i < files.Length; i++ )
        {
            string json = File.ReadAllText( files[ i ] );
            Spell spell = JsonUtility.FromJson<Spell>( json );
            allSpells.Add( spell.Name , CreateSpell( spell.Type , json ) );
        }
    }

    private Spell CreateSpell(SpellType spellType, string json)
    {
        switch( spellType )
        {
            case SpellType.FIREBASICSPELL:
                return JsonUtility.FromJson<FireBasicSpell>( json );
            case SpellType.ICEBASICSPELL:
                return JsonUtility.FromJson<IceBasicSpell>( json );
            case SpellType.LIGHTNINGBASICSPELL:
                return JsonUtility.FromJson<LightningBasicSpell>( json );
        }

        return null;
    }

    public void LoadPlayerSpells( List<string> spells )
    {
        int spellCount = spells.Count;
        for(int spellIndex = 0; spellIndex < spellCount; spellIndex++ )
        {
            string spellName = spells[ spellIndex ];
            playerSpells[ spellName ] = allSpells[ spellName ];
        }
    }

    public void LoadEnemySpells( List<string> spells )
    {
        int spellCount = spells.Count;
        for( int spellIndex = 0; spellIndex < spellCount; spellIndex++ )
        {
            string spellName = spells[ spellIndex ];
            enemySpells[ spellName ] = allSpells[ spellName ];
        }
    }
}
